using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Dyno_Geely {
    public class RecvDynoAckEventArgs : EventArgs {
        public MsgAckBaseStr MsgACK { get; set; }
    }

    public class DynoCmd {
        public event EventHandler<RecvDynoAckEventArgs> RecvDynoAck;
        private const int BUFSIZE = 4096;
        private readonly Logger _log;
        private readonly Config _cfg;
        private TcpClient _client;
        private NetworkStream _clientStream;
        private readonly byte[] _recvBuf;
        private readonly ManualResetEvent _RecvFlag;
        private readonly IsoDateTimeConverter _dateTimeConverter;
        private readonly object _msgAckRecvLock = new object();
        private MsgAckBaseStr _msgAckRecv;

        public bool Connected { get; private set; }
        public string ClientID { get; private set; }
        public string ServiceID { get; private set; }
        public string MsgID {
            get {
                return MD5Encrypt(GetMD5InitString());
            }
        }

        public DynoCmd(Config cfg) {
            _log = new Logger("DynoCmd", ".\\log", EnumLogLevel.LogLevelAll, true, 100);
            _log.TraceInfo("================================================================================");
            _log.TraceInfo("============================== DynoCmd Started =================================");
            _log.TraceInfo("================================================================================");
            _cfg = cfg;
            _recvBuf = new byte[BUFSIZE];
            _RecvFlag = new ManualResetEvent(true);
            Connected = false;
            ClientID = "SaiHe";
            ServiceID = MD5Encrypt(GetMD5InitString());
            _dateTimeConverter = new IsoDateTimeConverter { DateTimeFormat = "yyyy-M-dd HH:mm:ss" };
        }

        ~DynoCmd() {
            SafeClose();
        }

        private void SafeClose() {
            if (_clientStream != null) {
                _clientStream.Close();
            }
            if (_client != null) {
                _client.Close();
            }
            Connected = false;
        }

        public bool ConnectServer() {
            try {
                _client = new TcpClient(_cfg.Main.Data.DynoIP, _cfg.Main.Data.DynoPort);
                _clientStream = _client.GetStream();
                _log.TraceInfo("TcpClient init success");
                Connected = true;
            } catch (Exception ex) {
                SafeClose();
                _log.TraceError("TcpClient init error: " + ex.Message);
                Connected = false;
            }
            return Connected;
        }

        private bool TestConnect() {
            if (!Connected) {
                return false;
            }
            Socket cltSocket = _client.Client;
            bool bRet = false;
            bool blockingState = cltSocket.Blocking;
            try {
                byte[] tmp = new byte[1];
                cltSocket.Blocking = false;
                cltSocket.Send(tmp, 0, 0);
                bRet = true;
            } catch (SocketException ex) {
                if (ex.NativeErrorCode.Equals(10035)) {
                    // 10035 == WSAEWOULDBLOCK
                    bRet = true;
                    _log.TraceWarning(string.Format("Still Connected, but the Send would block[{0}]", ex.NativeErrorCode));
                } else {
                    bRet = false;
                    _log.TraceError(string.Format("Disconnected: {0}[{1}]", ex.Message, ex.NativeErrorCode));
                }
            } finally {
                cltSocket.Blocking = blockingState;
            }
            return bRet;
        }

        public bool SafeTestConnect(int times = 3) {
            bool bRet = false;
            for (int i = 0; i < times; i++) {
                bRet = bRet || TestConnect();
            }
            for (int i = 0; i < times && !bRet; i++) {
                SafeClose();
                bRet = ConnectServer();
            }
            return bRet;
        }

        public bool ReconnectServer() {
            SafeClose();
            return ConnectServer();
        }

        public void SendCmd(MsgBaseStr msgCmd) {
            Connected = SafeTestConnect();
            if (!Connected) {
                _log.TraceError("Can't connect server");
                throw new ApplicationException("Can't connect server");
            }
            msgCmd.MsgID = MsgID;
            msgCmd.ServiceID = ServiceID;
            string strSend = JsonConvert.SerializeObject(msgCmd, _dateTimeConverter);
            byte[] sendMessage = Encoding.UTF8.GetBytes(strSend);
            _clientStream.Write(sendMessage, 0, sendMessage.Length);
            _clientStream.Flush();
            string JsonFormatted = JsonConvert.SerializeObject(msgCmd, Newtonsoft.Json.Formatting.Indented, _dateTimeConverter);
            _log.TraceInfo("Dyno client send: " + Environment.NewLine + JsonFormatted);
            Task.Factory.StartNew(RecvMsg, msgCmd.MsgID);
        }

        private void RecvMsg(object obj) {
            string MsgID = (string)obj;
            RecvDynoAckEventArgs args = new RecvDynoAckEventArgs();
            int bytesRead;
            string strRecv = string.Empty;
            try {
                do {
                    bytesRead = _clientStream.Read(_recvBuf, 0, BUFSIZE);
                    strRecv += Encoding.UTF8.GetString(_recvBuf, 0, bytesRead);
                } while (_clientStream.DataAvailable);
            } catch (Exception ex) {
                _log.TraceError("RecvMsg() occur error: " + ex.Message);
                return;
            }
            //_log.TraceInfo("Dyno client received raw message: " + Environment.NewLine + strRecv);
            // 去除头部的一个'{'和尾部的一个'}'
            // 不能使用 strRecv.Trim(new char[] { '{', '}' })，因为会删除尾部连续多个'}'
            if (strRecv.StartsWith("{")) {
                strRecv = strRecv.Substring(1, strRecv.Length - 1);
            }
            if (strRecv.EndsWith("}")) {
                strRecv = strRecv.Substring(0, strRecv.Length - 1);
            }
            string[] strRecvs = strRecv.Split(new string[] { "}{" }, StringSplitOptions.RemoveEmptyEntries);
            lock (_msgAckRecvLock) {
                _msgAckRecv = null;
                for (int i = 0; i < strRecvs.Length; i++) {
                    strRecvs[i] = "{" + strRecvs[i] + "}";
                    MsgAckBaseStr msgAck = JsonConvert.DeserializeObject<MsgAckBaseStr>(strRecvs[i], _dateTimeConverter);
                    if (msgAck.MsgID == MsgID) {
                        JObject JObj = JObject.Parse(strRecvs[i]);
                        _log.TraceInfo("Dyno client find corresponding MsgAckBaseStr: " + Environment.NewLine + JObj.ToString());
                        args.MsgACK = msgAck;
                        _msgAckRecv = msgAck;
                        break;
                    }
                }
                if (_msgAckRecv == null) {
                    _log.TraceError("Dyno client didn't receive corresponding MsgAckBaseStr");
                }
                _RecvFlag.Set();
                RecvDynoAck?.Invoke(this, args);
            }
        }

        public static string MD5Encrypt(string strText) {
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] result = md5.ComputeHash(Encoding.UTF8.GetBytes(strText));
            string strResult = string.Empty;
            for (int i = 0; i < result.Length; i++) {
                strResult += result[i].ToString("X2");
            }
            return strResult;
        }

        public static string GetMD5InitString() {
            return "SAIHE" + DateTime.Now.ToLocalTime().ToString("yyyy-MM-dd HH:mm:ss.fff") + Guid.NewGuid().ToString();
        }

        private void Transact(MsgBaseStr msgCmd, bool bShowDlg) {
            SendCmd(msgCmd);
            _RecvFlag.Reset();
            bool recvResult = false;
            if (bShowDlg) {
                LoadingForm frmLoading = new LoadingForm();
                frmLoading.BackgroundWorkAction = () => {
                    frmLoading.CurrentMsg = new KeyValuePair<int, string>(50, string.Format("执行{0}命令中。。。", msgCmd.Cmd));
                    if (_RecvFlag.WaitOne(_cfg.Main.Data.RecvTimeout, false)) {
                        recvResult = true;
                    }
                };
                frmLoading.ShowDialog();
            } else {
                if (_RecvFlag.WaitOne(_cfg.Main.Data.RecvTimeout, false)) {
                    recvResult = true;
                }
            }
            if (!recvResult) {
                SafeClose();
                _log.TraceError("MsgCmd[" + msgCmd.Cmd + "]Receive timeout");
                throw new ApplicationException("MsgCmd[" + msgCmd.Cmd + "]Receive timeout");
            }
        }

        private bool DoCmd(string strCmd, object cmdParams, bool bShowDlg) {
            MsgBaseStr msgCmd = new MsgBaseStr {
                Cmd = strCmd,
                MsgID = MsgID,
                ServiceID = ServiceID
            };
            msgCmd.Params = cmdParams;
            try {
                Transact(msgCmd, bShowDlg);
                return true;
            } catch (Exception ex) {
                _log.TraceError(string.Format("{0} error: {1}", msgCmd.Cmd, ex.Message));
                return false;
            }
        }

        public bool SocketLongConnection(ref SocketLongConnectionAckParams ackParams) {
            SocketLongConnectionParams cmdParams = new SocketLongConnectionParams {
                ClientID = ClientID
            };
            if (!DoCmd("SocketLongConnection", cmdParams, false)) {
                _log.TraceError("DoCmd(\"SocketLongConnection\") return false");
                return false;
            }
            if (_msgAckRecv != null && _msgAckRecv.Cmd == "SocketLongConnectionAck" && _msgAckRecv.Params != null) {
                ackParams = JsonConvert.DeserializeObject<SocketLongConnectionAckParams>(((JObject)_msgAckRecv.Params).ToString(), _dateTimeConverter);
                return true;
            } else {
                _log.TraceError("_msgAckRecv.Cmd[\"SocketLongConnectionAck\"] is wrong");
                return false;
            }
        }

        public bool LoginCmd() {
            LoginParams cmdParams = new LoginParams {
                ClientID = ClientID
            };
            if (!DoCmd("Login", cmdParams, false)) {
                _log.TraceError("DoCmd(\"Login\") return false");
                return false;
            }
            if (_msgAckRecv != null && _msgAckRecv.Cmd == "LoginAck") {
                return true;
            } else {
                _log.TraceError("_msgAckRecv.Cmd[\"LoginAck\"] is wrong");
                return false;
            }
        }

        public bool LogoutCmd() {
            LoginParams cmdParams = new LoginParams {
                ClientID = ClientID
            };
            if (!DoCmd("Logout", cmdParams, false)) {
                _log.TraceError("DoCmd(\"Logout\") return false");
                return false;
            }
            if (_msgAckRecv != null && _msgAckRecv.Cmd == "LogoutAck") {
                return true;
            } else {
                _log.TraceError("_msgAckRecv.Cmd[\"LogoutAck\"] is wrong");
                return false;
            }
        }

        private bool DeviceCtrlCmd(DeviceCtrlParams.CtrlIDStr strCtrlID) {
            MsgBaseStr msgCmd = new MsgBaseStr {
                Cmd = "DeviceCtrl",
                MsgID = MsgID,
                ServiceID = ServiceID
            };
            DeviceCtrlParams cmdParams = new DeviceCtrlParams {
                ctrlid = strCtrlID
            };
            msgCmd.Params = cmdParams;
            try {
                Transact(msgCmd, true);
            } catch (Exception ex) {
                _log.TraceError(string.Format("{0}.{1} error: {2}", msgCmd.Cmd, strCtrlID, ex.Message));
                return false;
            }
            if (_msgAckRecv != null && _msgAckRecv.Cmd == "DeviceCtrlAck") {
                return true;
            } else {
                _log.TraceError("_msgAckRecv.Cmd[\"DeviceCtrlAck\"] is wrong");
                return false;
            }
        }

        public bool DynoBeamDownCmd() {
            return DeviceCtrlCmd(DeviceCtrlParams.CtrlIDStr.CTRLID_DYNO_BEAM_DOWN);
        }

        public bool DynoBeamUpCmd() {
            return DeviceCtrlCmd(DeviceCtrlParams.CtrlIDStr.CTRLID_DYNO_BEAM_UP);
        }

        public bool StartDynoPreheatCmd(bool stopCheck, out string msg) {
            msg = string.Empty;
            StartDynoPreheatParams cmdParams = new StartDynoPreheatParams {
                ClientID = ClientID,
                stopCheck = stopCheck
            };
            if (!DoCmd("StartDynoPreheat", cmdParams, true)) {
                _log.TraceError("DoCmd(\"StartDynoPreheat\") return false");
                return false;
            }
            if (_msgAckRecv != null && _msgAckRecv.Cmd == "StartDynoPreheatAck" && _msgAckRecv.Params != null) {
                StartDynoPreheatAckParams ackParams = JsonConvert.DeserializeObject<StartDynoPreheatAckParams>(((JObject)_msgAckRecv.Params).ToString(), _dateTimeConverter);
                msg = ackParams.msg;
                return true;
            } else {
                _log.TraceError("_msgAckRecv.Cmd[\"StartDynoPreheatAck\"] is wrong");
                return false;
            }
        }

        public bool GetDynoPreheatRealTimeDataCmd(ref GetDynoPreheatRealTimeDataAckParams ackParams) {
            GetDynoPreheatRealTimeDataParams cmdParams = new GetDynoPreheatRealTimeDataParams {
                ClientID = ClientID
            };
            if (!DoCmd("GetDynoPreheatRealTimeData", cmdParams, false)) {
                _log.TraceError("DoCmd(\"GetDynoPreheatRealTimeData\") return false");
                return false;
            }
            if (_msgAckRecv != null && _msgAckRecv.Cmd == "GetDynoPreheatRealTimeDataAck" && _msgAckRecv.Params != null) {
                ackParams = JsonConvert.DeserializeObject<GetDynoPreheatRealTimeDataAckParams>(((JObject)_msgAckRecv.Params).ToString(), _dateTimeConverter);
                return true;
            } else {
                _log.TraceError("_msgAckRecv.Cmd[\"GetDynoPreheatRealTimeDataAck\"] is wrong");
                return false;
            }
        }

        public bool SaveDynoPreheatDataCmd(SaveDynoPreheatDataParams cmdParams) {
            if (!DoCmd("SaveDynoPreheatData", cmdParams, true)) {
                _log.TraceError("DoCmd(\"SaveDynoPreheatData\") return false");
                return false;
            }
            if (_msgAckRecv != null && _msgAckRecv.Cmd == "SaveDynoPreheatDataAck") {
                return true;
            } else {
                _log.TraceError("_msgAckRecv.Cmd[\"SaveDynoPreheatDataAck\"] is wrong");
                return false;
            }
        }

        public bool StartGasBoxPreheatSelfCheckCmd(bool stopCheck, int step, bool isQY, bool isRetry) {
            StartGasBoxPreheatSelfCheckParams cmdParams = new StartGasBoxPreheatSelfCheckParams {
                ClientID = ClientID,
                stopCheck = stopCheck,
                step = step,
                isQY = isQY,
                isRetry = isRetry
            };
            if (!DoCmd("StartGasBoxPreheatSelfCheck", cmdParams, true)) {
                _log.TraceError("DoCmd(\"StartGasBoxPreheatSelfCheck\") return false");
                return false;
            }
            if (_msgAckRecv != null && _msgAckRecv.Cmd == "StartGasBoxPreheatSelfCheckAck") {
                return true;
            } else {
                _log.TraceError("_msgAckRecv.Cmd[\"StartGasBoxPreheatSelfCheckAck\"] is wrong");
                return false;
            }
        }

        public bool GetGasBoxPreheatSelfCheckRealTimeDataCmd(ref GetGasBoxPreheatSelfCheckRealTimeDataAckParams ackParams) {
            GetGasBoxPreheatSelfCheckRealTimeDataParams cmdParams = new GetGasBoxPreheatSelfCheckRealTimeDataParams {
                ClientID = ClientID
            };
            if (!DoCmd("GetGasBoxPreheatSelfCheckRealTimeData", cmdParams, false)) {
                _log.TraceError("DoCmd(\"GetGasBoxPreheatSelfCheckRealTimeData\") return false");
                return false;
            }
            if (_msgAckRecv != null && _msgAckRecv.Cmd == "GetGasBoxPreheatSelfCheckRealTimeDataAck" && _msgAckRecv.Params != null) {
                ackParams = JsonConvert.DeserializeObject<GetGasBoxPreheatSelfCheckRealTimeDataAckParams>(((JObject)_msgAckRecv.Params).ToString(), _dateTimeConverter);
                return true;
            } else {
                _log.TraceError("_msgAckRecv.Cmd[\"GetGasBoxPreheatSelfCheckRealTimeDataAck\"] is wrong");
                return false;
            }
        }

        public bool StartFlowmeterCheckCmd(bool stopCheck, int step, double tempe, double pressure, ref StartFlowmeterCheckAckParams ackParams) {
            StartFlowmeterCheckParams cmdParams = new StartFlowmeterCheckParams {
                ClientID = ClientID,
                stopCheck = stopCheck,
                step = step,
                FlowmeterTargetPressure = pressure,
                FlowmeterTargetTempe = tempe
            };
            if (!DoCmd("StartFlowmeterCheck", cmdParams, true)) {
                _log.TraceError("DoCmd(\"StartFlowmeterCheck\") return false");
                return false;
            }
            if (_msgAckRecv != null && _msgAckRecv.Cmd == "StartFlowmeterCheckAck" && _msgAckRecv.Params != null) {
                ackParams = JsonConvert.DeserializeObject<StartFlowmeterCheckAckParams>(((JObject)_msgAckRecv.Params).ToString(), _dateTimeConverter);
                return true;
            } else {
                _log.TraceError("_msgAckRecv.Cmd[\"StartFlowmeterCheckAck\"] is wrong");
                return false;
            }
        }

        public bool GetFlowmeterCheckRealTimeDataCmd(bool Abandon, ref GetFlowmeterCheckRealTimeDataAckParams ackParams) {
            GetFlowmeterCheckRealTimeDataParams cmdParams = new GetFlowmeterCheckRealTimeDataParams {
                ClientID = ClientID,
                Abandon = Abandon
            };
            if (!DoCmd("GetFlowmeterCheckRealTimeData", cmdParams, false)) {
                _log.TraceError("DoCmd(\"GetFlowmeterCheckRealTimeData\") return false");
                return false;
            }
            if (_msgAckRecv != null && _msgAckRecv.Cmd == "GetFlowmeterCheckRealTimeDataAck" && _msgAckRecv.Params != null) {
                ackParams = JsonConvert.DeserializeObject<GetFlowmeterCheckRealTimeDataAckParams>(((JObject)_msgAckRecv.Params).ToString(), _dateTimeConverter);
                return true;
            } else {
                _log.TraceError("_msgAckRecv.Cmd[\"GetFlowmeterCheckRealTimeDataAck\"] is wrong");
                return false;
            }
        }

        public bool GetSmokerPreheatSelfCheckRealTimeDataCmd(ref GetSmokerPreheatSelfCheckRealTimeDataAckParams ackParams) {
            GetSmokerPreheatSelfCheckRealTimeDataParams cmdParams = new GetSmokerPreheatSelfCheckRealTimeDataParams {
                ClientID = ClientID
            };
            if (!DoCmd("GetSmokerPreheatSelfCheckRealTimeData", cmdParams, false)) {
                _log.TraceError("DoCmd(\"GetSmokerPreheatSelfCheckRealTimeData\") return false");
                return false;
            }
            if (_msgAckRecv != null && _msgAckRecv.Cmd == "GetSmokerPreheatSelfCheckRealTimeDataAck" && _msgAckRecv.Params != null) {
                ackParams = JsonConvert.DeserializeObject<GetSmokerPreheatSelfCheckRealTimeDataAckParams>(((JObject)_msgAckRecv.Params).ToString(), _dateTimeConverter);
                return true;
            } else {
                _log.TraceError("_msgAckRecv.Cmd[\"GetSmokerPreheatSelfCheckRealTimeDataAck\"] is wrong");
                return false;
            }
        }

        public bool SaveSmokerPreheatSelfCheckDataCmd(SaveSmokerPreheatSelfCheckDataParams cmdParams) {
            if (!DoCmd("SaveSmokerPreheatSelfCheckData", cmdParams, true)) {
                _log.TraceError("DoCmd(\"SaveSmokerPreheatSelfCheckData\") return false");
                return false;
            }
            if (_msgAckRecv != null && _msgAckRecv.Cmd == "SaveSmokerPreheatSelfCheckDataAck") {
                return true;
            } else {
                _log.TraceError("_msgAckRecv.Cmd[\"SaveSmokerPreheatSelfCheckDataAck\"] is wrong");
                return false;
            }
        }

        public bool GetWeatherCheckRealTimeDataCmd(ref GetWeatherCheckRealTimeDataAckParams ackParams) {
            GetWeatherCheckRealTimeDataParams cmdParams = new GetWeatherCheckRealTimeDataParams {
                ClientID = ClientID
            };
            if (!DoCmd("GetWeatherCheckRealTimeData", cmdParams, false)) {
                _log.TraceError("DoCmd(\"GetWeatherCheckRealTimeData\") return false");
                return false;
            }
            if (_msgAckRecv != null && _msgAckRecv.Cmd == "GetWeatherCheckRealTimeDataAck" && _msgAckRecv.Params != null) {
                ackParams = JsonConvert.DeserializeObject<GetWeatherCheckRealTimeDataAckParams>(((JObject)_msgAckRecv.Params).ToString(), _dateTimeConverter);
                return true;
            } else {
                _log.TraceError("_msgAckRecv.Cmd[\"GetWeatherCheckRealTimeDataAck\"] is wrong");
                return false;
            }
        }

        public bool SaveWeatherCheckDataCmd(SaveWeatherCheckDataParams cmdParams) {
            if (!DoCmd("SaveWeatherCheckData", cmdParams, true)) {
                _log.TraceError("DoCmd(\"SaveWeatherCheckData\") return false");
                return false;
            }
            if (_msgAckRecv != null && _msgAckRecv.Cmd == "SaveWeatherCheckDataAck") {
                return true;
            } else {
                _log.TraceError("_msgAckRecv.Cmd[\"SaveWeatherCheckDataAck\"] is wrong");
                return false;
            }
        }

        public bool StartTachometerCheckCmd(ref StartTachometerCheckAckParams ackParams) {
            StartTachometerCheckParams cmdParams = new StartTachometerCheckParams {
                ClientID = ClientID
            };
            if (!DoCmd("StartTachometerCheck", cmdParams, true)) {
                _log.TraceError("DoCmd(\"StartTachometerCheck\") return false");
                return false;
            }
            if (_msgAckRecv != null && _msgAckRecv.Cmd == "StartTachometerCheckAck" && _msgAckRecv.Params != null) {
                ackParams = JsonConvert.DeserializeObject<StartTachometerCheckAckParams>(((JObject)_msgAckRecv.Params).ToString(), _dateTimeConverter);
                return true;
            } else {
                _log.TraceError("_msgAckRecv.Cmd[\"StartFlowmeterCheckAck\"] is wrong");
                return false;
            }
        }

        public bool GetTachometerCheckRealTimeDataCmd(ref GetTachometerCheckRealTimeDataAckParams ackParams) {
            GetTachometerCheckRealTimeDataParams cmdParams = new GetTachometerCheckRealTimeDataParams {
                ClientID = ClientID
            };
            if (!DoCmd("GetTachometerCheckRealTimeData", cmdParams, false)) {
                _log.TraceError("DoCmd(\"GetTachometerCheckRealTimeData\") return false");
                return false;
            }
            if (_msgAckRecv != null && _msgAckRecv.Cmd == "GetTachometerCheckRealTimeDataAck" && _msgAckRecv.Params != null) {
                ackParams = JsonConvert.DeserializeObject<GetTachometerCheckRealTimeDataAckParams>(((JObject)_msgAckRecv.Params).ToString(), _dateTimeConverter);
                return true;
            } else {
                _log.TraceError("_msgAckRecv.Cmd[\"GetFlowmeterCheckRealTimeDataAck\"] is wrong");
                return false;
            }
        }

        public bool SaveTachometerCheckCmd(SaveTachometerCheckParams cmdParams) {
            if (!DoCmd("SaveTachometerCheck", cmdParams, true)) {
                _log.TraceError("DoCmd(\"SaveTachometerCheck\") return false");
                return false;
            }
            if (_msgAckRecv != null && _msgAckRecv.Cmd == "SaveTachometerCheckAck") {
                return true;
            } else {
                _log.TraceError("_msgAckRecv.Cmd[\"SaveTachometerCheckAck\"] is wrong");
                return false;
            }
        }

        public bool GetOilThermometerPreheatSelfCheckRealTimeDataCmd(ref GetOilThermometerPreheatSelfCheckRealTimeDataAckParams ackParams) {
            GetOilThermometerPreheatSelfCheckRealTimeDataParams cmdParams = new GetOilThermometerPreheatSelfCheckRealTimeDataParams {
                ClientID = ClientID
            };
            if (!DoCmd("GetOilThermometerPreheatSelfCheckRealTimeData", cmdParams, false)) {
                _log.TraceError("DoCmd(\"GetOilThermometerPreheatSelfCheckRealTimeData\") return false");
                return false;
            }
            if (_msgAckRecv != null && _msgAckRecv.Cmd == "GetOilThermometerPreheatSelfCheckRealTimeDataAck" && _msgAckRecv.Params != null) {
                ackParams = JsonConvert.DeserializeObject<GetOilThermometerPreheatSelfCheckRealTimeDataAckParams>(((JObject)_msgAckRecv.Params).ToString(), _dateTimeConverter);
                return true;
            } else {
                _log.TraceError("_msgAckRecv.Cmd[\"GetOilThermometerPreheatSelfCheckRealTimeDataAck\"] is wrong");
                return false;
            }
        }

        public bool SaveOilThermometerPreheatSelfCheckCmd(SaveOilThermometerPreheatSelfCheckParams cmdParams) {
            if (!DoCmd("SaveOilThermometerPreheatSelfCheck", cmdParams, true)) {
                _log.TraceError("DoCmd(\"SaveOilThermometerPreheatSelfCheck\") return false");
                return false;
            }
            if (_msgAckRecv != null && _msgAckRecv.Cmd == "SaveOilThermometerPreheatSelfCheckAck") {
                return true;
            } else {
                _log.TraceError("_msgAckRecv.Cmd[\"SaveOilThermometerPreheatSelfCheckAck\"] is wrong");
                return false;
            }
        }

        public bool GetPreheatStatusAndTimeAndSurplusTimeCmd(ref GetPreheatStatusAndTimeAndSurplusTimeAckParams ackParams) {
            GetPreheatStatusAndTimeAndSurplusTimeParams cmdParams = new GetPreheatStatusAndTimeAndSurplusTimeParams {
                ClientID = ClientID
            };
            if (!DoCmd("GetPreheatStatusAndTimeAndSurplusTime", cmdParams, false)) {
                _log.TraceError("DoCmd(\"GetPreheatStatusAndTimeAndSurplusTime\") return false");
                return false;
            }
            if (_msgAckRecv != null && _msgAckRecv.Cmd == "GetPreheatStatusAndTimeAndSurplusTimeAck" && _msgAckRecv.Params != null) {
                ackParams = JsonConvert.DeserializeObject<GetPreheatStatusAndTimeAndSurplusTimeAckParams>(((JObject)_msgAckRecv.Params).ToString(), _dateTimeConverter);
                return true;
            } else {
                _log.TraceError("_msgAckRecv.Cmd[\"GetPreheatStatusAndTimeAndSurplusTimeAck\"] is wrong");
                return false;
            }
        }

        public bool GetOneFinishCheckVehiclesInfo(string WJBGBH, ref GetOneFinishCheckVehiclesInfoAckParams ackParams) {
            GetOneFinishCheckVehiclesInfoParams cmdParams = new GetOneFinishCheckVehiclesInfoParams {
                ClientID = ClientID,
                WJBGBH = WJBGBH
            };
            if (!DoCmd("GetOneFinishCheckVehiclesInfo", cmdParams, false)) {
                _log.TraceError("DoCmd(\"GetOneFinishCheckVehiclesInfo\") return false");
                return false;
            }
            if (_msgAckRecv != null && _msgAckRecv.Cmd == "GetOneFinishCheckVehiclesInfoAck" && _msgAckRecv.Params != null) {
                ackParams = JsonConvert.DeserializeObject<GetOneFinishCheckVehiclesInfoAckParams>(((JObject)_msgAckRecv.Params).ToString(), _dateTimeConverter);
                return true;
            } else {
                _log.TraceError("_msgAckRecv.Cmd[\"GetOneFinishCheckVehiclesInfoAck\"] is wrong");
                return false;
            }
        }

        public bool GetFinishCheckVehiclesInfoCmd(GetFinishCheckVehiclesInfoParams cmdParams, ref GetFinishCheckVehiclesInfoAckParams ackParams) {
            if (!DoCmd("GetFinishCheckVehiclesInfo", cmdParams, false)) {
                _log.TraceError("DoCmd(\"GetFinishCheckVehiclesInfo\") return false");
                return false;
            }
            if (_msgAckRecv != null && _msgAckRecv.Cmd == "GetFinishCheckVehiclesInfoAck" && _msgAckRecv.Params != null) {
                ackParams = JsonConvert.DeserializeObject<GetFinishCheckVehiclesInfoAckParams>(((JObject)_msgAckRecv.Params).ToString(), _dateTimeConverter);
                return true;
            } else {
                _log.TraceError("_msgAckRecv.Cmd[\"GetFinishCheckVehiclesInfoAck\"] is wrong");
                return false;
            }
        }

        public bool SaveInUseVehicleInfoCmd(SaveInUseVehicleInfoParams cmdParams) {
            if (!DoCmd("SaveInUseVehicleInfo", cmdParams, true)) {
                _log.TraceError("DoCmd(\"SaveInUseVehicleInfo\") return false");
                return false;
            }
            if (_msgAckRecv != null && _msgAckRecv.Cmd == "SaveInUseVehicleInfoAck") {
                return true;
            } else {
                _log.TraceError("_msgAckRecv.Cmd[\"SaveInUseVehicleInfoAck\"] is wrong");
                return false;
            }
        }

        public bool SaveNewVehicleInfoCmd(SaveNewVehicleInfoParams cmdParams) {
            if (!DoCmd("SaveNewVehicleInfo", cmdParams, true)) {
                _log.TraceError("DoCmd(\"SaveNewVehicleInfo\") return false");
                return false;
            }
            if (_msgAckRecv != null && _msgAckRecv.Cmd == "SaveNewVehicleInfoAck") {
                return true;
            } else {
                _log.TraceError("_msgAckRecv.Cmd[\"SaveNewVehicleInfoAck\"] is wrong");
                return false;
            }
        }

        public bool StartGasboxPrepareCmd(bool stop, bool Abandon, string rylx) {
            StartGasboxPrepareParams cmdParams = new StartGasboxPrepareParams {
                ClientID = ClientID,
                IsStopGasboxPrepare = stop,
                Abandon = Abandon,
                rylx = rylx
            };
            if (!DoCmd("StartGasboxPrepare", cmdParams, true)) {
                _log.TraceError("DoCmd(\"StartGasboxPrepare\") return false");
                return false;
            }
            if (_msgAckRecv != null && _msgAckRecv.Cmd == "StartGasboxPrepareAck" && _msgAckRecv.Params != null) {
                return true;
            } else {
                _log.TraceError("_msgAckRecv.Cmd[\"StartGasboxPrepareAck\"] is wrong");
                return false;
            }
        }

        public bool GetGasboxPrepareRealTimeDataCmd(bool start, bool Abandon, ref GetGasboxPrepareRealTimeDataAckParams ackParams) {
            GetGasboxPrepareRealTimeDataParams cmdParams = new GetGasboxPrepareRealTimeDataParams {
                ClientID = ClientID,
                IsStartCheck = start,
                Abandon = Abandon
            };
            if (!DoCmd("GetGasboxPrepareRealTimeData", cmdParams, false)) {
                _log.TraceError("DoCmd(\"GetGasboxPrepareRealTimeData\") return false");
                return false;
            }
            if (_msgAckRecv != null && _msgAckRecv.Cmd == "GetGasboxPrepareRealTimeDataAck" && _msgAckRecv.Params != null) {
                ackParams = JsonConvert.DeserializeObject<GetGasboxPrepareRealTimeDataAckParams>(((JObject)_msgAckRecv.Params).ToString(), _dateTimeConverter);
                return true;
            } else {
                _log.TraceError("_msgAckRecv.Cmd[\"GetGasboxPrepareRealTimeDataAck\"] is wrong");
                return false;
            }
        }

        public bool StartFlowmeterPrepareCmd(bool stopPrepare, bool stopCheck) {
            StartFlowmeterPrepareParams cmdParams = new StartFlowmeterPrepareParams {
                ClientID = ClientID,
                IsStopFlowmeterPrepare = stopPrepare,
                stopCheck = stopCheck
            };
            if (!DoCmd("StartFlowmeterPrepare", cmdParams, true)) {
                _log.TraceError("DoCmd(\"StartFlowmeterPrepare\") return false");
                return false;
            }
            if (_msgAckRecv != null && _msgAckRecv.Cmd == "StartFlowmeterPrepareAck") {
                return true;
            } else {
                _log.TraceError("_msgAckRecv.Cmd[\"StartFlowmeterPrepareAck\"] is wrong");
                return false;
            }
        }

        public bool GetFlowmeterPrepareRealTimeDataCmd(bool start, bool Abandon, ref GetFlowmeterPrepareRealTimeDataAckParams ackParams) {
            GetFlowmeterPrepareRealTimeDataParams cmdParams = new GetFlowmeterPrepareRealTimeDataParams {
                ClientID = ClientID,
                IsStartCheck = start,
                Abandon = Abandon
            };
            if (!DoCmd("GetFlowmeterPrepareRealTimeData", cmdParams, false)) {
                _log.TraceError("DoCmd(\"GetFlowmeterPrepareRealTimeData\") return false");
                return false;
            }
            if (_msgAckRecv != null && _msgAckRecv.Cmd == "GetFlowmeterPrepareRealTimeDataAck" && _msgAckRecv.Params != null) {
                ackParams = JsonConvert.DeserializeObject<GetFlowmeterPrepareRealTimeDataAckParams>(((JObject)_msgAckRecv.Params).ToString(), _dateTimeConverter);
                return true;
            } else {
                _log.TraceError("_msgAckRecv.Cmd[\"GetFlowmeterPrepareRealTimeDataAck\"] is wrong");
                return false;
            }
        }

        public bool StartSmokePrepareCmd(bool stopPrepare, bool stopCheck) {
            StartSmokePrepareParams cmdParams = new StartSmokePrepareParams {
                ClientID = ClientID,
                IsStopSmokePrepare = stopPrepare,
                stopCheck = stopCheck
            };
            if (!DoCmd("StartSmokePrepare", cmdParams, true)) {
                _log.TraceError("DoCmd(\"StartSmokePrepare\") return false");
                return false;
            }
            if (_msgAckRecv != null && _msgAckRecv.Cmd == "StartSmokePrepareAck") {
                return true;
            } else {
                _log.TraceError("_msgAckRecv.Cmd[\"StartSmokePrepareAck\"] is wrong");
                return false;
            }
        }

        public bool GetSmokePrepareRealTimeDataCmd(bool start, bool Abandon, ref GetSmokePrepareRealTimeDataAckParams ackParams) {
            GetSmokePrepareRealTimeDataParams cmdParams = new GetSmokePrepareRealTimeDataParams {
                ClientID = ClientID,
                IsStartCheck = start,
                Abandon = Abandon
            };
            if (!DoCmd("GetSmokePrepareRealTimeData", cmdParams, false)) {
                _log.TraceError("DoCmd(\"GetSmokePrepareRealTimeData\") return false");
                return false;
            }
            if (_msgAckRecv != null && _msgAckRecv.Cmd == "GetSmokePrepareRealTimeDataAck" && _msgAckRecv.Params != null) {
                ackParams = JsonConvert.DeserializeObject<GetSmokePrepareRealTimeDataAckParams>(((JObject)_msgAckRecv.Params).ToString(), _dateTimeConverter);
                return true;
            } else {
                _log.TraceError("_msgAckRecv.Cmd[\"GetSmokePrepareRealTimeDataAck\"] is wrong");
                return false;
            }
        }

        public bool GetOilTempPrepareRealTimeDataCmd(bool start, bool Abandon, ref GetOilTempPrepareRealTimeDataAckParams ackParams) {
            GetOilTempPrepareRealTimeDataParams cmdParams = new GetOilTempPrepareRealTimeDataParams {
                ClientID = ClientID,
                IsStartCheck = start,
                Abandon = Abandon
            };
            if (!DoCmd("GetOilTempPrepareRealTimeData", cmdParams, false)) {
                _log.TraceError("DoCmd(\"GetOilTempPrepareRealTimeData\") return false");
                return false;
            }
            if (_msgAckRecv != null && _msgAckRecv.Cmd == "GetOilTempPrepareRealTimeDataAck" && _msgAckRecv.Params != null) {
                ackParams = JsonConvert.DeserializeObject<GetOilTempPrepareRealTimeDataAckParams>(((JObject)_msgAckRecv.Params).ToString(), _dateTimeConverter);
                return true;
            } else {
                _log.TraceError("_msgAckRecv.Cmd[\"GetOilTempPrepareRealTimeDataAck\"] is wrong");
                return false;
            }
        }

        public bool GetTachometerPrepareRealTimeDataCmd(bool start, bool Abandon, ref GetTachometerPrepareRealTimeDataAckParams ackParams) {
            GetTachometerPrepareRealTimeDataParams cmdParams = new GetTachometerPrepareRealTimeDataParams {
                ClientID = ClientID,
                IsStartCheck = start,
                Abandon = Abandon
            };
            if (!DoCmd("GetTachometerPrepareRealTimeData", cmdParams, false)) {
                _log.TraceError("DoCmd(\"GetTachometerPrepareRealTimeData\") return false");
                return false;
            }
            if (_msgAckRecv != null && _msgAckRecv.Cmd == "GetTachometerPrepareRealTimeDataAck" && _msgAckRecv.Params != null) {
                ackParams = JsonConvert.DeserializeObject<GetTachometerPrepareRealTimeDataAckParams>(((JObject)_msgAckRecv.Params).ToString(), _dateTimeConverter);
                return true;
            } else {
                _log.TraceError("_msgAckRecv.Cmd[\"GetTachometerPrepareRealTimeDataAck\"] is wrong");
                return false;
            }
        }

        public bool GetWeatherPrepareRealTimeDataCmd(bool start, bool Abandon, ref GetWeatherPrepareRealTimeDataAckParams ackParams) {
            GetWeatherPrepareRealTimeDataParams cmdParams = new GetWeatherPrepareRealTimeDataParams {
                ClientID = ClientID,
                IsStartCheck = start,
                Abandon = Abandon
            };
            if (!DoCmd("GetWeatherPrepareRealTimeData", cmdParams, false)) {
                _log.TraceError("DoCmd(\"GetWeatherPrepareRealTimeData\") return false");
                return false;
            }
            if (_msgAckRecv != null && _msgAckRecv.Cmd == "GetWeatherPrepareRealTimeDataAck" && _msgAckRecv.Params != null) {
                ackParams = JsonConvert.DeserializeObject<GetWeatherPrepareRealTimeDataAckParams>(((JObject)_msgAckRecv.Params).ToString(), _dateTimeConverter);
                return true;
            } else {
                _log.TraceError("_msgAckRecv.Cmd[\"GetWeatherPrepareRealTimeDataAck\"] is wrong");
                return false;
            }
        }

        public bool StartLdCheckCmd(bool stop, int pauCount) {
            StartLdCheckParams cmdParams = new StartLdCheckParams {
                ClientID = ClientID,
                stopCheck = stop,
                LDLoadPauCount = pauCount
            };
            if (!DoCmd("StartLdCheck", cmdParams, true)) {
                _log.TraceError("DoCmd(\"StartLdCheck\") return false");
                return false;
            }
            if (_msgAckRecv != null && _msgAckRecv.Cmd == "StartLdCheckAck") {
                return true;
            } else {
                _log.TraceError("_msgAckRecv.Cmd[\"StartLdCheckAck\"] is wrong");
                return false;
            }
        }

        public bool GetLdRealTimeDataCmd(bool getMaxRpm, ref GetLdRealTimeDataAckParams ackParams) {
            GetLdRealTimeDataParams cmdParams = new GetLdRealTimeDataParams {
                ClientID = ClientID,
                canGetMaxRpm = getMaxRpm
            };
            if (!DoCmd("GetLdRealTimeData", cmdParams, false)) {
                _log.TraceError("DoCmd(\"GetLdRealTimeData\") return false");
                return false;
            }
            if (_msgAckRecv != null && _msgAckRecv.Cmd == "GetLdRealTimeDataAck" && _msgAckRecv.Params != null) {
                ackParams = JsonConvert.DeserializeObject<GetLdRealTimeDataAckParams>(((JObject)_msgAckRecv.Params).ToString(), _dateTimeConverter);
                return true;
            } else {
                _log.TraceError("_msgAckRecv.Cmd[\"GetLdRealTimeDataAck\"] is wrong");
                return false;
            }
        }

        public bool GetLdCheckResultDataCmd(ref GetLdCheckResultAckParams ackParams) {
            GetLdCheckResultDataParams cmdParams = new GetLdCheckResultDataParams {
                ClientID = ClientID
            };
            if (!DoCmd("GetLdCheckResultData", cmdParams, false)) {
                _log.TraceError("DoCmd(\"GetLdCheckResultData\") return false");
                return false;
            }
            if (_msgAckRecv != null && _msgAckRecv.Cmd == "GetLdCheckResultAck" && _msgAckRecv.Params != null) {
                ackParams = JsonConvert.DeserializeObject<GetLdCheckResultAckParams>(((JObject)_msgAckRecv.Params).ToString(), _dateTimeConverter);
                return true;
            } else {
                _log.TraceError("_msgAckRecv.Cmd[\"GetLdCheckResultAck\"] is wrong");
                return false;
            }
        }

        public bool StartASMCheckCmd(bool stop, bool retry) {
            StartASMCheckParams cmdParams = new StartASMCheckParams {
                ClientID = ClientID,
                stopCheck = stop,
                retryCheck = retry
            };
            if (!DoCmd("StartASMCheck", cmdParams, true)) {
                _log.TraceError("DoCmd(\"StartASMCheck\") return false");
                return false;
            }
            if (_msgAckRecv != null && _msgAckRecv.Cmd == "StartASMCheckAck") {
                return true;
            } else {
                _log.TraceError("_msgAckRecv.Cmd[\"StartASMCheckAck\"] is wrong");
                return false;
            }
        }

        public bool GetASMRealTimeDataCmd(ref GetASMRealTimeDataAckParams ackParams) {
            GetASMRealTimeDataParams cmdParams = new GetASMRealTimeDataParams {
                ClientID = ClientID,
            };
            if (!DoCmd("GetASMRealTimeData", cmdParams, false)) {
                _log.TraceError("DoCmd(\"GetASMRealTimeData\") return false");
                return false;
            }
            if (_msgAckRecv != null && _msgAckRecv.Cmd == "GetASMRealTimeDataAck" && _msgAckRecv.Params != null) {
                ackParams = JsonConvert.DeserializeObject<GetASMRealTimeDataAckParams>(((JObject)_msgAckRecv.Params).ToString(), _dateTimeConverter);
                return true;
            } else {
                _log.TraceError("_msgAckRecv.Cmd[\"GetASMRealTimeDataAck\"] is wrong");
                return false;
            }
        }

        public bool GetASMCheckResultDataCmd(ref GetASMCheckResultAckParams ackParams) {
            GetASMCheckResultDataParams cmdParams = new GetASMCheckResultDataParams {
                ClientID = ClientID
            };
            if (!DoCmd("GetASMCheckResultData", cmdParams, false)) {
                _log.TraceError("DoCmd(\"GetASMCheckResultData\") return false");
                return false;
            }
            if (_msgAckRecv != null && _msgAckRecv.Cmd == "GetASMCheckResultAck" && _msgAckRecv.Params != null) {
                ackParams = JsonConvert.DeserializeObject<GetASMCheckResultAckParams>(((JObject)_msgAckRecv.Params).ToString(), _dateTimeConverter);
                return true;
            } else {
                _log.TraceError("_msgAckRecv.Cmd[\"GetASMCheckResultAck\"] is wrong");
                return false;
            }
        }

        public bool StartFALCheckCmd(bool stop) {
            StartFalCheckParams cmdParams = new StartFalCheckParams {
                ClientID = ClientID,
                stopCheck = stop
            };
            if (!DoCmd("StartFalCheck", cmdParams, true)) {
                _log.TraceError("DoCmd(\"StartFalCheck\") return false");
                return false;
            }
            if (_msgAckRecv != null && _msgAckRecv.Cmd == "StartFalCheckAck") {
                return true;
            } else {
                _log.TraceError("_msgAckRecv.Cmd[\"StartFalCheckAck\"] is wrong");
                return false;
            }
        }

        public bool GetFALRealTimeDataCmd(ref GetFalRealTimeDataAckParams ackParams) {
            GetFalRealTimeDataParams cmdParams = new GetFalRealTimeDataParams {
                ClientID = ClientID,
            };
            if (!DoCmd("GetFalRealTimeData", cmdParams, false)) {
                _log.TraceError("DoCmd(\"GetFalRealTimeData\") return false");
                return false;
            }
            if (_msgAckRecv != null && _msgAckRecv.Cmd == "GetFalRealTimeDataAck" && _msgAckRecv.Params != null) {
                ackParams = JsonConvert.DeserializeObject<GetFalRealTimeDataAckParams>(((JObject)_msgAckRecv.Params).ToString(), _dateTimeConverter);
                return true;
            } else {
                _log.TraceError("_msgAckRecv.Cmd[\"GetFalRealTimeDataAck\"] is wrong");
                return false;
            }
        }

        public bool GetFALCheckResultDataCmd(ref GetFalCheckResultAckParams ackParams) {
            GetFalCheckResultDataParams cmdParams = new GetFalCheckResultDataParams {
                ClientID = ClientID
            };
            if (!DoCmd("GetFalCheckResultData", cmdParams, false)) {
                _log.TraceError("DoCmd(\"GetFalCheckResultData\") return false");
                return false;
            }
            if (_msgAckRecv != null && _msgAckRecv.Cmd == "GetFalCheckResultAck" && _msgAckRecv.Params != null) {
                ackParams = JsonConvert.DeserializeObject<GetFalCheckResultAckParams>(((JObject)_msgAckRecv.Params).ToString(), _dateTimeConverter);
                return true;
            } else {
                _log.TraceError("_msgAckRecv.Cmd[\"GetFalCheckResultAck\"] is wrong");
                return false;
            }
        }

        public bool StartTSICheckCmd(bool stop, bool retry) {
            StartTsiCheckParams cmdParams = new StartTsiCheckParams {
                ClientID = ClientID,
                stopCheck = stop,
                retryCheck = retry
            };
            if (!DoCmd("StartTsiCheck", cmdParams, true)) {
                _log.TraceError("DoCmd(\"StartTsiCheck\") return false");
                return false;
            }
            if (_msgAckRecv != null && _msgAckRecv.Cmd == "StartTsiCheckAck") {
                return true;
            } else {
                _log.TraceError("_msgAckRecv.Cmd[\"StartTsiCheckAck\"] is false");
                return false;
            }
        }

        public bool GetTSIRealTimeDataCmd(GetTsiRealTimeDataParams cmdParams, ref GetTsiRealTimeDataAckParams ackParams) {
            if (!DoCmd("GetTsiRealTimeData", cmdParams, false)) {
                _log.TraceError("DoCmd(\"GetTsiRealTimeData\") return false");
                return false;
            }
            if (_msgAckRecv != null && _msgAckRecv.Cmd == "GetTsiRealTimeDataAck" && _msgAckRecv.Params != null) {
                ackParams = JsonConvert.DeserializeObject<GetTsiRealTimeDataAckParams>(((JObject)_msgAckRecv.Params).ToString(), _dateTimeConverter);
                return true;
            } else {
                _log.TraceError("_msgAckRecv.Cmd[\"GetTsiRealTimeDataAck\"] is wrong");
                return false;
            }
        }

        public bool GetTSICheckResultDataCmd(ref GetTsiCheckResultAckParams ackParams) {
            GetTsiCheckResultDataParams cmdParams = new GetTsiCheckResultDataParams {
                ClientID = ClientID
            };
            if (!DoCmd("GetTsiCheckResultData", cmdParams, false)) {
                _log.TraceError("DoCmd(\"GetTsiCheckResultData\") return false");
                return false;
            }
            if (_msgAckRecv != null && _msgAckRecv.Cmd == "GetTsiCheckResultAck" && _msgAckRecv.Params != null) {
                ackParams = JsonConvert.DeserializeObject<GetTsiCheckResultAckParams>(((JObject)_msgAckRecv.Params).ToString(), _dateTimeConverter);
                return true;
            } else {
                _log.TraceError("_msgAckRecv.Cmd[\"GetTsiCheckResultAck\"] is wrong");
                return false;
            }
        }

        public bool StartVMASCheckCmd(bool stop, bool retry, ref StartVmasCheckAckParams ackParams) {
            StartVmasCheckParams cmdParams = new StartVmasCheckParams {
                ClientID = ClientID,
                stopCheck = stop,
                retryCheck = retry
            };
            if (!DoCmd("StartVmasCheck", cmdParams, true)) {
                _log.TraceError("DoCmd(\"StartVmasCheck\") return false");
                return false;
            }
            if (_msgAckRecv != null && _msgAckRecv.Cmd == "StartVmasCheckAck" && _msgAckRecv.Params != null) {
                ackParams = JsonConvert.DeserializeObject<StartVmasCheckAckParams>(((JObject)_msgAckRecv.Params).ToString(), _dateTimeConverter);
                return true;
            } else {
                _log.TraceError("_msgAckRecv.Cmd[\"StartVmasCheckAck\"] is wrong");
                return false;
            }
        }

        public bool GetVMASRealTimeDataCmd(double overTime, ref GetVmasRealTimeDataAckParams ackParams) {
            GetVmasRealTimeDataParams cmdParams = new GetVmasRealTimeDataParams {
                ClientID = ClientID,
                VmasContinueDiffTime = overTime
            };
            if (!DoCmd("GetVmasRealTimeData", cmdParams, false)) {
                _log.TraceError("DoCmd(\"GetFalRealTimeData\") return false");
                return false;
            }
            if (_msgAckRecv != null && _msgAckRecv.Cmd == "GetVmasRealTimeDataAck" && _msgAckRecv.Params != null) {
                ackParams = JsonConvert.DeserializeObject<GetVmasRealTimeDataAckParams>(((JObject)_msgAckRecv.Params).ToString(), _dateTimeConverter);
                return true;
            } else {
                _log.TraceError("_msgAckRecv.Cmd[\"GetVmasRealTimeDataAck\"] is wrong");
                return false;
            }
        }

        public bool GetVMASCheckResultDataCmd(ref GetVmasCheckResultAckParams ackParams) {
            GetVmasCheckResultDataParams cmdParams = new GetVmasCheckResultDataParams {
                ClientID = ClientID
            };
            if (!DoCmd("GetVmasCheckResultData", cmdParams, false)) {
                _log.TraceError("DoCmd(\"GetVmasCheckResultData\") return false");
                return false;
            }
            if (_msgAckRecv != null && _msgAckRecv.Cmd == "GetVmasCheckResultAck" && _msgAckRecv.Params != null) {
                ackParams = JsonConvert.DeserializeObject<GetVmasCheckResultAckParams>(((JObject)_msgAckRecv.Params).ToString(), _dateTimeConverter);
                return true;
            } else {
                _log.TraceError("_msgAckRecv.Cmd[\"GetVmasCheckResultAck\"] is wrong");
                return false;
            }
        }

    }
}
