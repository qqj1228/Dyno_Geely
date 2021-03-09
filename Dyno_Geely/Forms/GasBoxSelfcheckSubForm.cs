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

namespace Dyno_Geely {
    public partial class GasBoxSelfcheckSubForm : Form {
        private float _lastHeight;
        private readonly DynoCmd _dynoCmd;
        private readonly MainSetting _mainCfg;
        private readonly Dictionary<Form, bool> _dicResults;
        private readonly Dictionary<Form, bool> _dicStops;
        private readonly bool _bDiesel;
        private readonly System.Timers.Timer _timer;
        public event EventHandler<SelfcheckDoneEventArgs> SelfcheckDone;

        public GasBoxSelfcheckSubForm(DynoCmd dynoCmd, MainSetting mainCfg, Dictionary<Form, bool> dicResults, Dictionary<Form, bool> dicStops, bool bDiesel) {
            InitializeComponent();
            _lastHeight = Height;
            _dynoCmd = dynoCmd;
            _mainCfg = mainCfg;
            _dicResults = dicResults;
            _dicStops = dicStops;
            _bDiesel = bDiesel;
            _timer = new System.Timers.Timer(_mainCfg.RealtimeInterval);
            _timer.Elapsed += OnTimer;
            _timer.AutoReset = true;
        }

        private void OnTimer(object source, System.Timers.ElapsedEventArgs e) {
            GetGasboxPrepareRealTimeDataAckParams ackParams = new GetGasboxPrepareRealTimeDataAckParams();
            if (_dynoCmd.GetGasboxPrepareRealTimeDataCmd(true, false, ref ackParams, out string errMsg) && ackParams != null) {
                if (_timer != null && _timer.Enabled) {
                    try {
                        Invoke((EventHandler)delegate {
                            if (ackParams.msg != null && ackParams.msg.Length > 0) {
                                lblMsg.Text = ackParams.msg + ", 剩余" + ackParams.NowOperationTimeRemaining + "秒";
                            } else {
                                lblMsg.Text = "尾气分析仪自检, 剩余" + ackParams.NowOperationTimeRemaining + "秒";
                            }
                            lblAmibientHC.Text = ackParams.AmibientHC;
                            lblAmibientCO.Text = ackParams.AmibientCO;
                            lblAmibientCO2.Text = ackParams.AmibientCO2;
                            lblAmibientNO.Text = ackParams.AmibientNO;
                            lblAmibientO2.Text = ackParams.AmibientO2;
                            lblBackHC.Text = ackParams.BackHC;
                            lblBackCO.Text = ackParams.BackCO;
                            lblBackCO2.Text = ackParams.BackCO2;
                            lblBackNO.Text = ackParams.BackNO;
                            lblBackO2.Text = ackParams.BackO2;
                            lblStep.Text = ackParams.step.ToString();
                            lblResidualHC.Text = ackParams.ResidualHC ?? "--";
                            lblCO2COGas.Text = ackParams.QYSumCO2COLimit.ToString("F");
                            lblCO2CODiesel.Text = ackParams.CYSumCO2COLimit.ToString("F");
                            lblCO2CO.Text = ackParams.SumCO2CO.ToString("F");
                            lblZero.Text = ackParams.Zero ? "成功" : "失败";
                            lblAmibientCheck.Text = ackParams.AmibientCheck ? "成功" : "失败";
                            lblBackGroundCheck.Text = ackParams.BackGroundCheck ? "成功" : "失败";
                            lblHCResidualCheck.Text = ackParams.HCResidualCheck ? "成功" : "失败";
                            lblO2SpanCheck.Text = ackParams.O2SpanCheck ? "成功" : "失败";
                            lblLowFlowCheck.Text = ackParams.TestGasInLowFlowCheck ? "成功" : "失败";
                            if (ackParams.step >= 8 || _dicStops[this]) {
                                _timer.Enabled = false;
                                bool bResult = lblZero.Text == "成功";
                                bResult = bResult && lblAmibientCheck.Text == "成功";
                                bResult = bResult && lblBackGroundCheck.Text == "成功";
                                bResult = bResult && lblHCResidualCheck.Text == "成功";
                                bResult = bResult && lblO2SpanCheck.Text == "成功";
                                bResult = bResult && lblLowFlowCheck.Text == "成功";
                                if (_bDiesel) {
                                    //bResult = bResult && ackParams.SumCO2CO > ackParams.CYSumCO2COLimit;
                                    bResult = ackParams.SumCO2CO > ackParams.CYSumCO2COLimit;
                                } else {
                                    //bResult = bResult && ackParams.SumCO2CO > ackParams.QYSumCO2COLimit;
                                    bResult = ackParams.SumCO2CO > ackParams.QYSumCO2COLimit;
                                }
                                _dicResults[this] = bResult;
                                lblResult.Text = _dicResults[this] ? "成功" : "失败";
                                ackParams = new GetGasboxPrepareRealTimeDataAckParams();
                                _dynoCmd.GetGasboxPrepareRealTimeDataCmd(false, true, ref ackParams, out errMsg);
                                SelfcheckDoneEventArgs args = new SelfcheckDoneEventArgs {
                                    Result = _dicResults[this]
                                };
                                SelfcheckDone?.Invoke(this, args);
                            }
                        });
                    } catch (ObjectDisposedException) {
                        // 关闭窗口后仍有一定几率会进入主UI线程，此时访问界面元素会引发此异常，直接忽略即可
                    }
                }
            }
        }

        public void StartSelfcheck(bool bStart) {
            string fuel = "汽油";
            if (_bDiesel) {
                fuel = "柴油";
            }
            if (bStart) {
                if (!_dynoCmd.StartGasboxPrepareCmd(false, false, fuel, out string errMsg)) {
                    MessageBox.Show("执行开始分析仪准备命令失败", "执行命令出错", MessageBoxButtons.OK, MessageBoxIcon.Error);
                } else {
                    _timer.Enabled = true;
                }
            } else {
                _timer.Enabled = false;
                Thread.Sleep(_mainCfg.RealtimeInterval);
                if (!_dynoCmd.StartGasboxPrepareCmd(true, false, fuel, out string errMsg) && errMsg != "ati >= 0") {
                    MessageBox.Show("执行停止分析仪准备命令失败", "执行命令出错", MessageBoxButtons.OK, MessageBoxIcon.Error);
                } else if (errMsg.Length > 0) {
                    if (errMsg == "ati >= 0") {
                        lblMsg.Text = "已手动停止尾气分析仪自检";
                    } else if (errMsg != "OK") {
                        lblMsg.Text = errMsg;
                    }
                }
            }
        }

        private void GasBoxSelfcheckSubForm_Load(object sender, EventArgs e) {
            lblMsg.Text = "尾气分析仪自检";
        }

        private void GasBoxSelfcheckSubForm_Resize(object sender, EventArgs e) {
            if (_lastHeight == 0) {
                return;
            }
            float scale = Height / _lastHeight;
            layoutMain.Font = new Font(layoutMain.Font.FontFamily, layoutMain.Font.Size * scale, layoutMain.Font.Style);
            lblMsg.Font = new Font(lblMsg.Font.FontFamily, lblMsg.Font.Size * scale, lblMsg.Font.Style);
            _lastHeight = Height;
        }

        private void BtnStart_Click(object sender, EventArgs e) {
            lblZero.Text = "--";
            lblAmibientCheck.Text = "--";
            lblBackGroundCheck.Text = "--";
            lblHCResidualCheck.Text = "--";
            lblO2SpanCheck.Text = "--";
            lblLowFlowCheck.Text = "--";
            lblResult.Text = "--";
            StartSelfcheck(true);
        }

        private void BtnStop_Click(object sender, EventArgs e) {
            _dynoCmd.ReconnectServer();
            StartSelfcheck(false);
        }

        private void GasBoxSelfcheckSubForm_FormClosing(object sender, FormClosingEventArgs e) {
            _timer.Elapsed -= OnTimer;
            _timer.Enabled = false;
        }
    }
}
