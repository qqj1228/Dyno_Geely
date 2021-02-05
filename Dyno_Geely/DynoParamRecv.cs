using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Sockets;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Dyno_Geely {
    public class DynoParamRecv {
        private readonly TcpClient _client;
        private readonly NetworkStream _clientStream;
        private readonly int _bufSize;
        private readonly byte[] _recvBuf;
        private string _strRecv;
        public event EventHandler<DynoParamRecvEventArgs> DynoParamRecvEvent;

        public DynoParamRecv(string hostName, int port) {
            try {
                _client = new TcpClient(hostName, port);
                _clientStream = _client.GetStream();
            } catch (Exception) {
                if (_clientStream != null) {
                    _clientStream.Close();
                }
                if (_client != null) {
                    _client.Close();
                }
                throw;
            }
            _bufSize = 4096;
            _recvBuf = new byte[_bufSize];
            _strRecv = "";
        }

        ~DynoParamRecv() {
            if (_clientStream != null) {
                _clientStream.Close();
            }
            if (_client != null) {
                _client.Close();
            }
        }

        public void SendVIN(string strVIN) {
            byte[] sendMessage = Encoding.UTF8.GetBytes(strVIN);
            _clientStream.Write(sendMessage, 0, sendMessage.Length);
            _clientStream.Flush();
            Task.Factory.StartNew(RecvMsg);
        }

        private void RecvMsg() {
            DynoParamRecvEventArgs args = new DynoParamRecvEventArgs();
            int bytesRead;
            _strRecv = "";
            try {
                do {
                    bytesRead = _clientStream.Read(_recvBuf, 0, _bufSize);
                    _strRecv += Encoding.UTF8.GetString(_recvBuf, 0, bytesRead);
                } while (_clientStream.DataAvailable);
            } catch (Exception ex) {
                args.Code = "600";
                args.Msg = "发送VIN号后，接收返回信息出错：" + ex.Message;
                DynoParamRecvEvent?.Invoke(this, args);
                return;
            }

            // TCP接收的数据会有粘包现象，需要拆包操作
            if (_strRecv.StartsWith("200")) {
                if (_strRecv.Length == 3) {
                    _strRecv = "";
                    try {
                        do {
                            bytesRead = _clientStream.Read(_recvBuf, 0, _bufSize);
                            _strRecv += Encoding.UTF8.GetString(_recvBuf, 0, bytesRead);
                        } while (_clientStream.DataAvailable);
                    } catch (Exception ex) {
                        args.Code = "600";
                        args.Msg = "接收测功机参数出错：" + ex.Message;
                        DynoParamRecvEvent?.Invoke(this, args);
                        return;
                    }
                } else {
                    _strRecv = _strRecv.Substring(3);
                }
                args.Code = "200";
                args.Msg = _strRecv;
            } else {
                if (_strRecv.Length >= 3) {
                    args.Code = _strRecv.Substring(0, 3);
                    if (args.Code == "400") {
                        args.Msg = "VIN号格式错误";
                    } else {
                        args.Msg = _strRecv.Substring(3);
                    }
                } else {
                    args.Code = "600";
                    args.Msg = "未知错误";
                }
            }
            DynoParamRecvEvent?.Invoke(this, args);
        }
    }

    public class DynoParamRecvEventArgs : EventArgs {
        public string Code { get; set; }
        public string Msg { get; set; }
    }

}
