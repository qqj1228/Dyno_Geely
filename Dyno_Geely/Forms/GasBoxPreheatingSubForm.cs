using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Dyno_Geely.Forms {
    public partial class GasBoxPreheatingSubForm : Form {
        private float _lastHeight;
        private readonly DynoCmd _dynoCmd;
        private readonly MainSetting _mainCfg;
        private readonly Dictionary<Form, bool> _dicResults;
        private readonly System.Timers.Timer _timer;
        public event EventHandler<PreheatingDoneEventArgs> PreheatingDone;
        // -1: 停止检测; 0：预热；1：清零；2：泄露检查；3：低流量检查；4：氧量程检查
        private int _step;
        // 结果数组, 存放每一步的结果, -1: 失败, 0: 无结果, 1: 成功
        // [0]：预热；[1]：清零；[2]：泄露检查；[3]：低流量检查；[4]：氧量程检查
        private readonly int[] _iResults;

        public GasBoxPreheatingSubForm(DynoCmd dynoCmd, MainSetting mainCfg, Dictionary<Form, bool> dicResults) {
            InitializeComponent();
            _lastHeight = Height;
            _dynoCmd = dynoCmd;
            _mainCfg = mainCfg;
            _dicResults = dicResults;
            _timer = new System.Timers.Timer(_mainCfg.RealtimeInterval);
            _timer.Elapsed += OnTimer;
            _timer.AutoReset = true;
            _step = -1;
            _iResults = new int[] { 0, 0, 0, 0, 0 };
        }

        private void DoStep() {
            if (lblWarmUp.Text == "成功") {
                _iResults[0] = 1;
            } else if (lblWarmUp.Text == "失败") {
                _iResults[0] = -1;
            }
            if (lblZero.Text == "成功") {
                _iResults[1] = 1;
            } else if (lblZero.Text == "失败") {
                _iResults[1] = -1;
            }
            if (lblLeak.Text == "成功") {
                _iResults[2] = 1;
            } else if (lblLeak.Text == "失败") {
                _iResults[2] = -1;
            }
            if (lblLowFlow.Text == "成功") {
                _iResults[3] = 1;
            } else if (lblLowFlow.Text == "失败") {
                _iResults[3] = -1;
            }
            if (lblO2Span.Text == "成功") {
                _iResults[4] = 1;
            } else if (lblO2Span.Text == "失败") {
                _iResults[4] = -1;
            }

            if (_iResults[_step] == 1) {
                _timer.Enabled = false;
                if (_step < _iResults.Length - 1) {
                    StartGasBoxPreheatSelfCheckParams cmdParams = new StartGasBoxPreheatSelfCheckParams() {
                        ClientID = _dynoCmd.ClientID,
                        stopCheck = false,
                        step = ++_step,
                        isQY = true,
                        isRetry = false
                    };
                    if (!_dynoCmd.StartGasBoxPreheatSelfCheckCmd(cmdParams, out string errMsg)) {
                        MessageBox.Show("执行开始分析仪预热命令失败", "执行命令出错", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    } else {
                        _timer.Enabled = true;
                    }
                } else {
                    ++_step;
                }
            }
        }

        private void OnTimer(object source, System.Timers.ElapsedEventArgs e) {
            GetGasBoxPreheatSelfCheckRealTimeDataAckParams ackParams = new GetGasBoxPreheatSelfCheckRealTimeDataAckParams();
            if (_dynoCmd.GetGasBoxPreheatSelfCheckRealTimeDataCmd(ref ackParams, out string errMsg) && ackParams != null) {
                if (_timer != null && _timer.Enabled) {
                    try {
                        Invoke((EventHandler)delegate {
                            if (ackParams.msg != null && ackParams.msg.Length > 0) {
                                lblMsg.Text = ackParams.msg;
                            }
                            lblHC.Text = ackParams.HC.ToString("F");
                            lblNO.Text = ackParams.NO.ToString("F");
                            lblCO.Text = ackParams.CO.ToString("F");
                            lblCO2.Text = ackParams.CO2.ToString("F");
                            lblO2.Text = ackParams.O2.ToString("F");
                            lblPEF.Text = ackParams.PEF.ToString("F");
                            if (ackParams.GasBoxPreheatWarmUpResult != null && ackParams.GasBoxPreheatWarmUpResult.Length > 0) {
                                lblWarmUp.Text = ackParams.GasBoxPreheatWarmUpResult;
                            }
                            if (ackParams.GasBoxPreheatZeroResult != null && ackParams.GasBoxPreheatZeroResult.Length > 0) {
                                lblZero.Text = ackParams.GasBoxPreheatZeroResult;
                            }
                            if (ackParams.GasBoxPreheatLeakCheckResult != null && ackParams.GasBoxPreheatLeakCheckResult.Length > 0) {
                                lblLeak.Text = ackParams.GasBoxPreheatLeakCheckResult;
                            }
                            if (ackParams.GasBoxPreheatLowFlowResult != null && ackParams.GasBoxPreheatLowFlowResult.Length > 0) {
                                lblLowFlow.Text = ackParams.GasBoxPreheatLowFlowResult;
                            }
                            if (ackParams.GasBoxPreheatO2SpanCheckResult != null && ackParams.GasBoxPreheatO2SpanCheckResult.Length > 0) {
                                lblO2Span.Text = ackParams.GasBoxPreheatO2SpanCheckResult;
                            }
                            if (ackParams.GasBoxPreheatResult != null && ackParams.GasBoxPreheatResult.Length > 0) {
                                lblResult.Text = ackParams.GasBoxPreheatResult;
                            }
                        });
                    } catch (ObjectDisposedException) {
                        // 关闭窗口后仍有一定几率会进入主UI线程，此时访问界面元素会引发此异常，直接忽略即可
                    }
                    switch (_step) {
                    case 0:
                        DoStep();
                        break;
                    case 1:
                        DoStep();
                        break;
                    case 2:
                        DoStep();
                        break;
                    case 3:
                        DoStep();
                        break;
                    case 4:
                        DoStep();
                        break;
                    }
                    foreach (int item in _iResults) {
                        if (item < 0) {
                            _timer.Enabled = false;
                            try {
                                Invoke((EventHandler)delegate {
                                    lblMsg.Text = "尾气分析仪预热失败!";
                                    lblHC.Text = "--";
                                    lblNO.Text = "--";
                                    lblCO.Text = "--";
                                    lblCO2.Text = "--";
                                    lblO2.Text = "--";
                                    lblPEF.Text = "--";
                                });
                            } catch (ObjectDisposedException) {
                                // 关闭窗口后仍有一定几率会进入主UI线程，此时访问界面元素会引发此异常，直接忽略即可
                            }
                            break;
                        }
                    }
                    int iResult = 0;
                    if (_step > 4) {
                        foreach (int item in _iResults) {
                            iResult += item;
                        }
                        if (iResult >= 5 && ackParams.GasBoxPreheatResult == "成功") {
                            _timer.Enabled = false;
                            _dicResults[this] = true;
                            try {
                                Invoke((EventHandler)delegate {
                                    lblMsg.Text = "尾气分析仪预热成功";
                                    lblHC.Text = "--";
                                    lblNO.Text = "--";
                                    lblCO.Text = "--";
                                    lblCO2.Text = "--";
                                    lblO2.Text = "--";
                                    lblPEF.Text = "--";
                                });
                            } catch (ObjectDisposedException) {
                                // 关闭窗口后仍有一定几率会进入主UI线程，此时访问界面元素会引发此异常，直接忽略即可
                            }
                            PreheatingDoneEventArgs args = new PreheatingDoneEventArgs {
                                Result = _dicResults[this]
                            };
                            PreheatingDone?.Invoke(this, args);
                        }
                    }
                }
            }
        }

        private void GasBoxPreheatingSubForm_Load(object sender, EventArgs e) {
            lblMsg.Text = "尾气分析仪预热";
        }

        private void BtnStart_Click(object sender, EventArgs e) {
            _step = 0;
            for (int i = 0; i < _iResults.Length; i++) {
                _iResults[i] = 0;
            }
            StartGasBoxPreheatSelfCheckParams cmdParams = new StartGasBoxPreheatSelfCheckParams {
                ClientID = _dynoCmd.ClientID,
                stopCheck = false,
                step = _step,
                isQY = true,
                isRetry = false
            };
            if (!_dynoCmd.StartGasBoxPreheatSelfCheckCmd(cmdParams, out string msg)) {
                MessageBox.Show("执行开始分析仪预热命令失败", "执行命令出错", MessageBoxButtons.OK, MessageBoxIcon.Error);
            } else {
                _timer.Enabled = true;
            }
        }

        private void GasBoxPreheatingSubForm_FormClosing(object sender, FormClosingEventArgs e) {
            _timer.Elapsed -= OnTimer;
            _timer.Enabled = false;
        }

        private void BtnStop_Click(object sender, EventArgs e) {
            _timer.Enabled = false;
            Thread.Sleep(_mainCfg.RealtimeInterval);
            //_dynoCmd.ReconnectServer();
            StartGasBoxPreheatSelfCheckParams cmdParams = new StartGasBoxPreheatSelfCheckParams() {
                ClientID = _dynoCmd.ClientID,
                stopCheck = true,
                step = -1,
                isQY = true,
                isRetry = false
            };
            if (!_dynoCmd.StartGasBoxPreheatSelfCheckCmd(cmdParams, out string errMsg) && errMsg != "ati >= 0") {
                MessageBox.Show("执行停止分析仪预热命令失败", "执行命令出错", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (errMsg.Length > 0) {
                if (errMsg == "ati >= 0") {
                    lblMsg.Text = "手动停止尾气分析仪预热";
                } else if (errMsg != "OK") {
                    lblMsg.Text = errMsg;
                }
            }
            lblHC.Text = "--";
            lblNO.Text = "--";
            lblCO.Text = "--";
            lblCO2.Text = "--";
            lblO2.Text = "--";
            lblPEF.Text = "--";
        }

        private void GasBoxPreheatingSubForm_Resize(object sender, EventArgs e) {
            if (_lastHeight == 0) {
                return;
            }
            float scale = Height / _lastHeight;
            layoutMain.Font = new Font(layoutMain.Font.FontFamily, layoutMain.Font.Size * scale, layoutMain.Font.Style);
            lblMsg.Font = new Font(lblMsg.Font.FontFamily, lblMsg.Font.Size * scale, lblMsg.Font.Style);
            _lastHeight = Height;
        }
    }
}
