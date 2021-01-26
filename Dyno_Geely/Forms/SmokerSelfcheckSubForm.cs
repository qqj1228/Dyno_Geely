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
    public partial class SmokerSelfcheckSubForm : Form {
        private float _lastHeight;
        private readonly DynoCmd _dynoCmd;
        private readonly MainSetting _mainCfg;
        private readonly Dictionary<Form, bool> _dicResults;
        private readonly Dictionary<Form, bool> _dicStops;
        private readonly string[] _strStep;
        private readonly System.Timers.Timer _timer;
        public event EventHandler<SelfcheckDoneEventArgs> SelfcheckDone;

        public SmokerSelfcheckSubForm(DynoCmd dynoCmd, MainSetting mainCfg, Dictionary<Form, bool> dicResults, Dictionary<Form, bool> dicStops) {
            InitializeComponent();
            _lastHeight = this.Height;
            _dynoCmd = dynoCmd;
            _mainCfg = mainCfg;
            _dicResults = dicResults;
            _dicStops = dicStops;
            _strStep = new string[] { "启动", "清零", "实时测量", "量距校准", "完成" };
            _timer = new System.Timers.Timer(_mainCfg.RealtimeInterval);
            _timer.Elapsed += OnTimer;
            _timer.AutoReset = true;
        }

        private void OnTimer(object source, System.Timers.ElapsedEventArgs e) {
            GetSmokePrepareRealTimeDataAckParams ackParams = new GetSmokePrepareRealTimeDataAckParams();
            if (_dynoCmd.GetSmokePrepareRealTimeDataCmd(true, false, ref ackParams) && ackParams != null) {
                if (_timer != null && _timer.Enabled) {
                    try {
                        Invoke((EventHandler)delegate {
                            if (ackParams.msg != null && ackParams.msg.Length > 0) {
                                lblMsg.Text = ackParams.msg;
                            }
                            if (ackParams.step >= 0 && ackParams.step < 5) {
                                lblStep.Text = _strStep[ackParams.step];
                            } else {
                                lblStep.Text = "--";
                            }
                            lblNs.Text = ackParams.Ns;
                            lblK.Text = ackParams.K;
                            lblCO2.Text = ackParams.CO2.ToString("F");
                            if (lblZero.Text != "完成") {
                                lblZero.Text = ackParams.Zero ?? "--";
                            }
                            if (lblDistancepointCheck.Text != "成功") {
                                lblDistancepointCheck.Text = ackParams.DistancepointCheck ? "成功" : "失败";
                            }
                            if (ackParams.step >= 4 || _dicStops[this]) {
                                _timer.Enabled = false;
                                bool bResult = lblZero.Text == "完成";
                                bResult = bResult && lblDistancepointCheck.Text == "成功";
                                _dicResults[this] = bResult;
                                lblResult.Text = _dicResults[this] ? "成功" : "失败";
                                ackParams = new GetSmokePrepareRealTimeDataAckParams();
                                _dynoCmd.GetSmokePrepareRealTimeDataCmd(false, true, ref ackParams);
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
                if (!_dynoCmd.StartSmokePrepareCmd(false, false)) {
                    MessageBox.Show("执行开始烟度计准备命令失败", "执行命令出错", MessageBoxButtons.OK, MessageBoxIcon.Error);
                } else {
                    _timer.Enabled = true;
                }
            } else {
                if (!_dynoCmd.StartSmokePrepareCmd(true, true)) {
                    MessageBox.Show("执行停止烟度计准备命令失败", "执行命令出错", MessageBoxButtons.OK, MessageBoxIcon.Error);
                } else {
                    _timer.Enabled = false;
                    lblMsg.Text = "已手动停止烟度计自检";
                }
            }
        }

        private void SmokerSelfcheckSubForm_Load(object sender, EventArgs e) {
            lblMsg.Text = "烟度计自检";
        }

        private void SmokerSelfcheckSubForm_Resize(object sender, EventArgs e) {
            if (_lastHeight == 0) {
                return;
            }
            float scale = this.Height / _lastHeight;
            layoutMain.Font = new Font(layoutMain.Font.FontFamily, layoutMain.Font.Size * scale, layoutMain.Font.Style);
            lblMsg.Font = new Font(lblMsg.Font.FontFamily, lblMsg.Font.Size * scale, lblMsg.Font.Style);
            _lastHeight = this.Height;
        }

        private void SmokerSelfcheckSubForm_FormClosing(object sender, FormClosingEventArgs e) {
            _timer.Elapsed -= OnTimer;
            _timer.Enabled = false;
        }

        private void BtnStart_Click(object sender, EventArgs e) {
            lblZero.Text = "--";
            lblDistancepointCheck.Text = "--";
            lblResult.Text = "--";
            StartSelfcheck(true);
        }

        private void BtnStop_Click(object sender, EventArgs e) {
            _dynoCmd.ReconnectServer();
            StartSelfcheck(false);
        }
    }
}
