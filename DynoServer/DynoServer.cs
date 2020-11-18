using Dyno_Geely;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace DynoServer {
    public class DynoServer {
        private const int BUFSIZE = 1024;
        private delegate void AckCmdFunction(MsgBaseStr MsgCmd, MsgAckBaseStr MsgCmdAck);
        private readonly Dictionary<string, AckCmdFunction> MsgAckCmd;
        private readonly TcpListener _listener;
        private readonly IsoDateTimeConverter _dateTimeConverter;
        private readonly string _clientID = "SaiHe";
        private int _counter = 0;
        private bool _bDynoSpeedUp = true;
        private bool _bDynoRun = false;
        private int _step = -1;
        private bool _bStopCheck = false;

        public DynoServer(int port) {
            MsgAckCmd = new Dictionary<string, AckCmdFunction> {
                { "Login", LoginAckCmd },
                { "Logout", LogoutAckCmd },
                { "DeviceCtrl", DeviceCtrlAckCmd },
                { "StartDynoPreheat", StartDynoPreheatAckCmd },
                { "GetDynoPreheatRealTimeData", GetDynoPreheatRealTimeDataAckCmd },
                { "SaveDynoPreheatData", SaveDynoPreheatDataAckCmd },
                { "StartGasBoxPreheatSelfCheck", StartGasBoxPreheatSelfCheckAckCmd },
                { "GetGasBoxPreheatSelfCheckRealTimeData", GetGasBoxPreheatSelfCheckRealTimeDataAckCmd },
                { "StartFlowmeterCheck", StartFlowmeterCheckAckCmd },
                { "GetFlowmeterCheckRealTimeData", GetFlowmeterCheckRealTimeDataAckCmd },
                { "GetSmokerPreheatSelfCheckRealTimeData", GetSmokerPreheatSelfCheckRealTimeDataAckCmd },
                { "SaveSmokerPreheatSelfCheckData", SaveSmokerPreheatSelfCheckDataAckCmd },
                { "GetWeatherCheckRealTimeData", GetWeatherCheckRealTimeDataAckCmd },
                { "SaveWeatherCheckData", SaveWeatherCheckDataAckCmd },
                { "StartGasboxPrepare", StartGasboxPrepareAckCmd },
                { "GetGasboxPrepareRealTimeData", GetGasboxPrepareRealTimeDataAckCmd },
                { "StartFlowmeterPrepare", StartFlowmeterPrepareAckCmd },
                { "GetFlowmeterPrepareRealTimeData", GetFlowmeterPrepareRealTimeDataAckCmd },
                { "StartSmokePrepare", StartSmokePrepareAckCmd },
                { "GetSmokePrepareRealTimeData", GetSmokePrepareRealTimeDataAckCmd },
                { "GetOilTempPrepareRealTimeData", GetOilTempPrepareRealTimeDataAckCmd },
                { "GetTachometerPrepareRealTimeData", GetTachometerPrepareRealTimeDataAckCmd },
                { "GetWeatherPrepareRealTimeData", GetWeatherPrepareRealTimeDataAckCmd },
                { "StartLdCheck", StartLdCheckAckCmd },
                { "GetLdRealTimeData", GetLdRealTimeDataAckCmd },
                { "GetLdCheckResultData", GetLdCheckResultAckCmd },
                { "StartASMCheck", StartASMCheckAckCmd },
                { "GetASMRealTimeData", GetASMRealTimeDataAckCmd },
                { "GetASMCheckResultData", GetASMCheckResultAckCmd },
                { "StartFalCheck", StartFALCheckAckCmd },
                { "GetFalRealTimeData", GetFALRealTimeDataAckCmd },
                { "GetFalCheckResultData", GetFALCheckResultAckCmd },
                { "StartTsiCheck", StartTSICheckAckCmd },
                { "GetTsiRealTimeData", GetTSIRealTimeDataAckCmd },
                { "GetTsiCheckResultData", GetTSICheckResultAckCmd },
                { "StartVmasCheck", StartVMASCheckAckCmd },
                { "GetVmasRealTimeData", GetVMASRealTimeDataAckCmd },
                { "GetVmasCheckResultData", GetVMASCheckResultAckCmd },
            };
            _listener = new TcpListener(IPAddress.Any, port);
            _dateTimeConverter = new IsoDateTimeConverter { DateTimeFormat = "yyyyMMddHHmmss" };
        }

        public void ListenForClients() {
            _listener.Start();
            IPEndPoint serverAddress = (IPEndPoint)_listener.LocalEndpoint;
            Console.WriteLine(string.Format("DynoServer start listenning on {0}:{1}", serverAddress.Address, serverAddress.Port));
            while (true) {
                try {
                    TcpClient client = _listener.AcceptTcpClient();
                    Task.Factory.StartNew(HandleClientComm, client);
                } catch (Exception ex) {
                    Console.WriteLine("TCP listener occur error: " + ex.Message);
                }
            }
        }

        private void HandleClientComm(object obj) {
            TcpClient client = (TcpClient)obj;
            NetworkStream clientStream = client.GetStream();
            byte[] recv = new byte[BUFSIZE];
            string strRecv = string.Empty;
            int bytesRead;
            while (true) {
                try {
                    do {
                        bytesRead = clientStream.Read(recv, 0, BUFSIZE);
                        strRecv += Encoding.UTF8.GetString(recv, 0, bytesRead);
                    } while (clientStream.DataAvailable);
                } catch (Exception ex) {
                    Console.WriteLine("TCP client occur error: " + ex.Message);
                    return;
                }
                if (bytesRead == 0) {
                    break;
                }
                strRecv = Encoding.UTF8.GetString(recv, 0, bytesRead);
                IPEndPoint remoteAddress = (IPEndPoint)client.Client.RemoteEndPoint;
                Console.WriteLine(string.Format("Received message from {0}:{1}", remoteAddress.Address, remoteAddress.Port));
                if (strRecv.Length > 0) {
                    JObject JObj = JObject.Parse(strRecv);
                    Console.WriteLine("Dyno server received: " + Environment.NewLine + JObj.ToString());
                } else {
                    Console.WriteLine("Dyno server received empty string");
                }
                MsgBaseStr MsgCmd = JsonConvert.DeserializeObject<MsgBaseStr>(strRecv, _dateTimeConverter);
                MsgAckBaseStr MsgCmdAck = new MsgAckBaseStr();
                MsgAckCmd[MsgCmd.Cmd](MsgCmd, MsgCmdAck);
                byte[] sendMessage;
                string JsonFormatted = JsonConvert.SerializeObject(MsgCmdAck, Newtonsoft.Json.Formatting.Indented, _dateTimeConverter);
                Console.WriteLine("Send MsgAckBaseStr: " + Environment.NewLine + JsonFormatted);
                string strSend = JsonConvert.SerializeObject(MsgCmdAck, _dateTimeConverter);
                sendMessage = Encoding.UTF8.GetBytes(strSend);
                clientStream.Write(sendMessage, 0, sendMessage.Length);
                clientStream.Flush();
            }
            clientStream.Close();
            client.Close();
        }

        private void PackageAckCmd(MsgBaseStr MsgCmd, MsgAckBaseStr MsgCmdAck, string strAckCmd, object ackParams) {
            MsgCmdAck.Cmd = strAckCmd;
            MsgCmdAck.Message = "OK";
            MsgCmdAck.Result = true;
            MsgCmdAck.MsgID = MsgCmd.MsgID;
            MsgCmdAck.ServiceID = MsgCmd.ServiceID;
            MsgCmdAck.Params = ackParams;
        }

        private void LoginAckCmd(MsgBaseStr MsgCmd, MsgAckBaseStr MsgCmdAck) {
            LoginAckParams ackParams = new LoginAckParams {
                permissionCtrl = "Ctrl",
                permissionView = "View"
            };
            PackageAckCmd(MsgCmd, MsgCmdAck, "LoginAck", ackParams);
        }

        private void LogoutAckCmd(MsgBaseStr MsgCmd, MsgAckBaseStr MsgCmdAck) {
            PackageAckCmd(MsgCmd, MsgCmdAck, "LogoutAck", null);
        }

        private void DeviceCtrlAckCmd(MsgBaseStr MsgCmd, MsgAckBaseStr MsgCmdAck) {
            DeviceCtrlAckParams ackParams = new DeviceCtrlAckParams {
                Calibrating = false,
                CalibResult = false
            };
            PackageAckCmd(MsgCmd, MsgCmdAck, "DeviceCtrlAck", ackParams);
        }

        private void StartDynoPreheatAckCmd(MsgBaseStr MsgCmd, MsgAckBaseStr MsgCmdAck) {
            bool stopCheck = false;
            if (MsgCmd.Params != null) {
                JObject cmdParams = (JObject)MsgCmd.Params;
                stopCheck = (bool)cmdParams["stopCheck"];
            }
            _counter = 0;
            StartDynoPreheatAckParams ackParams;
            if (stopCheck) {
                ackParams = new StartDynoPreheatAckParams {
                    msg = "测功机停止预热"
                };
                _bDynoRun = false;
            } else {
                ackParams = new StartDynoPreheatAckParams {
                    msg = "测功机开始预热"
                };
                _bDynoRun = true;
            }
            PackageAckCmd(MsgCmd, MsgCmdAck, "StartDynoPreheatAck", ackParams);
        }

        private void GetDynoPreheatRealTimeDataAckCmd(MsgBaseStr MsgCmd, MsgAckBaseStr MsgCmdAck) {
            GetDynoPreheatRealTimeDataAckParams ackParams;
            if (_bDynoRun) {
                if (_bDynoSpeedUp) {
                    ackParams = new GetDynoPreheatRealTimeDataAckParams {
                        msg = "",
                        speed = _counter++ * 10
                    };
                } else {
                    ackParams = new GetDynoPreheatRealTimeDataAckParams {
                        msg = "",
                        speed = _counter-- * 10
                    };
                }
                if (_counter >= 6) {
                    _bDynoSpeedUp = false;
                }
                if (_counter <= 0) {
                    _bDynoSpeedUp = true;
                }
            } else {
                ackParams = new GetDynoPreheatRealTimeDataAckParams {
                    msg = "",
                    speed = 0
                };
            }
            PackageAckCmd(MsgCmd, MsgCmdAck, "GetDynoPreheatRealTimeDataAck", ackParams);
        }

        private void SaveDynoPreheatDataAckCmd(MsgBaseStr MsgCmd, MsgAckBaseStr MsgCmdAck) {
            PackageAckCmd(MsgCmd, MsgCmdAck, "SaveDynoPreheatDataAck", null);
        }

        private void StartGasBoxPreheatSelfCheckAckCmd(MsgBaseStr MsgCmd, MsgAckBaseStr MsgCmdAck) {
            if (MsgCmd.Params != null) {
                JObject cmdParams = (JObject)MsgCmd.Params;
                _bStopCheck = (bool)cmdParams["stopCheck"];
                _step = (int)cmdParams["step"];
            }
            _counter = 0;
            PackageAckCmd(MsgCmd, MsgCmdAck, "StartGasBoxPreheatSelfCheckAck", null);
        }

        private void GetGasBoxPreheatSelfCheckRealTimeDataAckCmd(MsgBaseStr MsgCmd, MsgAckBaseStr MsgCmdAck) {
            Random rd = new Random();
            GetGasBoxPreheatSelfCheckRealTimeDataAckParams ackParams = new GetGasBoxPreheatSelfCheckRealTimeDataAckParams {
                HC = rd.NextDouble() * 100,
                NO = rd.NextDouble() * 100,
                CO = rd.NextDouble() * 100,
                CO2 = rd.NextDouble() * 100,
                O2 = rd.NextDouble() * 100,
                PEF = rd.NextDouble() * 100
            };
            if (_bStopCheck) {
                ackParams.msg = "停止尾气分析仪预热";
            } else {
                switch (_step) {
                case 0:
                    ackParams.msg = "开始预热检查";
                    if (++_counter > 5) {
                        ackParams.GasBoxPreheatWarmUpResult = "成功";
                        _counter = 0;
                    }
                    break;
                case 1:
                    ackParams.msg = "开始清零检查";
                    if (++_counter > 5) {
                        ackParams.GasBoxPreheatZeroResult = "成功";
                        _counter = 0;
                    }
                    break;
                case 2:
                    ackParams.msg = "开始泄漏检查";
                    if (++_counter > 5) {
                        ackParams.GasBoxPreheatLeakCheckResult = "成功";
                        _counter = 0;
                    }
                    break;
                case 3:
                    ackParams.msg = "开始低流量检查";
                    if (++_counter > 5) {
                        ackParams.GasBoxPreheatLowFlowResult = "成功";
                        _counter = 0;
                    }
                    break;
                case 4:
                    ackParams.msg = "开始氧量程检查";
                    if (++_counter > 5) {
                        ackParams.GasBoxPreheatO2SpanCheckResult = "成功";
                        ackParams.GasBoxPreheatResult = "成功";
                        _counter = 0;
                    }
                    break;
                }
            }
            PackageAckCmd(MsgCmd, MsgCmdAck, "GetGasBoxPreheatSelfCheckRealTimeDataAck", ackParams);
        }

        private void StartFlowmeterCheckAckCmd(MsgBaseStr MsgCmd, MsgAckBaseStr MsgCmdAck) {
            if (MsgCmd.Params != null) {
                JObject cmdParams = (JObject)MsgCmd.Params;
                _bStopCheck = (bool)cmdParams["stopCheck"];
                _step = (int)cmdParams["step"];
            }
            _counter = 0;
            StartFlowmeterCheckAckParams ackParams = new StartFlowmeterCheckAckParams {
                FlowmeterO2SpanLow = 19.8,
                FlowmeterO2SpanHight = 21.8,
                FlowmeterLowFlowSpan = 97,
                FlowmeterTargetPressure = 101.3,
                FlowmeterTargetTempe = 23.5
            };
            PackageAckCmd(MsgCmd, MsgCmdAck, "StartFlowmeterCheckAck", ackParams);
        }

        private void GetFlowmeterCheckRealTimeDataAckCmd(MsgBaseStr MsgCmd, MsgAckBaseStr MsgCmdAck) {
            Random rd = new Random();
            GetFlowmeterCheckRealTimeDataAckParams ackParams = new GetFlowmeterCheckRealTimeDataAckParams {
                Flow = rd.NextDouble() * 100,
                Temperature = rd.NextDouble() * 100,
                Pressure = rd.NextDouble() * 1000,
                DiluteO2 = rd.NextDouble() * 100
            };
            if (_bStopCheck) {
                ackParams.msg = "停止流量计预热";
            } else {
                switch (_step) {
                case 0:
                    ackParams.msg = "开始流量检查";
                    if (++_counter > 5) {
                        ackParams.FlowCheckResult = "成功";
                        _counter = 0;
                    }
                    break;
                case 1:
                    ackParams.msg = "开始氧量程检查";
                    if (++_counter > 5) {
                        ackParams.O2SpanCheckResult = "成功";
                        _counter = 0;
                    }
                    break;
                case 2:
                    ackParams.msg = "开始温度检查";
                    if (++_counter > 5) {
                        ackParams.TempeCheckResult = "成功";
                        _counter = 0;
                    }
                    break;
                case 3:
                    ackParams.msg = "开始压力检查";
                    if (++_counter > 5) {
                        ackParams.PressureCheckResult = "成功";
                        ackParams.FlowmeterCheckResult = "成功";
                        _counter = 0;
                    }
                    break;
                }
            }
            PackageAckCmd(MsgCmd, MsgCmdAck, "GetFlowmeterCheckRealTimeDataAck", ackParams);
        }

        private void GetSmokerPreheatSelfCheckRealTimeDataAckCmd(MsgBaseStr MsgCmd, MsgAckBaseStr MsgCmdAck) {
            Random rd = new Random();
            GetSmokerPreheatSelfCheckRealTimeDataAckParams ackParams = new GetSmokerPreheatSelfCheckRealTimeDataAckParams {
                msg = "不透光烟度计读取实时k值",
                K = rd.NextDouble() * 100,
            };
            PackageAckCmd(MsgCmd, MsgCmdAck, "GetSmokerPreheatSelfCheckRealTimeDataAck", ackParams);
        }

        private void SaveSmokerPreheatSelfCheckDataAckCmd(MsgBaseStr MsgCmd, MsgAckBaseStr MsgCmdAck) {
            PackageAckCmd(MsgCmd, MsgCmdAck, "SaveSmokerPreheatSelfCheckDataAck", null);
        }

        private void GetWeatherCheckRealTimeDataAckCmd(MsgBaseStr MsgCmd, MsgAckBaseStr MsgCmdAck) {
            Random rd = new Random();
            GetWeatherCheckRealTimeDataAckParams ackParams = new GetWeatherCheckRealTimeDataAckParams {
                msg = "读取气象站实时数据",
                EnvTemperature = rd.NextDouble() * 100,
                EnvHumidity = rd.NextDouble() * 100,
                EnvPressure = rd.NextDouble() * 1000,
            };
            PackageAckCmd(MsgCmd, MsgCmdAck, "GetWeatherCheckRealTimeDataAck", ackParams);
        }

        private void SaveWeatherCheckDataAckCmd(MsgBaseStr MsgCmd, MsgAckBaseStr MsgCmdAck) {
            PackageAckCmd(MsgCmd, MsgCmdAck, "SaveWeatherCheckDataAck", null);
        }

        private void StartGasboxPrepareAckCmd(MsgBaseStr MsgCmd, MsgAckBaseStr MsgCmdAck) {
            if (MsgCmd.Params != null) {
                JObject cmdParams = (JObject)MsgCmd.Params;
                _bStopCheck = (bool)cmdParams["IsStopGasboxPrepare"];
            }
            _step = 0;
            _counter = 3;
            StartGasboxPrepareAckParams ackParams = new StartGasboxPrepareAckParams {
                ClientID = _clientID,
            };
            PackageAckCmd(MsgCmd, MsgCmdAck, "StartGasboxPrepareAck", ackParams);
        }

        private void GetGasboxPrepareRealTimeDataAckCmd(MsgBaseStr MsgCmd, MsgAckBaseStr MsgCmdAck) {
            Random rd = new Random();
            GetGasboxPrepareRealTimeDataAckParams ackParams = new GetGasboxPrepareRealTimeDataAckParams {
                AmibientHC = (rd.NextDouble() * 100).ToString("F"),
                AmibientCO = (rd.NextDouble() * 100).ToString("F"),
                AmibientCO2 = (rd.NextDouble() * 100).ToString("F"),
                AmibientNO = (rd.NextDouble() * 100).ToString("F"),
                AmibientO2 = (rd.NextDouble() * 100).ToString("F"),
                BackHC = (rd.NextDouble() * 100).ToString("F"),
                BackCO = (rd.NextDouble() * 100).ToString("F"),
                BackCO2 = (rd.NextDouble() * 100).ToString("F"),
                BackNO = (rd.NextDouble() * 100).ToString("F"),
                BackO2 = (rd.NextDouble() * 100).ToString("F"),
                NowOperationTimeRemaining = _counter.ToString(),
                SumCO2CO = rd.NextDouble() * 100,
                step = _step
            };
            if (_bStopCheck) {
                ackParams.msg = "停止尾气分析仪自检";
                ackParams.step = 9;
                _counter = 3;
            } else {
                switch (_step) {
                case 0:
                    ackParams.msg = "尾气分析仪开始自检";
                    ++_step;
                    break;
                case 1:
                    ackParams.msg = "开始清零。。。";
                    if (--_counter < 0) {
                        ++_step;
                        _counter = 3;
                    }
                    break;
                case 2:
                    ackParams.msg = "结束清零";
                    ++_step;
                    ackParams.Zero = true;
                    break;
                case 3:
                    ackParams.msg = "正在检查环境空气。。。";
                    if (--_counter < 0) {
                        ackParams.AmibientCheck = true;
                        ++_step;
                        _counter = 3;
                    }
                    break;
                case 4:
                    ackParams.msg = "正在检查背景空气。。。";
                    if (--_counter < 0) {
                        ackParams.BackGroundCheck = true;
                        ++_step;
                        _counter = 3;
                    }
                    break;
                case 5:
                    ackParams.msg = "正在检查HC残留。。。";
                    ackParams.ResidualHC = (rd.NextDouble() * 100).ToString("F");
                    if (--_counter < 0) {
                        ackParams.HCResidualCheck = true;
                        ++_step;
                        _counter = 3;
                    }
                    ackParams.HCOperationTimeRemaining = _counter.ToString();
                    break;
                case 6:
                    ackParams.msg = "正在检查氧量程。。。";
                    if (--_counter < 0) {
                        ackParams.O2SpanCheck = true;
                        ++_step;
                        _counter = 3;
                    }
                    break;
                case 7:
                    ackParams.msg = "正在检查采样低流量。。。";
                    if (--_counter < 0) {
                        ackParams.TestGasInLowFlowCheck = true;
                        ++_step;
                        _counter = 3;
                    }
                    break;
                case 8:
                    ackParams.msg = "尾气分析仪自检完成";
                    ackParams.SumCO2CO = 60;
                    _counter = 3;
                    break;
                }
            }
            PackageAckCmd(MsgCmd, MsgCmdAck, "GetGasboxPrepareRealTimeDataAck", ackParams);
        }

        private void StartFlowmeterPrepareAckCmd(MsgBaseStr MsgCmd, MsgAckBaseStr MsgCmdAck) {
            if (MsgCmd.Params != null) {
                JObject cmdParams = (JObject)MsgCmd.Params;
                _bStopCheck = (bool)cmdParams["IsStopFlowmeterPrepare"];
            }
            _step = 0;
            _counter = 15;
            PackageAckCmd(MsgCmd, MsgCmdAck, "StartFlowmeterPrepareAck", null);
        }

        private void GetFlowmeterPrepareRealTimeDataAckCmd(MsgBaseStr MsgCmd, MsgAckBaseStr MsgCmdAck) {
            Random rd = new Random();
            GetFlowmeterPrepareRealTimeDataAckParams ackParams = new GetFlowmeterPrepareRealTimeDataAckParams {
                flow = (rd.NextDouble() * 100).ToString("F"),
                O2 = (rd.NextDouble() * 100).ToString("F"),
                time = _counter.ToString(),
                step = _step
            };
            if (_bStopCheck) {
                ackParams.msg = "停止流量计自检";
                ackParams.step = 6;
                _counter = 15;
            } else {
                switch (_step) {
                case 0:
                    ackParams.msg = "流量计开始自检";
                    if (--_counter < 13) {
                        ++_step;
                    }
                    break;
                case 1:
                    ackParams.msg = "开始清零。。。";
                    if (--_counter < 10) {
                        ++_step;
                    }
                    break;
                case 2:
                    ackParams.msg = "结束清零";
                    if (--_counter < 7) {
                        ++_step;
                        ackParams.ZeroResult = "成功";
                    }
                    break;
                case 3:
                    ackParams.msg = "正在检查流量。。。";
                    if (--_counter < 4) {
                        ++_step;
                        ackParams.FlowCheckResult = "成功";
                    }
                    break;
                case 4:
                    ackParams.msg = "正在检查氧量程。。。";
                    if (--_counter < 1) {
                        ++_step;
                        ackParams.O2SpanCheckResult = "成功";
                    }
                    break;
                case 5:
                    ackParams.msg = "流量计检查完毕,可进行车辆试验";
                    _counter = 15;
                    break;
                }
            }
            PackageAckCmd(MsgCmd, MsgCmdAck, "GetFlowmeterPrepareRealTimeDataAck", ackParams);
        }

        private void StartSmokePrepareAckCmd(MsgBaseStr MsgCmd, MsgAckBaseStr MsgCmdAck) {
            if (MsgCmd.Params != null) {
                JObject cmdParams = (JObject)MsgCmd.Params;
                _bStopCheck = (bool)cmdParams["IsStopSmokePrepare"];
            }
            _step = 0;
            _counter = 12;
            PackageAckCmd(MsgCmd, MsgCmdAck, "StartSmokePrepareAck", null);
        }

        private void GetSmokePrepareRealTimeDataAckCmd(MsgBaseStr MsgCmd, MsgAckBaseStr MsgCmdAck) {
            Random rd = new Random();
            GetSmokePrepareRealTimeDataAckParams ackParams = new GetSmokePrepareRealTimeDataAckParams {
                Ns = (rd.NextDouble() * 100).ToString("F"),
                K = (rd.NextDouble() * 100).ToString("F"),
                CO2 = rd.NextDouble() * 100,
                step = _step
            };
            if (_bStopCheck) {
                ackParams.msg = "停止烟度计自检";
                ackParams.step = 5;
                _counter = 12;
            } else {
                switch (_step) {
                case 0:
                    ackParams.msg = "烟度计开始自检";
                    if (--_counter < 10) {
                        ++_step;
                    }
                    break;
                case 1:
                    ackParams.msg = "开始清零。。。";
                    if (--_counter < 7) {
                        ++_step;
                    }
                    break;
                case 2:
                    ackParams.msg = "结束清零";
                    if (--_counter < 4) {
                        ++_step;
                        ackParams.Zero = "完成";
                    }
                    break;
                case 3:
                    ackParams.msg = "正在校准量距点。。。";
                    if (--_counter < 1) {
                        ++_step;
                        ackParams.DistancepointCheck = true;
                    }
                    break;
                case 4:
                    ackParams.msg = "请将取样探头插入排气管";
                    _counter = 12;
                    break;
                }
            }
            PackageAckCmd(MsgCmd, MsgCmdAck, "GetSmokePrepareRealTimeDataAck", ackParams);
        }

        private void GetOilTempPrepareRealTimeDataAckCmd(MsgBaseStr MsgCmd, MsgAckBaseStr MsgCmdAck) {
            Random rd = new Random();
            GetOilTempPrepareRealTimeDataAckParams ackParams = new GetOilTempPrepareRealTimeDataAckParams {
                oilTemp = rd.NextDouble() * 100,
                oilTitle = "开始油温计自检"
            };
            PackageAckCmd(MsgCmd, MsgCmdAck, "GetOilTempPrepareRealTimeDataAck", ackParams);
        }

        private void GetTachometerPrepareRealTimeDataAckCmd(MsgBaseStr MsgCmd, MsgAckBaseStr MsgCmdAck) {
            Random rd = new Random();
            GetTachometerPrepareRealTimeDataAckParams ackParams = new GetTachometerPrepareRealTimeDataAckParams {
                RPM = (rd.NextDouble() * 1000).ToString("F"),
            };
            PackageAckCmd(MsgCmd, MsgCmdAck, "GetTachometerPrepareRealTimeDataAck", ackParams);
        }

        private void GetWeatherPrepareRealTimeDataAckCmd(MsgBaseStr MsgCmd, MsgAckBaseStr MsgCmdAck) {
            Random rd = new Random();
            GetWeatherPrepareRealTimeDataAckParams ackParams = new GetWeatherPrepareRealTimeDataAckParams {
                temperature = rd.NextDouble() * 100,
                humidity = rd.NextDouble() * 100,
                amibientPressure = rd.NextDouble() * 100,
            };
            PackageAckCmd(MsgCmd, MsgCmdAck, "GetWeatherPrepareRealTimeDataAck", ackParams);
        }

        private void StartLdCheckAckCmd(MsgBaseStr MsgCmd, MsgAckBaseStr MsgCmdAck) {
            if (MsgCmd.Params != null) {
                JObject cmdParams = (JObject)MsgCmd.Params;
                _bStopCheck = (bool)cmdParams["stopCheck"];
            }
            _step = 0;
            _counter = 0;
            PackageAckCmd(MsgCmd, MsgCmdAck, "StartLdCheckAck", null);
        }

        private void GetLdRealTimeDataAckCmd(MsgBaseStr MsgCmd, MsgAckBaseStr MsgCmdAck) {
            if (MsgCmd.Params != null) {
                JObject cmdParams = (JObject)MsgCmd.Params;
                _bStopCheck = (bool)cmdParams["stopCheck"];
            }
            Random rd = new Random();
            GetLdRealTimeDataAckParams ackParams = new GetLdRealTimeDataAckParams {
                RPM = _step * 369,
                Speed = _step * 8,
                Power = _step * 7.5,
                K = rd.NextDouble() * 100,
                NOx = rd.NextDouble() * 100,
                CO2 = rd.NextDouble() * 100,
                step = _step
            };
            if (_bStopCheck) {
                ackParams.msg = "停止加载减速测试";
                ackParams.step = 0;
                _counter = 0;
            } else {
                switch (_step) {
                case 0:
                    ackParams.msg = "获取最大转速";
                    if (_counter++ > 2) {
                        ++_step;
                        _counter = 0;
                    }
                    break;
                case 1:
                    ackParams.msg = "最大功率点扫描--加速阶段";
                    if (_counter++ > 2) {
                        ++_step;
                        _counter = 0;
                    }
                    break;
                case 2:
                    ackParams.msg = "最大功率点扫描--开始";
                    if (_counter++ > 2) {
                        ++_step;
                        _counter = 0;
                    }
                    break;
                case 3:
                    ackParams.msg = "最大功率点扫描--进行中";
                    if (_counter++ > 2) {
                        ++_step;
                        _counter = 0;
                    }
                    break;
                case 4:
                    ackParams.msg = "测试加载前";
                    if (_counter++ > 2) {
                        ++_step;
                        _counter = 0;
                    }
                    break;
                case 5:
                    ackParams.msg = "测试加载后";
                    if (_counter++ > 2) {
                        ++_step;
                        _counter = 0;
                    }
                    break;
                case 6:
                    ackParams.msg = "测试阶段--速度稳定";
                    if (_counter++ > 2) {
                        ++_step;
                        _counter = 0;
                    }
                    break;
                case 7:
                    ackParams.msg = "测试阶段--记录数据9s";
                    if (_counter++ > 2) {
                        ++_step;
                        _counter = 0;
                    }
                    break;
                case 8:
                    ackParams.msg = "降至怠速";
                    if (_counter++ > 2) {
                        ++_step;
                        _counter = 0;
                    }
                    break;
                case 9:
                    ackParams.msg = "怠速60秒";
                    if (_counter++ > 2) {
                        ++_step;
                        _counter = 0;
                    }
                    break;
                case 10:
                    ackParams.msg = "最终判定";
                    if (_counter++ > 2) {
                        ++_step;
                        _counter = 0;
                    }
                    break;
                case 11:
                    ackParams.msg = "检测完成，结果展示";
                    if (_counter++ > 10) {
                        ++_step;
                        _counter = 0;
                    }
                    break;
                case 12:
                    ackParams.msg = "检测完成，结果展示结束";
                    if (_counter++ > 2) {
                        _counter = 0;
                    }
                    break;
                }
            }
            PackageAckCmd(MsgCmd, MsgCmdAck, "GetLdRealTimeDataAck", ackParams);
        }

        private void GetLdCheckResultAckCmd(MsgBaseStr MsgCmd, MsgAckBaseStr MsgCmdAck) {
            Random rd = new Random();
            GetLdCheckResultAckParams ackParams = new GetLdCheckResultAckParams {
                msg = "获取加载减速检测结果",
                KLimit = 100,
                NOx80Limit = 80,
                RealMaxPowerLimit = 70,
                VelMaxHP = rd.NextDouble() * 100,
                K100 = rd.NextDouble() * 100,
                K80 = rd.NextDouble() * 100,
                NOx80 = rd.NextDouble() * 100,
                RealMaxPower = rd.NextDouble() * 100,
                LdCheckeResult = "合格",
                step = _step
            };
            PackageAckCmd(MsgCmd, MsgCmdAck, "GetLdCheckResultAck", ackParams);
        }

        private void StartASMCheckAckCmd(MsgBaseStr MsgCmd, MsgAckBaseStr MsgCmdAck) {
            if (MsgCmd.Params != null) {
                JObject cmdParams = (JObject)MsgCmd.Params;
                _bStopCheck = (bool)cmdParams["stopCheck"];
            }
            _step = 0;
            _counter = 0;
            PackageAckCmd(MsgCmd, MsgCmdAck, "StartASMCheckAck", null);
        }

        private void GetASMRealTimeDataAckCmd(MsgBaseStr MsgCmd, MsgAckBaseStr MsgCmdAck) {
            if (MsgCmd.Params != null) {
                JObject cmdParams = (JObject)MsgCmd.Params;
                _bStopCheck = (bool)cmdParams["stopCheck"];
            }
            GetASMRealTimeDataAckParams ackParams = new GetASMRealTimeDataAckParams {
                RPM = _step * 369,
                speed = _step * 6,
                testTime = _step + _counter * 0.5,
                singleWorkingCondition = _counter * 0.5,
                accTime = _step,
                stableTime = _step,
                step = _step
            };
            if (_bStopCheck) {
                ackParams.msg = "停止稳态工况测试";
                ackParams.step = 0;
                _counter = 0;
            } else {
                switch (_step) {
                case 0:
                    ackParams.msg = "5025加速阶段";
                    if (_counter++ > 2) {
                        ackParams.SpeedMax = 50;
                        ackParams.SpeedMin = 0;
                        ackParams.PaintGreen = 23;
                        ackParams.PaintOrange = 21;
                        ackParams.speedContinuityOverProofTime = 1;
                        ackParams.speedCumulativeOverProofTime = 3;
                        ackParams.torqueContinuityOverProofTime = 1;
                        ackParams.torqueCumulativeOverProofTime = 3;
                        ++_step;
                        _counter = 0;
                    }
                    break;
                case 1:
                    ackParams.msg = "5025稳定阶段";
                    if (_counter++ > 2) {
                        ackParams.SpeedMax = 30;
                        ackParams.SpeedMin = 20;
                        ackParams.PaintGreen = 23;
                        ackParams.PaintOrange = 21;
                        ackParams.speedContinuityOverProofTime = 1;
                        ackParams.speedCumulativeOverProofTime = 3;
                        ackParams.torqueContinuityOverProofTime = 1;
                        ackParams.torqueCumulativeOverProofTime = 3;
                        ++_step;
                        _counter = 0;
                    }
                    break;
                case 2:
                    ackParams.msg = "5025十秒准备阶段";
                    if (_counter++ > 2) {
                        ackParams.SpeedMax = 30;
                        ackParams.SpeedMin = 20;
                        ackParams.PaintGreen = 23;
                        ackParams.PaintOrange = 21;
                        ackParams.speedContinuityOverProofTime = 1;
                        ackParams.speedCumulativeOverProofTime = 3;
                        ackParams.torqueContinuityOverProofTime = 1;
                        ackParams.torqueCumulativeOverProofTime = 3;
                        ++_step;
                        _counter = 0;
                    }
                    break;
                case 3:
                    ackParams.msg = "5025快速检查阶段";
                    if (_counter++ > 2) {
                        ackParams.SpeedMax = 30;
                        ackParams.SpeedMin = 20;
                        ackParams.PaintGreen = 23;
                        ackParams.PaintOrange = 21;
                        ackParams.speedContinuityOverProofTime = 1;
                        ackParams.speedCumulativeOverProofTime = 3;
                        ackParams.torqueContinuityOverProofTime = 1;
                        ackParams.torqueCumulativeOverProofTime = 3;
                        ++_step;
                        _counter = 0;
                    }
                    break;
                case 4:
                    ackParams.msg = "5025判定快速检查结果阶段";
                    if (_counter++ > 2) {
                        ackParams.SpeedMax = 30;
                        ackParams.SpeedMin = 20;
                        ackParams.PaintGreen = 23;
                        ackParams.PaintOrange = 21;
                        ackParams.speedContinuityOverProofTime = 1;
                        ackParams.speedCumulativeOverProofTime = 3;
                        ackParams.torqueContinuityOverProofTime = 1;
                        ackParams.torqueCumulativeOverProofTime = 3;
                        ++_step;
                        _counter = 0;
                    }
                    break;
                case 5:
                    ackParams.msg = "5025工况70秒检测阶段";
                    if (_counter++ > 2) {
                        ackParams.SpeedMax = 30;
                        ackParams.SpeedMin = 20;
                        ackParams.PaintGreen = 23;
                        ackParams.PaintOrange = 21;
                        ackParams.speedContinuityOverProofTime = 1;
                        ackParams.speedCumulativeOverProofTime = 3;
                        ackParams.torqueContinuityOverProofTime = 1;
                        ackParams.torqueCumulativeOverProofTime = 3;
                        ++_step;
                        _counter = 0;
                    }
                    break;
                case 6:
                    ackParams.msg = "5025工况70秒判定阶段";
                    if (_counter++ > 2) {
                        ackParams.SpeedMax = 50;
                        ackParams.SpeedMin = 0;
                        ackParams.PaintGreen = 0;
                        ackParams.PaintOrange = 0;
                        ackParams.speedContinuityOverProofTime = 1;
                        ackParams.speedCumulativeOverProofTime = 3;
                        ackParams.torqueContinuityOverProofTime = 1;
                        ackParams.torqueCumulativeOverProofTime = 3;
                        ++_step;
                        _counter = 0;
                    }
                    break;
                case 7:
                    ackParams.msg = "2540加速阶段";
                    if (_counter++ > 2) {
                        ackParams.SpeedMax = 70;
                        ackParams.SpeedMin = 10;
                        ackParams.PaintGreen = 38;
                        ackParams.PaintOrange = 36;
                        ackParams.speedContinuityOverProofTime = 1;
                        ackParams.speedCumulativeOverProofTime = 3;
                        ackParams.torqueContinuityOverProofTime = 1;
                        ackParams.torqueCumulativeOverProofTime = 3;
                        ++_step;
                        _counter = 0;
                    }
                    break;
                case 8:
                    ackParams.msg = "2540稳定阶段";
                    if (_counter++ > 2) {
                        ackParams.SpeedMax = 50;
                        ackParams.SpeedMin = 30;
                        ackParams.PaintGreen = 38;
                        ackParams.PaintOrange = 36;
                        ackParams.speedContinuityOverProofTime = 1;
                        ackParams.speedCumulativeOverProofTime = 3;
                        ackParams.torqueContinuityOverProofTime = 1;
                        ackParams.torqueCumulativeOverProofTime = 3;
                        ++_step;
                        _counter = 0;
                    }
                    break;
                case 9:
                    ackParams.msg = "2540十秒准备阶段";
                    if (_counter++ > 2) {
                        ackParams.SpeedMax = 50;
                        ackParams.SpeedMin = 30;
                        ackParams.PaintGreen = 38;
                        ackParams.PaintOrange = 36;
                        ackParams.speedContinuityOverProofTime = 1;
                        ackParams.speedCumulativeOverProofTime = 3;
                        ackParams.torqueContinuityOverProofTime = 1;
                        ackParams.torqueCumulativeOverProofTime = 3;
                        ++_step;
                        _counter = 0;
                    }
                    break;
                case 10:
                    ackParams.msg = "2540快速检查阶段";
                    if (_counter++ > 2) {
                        ackParams.SpeedMax = 50;
                        ackParams.SpeedMin = 30;
                        ackParams.PaintGreen = 38;
                        ackParams.PaintOrange = 36;
                        ackParams.speedContinuityOverProofTime = 1;
                        ackParams.speedCumulativeOverProofTime = 3;
                        ackParams.torqueContinuityOverProofTime = 1;
                        ackParams.torqueCumulativeOverProofTime = 3;
                        ++_step;
                        _counter = 0;
                    }
                    break;
                case 11:
                    ackParams.msg = "2540判定快速检查结果阶段";
                    if (_counter++ > 10) {
                        ackParams.SpeedMax = 50;
                        ackParams.SpeedMin = 30;
                        ackParams.PaintGreen = 38;
                        ackParams.PaintOrange = 36;
                        ackParams.speedContinuityOverProofTime = 1;
                        ackParams.speedCumulativeOverProofTime = 3;
                        ackParams.torqueContinuityOverProofTime = 1;
                        ackParams.torqueCumulativeOverProofTime = 3;
                        ++_step;
                        _counter = 0;
                    }
                    break;
                case 12:
                    ackParams.msg = "2540工况70秒检测阶段";
                    if (_counter++ > 2) {
                        ackParams.SpeedMax = 50;
                        ackParams.SpeedMin = 30;
                        ackParams.PaintGreen = 38;
                        ackParams.PaintOrange = 36;
                        ackParams.speedContinuityOverProofTime = 1;
                        ackParams.speedCumulativeOverProofTime = 3;
                        ackParams.torqueContinuityOverProofTime = 1;
                        ackParams.torqueCumulativeOverProofTime = 3;
                        ++_step;
                        _counter = 0;
                    }
                    break;
                case 13:
                    ackParams.msg = "2540工况70秒判定阶段";
                    if (_counter++ > 2) {
                        ackParams.SpeedMax = 70;
                        ackParams.SpeedMin = 0;
                        ackParams.PaintGreen = 0;
                        ackParams.PaintOrange = 0;
                        ackParams.speedContinuityOverProofTime = 1;
                        ackParams.speedCumulativeOverProofTime = 3;
                        ackParams.torqueContinuityOverProofTime = 1;
                        ackParams.torqueCumulativeOverProofTime = 3;
                        ++_step;
                        _counter = 0;
                    }
                    break;
                case 14:
                    ackParams.msg = "检测完成，可以进行检测结果展示";
                    if (_counter++ > 2) {
                        ackParams.SpeedMax = 70;
                        ackParams.SpeedMin = 0;
                        ackParams.PaintGreen = 0;
                        ackParams.PaintOrange = 0;
                        ackParams.speedContinuityOverProofTime = 1;
                        ackParams.speedCumulativeOverProofTime = 3;
                        ackParams.torqueContinuityOverProofTime = 1;
                        ackParams.torqueCumulativeOverProofTime = 3;
                        ++_step;
                        _counter = 0;
                    }
                    break;
                case 15:
                    ackParams.msg = "结果展示结束";
                    if (_counter++ > 2) {
                        ackParams.SpeedMax = 70;
                        ackParams.SpeedMin = 0;
                        ackParams.PaintGreen = 0;
                        ackParams.PaintOrange = 0;
                        ackParams.speedContinuityOverProofTime = 1;
                        ackParams.speedCumulativeOverProofTime = 3;
                        ackParams.torqueContinuityOverProofTime = 1;
                        ackParams.torqueCumulativeOverProofTime = 3;
                        _counter = 0;
                    }
                    break;
                }
            }
            PackageAckCmd(MsgCmd, MsgCmdAck, "GetASMRealTimeDataAck", ackParams);
        }

        private void GetASMCheckResultAckCmd(MsgBaseStr MsgCmd, MsgAckBaseStr MsgCmdAck) {
            Random rd = new Random();
            GetASMCheckResultAckParams ackParams = new GetASMCheckResultAckParams {
                msg = "获取加载减速检测结果",
                HC5025Limit = "190",
                CO5025Limit = "1.8",
                NO5025Limit = "3400",
                HC5025 = (rd.NextDouble() * 1000).ToString("F"),
                CO5025 = (rd.NextDouble() * 100).ToString("F"),
                NO5025 = (rd.NextDouble() * 1000).ToString("F"),
                HC5025Evl = "合格",
                CO5025Evl = "合格",
                NO5025Evl = "合格",
                HC2540Limit = "230",
                CO2540Limit = "2.4",
                NO2540Limit = "3200",
                HC2540 = (rd.NextDouble() * 1000).ToString("F"),
                CO2540 = (rd.NextDouble() * 100).ToString("F"),
                NO2540 = (rd.NextDouble() * 1000).ToString("F"),
                HC2540Evl = "合格",
                CO2540Evl = "合格",
                NO2540Evl = "合格",
                ASMCheckeResult = "合格",
                step = _step
            };
            PackageAckCmd(MsgCmd, MsgCmdAck, "GetASMCheckResultAck", ackParams);
        }

        private void StartFALCheckAckCmd(MsgBaseStr MsgCmd, MsgAckBaseStr MsgCmdAck) {
            if (MsgCmd.Params != null) {
                JObject cmdParams = (JObject)MsgCmd.Params;
                _bStopCheck = (bool)cmdParams["stopCheck"];
            }
            _step = 0;
            _counter = 0;
            PackageAckCmd(MsgCmd, MsgCmdAck, "StartFalCheckAck", null);
        }

        private void GetFALRealTimeDataAckCmd(MsgBaseStr MsgCmd, MsgAckBaseStr MsgCmdAck) {
            if (MsgCmd.Params != null) {
                JObject cmdParams = (JObject)MsgCmd.Params;
                _bStopCheck = (bool)cmdParams["stopCheck"];
            }
            Random rd = new Random();
            GetFalRealTimeDataAckParams ackParams = new GetFalRealTimeDataAckParams {
                RPM = _step * 369,
                CurrentStageTime = _counter,
                K = rd.NextDouble() + _step,
                step = _step
            };
            if (_bStopCheck) {
                ackParams.msg = "停止自由加速不透光测试";
                ackParams.step = 0;
                _counter = 0;
            } else {
                switch (_step) {
                case 0:
                    ackParams.msg = "吹拂1";
                    if (_counter++ > 2) {
                        ++_step;
                        _counter = 0;
                    }
                    break;
                case 1:
                    ackParams.msg = "吹拂2";
                    if (_counter++ > 2) {
                        ++_step;
                        _counter = 0;
                    }
                    break;
                case 2:
                    ackParams.msg = "吹拂3";
                    if (_counter++ > 2) {
                        ++_step;
                        _counter = 0;
                    }
                    break;
                case 3:
                    ackParams.msg = "提示插管";
                    if (_counter++ > 2) {
                        ++_step;
                        _counter = 0;
                    }
                    break;
                case 4:
                    ackParams.msg = "提示插管";
                    if (_counter++ > 2) {
                        ++_step;
                        _counter = 0;
                    }
                    break;
                case 5:
                    ackParams.msg = "测试第1次";
                    if (_counter++ > 2) {
                        ++_step;
                        _counter = 0;
                    }
                    break;
                case 6:
                    ackParams.msg = "测试第2次";
                    if (_counter++ > 2) {
                        ++_step;
                        _counter = 0;
                    }
                    break;
                case 7:
                    ackParams.msg = "测试第3次";
                    if (_counter++ > 2) {
                        ++_step;
                        _counter = 0;
                    }
                    break;
                case 8:
                    ackParams.msg = "进入加速阶段";
                    if (_counter++ > 2) {
                        ++_step;
                        _counter = 0;
                    }
                    break;
                case 9:
                    ackParams.msg = "加速阶段松开油门等待阶段1";
                    if (_counter++ > 2) {
                        ackParams.K1 = _step + rd.NextDouble();
                        ++_step;
                        _counter = 0;
                    }
                    break;
                case 10:
                    ackParams.msg = "加速阶段松开油门等待阶段2";
                    if (_counter++ > 2) {
                        ackParams.K2 = _step + rd.NextDouble();
                        ++_step;
                        _counter = 0;
                    }
                    break;
                case 11:
                    ackParams.msg = "加速阶段松开油门等待阶段3";
                    if (_counter++ > 10) {
                        ackParams.K3 = _step + rd.NextDouble();
                        ++_step;
                        _counter = 0;
                    }
                    break;
                case 12:
                    ackParams.msg = "开始判定结果";
                    if (_counter++ > 2) {
                        ++_step;
                        _counter = 0;
                    }
                    break;
                case 13:
                    ackParams.msg = "最终结果判定";
                    if (_counter++ > 2) {
                        ++_step;
                        _counter = 0;
                    }
                    break;
                case 14:
                    ackParams.msg = "检测完成，结果展示";
                    if (_counter++ > 2) {
                        ++_step;
                        _counter = 0;
                    }
                    break;
                case 15:
                    ackParams.msg = "检测完成，结果展示结束";
                    if (_counter++ > 2) {
                        _counter = 0;
                    }
                    break;
                }
            }
            PackageAckCmd(MsgCmd, MsgCmdAck, "GetFalRealTimeDataAck", ackParams);
        }

        private void GetFALCheckResultAckCmd(MsgBaseStr MsgCmd, MsgAckBaseStr MsgCmdAck) {
            Random rd = new Random();
            GetFalCheckResultAckParams ackParams = new GetFalCheckResultAckParams {
                msg = "获取自由加速不透光检测结果",
                KLimit = 2.5,
                K1 = rd.NextDouble() * 10,
                K2 = rd.NextDouble() * 10,
                K3 = rd.NextDouble() * 10,
                KAvg = rd.NextDouble() * 10,
                FalCheckeResult = "合格",
                step = _step
            };
            PackageAckCmd(MsgCmd, MsgCmdAck, "GetFalCheckResultAck", ackParams);
        }

        private void StartTSICheckAckCmd(MsgBaseStr MsgCmd, MsgAckBaseStr MsgCmdAck) {
            if (MsgCmd.Params != null) {
                JObject cmdParams = (JObject)MsgCmd.Params;
                _bStopCheck = (bool)cmdParams["stopCheck"];
            }
            _step = 0;
            _counter = 0;
            PackageAckCmd(MsgCmd, MsgCmdAck, "StartTsiCheckAck", null);
        }

        private void GetTSIRealTimeDataAckCmd(MsgBaseStr MsgCmd, MsgAckBaseStr MsgCmdAck) {
            if (MsgCmd.Params != null) {
                JObject cmdParams = (JObject)MsgCmd.Params;
                _bStopCheck = (bool)cmdParams["stopCheck"];
            }
            Random rd = new Random();
            GetTsiRealTimeDataAckParams ackParams = new GetTsiRealTimeDataAckParams {
                RPM = _step * 369,
                CurrentStageTime = _counter,
                lmd = rd.NextDouble(),
                oilTemp = rd.NextDouble() * 100,
                step = _step
            };
            if (_bStopCheck) {
                ackParams.msg = "停止双怠速测试";
                ackParams.step = 0;
                _counter = 0;
            } else {
                switch (_step) {
                case 0:
                    ackParams.msg = "70%额定转速";
                    if (_counter++ > 2) {
                        ++_step;
                        _counter = 0;
                    }
                    break;
                case 1:
                    ackParams.msg = "提示插入取样管";
                    if (_counter++ > 2) {
                        ++_step;
                        _counter = 0;
                    }
                    break;
                case 2:
                    ackParams.msg = "高怠速15秒";
                    if (_counter++ > 2) {
                        ++_step;
                        _counter = 0;
                    }
                    break;
                case 3:
                    ackParams.msg = "高怠速30秒";
                    if (_counter++ > 2) {
                        ++_step;
                        _counter = 0;
                    }
                    break;
                case 4:
                    ackParams.msg = "高怠速判定";
                    if (_counter++ > 2) {
                        ackParams.Hresult = "合格";
                        ++_step;
                        _counter = 0;
                    }
                    break;
                case 5:
                    ackParams.msg = "低怠速15秒";
                    if (_counter++ > 2) {
                        ++_step;
                        _counter = 0;
                    }
                    break;
                case 6:
                    ackParams.msg = "低怠速30秒";
                    if (_counter++ > 2) {
                        ++_step;
                        _counter = 0;
                    }
                    break;
                case 7:
                    ackParams.msg = "低怠速判定";
                    if (_counter++ > 2) {
                        ackParams.Lresult = "合格";
                        ++_step;
                        _counter = 0;
                    }
                    break;
                case 8:
                    ackParams.msg = "检测完成，可以进行检测结果展示";
                    if (_counter++ > 10) {
                        ++_step;
                        _counter = 0;
                    }
                    break;
                case 9:
                    ackParams.msg = "结果展示结束";
                    if (_counter++ > 2) {
                        _counter = 0;
                    }
                    break;
                }
            }
            PackageAckCmd(MsgCmd, MsgCmdAck, "GetTsiRealTimeDataAck", ackParams);
        }

        private void GetTSICheckResultAckCmd(MsgBaseStr MsgCmd, MsgAckBaseStr MsgCmdAck) {
            Random rd = new Random();
            GetTsiCheckResultAckParams ackParams = new GetTsiCheckResultAckParams {
                msg = "获取双怠速检测结果",
                HighCOLimit = 3,
                HighHCLimit = 900,
                HighCO = rd.NextDouble() * 10,
                HighHC = rd.NextDouble() * 1000,
                HighIdeResult = "合格",
                LowCOLimit = 4.5,
                LowHCLimit = 1200,
                LowCO = rd.NextDouble() * 10,
                LowHC = rd.NextDouble() * 1000,
                LowIdeResult = "合格",
                LumdaLimit = 0.95,
                Lumda = rd.NextDouble(),
                LumdaResult = "合格",
                TsiCheckeResult = "合格",
                step = _step
            };
            PackageAckCmd(MsgCmd, MsgCmdAck, "GetTsiCheckResultAck", ackParams);
        }

        private void StartVMASCheckAckCmd(MsgBaseStr MsgCmd, MsgAckBaseStr MsgCmdAck) {
            if (MsgCmd.Params != null) {
                JObject cmdParams = (JObject)MsgCmd.Params;
                _bStopCheck = (bool)cmdParams["stopCheck"];
            }
            _step = 0;
            _counter = 0;
            StartVmasCheckAckParams ackParams = new StartVmasCheckAckParams {
                VmasCheckSpeedSpan = 3
            };
            PackageAckCmd(MsgCmd, MsgCmdAck, "StartVmasCheckAck", ackParams);
        }

        private void GetVMASRealTimeDataAckCmd(MsgBaseStr MsgCmd, MsgAckBaseStr MsgCmdAck) {
            if (MsgCmd.Params != null) {
                JObject cmdParams = (JObject)MsgCmd.Params;
                _bStopCheck = (bool)cmdParams["stopCheck"];
            }
            Random rd = new Random();
            GetVmasRealTimeDataAckParams ackParams = new GetVmasRealTimeDataAckParams {
                speed = (rd.NextDouble() * 100) % 60,
                RPM = _step * 369,
                speedContinuityOverProofTime = 2,
                power = rd.NextDouble() * 100,
                HC = rd.NextDouble() * 1000,
                NO = rd.NextDouble() * 1000,
                CO = rd.NextDouble() * 100,
                CO2 = rd.NextDouble() * 100,
                O2 = rd.NextDouble() * 100,
                dilutionO2 = rd.NextDouble() * 100,
                environmentalO2 = rd.NextDouble() * 100,
                dilutionRatio = rd.NextDouble(),
                flow = rd.NextDouble() * 100,
                MoveStep = _step + 1,
                step = _step
            };
            if (_bStopCheck) {
                ackParams.msg = "停止简易瞬态工况测试";
                ackParams.step = 0;
                _counter = 0;
            } else {
                switch (_step) {
                case 0:
                    ackParams.msg = "准备阶段";
                    if (_counter++ > 10) {
                        ++_step;
                        _counter = 0;
                    }
                    break;
                case 1:
                    ackParams.msg = "怠速状态";
                    if (_counter++ > 10) {
                        ++_step;
                        _counter = 0;
                    }
                    break;
                case 2:
                    ackParams.msg = "测试阶段";
                    if (_counter++ > 10) {
                        ++_step;
                        _counter = 0;
                    }
                    break;
                case 3:
                    ackParams.msg = "检测完成，可以进行检测结果展示";
                    if (_counter++ > 10) {
                        ++_step;
                        _counter = 0;
                    }
                    break;
                case 4:
                    ackParams.msg = "结果展示结束";
                    if (_counter++ > 10) {
                        _counter = 0;
                    }
                    break;
                }
            }
            PackageAckCmd(MsgCmd, MsgCmdAck, "GetVmasRealTimeDataAck", ackParams);
        }

        private void GetVMASCheckResultAckCmd(MsgBaseStr MsgCmd, MsgAckBaseStr MsgCmdAck) {
            Random rd = new Random();
            GetVmasCheckResultAckParams ackParams = new GetVmasCheckResultAckParams {
                msg = "获取简易瞬态工况检测结果",
                HCLimit = "1.6",
                COLimit = "8",
                NOLimit = "1.3",
                HCTest = (rd.NextDouble() * 10).ToString("F"),
                COTest = (rd.NextDouble() * 10).ToString("F"),
                NOTest = (rd.NextDouble() * 10).ToString("F"),
                HCNOTest = (rd.NextDouble() * 10).ToString("F"),
                HCEvl = "合格",
                COEvl = "合格",
                NOEvl = "合格",
                VmasCheckeResult = "合格",
                step = _step
            };
            PackageAckCmd(MsgCmd, MsgCmdAck, "GetVmasCheckResultAck", ackParams);
        }

    }
}
