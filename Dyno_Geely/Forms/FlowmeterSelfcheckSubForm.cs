using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Dyno_Geely {
    public partial class FlowmeterSelfcheckSubForm : Form {
        private float _lastHeight;
        private readonly DynoCmd _dynoCmd;
        private readonly MainSetting _mainCfg;
        private readonly Dictionary<Form, bool> _dicResults;
        private readonly Dictionary<Form, bool> _dicStops;
        private readonly System.Timers.Timer _timer;
        public event EventHandler<SelfcheckDoneEventArgs> SelfcheckDone;

        public FlowmeterSelfcheckSubForm(DynoCmd dynoCmd, MainSetting mainCfg, Dictionary<Form, bool> dicResults, Dictionary<Form, bool> dicStops) {
            InitializeComponent();
            _lastHeight = this.Height;
            _dynoCmd = dynoCmd;
            _mainCfg = mainCfg;
            _dicResults = dicResults;
            _dicStops = dicStops;
            _timer = new System.Timers.Timer(_mainCfg.RealtimeInterval);
            _timer.Elapsed += OnTimer;
            _timer.AutoReset = true;
        }

        private void OnTimer(object source, System.Timers.ElapsedEventArgs e) {
            GetFlowmeterPrepareRealTimeDataAckParams ackParams = new GetFlowmeterPrepareRealTimeDataAckParams();
            if (_dynoCmd.GetFlowmeterPrepareRealTimeDataCmd(true, false, ref ackParams) && ackParams != null) {
                if (_timer != null && _timer.Enabled) {
                    try {
                        Invoke((EventHandler)delegate {
                            if (ackParams.msg != null && ackParams.msg.Length > 0) {
                                lblMsg.Text = ackParams.msg;
                            }
                            lblStep.Text = ackParams.step.ToString();
                            lblFlow.Text = ackParams.flow;
                            lblO2.Text = ackParams.O2;
                            lblRestTime.Text = ackParams.time;
                            if (lblZero.Text != "成功") {
                                lblZero.Text = ackParams.ZeroResult ?? "--";
                            }
                            if (lblFlowCheck.Text != "成功") {
                                lblFlowCheck.Text = ackParams.FlowCheckResult ?? "--";
                            }
                            if (lblO2SpanCheck.Text != "成功") {
                                lblO2SpanCheck.Text = ackParams.O2SpanCheckResult ?? "--";
                            }
                            if ((ackParams.step >= 5 && lblMsg.Text == "流量计检查完毕,可进行车辆试验") || _dicStops[this]) {
                                _timer.Enabled = false;
                                bool bResult = lblZero.Text == "成功";
                                bResult = bResult && lblFlowCheck.Text == "成功";
                                bResult = bResult && lblO2SpanCheck.Text == "成功";
                                _dicResults[this] = bResult;
                                lblResult.Text = _dicResults[this] ? "成功" : "失败";
                                ackParams = new GetFlowmeterPrepareRealTimeDataAckParams();
                                _dynoCmd.GetFlowmeterPrepareRealTimeDataCmd(false, true, ref ackParams);
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
            if (bStart) {
                if (!_dynoCmd.StartFlowmeterPrepareCmd(false)) {
                    MessageBox.Show("执行开始流量计准备命令失败", "执行命令出错", MessageBoxButtons.OK, MessageBoxIcon.Error);
                } else {
                    _timer.Enabled = true;
                }
            } else {
                if (!_dynoCmd.StartFlowmeterPrepareCmd(true)) {
                    MessageBox.Show("执行停止流量计准备命令失败", "执行命令出错", MessageBoxButtons.OK, MessageBoxIcon.Error);
                } else {
                    _timer.Enabled = false;
                    lblMsg.Text = "已手动停止流量计自检";
                }
            }
        }

        private void FlowmeterSelfcheckSubForm_Load(object sender, EventArgs e) {
            lblMsg.Text = "流量计自检";
        }

        private void FlowmeterSelfcheckSubForm_Resize(object sender, EventArgs e) {
            if (_lastHeight == 0) {
                return;
            }
            float scale = this.Height / _lastHeight;
            layoutMain.Font = new Font(layoutMain.Font.FontFamily, layoutMain.Font.Size * scale, layoutMain.Font.Style);
            lblMsg.Font = new Font(lblMsg.Font.FontFamily, lblMsg.Font.Size * scale, lblMsg.Font.Style);
            _lastHeight = this.Height;
        }

        private void BtnStart_Click(object sender, EventArgs e) {
            lblZero.Text = "--";
            lblFlowCheck.Text = "--";
            lblO2SpanCheck.Text = "--";
            lblResult.Text = "--";
            StartSelfcheck(true);
        }

        private void BtnStop_Click(object sender, EventArgs e) {
            _dynoCmd.ReconnectServer();
            StartSelfcheck(false);
        }

        private void FlowmeterSelfcheckSubForm_FormClosing(object sender, FormClosingEventArgs e) {
            _timer.Elapsed -= OnTimer;
            _timer.Enabled = false;
        }
    }
}
