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
    public partial class TachometerPreheatingSubForm : Form {
        private float _lastHeight;
        private readonly DynoCmd _dynoCmd;
        private readonly MainSetting _mainCfg;
        private readonly Dictionary<Form, bool> _dicResults;
        private readonly System.Timers.Timer _timer;
        private const int OK_COUNTER = 3;
        private int _counter;
        public event EventHandler<PreheatingDoneEventArgs> PreheatingDone;
        private DateTime _startTime;
        private bool _bCommResult; // 通讯结果

        public TachometerPreheatingSubForm(DynoCmd dynoCmd, MainSetting mainCfg, Dictionary<Form, bool> dicResults) {
            InitializeComponent();
            _lastHeight = Height;
            _dynoCmd = dynoCmd;
            _mainCfg = mainCfg;
            _dicResults = dicResults;
            _timer = new System.Timers.Timer(_mainCfg.RealtimeInterval);
            _timer.Elapsed += OnTimer;
            _timer.AutoReset = true;
            _bCommResult = true;
        }

        private void OnTimer(object source, System.Timers.ElapsedEventArgs e) {
            GetTachometerCheckRealTimeDataAckParams ackParams = new GetTachometerCheckRealTimeDataAckParams();
            if (_dynoCmd.GetTachometerCheckRealTimeDataCmd(ref ackParams, out string errMsg) && ackParams != null) {
                if (_timer != null && _timer.Enabled) {
                    try {
                        Invoke((EventHandler)delegate {
                            lblGasRPM.Text = ackParams.RPM.ToString();
                            lblDieselRPM.Text = ackParams.CYRPM.ToString();
                        });
                    } catch (ObjectDisposedException) {
                        // 关闭窗口后仍有一定几率会进入主UI线程，此时访问界面元素会引发此异常，直接忽略即可
                    }
                    if (ackParams.RPM > 0 || ackParams.CYRPM > 0) {
                        if (++_counter >= OK_COUNTER) {
                            _timer.Enabled = false;
                            _dicResults[this] = true;
                            try {
                                Invoke((EventHandler)delegate {
                                    lblMsg.Text = "转速计预热成功";
                                    lblGasRPM.Text = "--";
                                    lblDieselRPM.Text = "--";
                                    lblResult.Text = _dicResults[this] ? "成功" : "失败";
                                });
                            } catch (ObjectDisposedException) {
                                // 关闭窗口后仍有一定几率会进入主UI线程，此时访问界面元素会引发此异常，直接忽略即可
                            }
                            SaveTachometerCheckParams cmdParams = new SaveTachometerCheckParams {
                                ClientID = _dynoCmd.ClientID,
                                StartTime = _startTime,
                                EndTime = DateTime.Now,
                                IdleSpeed = ackParams.RPM > ackParams.CYRPM ? ackParams.RPM : ackParams.CYRPM,
                                CommCheck = _bCommResult ? "1" : "2",
                                Result = lblResult.Text
                            };
                            if (!_dynoCmd.SaveTachometerCheckCmd(cmdParams, out errMsg)) {
                                MessageBox.Show("执行保存转速计预热数据命令失败", "执行命令出错", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                            PreheatingDoneEventArgs args = new PreheatingDoneEventArgs {
                                Result = _dicResults[this]
                            };
                            PreheatingDone?.Invoke(this, args);
                        }
                    }
                }
            } else {
                _bCommResult = false;
            }
        }

        private void BtnStart_Click(object sender, EventArgs e) {
            _counter = 0;
            lblMsg.Text = "开始转速计预热";
            StartTachometerCheckAckParams ackParams = new StartTachometerCheckAckParams();
            if (!_dynoCmd.StartTachometerCheckCmd(ref ackParams, out string errMsg)) {
                MessageBox.Show("执行开始转速计预热命令失败", "执行命令出错", MessageBoxButtons.OK, MessageBoxIcon.Error);
            } else {
                _timer.Enabled = true;
                _startTime = DateTime.Now;
                lblGasRPMLow.Text = ackParams.QYRpmLow.ToString();
                lblGasRPMHigh.Text = ackParams.QYRpmHight.ToString();
                lblDieselRPMLow.Text = ackParams.CYRpmLow.ToString();
                lblDieselRPMHigh.Text = ackParams.CYRpmLow.ToString();
            }
        }

        private void BtnStop_Click(object sender, EventArgs e) {
            _timer.Enabled = false;
            lblMsg.Text = "停止转速计预热";
            lblGasRPM.Text = "--";
            lblDieselRPM.Text = "--";
        }

        private void TachometerPreheatingSubForm_Load(object sender, EventArgs e) {
            lblMsg.Text = "转速计预热";
        }

        private void TachometerPreheatingSubForm_FormClosing(object sender, FormClosingEventArgs e) {
            _timer.Elapsed -= OnTimer;
            _timer.Enabled = false;
            _bCommResult = true;
        }

        private void TachometerPreheatingSubForm_Resize(object sender, EventArgs e) {
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
