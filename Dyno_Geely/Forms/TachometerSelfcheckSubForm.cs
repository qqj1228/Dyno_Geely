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
    public partial class TachometerSelfcheckSubForm : Form {
        private float _lastHeight;
        private readonly DynoCmd _dynoCmd;
        private readonly MainSetting _mainCfg;
        private readonly Dictionary<Form, bool> _dicResults;
        private readonly Dictionary<Form, bool> _dicStops;
        private readonly System.Timers.Timer _timer;
        private const int RPM_Tolerance = 10;
        public event EventHandler<SelfcheckDoneEventArgs> SelfcheckDone;

        public TachometerSelfcheckSubForm(DynoCmd dynoCmd, MainSetting mainCfg, Dictionary<Form, bool> dicResults, Dictionary<Form, bool> dicStops) {
            InitializeComponent();
            _lastHeight = Height;
            _dynoCmd = dynoCmd;
            _mainCfg = mainCfg;
            _dicResults = dicResults;
            _dicStops = dicStops;
            _timer = new System.Timers.Timer(_mainCfg.RealtimeInterval);
            _timer.Elapsed += OnTimer;
            _timer.AutoReset = true;
        }

        private void OnTimer(object source, System.Timers.ElapsedEventArgs e) {
            GetTachometerPrepareRealTimeDataAckParams ackParams = new GetTachometerPrepareRealTimeDataAckParams();
            if (_dynoCmd.GetTachometerPrepareRealTimeDataCmd(true, false, ref ackParams, out string errMsg) && ackParams != null) {
                if (_timer != null && _timer.Enabled) {
                    try {
                        Invoke((EventHandler)delegate {
                            lblGasRPMLow.Text = ackParams.QYRPMLow.ToString();
                            lblGasRPMHigh.Text = ackParams.QYRPMHigt.ToString();
                            lblGasRPM.Text = ackParams.RPM.ToString();
                            lblDieselRPMLow.Text = ackParams.CYRPMLow.ToString();
                            lblDieselRPMHigh.Text = ackParams.CYRPMHigt.ToString();
                            lblDieselRPM.Text = ackParams.CYRPM.ToString();
                            lblOBDRPM.Text = ackParams.OBDRPM.ToString();
                            bool RPMOK = (ackParams.RPM >= ackParams.QYRPMLow) && (ackParams.RPM <= ackParams.QYRPMHigt);
                            RPMOK = RPMOK || ((ackParams.CYRPM >= ackParams.CYRPMLow) && (ackParams.CYRPM <= ackParams.CYRPMHigt));
                            bool OBDOK = Math.Abs(ackParams.OBDRPM - ackParams.RPM) <= RPM_Tolerance;
                            OBDOK = OBDOK || Math.Abs(ackParams.OBDRPM - ackParams.CYRPM) <= RPM_Tolerance;
                            OBDOK = OBDOK && ackParams.OBDRPM > 0;
                            if ((RPMOK || OBDOK) || _dicStops[this]) {
                                _timer.Enabled = false;
                                _dicResults[this] = true;
                                ackParams = new GetTachometerPrepareRealTimeDataAckParams();
                                _dynoCmd.GetTachometerPrepareRealTimeDataCmd(false, true, ref ackParams, out errMsg);
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
            GetTachometerPrepareRealTimeDataAckParams ackParams = new GetTachometerPrepareRealTimeDataAckParams();
            if (bStart) {
                if (!_dynoCmd.GetTachometerPrepareRealTimeDataCmd(true, false, ref ackParams, out string errMsg)) {
                    MessageBox.Show("执行开始获取转速计实时数据命令失败", "执行命令出错", MessageBoxButtons.OK, MessageBoxIcon.Error);
                } else {
                    _timer.Enabled = true;
                }
            } else {
                if (!_dynoCmd.GetTachometerPrepareRealTimeDataCmd(false, true, ref ackParams, out string errMsg)) {
                    MessageBox.Show("执行停止获取转速计实时数据命令失败", "执行命令出错", MessageBoxButtons.OK, MessageBoxIcon.Error);
                } else {
                    _timer.Enabled = false;
                    lblMsg.Text = "已手动停止转速计自检";
                }
            }
        }

        private void TachometerSelfcheckSubForm_Load(object sender, EventArgs e) {
            lblMsg.Text = "转速计自检";
        }

        private void TachometerSelfcheckSubForm_FormClosing(object sender, FormClosingEventArgs e) {
            _timer.Elapsed -= OnTimer;
            _timer.Enabled = false;
        }

        private void TachometerSelfcheckSubForm_Resize(object sender, EventArgs e) {
            if (_lastHeight == 0) {
                return;
            }
            float scale = Height / _lastHeight;
            layoutMain.Font = new Font(layoutMain.Font.FontFamily, layoutMain.Font.Size * scale, layoutMain.Font.Style);
            lblMsg.Font = new Font(lblMsg.Font.FontFamily, lblMsg.Font.Size * scale, lblMsg.Font.Style);
            _lastHeight = Height;
        }

        private void BtnStart_Click(object sender, EventArgs e) {
            lblGasRPM.Text = "--";
            StartSelfcheck(true);
        }

        private void BtnStop_Click(object sender, EventArgs e) {
            _dynoCmd.ReconnectServer();
            StartSelfcheck(false);
        }
    }
}
