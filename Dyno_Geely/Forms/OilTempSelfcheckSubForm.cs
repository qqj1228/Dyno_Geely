﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Dyno_Geely {
    public partial class OilTempSelfcheckSubForm : Form {
        private float _lastHeight;
        private readonly DynoCmd _dynoCmd;
        private readonly MainSetting _mainCfg;
        private readonly Dictionary<Form, bool> _dicResults;
        private readonly Dictionary<Form, bool> _dicStops;
        private readonly System.Timers.Timer _timer;
        private const int OK_COUNTER = 3;
        private int _counter;
        public event EventHandler<SelfcheckDoneEventArgs> SelfcheckDone;

        public OilTempSelfcheckSubForm(DynoCmd dynoCmd, MainSetting mainCfg, Dictionary<Form, bool> dicResults, Dictionary<Form, bool> dicStops) {
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
            GetOilTempPrepareRealTimeDataAckParams ackParams = new GetOilTempPrepareRealTimeDataAckParams();
            if (_dynoCmd.GetOilTempPrepareRealTimeDataCmd(true, false, ref ackParams) && ackParams != null) {
                if (_timer != null && _timer.Enabled) {
                    try {
                        Invoke((EventHandler)delegate {
                            if (ackParams.oilTitle != null && ackParams.oilTitle.Length > 0) {
                                lblMsg.Text = ackParams.oilTitle;
                            }
                            lblOilTemp.Text = ackParams.oilTemp.ToString("F");
                            if (ackParams.oilTemp > 0 || _dicStops[this]) {
                                if (++_counter >= OK_COUNTER || _dicStops[this]) {
                                    _timer.Enabled = false;
                                    _dicResults[this] = true;
                                    ackParams = new GetOilTempPrepareRealTimeDataAckParams();
                                    _dynoCmd.GetOilTempPrepareRealTimeDataCmd(false, true, ref ackParams);
                                    SelfcheckDoneEventArgs args = new SelfcheckDoneEventArgs {
                                        Result = _dicResults[this]
                                    };
                                    SelfcheckDone?.Invoke(this, args);
                                }
                            }
                        });
                    } catch (ObjectDisposedException) {
                        // 关闭窗口后仍有一定几率会进入主UI线程，此时访问界面元素会引发此异常，直接忽略即可
                    }
                }
            }
        }

        public void StartSelfcheck(bool bStart) {
            GetOilTempPrepareRealTimeDataAckParams ackParams = new GetOilTempPrepareRealTimeDataAckParams();
            if (bStart) {
                if (!_dynoCmd.GetOilTempPrepareRealTimeDataCmd(true, false, ref ackParams)) {
                    MessageBox.Show("执行开始获取油温计实时数据命令失败", "执行命令出错", MessageBoxButtons.OK, MessageBoxIcon.Error);
                } else {
                    _timer.Enabled = true;
                    _counter = 0;
                }
            } else {
                if (!_dynoCmd.GetOilTempPrepareRealTimeDataCmd(false, true, ref ackParams)) {
                    MessageBox.Show("执行停止获取油温计实时数据命令失败", "执行命令出错", MessageBoxButtons.OK, MessageBoxIcon.Error);
                } else {
                    _timer.Enabled = false;
                    lblMsg.Text = "已手动停止油温计自检";
                }
            }
        }

        private void OilTempSelfcheckSubForm_Load(object sender, EventArgs e) {
            lblMsg.Text = "油温计自检";
        }

        private void OilTempSelfcheckSubForm_Resize(object sender, EventArgs e) {
            if (_lastHeight == 0) {
                return;
            }
            float scale = this.Height / _lastHeight;
            layoutMain.Font = new Font(layoutMain.Font.FontFamily, layoutMain.Font.Size * scale, layoutMain.Font.Style);
            lblMsg.Font = new Font(lblMsg.Font.FontFamily, lblMsg.Font.Size * scale, lblMsg.Font.Style);
            _lastHeight = this.Height;
        }

        private void OilTempSelfcheckSubForm_FormClosing(object sender, FormClosingEventArgs e) {
            _timer.Elapsed -= OnTimer;
            _timer.Enabled = false;
        }

        private void BtnStart_Click(object sender, EventArgs e) {
            lblOilTemp.Text = "--";
            StartSelfcheck(true);
        }

        private void BtnStop_Click(object sender, EventArgs e) {
            _dynoCmd.ReconnectServer();
            StartSelfcheck(false);
        }
    }
}
