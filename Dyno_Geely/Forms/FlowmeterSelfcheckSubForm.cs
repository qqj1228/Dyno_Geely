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
    public partial class FlowmeterSelfcheckSubForm : Form {
        private float _lastHeight;
        private readonly DynoCmd _dynoCmd;
        private readonly MainSetting _mainCfg;
        private readonly Dictionary<Form, bool> _dicResults;
        private readonly Dictionary<Form, bool> _dicStops;
        private readonly string[] _strStep;
        private readonly System.Timers.Timer _timer;
        public event EventHandler<SelfcheckDoneEventArgs> SelfcheckDone;

        public FlowmeterSelfcheckSubForm(DynoCmd dynoCmd, MainSetting mainCfg, Dictionary<Form, bool> dicResults, Dictionary<Form, bool> dicStops) {
            InitializeComponent();
            _lastHeight = Height;
            _dynoCmd = dynoCmd;
            _mainCfg = mainCfg;
            _dicResults = dicResults;
            _dicStops = dicStops;
            _strStep = new string[] { "清零", "清零结果", "氧量程检查", "流量检查", "准备完成" };
            _timer = new System.Timers.Timer(_mainCfg.RealtimeInterval);
            _timer.Elapsed += OnTimer;
            _timer.AutoReset = true;
        }

        private void OnTimer(object source, System.Timers.ElapsedEventArgs e) {
            GetFlowmeterPrepareRealTimeDataAckParams ackParams = new GetFlowmeterPrepareRealTimeDataAckParams();
            if (_dynoCmd.GetFlowmeterPrepareRealTimeDataCmd(true, false, ref ackParams, out string errMsg) && ackParams != null) {
                if (_timer != null && _timer.Enabled) {
                    try {
                        Invoke((EventHandler)delegate {
                            if (ackParams.msg != null && ackParams.msg.Length > 0) {
                                lblMsg.Text = ackParams.msg + ", 剩余" + ackParams.time + "秒";
                            } else {
                                lblMsg.Text = "流量计自检, 剩余" + ackParams.time + "秒";
                            }
                            if (ackParams.step >= 0 && ackParams.step < 5) {
                                lblStep.Text = _strStep[ackParams.step];
                            } else {
                                lblStep.Text = "--";
                            }
                            lblFlow.Text = ackParams.flow;
                            lblO2.Text = ackParams.O2;
                            lblRestTime.Text = ackParams.time;
                            if (lblZero.Text != "合格") {
                                lblZero.Text = ackParams.ZeroResult ?? "--";
                            }
                            if (lblFlowCheck.Text != "合格") {
                                lblFlowCheck.Text = ackParams.FlowCheckResult ?? "--";
                            }
                            if (lblO2SpanCheck.Text != "合格") {
                                lblO2SpanCheck.Text = ackParams.O2SpanCheckResult ?? "--";
                            }
                            if (lblResult.Text != "合格") {
                                lblResult.Text = ackParams.FlowmeterPrepareResult ?? "--";
                            }
                            if ((ackParams.step >= 4) || _dicStops[this]) {
                                _timer.Enabled = false;
                                bool bResult = lblZero.Text == "合格";
                                //bResult = bResult && lblFlowCheck.Text == "合格";
                                //bResult = bResult && lblO2SpanCheck.Text == "合格";
                                _dicResults[this] = bResult;
                                //lblResult.Text = _dicResults[this] ? "合格" : "不合格";
                                ackParams = new GetFlowmeterPrepareRealTimeDataAckParams();
                                _dynoCmd.GetFlowmeterPrepareRealTimeDataCmd(false, true, ref ackParams, out errMsg);
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
                // 现在的测功机服务端软件使用的“DeviceVirtual.dll”虚拟流量计驱动需要发两次开始命令才能接收实时数据
                if (!_dynoCmd.StartFlowmeterPrepareCmd(false, false, out string errMsg)) {
                    MessageBox.Show("执行开始流量计准备命令失败", "执行命令出错", MessageBoxButtons.OK, MessageBoxIcon.Error);
                } else {
                    for (int i = 0; i < 3; i++) {
                        GetFlowmeterPrepareRealTimeDataAckParams ackParams = new GetFlowmeterPrepareRealTimeDataAckParams();
                        if (_dynoCmd.GetFlowmeterPrepareRealTimeDataCmd(true, false, ref ackParams, out errMsg) || ackParams != null || ackParams.step >= 0 || ackParams.msg != "手动终止检测") {
                            if (!_dynoCmd.StartFlowmeterPrepareCmd(false, false, out errMsg)) {
                                MessageBox.Show("执行开始流量计准备命令失败", "执行命令出错", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            } else {
                                _timer.Enabled = true;
                                break;
                            }
                        }
                    }
                }
            } else {
                _timer.Enabled = false;
                Thread.Sleep(_mainCfg.RealtimeInterval);
                if (!_dynoCmd.StartFlowmeterPrepareCmd(true, true, out string errMsg) && errMsg != "ati >= 0") {
                    MessageBox.Show("执行停止流量计准备命令失败", "执行命令出错", MessageBoxButtons.OK, MessageBoxIcon.Error);
                } else if (errMsg.Length > 0) {
                    if (errMsg == "ati >= 0") {
                        lblMsg.Text = "已手动停止流量计自检";
                    } else if (errMsg != "OK") {
                        lblMsg.Text = errMsg;
                    }
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
            float scale = Height / _lastHeight;
            layoutMain.Font = new Font(layoutMain.Font.FontFamily, layoutMain.Font.Size * scale, layoutMain.Font.Style);
            lblMsg.Font = new Font(lblMsg.Font.FontFamily, lblMsg.Font.Size * scale, lblMsg.Font.Style);
            _lastHeight = Height;
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
