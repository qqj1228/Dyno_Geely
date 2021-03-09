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
    public partial class DynoPreheatingSubForm : Form {
        private float _lastHeight;
        private readonly DynoCmd _dynoCmd;
        private readonly MainSetting _mainCfg;
        private readonly Dictionary<Form, bool> _dicResults;
        private readonly System.Timers.Timer _timer;
        public event EventHandler<PreheatingDoneEventArgs> PreheatingDone;
        private bool _bCanStop;
        private DateTime _startTime;

        public DynoPreheatingSubForm(DynoCmd dynoCmd, MainSetting mainCfg, Dictionary<Form, bool> dicResults) {
            InitializeComponent();
            _lastHeight = Height;
            _dynoCmd = dynoCmd;
            _mainCfg = mainCfg;
            _dicResults = dicResults;
            _timer = new System.Timers.Timer(_mainCfg.RealtimeInterval);
            _timer.Elapsed += OnTimer;
            _timer.AutoReset = true;
            _bCanStop = false;
        }

        private void OnTimer(object source, System.Timers.ElapsedEventArgs e) {
            GetDynoPreheatRealTimeDataAckParams ackParams = new GetDynoPreheatRealTimeDataAckParams();
            if (_dynoCmd.GetDynoPreheatRealTimeDataCmd(ref ackParams, out string errMsg) && ackParams != null) {
                if (_timer != null && _timer.Enabled) {
                    try {
                        Invoke((EventHandler)delegate {
                            if (ackParams.msg != null && ackParams.msg.Length > 0) {
                                lblMsg.Text = ackParams.msg;
                            }
                            lblSpeed.Text = ackParams.speed.ToString("F");
                        });
                    } catch (ObjectDisposedException) {
                        // 关闭窗口后仍有一定几率会进入主UI线程，此时访问界面元素会引发此异常，直接忽略即可
                    }
                    // 指示测功机曾转起来过
                    if (ackParams.speed > 10) {
                        _bCanStop = true;
                    }
                    if (ackParams.dynoPreheat || (ackParams.speed < 0.01 && _bCanStop)) {
                        _timer.Enabled = false;
                        _dicResults[this] = true;
                        try {
                            Invoke((EventHandler)delegate {
                                btnBeamDown.Enabled = false;
                                btnBeamUp.Enabled = true;
                                btnStart.Enabled = true;
                                btnStop.Enabled = false;
                                lblMsg.Text = "测功机预热成功结束";
                                btnBeamUp.PerformClick();
                            });
                        } catch (ObjectDisposedException) {
                            // 关闭窗口后仍有一定几率会进入主UI线程，此时访问界面元素会引发此异常，直接忽略即可
                        }
                        SaveDynoPreheatDataParams cmdParams = new SaveDynoPreheatDataParams {
                            ClientID = _dynoCmd.ClientID,
                            StartTime = _startTime,
                            EndTime = DateTime.Now,
                            Operator = _mainCfg.Name
                        };
                        if (!_dynoCmd.SaveDynoPreheatDataCmd(cmdParams, out errMsg)) {
                            MessageBox.Show("执行保存测功机预热数据命令失败", "执行命令出错", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            //return;
                        }
                        PreheatingDoneEventArgs args = new PreheatingDoneEventArgs {
                            Result = _dicResults[this]
                        };
                        PreheatingDone?.Invoke(this, args);
                    }
                }
            }
        }

        private void DynoPreheatingForm_Load(object sender, EventArgs e) {
            lblMsg.Text = "测功机预热";
            btnBeamDown.Enabled = true;
            btnBeamUp.Enabled = false;
            btnStart.Enabled = false;
            btnStop.Enabled = false;
        }

        private void BtnBeamDown_Click(object sender, EventArgs e) {
            btnBeamDown.Enabled = false;
            btnBeamUp.Enabled = true;
            btnStart.Enabled = true;
            btnStop.Enabled = false;
            if (!_dynoCmd.DynoBeamDownCmd(out string errMsg)) {
                MessageBox.Show("执行举升下降命令失败", "执行命令出错", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnBeamUp_Click(object sender, EventArgs e) {
            btnBeamDown.Enabled = true;
            btnBeamUp.Enabled = false;
            btnStart.Enabled = false;
            btnStop.Enabled = false;
            if (!_dynoCmd.DynoBeamUpCmd(out string errMsg)) {
                MessageBox.Show("执行举升上升命令失败", "执行命令出错", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnStart_Click(object sender, EventArgs e) {
            btnBeamDown.Enabled = false;
            btnBeamUp.Enabled = false;
            btnStart.Enabled = false;
            btnStop.Enabled = true;
            _bCanStop = false;
            _startTime = DateTime.Now;
            if (!_dynoCmd.StartDynoPreheatCmd(false, out string msg)) {
                MessageBox.Show("执行开始测功机预热命令失败", "执行命令出错", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            _timer.Enabled = true;
        }

        private void BtnStop_Click(object sender, EventArgs e) {
            _timer.Enabled = false;
            //_dynoCmd.ReconnectServer();
            System.Threading.Thread.Sleep(_mainCfg.RealtimeInterval);
            btnBeamDown.Enabled = false;
            btnBeamUp.Enabled = true;
            btnStart.Enabled = true;
            btnStop.Enabled = false;
            if (!_dynoCmd.StartDynoPreheatCmd(true, out string errMsg) && errMsg != "ati >= 0") {
                MessageBox.Show("执行停止测功机预热命令失败", "执行命令出错", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (errMsg.Length > 0) {
                if (errMsg == "ati >= 0") {
                    lblMsg.Text = "手动停止测功机预热";
                } else if (errMsg != "OK") {
                    lblMsg.Text = errMsg;
                }
            }
            lblSpeed.Text = "--";
        }

        private void DynoPreheatingSubForm_FormClosing(object sender, FormClosingEventArgs e) {
            _timer.Elapsed -= OnTimer;
            _timer.Enabled = false;
        }

        private void DynoPreheatingSubForm_Resize(object sender, EventArgs e) {
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
