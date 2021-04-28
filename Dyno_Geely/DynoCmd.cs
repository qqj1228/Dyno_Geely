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
            _dateTimeConverter = new IsoDateTimeConverter { DateTimeFormat = "yyyy-M-d HH:mm:ss" };
        }

        ~DynoCmd() {
            SafeClose();
        }

        public void SafeClose() {
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
            //Connected = SafeTestConnect();
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
            if (ConnectServer()) {
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
                        frmLoading.CurrentMsg = new KeyValuePair<int, string>(100, string.Format("执行{0}命令结束", msgCmd.Cmd));
                    };
                    frmLoading.ShowDialog();
                } else {
                    if (_RecvFlag.WaitOne(_cfg.Main.Data.RecvTimeout, false)) {
                        recvResult = true;
                    }
                }
                SafeClose();
                if (!recvResult) {
                    _log.TraceError("MsgCmd[" + msgCmd.Cmd + "]Receive timeout");
                    throw new ApplicationException("MsgCmd[" + msgCmd.Cmd + "]Receive timeout");
                }
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

        public bool SocketLongConnection(ref SocketLongConnectionAckParams ackParams, out string errMsg) {
            errMsg = string.Empty;
            SocketLongConnectionParams cmdParams = new SocketLongConnectionParams {
                ClientID = ClientID
            };
            if (!DoCmd("SocketLongConnection", cmdParams, false)) {
                _log.TraceError("DoCmd(\"SocketLongConnection\") return false");
                return false;
            }
            if (_msgAckRecv != null) {
                errMsg = _msgAckRecv.Message;
                if (_msgAckRecv.Cmd == "SocketLongConnectionAck" && _msgAckRecv.Params != null) {
                    ackParams = JsonConvert.DeserializeObject<SocketLongConnectionAckParams>(((JObject)_msgAckRecv.Params).ToString(), _dateTimeConverter);
                    return true;
                }
            }
            _log.TraceError("Return value of \"SocketLongConnectionAck\" is wrong");
            return false;
        }

        public bool LoginCmd(out string errMsg) {
            errMsg = string.Empty;
            LoginParams cmdParams = new LoginParams {
                ClientID = ClientID
            };
            if (!DoCmd("Login", cmdParams, false)) {
                _log.TraceError("DoCmd(\"Login\") return false");
                return false;
            }
            if (_msgAckRecv != null) {
                errMsg = _msgAckRecv.Message;
                if (_msgAckRecv.Cmd == "LoginAck") {
                    return true;
                }
            }
            _log.TraceError("Return value of \"LoginAck\" is wrong");
            return false;
        }

        public bool LogoutCmd(out string errMsg) {
            errMsg = string.Empty;
            LoginParams cmdParams = new LoginParams {
                ClientID = ClientID
            };
            if (!DoCmd("Logout", cmdParams, false)) {
                _log.TraceError("DoCmd(\"Logout\") return false");
                return false;
            }
            if (_msgAckRecv != null) {
                errMsg = _msgAckRecv.Message;
                if (_msgAckRecv.Cmd == "LogoutAck") {
                    return true;
                }
            }
            _log.TraceError("Return value of \"LogoutAck\" is wrong");
            return false;
        }

        private bool DeviceCtrlCmd(DeviceCtrlParams.CtrlIDStr strCtrlID, out string errMsg) {
            errMsg = string.Empty;
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
            if (_msgAckRecv != null) {
                errMsg = _msgAckRecv.Message;
                if (_msgAckRecv.Cmd == "DeviceCtrlAck") {
                    return true;
                }
            }
            _log.TraceError("Return value of \"DeviceCtrlAck\" is wrong");
            return false;
        }

        public bool DynoBeamDownCmd(out string errMsg) {
            return DeviceCtrlCmd(DeviceCtrlParams.CtrlIDStr.CTRLID_DYNO_BEAM_DOWN, out errMsg);
        }

        public bool DynoBeamUpCmd(out string errMsg) {
            return DeviceCtrlCmd(DeviceCtrlParams.CtrlIDStr.CTRLID_DYNO_BEAM_UP, out errMsg);
        }

        public bool StartDynoPreheatCmd(bool stopCheck, out string errMsg) {
            errMsg = string.Empty;
            StartDynoPreheatParams cmdParams = new StartDynoPreheatParams {
                ClientID = ClientID,
                stopCheck = stopCheck
            };
            if (!DoCmd("StartDynoPreheat", cmdParams, true)) {
                _log.TraceError("DoCmd(\"StartDynoPreheat\") return false");
                return false;
            }
            if (_msgAckRecv != null) {
                errMsg = _msgAckRecv.Message;
                if (_msgAckRecv.Cmd == "StartDynoPreheatAck" && _msgAckRecv.Params != null) {
                    StartDynoPreheatAckParams ackParams = JsonConvert.DeserializeObject<StartDynoPreheatAckParams>(((JObject)_msgAckRecv.Params).ToString(), _dateTimeConverter);
                    return true;
                }
            }
            _log.TraceError("Return value of \"StartDynoPreheatAck\" is wrong");
            return false;
        }

        public bool GetDynoPreheatRealTimeDataCmd(ref GetDynoPreheatRealTimeDataAckParams ackParams, out string errMsg) {
            errMsg = string.Empty;
            GetDynoPreheatRealTimeDataParams cmdParams = new GetDynoPreheatRealTimeDataParams {
                ClientID = ClientID
            };
            if (!DoCmd("GetDynoPreheatRealTimeData", cmdParams, false)) {
                _log.TraceError("DoCmd(\"GetDynoPreheatRealTimeData\") return false");
                return false;
            }
            if (_msgAckRecv != null) {
                errMsg = _msgAckRecv.Message;
                if (_msgAckRecv.Cmd == "GetDynoPreheatRealTimeDataAck" && _msgAckRecv.Params != null) {
                    ackParams = JsonConvert.DeserializeObject<GetDynoPreheatRealTimeDataAckParams>(((JObject)_msgAckRecv.Params).ToString(), _dateTimeConverter);
                    return true;
                }
            }
            _log.TraceError("Return value of \"GetDynoPreheatRealTimeDataAck\" is wrong");
            return false;
        }

        public bool SaveDynoPreheatDataCmd(SaveDynoPreheatDataParams cmdParams, out string errMsg) {
            errMsg = string.Empty;
            if (!DoCmd("SaveDynoPreheatData", cmdParams, true)) {
                _log.TraceError("DoCmd(\"SaveDynoPreheatData\") return false");
                return false;
            }
            if (_msgAckRecv != null) {
                errMsg = _msgAckRecv.Message;
                if (_msgAckRecv.Cmd == "SaveDynoPreheatDataAck") {
                    return true;
                }
            }
            _log.TraceError("Return value of \"SaveDynoPreheatDataAck\" is wrong");
            return false;
        }

        public bool StartGasBoxPreheatSelfCheckCmd(StartGasBoxPreheatSelfCheckParams cmdParams, out string errMsg) {
            errMsg = string.Empty;
            if (!DoCmd("StartGasBoxPreheatSelfCheck", cmdParams, true)) {
                _log.TraceError("DoCmd(\"StartGasBoxPreheatSelfCheck\") return false");
                return false;
            }
            if (_msgAckRecv != null) {
                errMsg = _msgAckRecv.Message;
                if (_msgAckRecv.Cmd == "StartGasBoxPreheatSelfCheckAck") {
                    return true;
                }
            }
            _log.TraceError("Return value of \"StartGasBoxPreheatSelfCheckAck\" is wrong");
            return false;
        }

        public bool GetGasBoxPreheatSelfCheckRealTimeDataCmd(ref GetGasBoxPreheatSelfCheckRealTimeDataAckParams ackParams, out string errMsg) {
            errMsg = string.Empty;
            GetGasBoxPreheatSelfCheckRealTimeDataParams cmdParams = new GetGasBoxPreheatSelfCheckRealTimeDataParams {
                ClientID = ClientID
            };
            if (!DoCmd("GetGasBoxPreheatSelfCheckRealTimeData", cmdParams, false)) {
                _log.TraceError("DoCmd(\"GetGasBoxPreheatSelfCheckRealTimeData\") return false");
                return false;
            }
            if (_msgAckRecv != null) {
                errMsg = _msgAckRecv.Message;
                if (_msgAckRecv.Cmd == "GetGasBoxPreheatSelfCheckRealTimeDataAck" && _msgAckRecv.Params != null) {
                    ackParams = JsonConvert.DeserializeObject<GetGasBoxPreheatSelfCheckRealTimeDataAckParams>(((JObject)_msgAckRecv.Params).ToString(), _dateTimeConverter);
                    return true;
                }
            }
            _log.TraceError("Return value of \"GetGasBoxPreheatSelfCheckRealTimeDataAck\" is wrong");
            return false;
        }

        public bool StartFlowmeterCheckCmd(StartFlowmeterCheckParams cmdParams, ref StartFlowmeterCheckAckParams ackParams, out string errMsg) {
            errMsg = string.Empty;
            if (!DoCmd("StartFlowmeterCheck", cmdParams, true)) {
                _log.TraceError("DoCmd(\"StartFlowmeterCheck\") return false");
                return false;
            }
            if (_msgAckRecv != null) {
                errMsg = _msgAckRecv.Message;
                if (_msgAckRecv.Cmd == "StartFlowmeterCheckAck" && _msgAckRecv.Params != null) {
                    ackParams = JsonConvert.DeserializeObject<StartFlowmeterCheckAckParams>(((JObject)_msgAckRecv.Params).ToString(), _dateTimeConverter);
                    return true;
                }
            }
            _log.TraceError("Return value of \"StartFlowmeterCheckAck\" is wrong");
            return false;
        }

        public bool GetFlowmeterCheckRealTimeDataCmd(bool Abandon, ref GetFlowmeterCheckRealTimeDataAckParams ackParams, out string errMsg) {
            errMsg = string.Empty;
            GetFlowmeterCheckRealTimeDataParams cmdParams = new GetFlowmeterCheckRealTimeDataParams {
                ClientID = ClientID,
                Abandon = Abandon
            };
            if (!DoCmd("GetFlowmeterCheckRealTimeData", cmdParams, false)) {
                _log.TraceError("DoCmd(\"GetFlowmeterCheckRealTimeData\") return false");
                return false;
            }
            if (_msgAckRecv != null) {
                errMsg = _msgAckRecv.Message;
                if (_msgAckRecv.Cmd == "GetFlowmeterCheckRealTimeDataAck" && _msgAckRecv.Params != null) {
                    ackParams = JsonConvert.DeserializeObject<GetFlowmeterCheckRealTimeDataAckParams>(((JObject)_msgAckRecv.Params).ToString(), _dateTimeConverter);
                    return true;
                }
            }
            _log.TraceError("Return value of \"GetFlowmeterCheckRealTimeDataAck\" is wrong");
            return false;
        }

        public bool GetSmokerPreheatSelfCheckRealTimeDataCmd(ref GetSmokerPreheatSelfCheckRealTimeDataAckParams ackParams, out string errMsg) {
            errMsg = string.Empty;
            GetSmokerPreheatSelfCheckRealTimeDataParams cmdParams = new GetSmokerPreheatSelfCheckRealTimeDataParams {
                ClientID = ClientID
            };
            if (!DoCmd("GetSmokerPreheatSelfCheckRealTimeData", cmdParams, false)) {
                _log.TraceError("DoCmd(\"GetSmokerPreheatSelfCheckRealTimeData\") return false");
                return false;
            }
            if (_msgAckRecv != null) {
                errMsg = _msgAckRecv.Message;
                if (_msgAckRecv.Cmd == "GetSmokerPreheatSelfCheckRealTimeDataAck" && _msgAckRecv.Params != null) {
                    ackParams = JsonConvert.DeserializeObject<GetSmokerPreheatSelfCheckRealTimeDataAckParams>(((JObject)_msgAckRecv.Params).ToString(), _dateTimeConverter);
                    return true;
                }
            }
            _log.TraceError("Return value of \"GetSmokerPreheatSelfCheckRealTimeDataAck\" is wrong");
            return false;
        }

        public bool SaveSmokerPreheatSelfCheckDataCmd(SaveSmokerPreheatSelfCheckDataParams cmdParams, out string errMsg) {
            errMsg = string.Empty;
            if (!DoCmd("SaveSmokerPreheatSelfCheckData", cmdParams, true)) {
                _log.TraceError("DoCmd(\"SaveSmokerPreheatSelfCheckData\") return false");
                return false;
            }
            if (_msgAckRecv != null) {
                errMsg = _msgAckRecv.Message;
                if (_msgAckRecv.Cmd == "SaveSmokerPreheatSelfCheckDataAck") {
                    return true;
                }
            }
            _log.TraceError("Return value of \"SaveSmokerPreheatSelfCheckDataAck\" is wrong");
            return false;
        }

        public bool GetWeatherCheckRealTimeDataCmd(ref GetWeatherCheckRealTimeDataAckParams ackParams, out string errMsg) {
            errMsg = string.Empty;
            GetWeatherCheckRealTimeDataParams cmdParams = new GetWeatherCheckRealTimeDataParams {
                ClientID = ClientID
            };
            if (!DoCmd("GetWeatherCheckRealTimeData", cmdParams, false)) {
                _log.TraceError("DoCmd(\"GetWeatherCheckRealTimeData\") return false");
                return false;
            }
            if (_msgAckRecv != null) {
                errMsg = _msgAckRecv.Message;
                if (_msgAckRecv.Cmd == "GetWeatherCheckRealTimeDataAck" && _msgAckRecv.Params != null) {
                    ackParams = JsonConvert.DeserializeObject<GetWeatherCheckRealTimeDataAckParams>(((JObject)_msgAckRecv.Params).ToString(), _dateTimeConverter);
                    return true;
                }
            }
            _log.TraceError("Return value of \"GetWeatherCheckRealTimeDataAck\" is wrong");
            return false;
        }

        public bool SaveWeatherCheckDataCmd(SaveWeatherCheckDataParams cmdParams, out string errMsg) {
            errMsg = string.Empty;
            if (!DoCmd("SaveWeatherCheckData", cmdParams, true)) {
                _log.TraceError("DoCmd(\"SaveWeatherCheckData\") return false");
                return false;
            }
            if (_msgAckRecv != null) {
                errMsg = _msgAckRecv.Message;
                if (_msgAckRecv.Cmd == "SaveWeatherCheckDataAck") {
                    return true;
                }
            }
            _log.TraceError("Return value of \"SaveWeatherCheckDataAck\" is wrong");
            return false;
        }

        public bool StartTachometerCheckCmd(ref StartTachometerCheckAckParams ackParams, out string errMsg) {
            errMsg = string.Empty;
            StartTachometerCheckParams cmdParams = new StartTachometerCheckParams {
                ClientID = ClientID
            };
            if (!DoCmd("StartTachometerCheck", cmdParams, true)) {
                _log.TraceError("DoCmd(\"StartTachometerCheck\") return false");
                return false;
            }
            if (_msgAckRecv != null) {
                errMsg = _msgAckRecv.Message;
                if (_msgAckRecv.Cmd == "StartTachometerCheckAck" && _msgAckRecv.Params != null) {
                    ackParams = JsonConvert.DeserializeObject<StartTachometerCheckAckParams>(((JObject)_msgAckRecv.Params).ToString(), _dateTimeConverter);
                    return true;
                }
            }
            _log.TraceError("Return value of \"StartTachometerCheckAck\" is wrong");
            return false;
        }

        public bool GetTachometerCheckRealTimeDataCmd(ref GetTachometerCheckRealTimeDataAckParams ackParams, out string errMsg) {
            errMsg = string.Empty;
            GetTachometerCheckRealTimeDataParams cmdParams = new GetTachometerCheckRealTimeDataParams {
                ClientID = ClientID
            };
            if (!DoCmd("GetTachometerCheckRealTimeData", cmdParams, false)) {
                _log.TraceError("DoCmd(\"GetTachometerCheckRealTimeData\") return false");
                return false;
            }
            if (_msgAckRecv != null) {
                errMsg = _msgAckRecv.Message;
                if (_msgAckRecv.Cmd == "GetTachometerCheckRealTimeDataAck" && _msgAckRecv.Params != null) {
                    ackParams = JsonConvert.DeserializeObject<GetTachometerCheckRealTimeDataAckParams>(((JObject)_msgAckRecv.Params).ToString(), _dateTimeConverter);
                    return true;
                }
            }
            _log.TraceError("Return value of \"GetTachometerCheckRealTimeDataAck\" is wrong");
            return false;
        }

        public bool SaveTachometerCheckCmd(SaveTachometerCheckParams cmdParams, out string errMsg) {
            errMsg = string.Empty;
            if (!DoCmd("SaveTachometerCheck", cmdParams, true)) {
                _log.TraceError("DoCmd(\"SaveTachometerCheck\") return false");
                return false;
            }
            if (_msgAckRecv != null) {
                errMsg = _msgAckRecv.Message;
                if (_msgAckRecv.Cmd == "SaveTachometerCheckAck") {
                    return true;
                }
            }
            _log.TraceError("Return value of \"SaveTachometerCheckAck\" is wrong");
            return false;
        }

        public bool GetOilThermometerPreheatSelfCheckRealTimeDataCmd(ref GetOilThermometerPreheatSelfCheckRealTimeDataAckParams ackParams, out string errMsg) {
            errMsg = string.Empty;
            GetOilThermometerPreheatSelfCheckRealTimeDataParams cmdParams = new GetOilThermometerPreheatSelfCheckRealTimeDataParams {
                ClientID = ClientID
            };
            if (!DoCmd("GetOilThermometerPreheatSelfCheckRealTimeData", cmdParams, false)) {
                _log.TraceError("DoCmd(\"GetOilThermometerPreheatSelfCheckRealTimeData\") return false");
                return false;
            }
            if (_msgAckRecv != null) {
                errMsg = _msgAckRecv.Message;
                if (_msgAckRecv.Cmd == "GetOilThermometerPreheatSelfCheckRealTimeDataAck" && _msgAckRecv.Params != null) {
                    ackParams = JsonConvert.DeserializeObject<GetOilThermometerPreheatSelfCheckRealTimeDataAckParams>(((JObject)_msgAckRecv.Params).ToString(), _dateTimeConverter);
                    return true;
                }
            }
            _log.TraceError("Return value of \"GetOilThermometerPreheatSelfCheckRealTimeDataAck\" is wrong");
            return false;
        }

        public bool SaveOilThermometerPreheatSelfCheckCmd(SaveOilThermometerPreheatSelfCheckParams cmdParams, out string errMsg) {
            errMsg = string.Empty;
            if (!DoCmd("SaveOilThermometerPreheatSelfCheck", cmdParams, true)) {
                _log.TraceError("DoCmd(\"SaveOilThermometerPreheatSelfCheck\") return false");
                return false;
            }
            if (_msgAckRecv != null) {
                errMsg = _msgAckRecv.Message;
                if (_msgAckRecv.Cmd == "SaveOilThermometerPreheatSelfCheckAck") {
                    return true;
                }
            }
            _log.TraceError("Return value of \"SaveOilThermometerPreheatSelfCheckAck\" is wrong");
            return false;
        }

        public bool GetPreheatStatusAndTimeAndSurplusTimeCmd(ref GetPreheatStatusAndTimeAndSurplusTimeAckParams ackParams, out string errMsg) {
            errMsg = string.Empty;
            GetPreheatStatusAndTimeAndSurplusTimeParams cmdParams = new GetPreheatStatusAndTimeAndSurplusTimeParams {
                ClientID = ClientID
            };
            if (!DoCmd("GetPreheatStatusAndTimeAndSurplusTime", cmdParams, false)) {
                _log.TraceError("DoCmd(\"GetPreheatStatusAndTimeAndSurplusTime\") return false");
                return false;
            }
            if (_msgAckRecv != null) {
                errMsg = _msgAckRecv.Message;
                if (_msgAckRecv.Cmd == "GetPreheatStatusAndTimeAndSurplusTimeAck" && _msgAckRecv.Params != null) {
                    ackParams = JsonConvert.DeserializeObject<GetPreheatStatusAndTimeAndSurplusTimeAckParams>(((JObject)_msgAckRecv.Params).ToString(), _dateTimeConverter);
                    return true;
                }
            }
            _log.TraceError("Return value of \"GetPreheatStatusAndTimeAndSurplusTimeAck\" is wrong");
            return false;
        }

        public bool GetOneFinishCheckVehiclesInfo(string WJBGBH, ref GetOneFinishCheckVehiclesInfoAckParams ackParams, out string errMsg) {
            errMsg = string.Empty;
            GetOneFinishCheckVehiclesInfoParams cmdParams = new GetOneFinishCheckVehiclesInfoParams {
                ClientID = ClientID,
                WJBGBH = WJBGBH
            };
            if (!DoCmd("GetOneFinishCheckVehiclesInfo", cmdParams, false)) {
                _log.TraceError("DoCmd(\"GetOneFinishCheckVehiclesInfo\") return false");
                return false;
            }
            if (_msgAckRecv != null) {
                errMsg = _msgAckRecv.Message;
                if (_msgAckRecv.Cmd == "GetOneFinishCheckVehiclesInfoAck" && _msgAckRecv.Params != null) {
                    ackParams = JsonConvert.DeserializeObject<GetOneFinishCheckVehiclesInfoAckParams>(((JObject)_msgAckRecv.Params).ToString(), _dateTimeConverter);
                    return true;
                }
            }
            _log.TraceError("Return value of \"GetOneFinishCheckVehiclesInfoAck\" is wrong");
            return false;
        }

        public bool GetFinishCheckVehiclesInfoCmd(GetFinishCheckVehiclesInfoParams cmdParams, ref GetFinishCheckVehiclesInfoAckParams ackParams, out string errMsg) {
            errMsg = string.Empty;
            if (!DoCmd("GetFinishCheckVehiclesInfo", cmdParams, false)) {
                _log.TraceError("DoCmd(\"GetFinishCheckVehiclesInfo\") return false");
                return false;
            }
            if (_msgAckRecv != null) {
                errMsg = _msgAckRecv.Message;
                if (_msgAckRecv.Cmd == "GetFinishCheckVehiclesInfoAck" && _msgAckRecv.Params != null) {
                    ackParams = JsonConvert.DeserializeObject<GetFinishCheckVehiclesInfoAckParams>(((JObject)_msgAckRecv.Params).ToString(), _dateTimeConverter);
                    return true;
                }
            }
            _log.TraceError("Return value of \"GetFinishCheckVehiclesInfoAck\" is wrong");
            return false;
        }

        public bool SaveInUseVehicleInfoCmd(SaveInUseVehicleInfoParams cmdParams, out string errMsg) {
            errMsg = string.Empty;
            if (!DoCmd("SaveInUseVehicleInfo", cmdParams, true)) {
                _log.TraceError("DoCmd(\"SaveInUseVehicleInfo\") return false");
                return false;
            }
            if (_msgAckRecv != null) {
                errMsg = _msgAckRecv.Message;
                if (_msgAckRecv.Cmd == "SaveInUseVehicleInfoAck") {
                    return true;
                }
            }
            _log.TraceError("Return value of \"SaveInUseVehicleInfoAck\" is wrong");
            return false;
        }

        public bool SaveNewVehicleInfoCmd(SaveNewVehicleInfoParams cmdParams, out string errMsg) {
            errMsg = string.Empty;
            if (!DoCmd("SaveNewVehicleInfo", cmdParams, true)) {
                _log.TraceError("DoCmd(\"SaveNewVehicleInfo\") return false");
                return false;
            }
            if (_msgAckRecv != null) {
                errMsg = _msgAckRecv.Message;
                if (_msgAckRecv.Cmd == "SaveNewVehicleInfoAck") {
                    return true;
                }
            }
            _log.TraceError("Return value of \"SaveNewVehicleInfoAck\" is wrong");
            return false;
        }

        public bool GetWaitCheckQueueInfoCmd(GetWaitCheckQueueInfoParams cmdParams, ref GetWaitCheckQueueInfoAckParams ackParams, out string errMsg) {
            errMsg = string.Empty;
            if (!DoCmd("GetWaitCheckQueueInfo", cmdParams, false)) {
                _log.TraceError("DoCmd(\"GetWaitCheckQueueInfo\") return false");
                return false;
            }
            if (_msgAckRecv != null) {
                errMsg = _msgAckRecv.Message;
                if (_msgAckRecv.Cmd == "GetWaitCheckQueueInfoAck" && _msgAckRecv.Params != null) {
                    ackParams = JsonConvert.DeserializeObject<GetWaitCheckQueueInfoAckParams>(((JObject)_msgAckRecv.Params).ToString(), _dateTimeConverter);
                    return true;
                }
            }
            _log.TraceError("Return value of \"GetWaitCheckQueueInfoAck\" is wrong");
            return false;
        }

        public bool GetOneWaitVehicleInfoCmd(GetOneWaitVehicleInfoParams cmdParams, ref GetOneWaitVehicleInfoAckParams ackParams, out string errMsg) {
            errMsg = string.Empty;
            if (!DoCmd("GetOneWaitVehicleInfo", cmdParams, false)) {
                _log.TraceError("DoCmd(\"GetOneWaitVehicleInfo\") return false");
                return false;
            }
            if (_msgAckRecv != null) {
                errMsg = _msgAckRecv.Message;
                if (_msgAckRecv.Cmd == "GetOneWaitVehicleInfoAck" && _msgAckRecv.Params != null) {
                    ackParams = JsonConvert.DeserializeObject<GetOneWaitVehicleInfoAckParams>(((JObject)_msgAckRecv.Params).ToString(), _dateTimeConverter);
                    return true;
                }
            }
            _log.TraceError("Return value of \"GetOneWaitVehicleInfoAck\" is wrong");
            return false;
        }

        public bool StartGasboxPrepareCmd(bool stop, bool Abandon, string rylx, out string errMsg) {
            errMsg = string.Empty;
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
            if (_msgAckRecv != null) {
                errMsg = _msgAckRecv.Message;
                if (_msgAckRecv.Cmd == "StartGasboxPrepareAck") {
                    return true;
                }
            }
            _log.TraceError("Return value of \"StartGasboxPrepareAck\" is wrong");
            return false;
        }

        public bool GetGasboxPrepareRealTimeDataCmd(bool start, bool Abandon, ref GetGasboxPrepareRealTimeDataAckParams ackParams, out string errMsg) {
            errMsg = string.Empty;
            GetGasboxPrepareRealTimeDataParams cmdParams = new GetGasboxPrepareRealTimeDataParams {
                ClientID = ClientID,
                IsStartCheck = start,
                Abandon = Abandon
            };
            if (!DoCmd("GetGasboxPrepareRealTimeData", cmdParams, false)) {
                _log.TraceError("DoCmd(\"GetGasboxPrepareRealTimeData\") return false");
                return false;
            }
            if (_msgAckRecv != null) {
                errMsg = _msgAckRecv.Message;
                if (_msgAckRecv.Cmd == "GetGasboxPrepareRealTimeDataAck" && _msgAckRecv.Params != null) {
                    ackParams = JsonConvert.DeserializeObject<GetGasboxPrepareRealTimeDataAckParams>(((JObject)_msgAckRecv.Params).ToString(), _dateTimeConverter);
                    return true;
                }
            }
            _log.TraceError("Return value of \"GetGasboxPrepareRealTimeDataAck\" is wrong");
            return false;
        }

        public bool StartFlowmeterPrepareCmd(bool stopPrepare, bool stopCheck, out string errMsg) {
            errMsg = string.Empty;
            StartFlowmeterPrepareParams cmdParams = new StartFlowmeterPrepareParams {
                ClientID = ClientID,
                IsStopFlowmeterPrepare = stopPrepare,
                stopCheck = stopCheck
            };
            if (!DoCmd("StartFlowmeterPrepare", cmdParams, true)) {
                _log.TraceError("DoCmd(\"StartFlowmeterPrepare\") return false");
                return false;
            }
            if (_msgAckRecv != null) {
                errMsg = _msgAckRecv.Message;
                if (_msgAckRecv.Cmd == "StartFlowmeterPrepareAck") {
                    return true;
                }
            }
            _log.TraceError("Return value of \"StartFlowmeterPrepareAck\" is wrong");
            return false;
        }

        public bool GetFlowmeterPrepareRealTimeDataCmd(bool start, bool Abandon, ref GetFlowmeterPrepareRealTimeDataAckParams ackParams, out string errMsg) {
            errMsg = string.Empty;
            GetFlowmeterPrepareRealTimeDataParams cmdParams = new GetFlowmeterPrepareRealTimeDataParams {
                ClientID = ClientID,
                IsStartCheck = start,
                Abandon = Abandon
            };
            if (!DoCmd("GetFlowmeterPrepareRealTimeData", cmdParams, false)) {
                _log.TraceError("DoCmd(\"GetFlowmeterPrepareRealTimeData\") return false");
                return false;
            }
            if (_msgAckRecv != null) {
                errMsg = _msgAckRecv.Message;
                if (_msgAckRecv.Cmd == "GetFlowmeterPrepareRealTimeDataAck" && _msgAckRecv.Params != null) {
                    ackParams = JsonConvert.DeserializeObject<GetFlowmeterPrepareRealTimeDataAckParams>(((JObject)_msgAckRecv.Params).ToString(), _dateTimeConverter);
                    return true;
                }
            }
            _log.TraceError("Return value of \"GetFlowmeterPrepareRealTimeDataAck\" is wrong");
            return false;
        }

        public bool StartSmokePrepareCmd(bool stopPrepare, bool stopCheck, out string errMsg) {
            errMsg = string.Empty;
            StartSmokePrepareParams cmdParams = new StartSmokePrepareParams {
                ClientID = ClientID,
                IsStopSmokePrepare = stopPrepare,
                stopCheck = stopCheck
            };
            if (!DoCmd("StartSmokePrepare", cmdParams, true)) {
                _log.TraceError("DoCmd(\"StartSmokePrepare\") return false");
                return false;
            }
            if (_msgAckRecv != null) {
                errMsg = _msgAckRecv.Message;
                if (_msgAckRecv.Cmd == "StartSmokePrepareAck") {
                    return true;
                }
            }
            _log.TraceError("Return value of \"StartSmokePrepareAck\" is wrong");
            return false;
        }

        public bool GetSmokePrepareRealTimeDataCmd(bool start, bool Abandon, ref GetSmokePrepareRealTimeDataAckParams ackParams, out string errMsg) {
            errMsg = string.Empty;
            GetSmokePrepareRealTimeDataParams cmdParams = new GetSmokePrepareRealTimeDataParams {
                ClientID = ClientID,
                IsStartCheck = start,
                Abandon = Abandon
            };
            if (!DoCmd("GetSmokePrepareRealTimeData", cmdParams, false)) {
                _log.TraceError("DoCmd(\"GetSmokePrepareRealTimeData\") return false");
                return false;
            }
            if (_msgAckRecv != null) {
                errMsg = _msgAckRecv.Message;
                if (_msgAckRecv.Cmd == "GetSmokePrepareRealTimeDataAck" && _msgAckRecv.Params != null) {
                    ackParams = JsonConvert.DeserializeObject<GetSmokePrepareRealTimeDataAckParams>(((JObject)_msgAckRecv.Params).ToString(), _dateTimeConverter);
                    return true;
                }
            }
            _log.TraceError("Return value of \"GetSmokePrepareRealTimeDataAck\" is wrong");
            return false;
        }

        public bool GetOilTempPrepareRealTimeDataCmd(bool start, bool Abandon, ref GetOilTempPrepareRealTimeDataAckParams ackParams, out string errMsg) {
            errMsg = string.Empty;
            GetOilTempPrepareRealTimeDataParams cmdParams = new GetOilTempPrepareRealTimeDataParams {
                ClientID = ClientID,
                IsStartCheck = start,
                Abandon = Abandon
            };
            if (!DoCmd("GetOilTempPrepareRealTimeData", cmdParams, false)) {
                _log.TraceError("DoCmd(\"GetOilTempPrepareRealTimeData\") return false");
                return false;
            }
            if (_msgAckRecv != null) {
                errMsg = _msgAckRecv.Message;
                if (_msgAckRecv.Cmd == "GetOilTempPrepareRealTimeDataAck" && _msgAckRecv.Params != null) {
                    ackParams = JsonConvert.DeserializeObject<GetOilTempPrepareRealTimeDataAckParams>(((JObject)_msgAckRecv.Params).ToString(), _dateTimeConverter);
                    return true;
                }
            }
            _log.TraceError("Return value of \"GetOilTempPrepareRealTimeDataAck\" is wrong");
            return false;
        }

        public bool GetTachometerPrepareRealTimeDataCmd(bool start, bool Abandon, ref GetTachometerPrepareRealTimeDataAckParams ackParams, out string errMsg) {
            errMsg = string.Empty;
            GetTachometerPrepareRealTimeDataParams cmdParams = new GetTachometerPrepareRealTimeDataParams {
                ClientID = ClientID,
                IsStartCheck = start,
                Abandon = Abandon
            };
            if (!DoCmd("GetTachometerPrepareRealTimeData", cmdParams, false)) {
                _log.TraceError("DoCmd(\"GetTachometerPrepareRealTimeData\") return false");
                return false;
            }
            if (_msgAckRecv != null) {
                errMsg = _msgAckRecv.Message;
                if (_msgAckRecv.Cmd == "GetTachometerPrepareRealTimeDataAck" && _msgAckRecv.Params != null) {
                    ackParams = JsonConvert.DeserializeObject<GetTachometerPrepareRealTimeDataAckParams>(((JObject)_msgAckRecv.Params).ToString(), _dateTimeConverter);
                    return true;
                }
            }
            _log.TraceError("Return value of \"GetTachometerPrepareRealTimeDataAck\" is wrong");
            return false;
        }

        public bool GetWeatherPrepareRealTimeDataCmd(bool start, bool Abandon, ref GetWeatherPrepareRealTimeDataAckParams ackParams, out string errMsg) {
            errMsg = string.Empty;
            GetWeatherPrepareRealTimeDataParams cmdParams = new GetWeatherPrepareRealTimeDataParams {
                ClientID = ClientID,
                IsStartCheck = start,
                Abandon = Abandon
            };
            if (!DoCmd("GetWeatherPrepareRealTimeData", cmdParams, false)) {
                _log.TraceError("DoCmd(\"GetWeatherPrepareRealTimeData\") return false");
                return false;
            }
            if (_msgAckRecv != null) {
                errMsg = _msgAckRecv.Message;
                if (_msgAckRecv.Cmd == "GetWeatherPrepareRealTimeDataAck" && _msgAckRecv.Params != null) {
                    ackParams = JsonConvert.DeserializeObject<GetWeatherPrepareRealTimeDataAckParams>(((JObject)_msgAckRecv.Params).ToString(), _dateTimeConverter);
                    return true;
                }
            }
            _log.TraceError("Return value of \"GetWeatherPrepareRealTimeDataAck\" is wrong");
            return false;
        }

        public bool StartLdCheckCmd(bool stop, int pauCount, out string errMsg) {
            errMsg = string.Empty;
            StartLdCheckParams cmdParams = new StartLdCheckParams {
                ClientID = ClientID,
                stopCheck = stop,
                LDLoadPauCount = pauCount
            };
            if (!DoCmd("StartLdCheck", cmdParams, true)) {
                _log.TraceError("DoCmd(\"StartLdCheck\") return false");
                return false;
            }
            if (_msgAckRecv != null) {
                errMsg = _msgAckRecv.Message;
                if (_msgAckRecv.Cmd == "StartLdCheckAck") {
                    return true;
                }
            }
            _log.TraceError("Return value of \"StartLdCheckAck\" is wrong");
            return false;
        }

        public bool GetLdRealTimeDataCmd(bool getMaxRpm, ref GetLdRealTimeDataAckParams ackParams, out string errMsg) {
            errMsg = string.Empty;
            GetLdRealTimeDataParams cmdParams = new GetLdRealTimeDataParams {
                ClientID = ClientID,
                canGetMaxRpm = getMaxRpm
            };
            if (!DoCmd("GetLdRealTimeData", cmdParams, false)) {
                _log.TraceError("DoCmd(\"GetLdRealTimeData\") return false");
                return false;
            }
            if (_msgAckRecv != null) {
                errMsg = _msgAckRecv.Message;
                if (_msgAckRecv.Cmd == "GetLdRealTimeDataAck" && _msgAckRecv.Params != null) {
                    ackParams = JsonConvert.DeserializeObject<GetLdRealTimeDataAckParams>(((JObject)_msgAckRecv.Params).ToString(), _dateTimeConverter);
                    return true;
                }
            }
            _log.TraceError("Return value of \"GetLdRealTimeDataAck\" is wrong");
            return false;
        }

        public bool GetLdCheckResultDataCmd(ref GetLdCheckResultAckParams ackParams, out string errMsg) {
            errMsg = string.Empty;
            GetLdCheckResultDataParams cmdParams = new GetLdCheckResultDataParams {
                ClientID = ClientID
            };
            if (!DoCmd("GetLdCheckResultData", cmdParams, false)) {
                _log.TraceError("DoCmd(\"GetLdCheckResultData\") return false");
                return false;
            }
            if (_msgAckRecv != null) {
                errMsg = _msgAckRecv.Message;
                if (_msgAckRecv.Cmd == "GetLdCheckResultAck" && _msgAckRecv.Params != null) {
                    ackParams = JsonConvert.DeserializeObject<GetLdCheckResultAckParams>(((JObject)_msgAckRecv.Params).ToString(), _dateTimeConverter);
                    return true;
                }
            }
            _log.TraceError("Return value of \"GetLdCheckResultAck\" is wrong");
            return false;
        }

        public bool StartASMCheckCmd(bool stop, bool retry, out string errMsg) {
            errMsg = string.Empty;
            StartASMCheckParams cmdParams = new StartASMCheckParams {
                ClientID = ClientID,
                stopCheck = stop,
                retryCheck = retry
            };
            if (!DoCmd("StartASMCheck", cmdParams, true)) {
                _log.TraceError("DoCmd(\"StartASMCheck\") return false");
                return false;
            }
            if (_msgAckRecv != null) {
                errMsg = _msgAckRecv.Message;
                if (_msgAckRecv.Cmd == "StartASMCheckAck") {
                    return true;
                }
            }
            _log.TraceError("Return value of \"StartASMCheckAck\" is wrong");
            return false;
        }

        public bool GetASMRealTimeDataCmd(ref GetASMRealTimeDataAckParams ackParams, out string errMsg) {
            errMsg = string.Empty;
            GetASMRealTimeDataParams cmdParams = new GetASMRealTimeDataParams {
                ClientID = ClientID,
            };
            if (!DoCmd("GetASMRealTimeData", cmdParams, false)) {
                _log.TraceError("DoCmd(\"GetASMRealTimeData\") return false");
                return false;
            }
            if (_msgAckRecv != null) {
                errMsg = _msgAckRecv.Message;
                if (_msgAckRecv.Cmd == "GetASMRealTimeDataAck" && _msgAckRecv.Params != null) {
                    ackParams = JsonConvert.DeserializeObject<GetASMRealTimeDataAckParams>(((JObject)_msgAckRecv.Params).ToString(), _dateTimeConverter);
                    return true;
                }
            }
            _log.TraceError("Return value of \"GetASMRealTimeDataAck\" is wrong");
            return false;
        }

        public bool GetASMCheckResultDataCmd(ref GetASMCheckResultAckParams ackParams, out string errMsg) {
            errMsg = string.Empty;
            GetASMCheckResultDataParams cmdParams = new GetASMCheckResultDataParams {
                ClientID = ClientID
            };
            if (!DoCmd("GetASMCheckResultData", cmdParams, false)) {
                _log.TraceError("DoCmd(\"GetASMCheckResultData\") return false");
                return false;
            }
            if (_msgAckRecv != null) {
                errMsg = _msgAckRecv.Message;
                if (_msgAckRecv.Cmd == "GetASMCheckResultAck" && _msgAckRecv.Params != null) {
                    ackParams = JsonConvert.DeserializeObject<GetASMCheckResultAckParams>(((JObject)_msgAckRecv.Params).ToString(), _dateTimeConverter);
                    return true;
                }
            }
            _log.TraceError("Return value of \"GetASMCheckResultAck\" is wrong");
            return false;
        }

        public bool StartFALCheckCmd(bool stop, out string errMsg) {
            errMsg = string.Empty;
            StartFalCheckParams cmdParams = new StartFalCheckParams {
                ClientID = ClientID,
                stopCheck = stop
            };
            if (!DoCmd("StartFalCheck", cmdParams, true)) {
                _log.TraceError("DoCmd(\"StartFalCheck\") return false");
                return false;
            }
            if (_msgAckRecv != null) {
                errMsg = _msgAckRecv.Message;
                if (_msgAckRecv.Cmd == "StartFalCheckAck") {
                    return true;
                }
            }
            _log.TraceError("Return value of \"StartFalCheckAck\" is wrong");
            return false;
        }

        public bool GetFALRealTimeDataCmd(ref GetFalRealTimeDataAckParams ackParams, out string errMsg) {
            errMsg = string.Empty;
            GetFalRealTimeDataParams cmdParams = new GetFalRealTimeDataParams {
                ClientID = ClientID,
            };
            if (!DoCmd("GetFalRealTimeData", cmdParams, false)) {
                _log.TraceError("DoCmd(\"GetFalRealTimeData\") return false");
                return false;
            }
            if (_msgAckRecv != null) {
                errMsg = _msgAckRecv.Message;
                if (_msgAckRecv.Cmd == "GetFalRealTimeDataAck" && _msgAckRecv.Params != null) {
                    ackParams = JsonConvert.DeserializeObject<GetFalRealTimeDataAckParams>(((JObject)_msgAckRecv.Params).ToString(), _dateTimeConverter);
                    return true;
                }
            }
            _log.TraceError("Return value of \"GetFalRealTimeDataAck\" is wrong");
            return false;
        }

        public bool GetFALCheckResultDataCmd(ref GetFalCheckResultAckParams ackParams, out string errMsg) {
            errMsg = string.Empty;
            GetFalCheckResultDataParams cmdParams = new GetFalCheckResultDataParams {
                ClientID = ClientID
            };
            if (!DoCmd("GetFalCheckResultData", cmdParams, false)) {
                _log.TraceError("DoCmd(\"GetFalCheckResultData\") return false");
                return false;
            }
            if (_msgAckRecv != null) {
                errMsg = _msgAckRecv.Message;
                if (_msgAckRecv.Cmd == "GetFalCheckResultAck" && _msgAckRecv.Params != null) {
                    ackParams = JsonConvert.DeserializeObject<GetFalCheckResultAckParams>(((JObject)_msgAckRecv.Params).ToString(), _dateTimeConverter);
                    return true;
                }
            }
            _log.TraceError("Return value of \"GetFalCheckResultAck\" is wrong");
            return false;
        }

        public bool StartTSICheckCmd(bool stop, bool retry, out string errMsg) {
            errMsg = string.Empty;
            StartTsiCheckParams cmdParams = new StartTsiCheckParams {
                ClientID = ClientID,
                stopCheck = stop,
                retryCheck = retry
            };
            if (!DoCmd("StartTsiCheck", cmdParams, true)) {
                _log.TraceError("DoCmd(\"StartTsiCheck\") return false");
                return false;
            }
            if (_msgAckRecv != null) {
                errMsg = _msgAckRecv.Message;
                if (_msgAckRecv.Cmd == "StartTsiCheckAck") {
                    return true;
                }
            }
            _log.TraceError("Return value of \"StartTsiCheckAck\" is wrong");
            return false;
        }

        public bool GetTSIRealTimeDataCmd(GetTsiRealTimeDataParams cmdParams, ref GetTsiRealTimeDataAckParams ackParams, out string errMsg) {
            errMsg = string.Empty;
            if (!DoCmd("GetTsiRealTimeData", cmdParams, false)) {
                _log.TraceError("DoCmd(\"GetTsiRealTimeData\") return false");
                return false;
            }
            if (_msgAckRecv != null) {
                errMsg = _msgAckRecv.Message;
                if (_msgAckRecv.Cmd == "GetTsiRealTimeDataAck" && _msgAckRecv.Params != null) {
                    ackParams = JsonConvert.DeserializeObject<GetTsiRealTimeDataAckParams>(((JObject)_msgAckRecv.Params).ToString(), _dateTimeConverter);
                    return true;
                }
            }
            _log.TraceError("Return value of \"GetTsiRealTimeDataAck\" is wrong");
            return false;
        }

        public bool GetTSICheckResultDataCmd(ref GetTsiCheckResultAckParams ackParams, out string errMsg) {
            errMsg = string.Empty;
            GetTsiCheckResultDataParams cmdParams = new GetTsiCheckResultDataParams {
                ClientID = ClientID
            };
            if (!DoCmd("GetTsiCheckResultData", cmdParams, false)) {
                _log.TraceError("DoCmd(\"GetTsiCheckResultData\") return false");
                return false;
            }
            if (_msgAckRecv != null) {
                errMsg = _msgAckRecv.Message;
                if (_msgAckRecv.Cmd == "GetTsiCheckResultAck" && _msgAckRecv.Params != null) {
                    ackParams = JsonConvert.DeserializeObject<GetTsiCheckResultAckParams>(((JObject)_msgAckRecv.Params).ToString(), _dateTimeConverter);
                    return true;
                }
            }
            _log.TraceError("Return value of \"GetTsiCheckResultAck\" is wrong");
            return false;
        }

        public bool StartVMASCheckCmd(bool stop, bool retry, ref StartVmasCheckAckParams ackParams, out string errMsg) {
            errMsg = string.Empty;
            StartVmasCheckParams cmdParams = new StartVmasCheckParams {
                ClientID = ClientID,
                stopCheck = stop,
                retryCheck = retry
            };
            if (!DoCmd("StartVmasCheck", cmdParams, true)) {
                _log.TraceError("DoCmd(\"StartVmasCheck\") return false");
                return false;
            }
            if (_msgAckRecv != null) {
                errMsg = _msgAckRecv.Message;
                if (_msgAckRecv.Cmd == "StartVmasCheckAck" && _msgAckRecv.Params != null) {
                    ackParams = JsonConvert.DeserializeObject<StartVmasCheckAckParams>(((JObject)_msgAckRecv.Params).ToString(), _dateTimeConverter);
                    return true;
                }
            }
            _log.TraceError("Return value of \"StartVmasCheckAck\" is wrong");
            return false;
        }

        public bool GetVMASRealTimeDataCmd(double overTime, ref GetVmasRealTimeDataAckParams ackParams, out string errMsg) {
            errMsg = string.Empty;
            GetVmasRealTimeDataParams cmdParams = new GetVmasRealTimeDataParams {
                ClientID = ClientID,
                VmasContinueDiffTime = overTime
            };
            if (!DoCmd("GetVmasRealTimeData", cmdParams, false)) {
                _log.TraceError("DoCmd(\"GetFalRealTimeData\") return false");
                return false;
            }
            if (_msgAckRecv != null) {
                errMsg = _msgAckRecv.Message;
                if (_msgAckRecv.Cmd == "GetVmasRealTimeDataAck" && _msgAckRecv.Params != null) {
                    ackParams = JsonConvert.DeserializeObject<GetVmasRealTimeDataAckParams>(((JObject)_msgAckRecv.Params).ToString(), _dateTimeConverter);
                    return true;
                }
            }
            _log.TraceError("Return value of \"GetVmasRealTimeDataAck\" is wrong");
            return false;
        }

        public bool GetVMASCheckResultDataCmd(ref GetVmasCheckResultAckParams ackParams, out string errMsg) {
            errMsg = string.Empty;
            GetVmasCheckResultDataParams cmdParams = new GetVmasCheckResultDataParams {
                ClientID = ClientID
            };
            if (!DoCmd("GetVmasCheckResultData", cmdParams, false)) {
                _log.TraceError("DoCmd(\"GetVmasCheckResultData\") return false");
                return false;
            }
            if (_msgAckRecv != null) {
                errMsg = _msgAckRecv.Message;
                if (_msgAckRecv.Cmd == "GetVmasCheckResultAck" && _msgAckRecv.Params != null) {
                    ackParams = JsonConvert.DeserializeObject<GetVmasCheckResultAckParams>(((JObject)_msgAckRecv.Params).ToString(), _dateTimeConverter);
                    return true;
                }
            }
            _log.TraceError("Return value of \"GetVmasCheckResultAck\" is wrong");
            return false;
        }

        public bool SetDataBaseInitInfoCmd(SetDataBaseInitInfoParams cmdParams, out string errMsg) {
            errMsg = string.Empty;
            if (!DoCmd("SetDataBaseInitInfo", cmdParams, false)) {
                _log.TraceError("DoCmd(\"SetDataBaseInitInfo\") return false");
                return false;
            }
            if (_msgAckRecv != null) {
                errMsg = _msgAckRecv.Message;
                if (_msgAckRecv.Cmd == "SetDataBaseInitInfoAck") {
                    return true;
                }
            }
            _log.TraceError("Return value of \"SetDataBaseInitInfoAck\" is wrong");
            return false;
        }

        public bool GetDataBaseInitInfoCmd(ref GetDataBaseInitInfoAckParams ackParams, out string errMsg) {
            errMsg = string.Empty;
            GetDataBaseInitInfoParams cmdParams = new GetDataBaseInitInfoParams {
                ClientID = ClientID
            };
            if (!DoCmd("GetDataBaseInitInfo", cmdParams, false)) {
                _log.TraceError("DoCmd(\"GetDataBaseInitInfo\") return false");
                return false;
            }
            if (_msgAckRecv != null) {
                errMsg = _msgAckRecv.Message;
                if (_msgAckRecv.Cmd == "GetDataBaseInitInfoAck" && _msgAckRecv.Params != null) {
                    ackParams = JsonConvert.DeserializeObject<GetDataBaseInitInfoAckParams>(((JObject)_msgAckRecv.Params).ToString(), _dateTimeConverter);
                    return true;
                }
            }
            _log.TraceError("Return value of \"GetDataBaseInitInfoAck\" is wrong");
            return false;
        }

        public bool StartQYDynoLoadGlideCheckCmd(StartQYDynoLoadGlideCheckParams cmdParams, ref StartQYDynoLoadGlideCheckAckParams ackParams, out string errMsg) {
            errMsg = string.Empty;
            if (!DoCmd("StartQYDynoLoadGlideCheck", cmdParams, true)) {
                _log.TraceError("DoCmd(\"StartQYDynoLoadGlideCheck\") return false");
                return false;
            }
            if (_msgAckRecv != null) {
                errMsg = _msgAckRecv.Message;
                if (_msgAckRecv.Cmd == "StartQYDynoLoadGlideCheckAck" && _msgAckRecv.Params != null) {
                    ackParams = JsonConvert.DeserializeObject<StartQYDynoLoadGlideCheckAckParams>(((JObject)_msgAckRecv.Params).ToString(), _dateTimeConverter);
                    return true;
                }
            }
            _log.TraceError("Return value of \"StartQYDynoLoadGlideCheckAck\" is wrong");
            return false;
        }

        public bool GetQYDynoLoadGlideCheckRealTimeDataCmd(ref GetQYDynoLoadGlideCheckRealTimeDataAckParams ackParams, out string errMsg) {
            errMsg = string.Empty;
            GetQYDynoLoadGlideCheckRealTimeDataParams cmdParams = new GetQYDynoLoadGlideCheckRealTimeDataParams {
                ClientID = ClientID
            };
            if (!DoCmd("GetQYDynoLoadGlideCheckRealTimeData", cmdParams, false)) {
                _log.TraceError("DoCmd(\"GetQYDynoLoadGlideCheckRealTimeData\") return false");
                return false;
            }
            if (_msgAckRecv != null) {
                errMsg = _msgAckRecv.Message;
                if (_msgAckRecv.Cmd == "GetQYDynoLoadGlideCheckRealTimeDataAck" && _msgAckRecv.Params != null) {
                    ackParams = JsonConvert.DeserializeObject<GetQYDynoLoadGlideCheckRealTimeDataAckParams>(((JObject)_msgAckRecv.Params).ToString(), _dateTimeConverter);
                    return true;
                }
            }
            _log.TraceError("Return value of \"GetQYDynoLoadGlideCheckRealTimeDataAck\" is wrong");
            return false;
        }

        public bool StartCYDynoLoadGlideCheckCmd(StartCYDynoLoadGlideCheckParams cmdParams, out string errMsg) {
            errMsg = string.Empty;
            if (!DoCmd("StartCYDynoLoadGlideCheck", cmdParams, true)) {
                _log.TraceError("DoCmd(\"StartCYDynoLoadGlideCheck\") return false");
                return false;
            }
            if (_msgAckRecv != null) {
                errMsg = _msgAckRecv.Message;
                if (_msgAckRecv.Cmd == "StartCYDynoLoadGlideCheckAck") {
                    return true;
                }
            }
            _log.TraceError("Return value of \"StartQYDynoLoadGlideCheckAck\" is wrong");
            return false;
        }

        public bool GetCYDynoLoadGlideCheckRealTimeDataCmd(double loadPower, ref GetCYDynoLoadGlideCheckRealTimeDataAckParams ackParams, out string errMsg) {
            errMsg = string.Empty;
            GetCYDynoLoadGlideCheckRealTimeDataParams cmdParams = new GetCYDynoLoadGlideCheckRealTimeDataParams {
                ClientID = ClientID,
                loadPower = loadPower
            };
            if (!DoCmd("GetCYDynoLoadGlideCheckRealTimeData", cmdParams, false)) {
                _log.TraceError("DoCmd(\"GetCYDynoLoadGlideCheckRealTimeData\") return false");
                return false;
            }
            if (_msgAckRecv != null) {
                errMsg = _msgAckRecv.Message;
                if (_msgAckRecv.Cmd == "GetCYDynoLoadGlideCheckRealTimeDataAck" && _msgAckRecv.Params != null) {
                    ackParams = JsonConvert.DeserializeObject<GetCYDynoLoadGlideCheckRealTimeDataAckParams>(((JObject)_msgAckRecv.Params).ToString(), _dateTimeConverter);
                    return true;
                }
            }
            _log.TraceError("Return value of \"GetCYDynoLoadGlideCheckRealTimeDataAck\" is wrong");
            return false;
        }

    }
}
