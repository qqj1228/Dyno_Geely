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
    public partial class SmokerPreheatingSubForm : Form {
        private float _lastHeight;
        private readonly DynoCmd _dynoCmd;
        private readonly MainSetting _mainCfg;
        private readonly Dictionary<Form, bool> _dicResults;
        private readonly System.Timers.Timer _timer;
        public event EventHandler<PreheatingDoneEventArgs> PreheatingDone;
        private double _k50Err, _k50, _k70Err, _k70;
        private DateTime _startTime;
        // 存放检测结果，[0]: 50%; [1]: 70%
        private readonly bool[] _bResults;

        public SmokerPreheatingSubForm(DynoCmd dynoCmd, MainSetting mainCfg, Dictionary<Form, bool> dicResults) {
            InitializeComponent();
            _lastHeight = Height;
            _dynoCmd = dynoCmd;
            _mainCfg = mainCfg;
            _dicResults = dicResults;
            _timer = new System.Timers.Timer(_mainCfg.RealtimeInterval);
            _timer.Elapsed += OnTimer;
            _timer.AutoReset = true;
            _k50Err = 0;
            _k50 = 0;
            _k70Err = 0;
            _k70 = 0;
            _bResults = new bool[] { false, false };
        }

        private void OnTimer(object source, System.Timers.ElapsedEventArgs e) {
            GetSmokerPreheatSelfCheckRealTimeDataAckParams ackParams = new GetSmokerPreheatSelfCheckRealTimeDataAckParams();
            if (_dynoCmd.GetSmokerPreheatSelfCheckRealTimeDataCmd(ref ackParams, out string errMsg) && ackParams != null) {
                if (_timer != null && _timer.Enabled) {
                    try {
                        Invoke((EventHandler)delegate {
                            if (ackParams.msg != null && ackParams.msg.Length > 0) {
                                lblMsg.Text = ackParams.msg;
                            }
                            lblK.Text = ackParams.K.ToString("F");
                        });
                    } catch (ObjectDisposedException) {
                        // 关闭窗口后仍有一定几率会进入主UI线程，此时访问界面元素会引发此异常，直接忽略即可
                    }
                }
            }
        }

        /// <summary>
        /// 计算误差率(%)，K：测量值，STD：滤光片值
        /// </summary>
        /// <param name="K"></param>
        /// <param name="STD"></param>
        /// <returns></returns>
        private double GetRatio(double K, double STD) {
            return Math.Round((K - STD) * 100 / STD, 2);
        }

        private void SmokerPreheatingSubForm_Load(object sender, EventArgs e) {
            lblMsg.Text = "烟度计预热";
            txtBoxErrStd.Text = _mainCfg.Smoker.ErrKStd.ToString("F");
            txtBox50Std.Text = _mainCfg.Smoker.K50Std.ToString("F");
            txtBox70Std.Text = _mainCfg.Smoker.K70Std.ToString("F");
        }

        private void TextBox_KeyPress(object sender, KeyPressEventArgs e) {
            if (e.KeyChar != (char)Keys.Back && !char.IsDigit(e.KeyChar)) {
                if (e.KeyChar == '.') {
                    TextBox textBox = (TextBox)sender;
                    if (textBox.Text.Contains('.')) {
                        e.Handled = true;
                    }
                } else {
                    e.Handled = true;
                }
            }
        }

        private void Btn50Ratio_Click(object sender, EventArgs e) {
            try {
                _k50 = Convert.ToDouble(lblK.Text);
                lbl50K.Text = lblK.Text;
                _k50Err = Math.Round(Math.Abs(_k50 - _mainCfg.Smoker.K50Std), 2);
                lbl50Ratio.Text = _k50Err.ToString();
                if (_mainCfg.Smoker.ErrKStd < _k50Err) {
                    _bResults[0] = false;
                    lbl50Result.Text = "失败";
                } else {
                    _bResults[0] = true;
                    lbl50Result.Text = "合格";
                }
            } catch (Exception ex) {
                lbl50Ratio.Text = "--";
                lbl50Result.Text = "--";
                MessageBox.Show(ex.Message, "50%误差值计算出错", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Btn70Ratio_Click(object sender, EventArgs e) {
            try {
                _k70 = Convert.ToDouble(lblK.Text);
                lbl70K.Text = lblK.Text;
                _k70Err = Math.Round(Math.Abs(_k70 - _mainCfg.Smoker.K70Std), 2);
                lbl70Ratio.Text = _k70Err.ToString();
                if (_mainCfg.Smoker.ErrKStd < _k70Err) {
                    _bResults[1] = false;
                    lbl70Result.Text = "失败";
                } else {
                    _bResults[1] = true;
                    lbl70Result.Text = "合格";
                }
            } catch (Exception ex) {
                lbl70Ratio.Text = "--";
                lbl70Result.Text = "--";
                MessageBox.Show(ex.Message, "70%误差值计算出错", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void TxtBoxErrKStd_Leave(object sender, EventArgs e) {
            if (txtBoxErrStd.Text.Trim().Length > 0) {
                try {
                    _mainCfg.Smoker.ErrKStd = Convert.ToDouble(txtBoxErrStd.Text.Trim());
                } catch (Exception ex) {
                    MessageBox.Show(ex.Message, "标准误差值输入出错", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void TxtBox50Std_Leave(object sender, EventArgs e) {
            if (txtBox50Std.Text.Trim().Length > 0) {
                try {
                    _mainCfg.Smoker.K50Std = Convert.ToDouble(txtBox50Std.Text.Trim());
                } catch (Exception ex) {
                    MessageBox.Show(ex.Message, "50%滤光片k值输入出错", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void TxtBox70Std_Leave(object sender, EventArgs e) {
            if (txtBox70Std.Text.Trim().Length > 0) {
                try {
                    _mainCfg.Smoker.K70Std = Convert.ToDouble(txtBox70Std.Text.Trim());
                } catch (Exception ex) {
                    MessageBox.Show(ex.Message, "70%滤光片k值输入出错", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void SmokerPreheatingSubForm_FormClosing(object sender, FormClosingEventArgs e) {
            _timer.Enabled = false;
            _timer.Elapsed -= OnTimer;
        }

        private void SmokerPreheatingSubForm_Resize(object sender, EventArgs e) {
            if (_lastHeight == 0) {
                return;
            }
            float scale = Height / _lastHeight;
            layoutMain.Font = new Font(layoutMain.Font.FontFamily, layoutMain.Font.Size * scale, layoutMain.Font.Style);
            lblMsg.Font = new Font(lblMsg.Font.FontFamily, lblMsg.Font.Size * scale, lblMsg.Font.Style);
            _lastHeight = Height;
        }

        private void SmokerPreheatingSubForm_VisibleChanged(object sender, EventArgs e) {
            if (Visible) {
                _timer.Enabled = true;
                _startTime = DateTime.Now;
            } else {
                _timer.Enabled = false;
                lblK.Text = "--";
            }
        }

        private void BtnDone_Click(object sender, EventArgs e) {
            _timer.Enabled = false;
            _dicResults[this] = _bResults[0] && _bResults[1];
            lblResult.Text = _dicResults[this] ? "成功" : "失败";
            SaveSmokerPreheatSelfCheckDataParams cmdParams = new SaveSmokerPreheatSelfCheckDataParams {
                ClientID = _dynoCmd.ClientID,
                Smoker70LabelValue = _mainCfg.Smoker.K70Std,
                Smoker70TestValue = _k70,
                Smoker70Ratio = _k70Err,
                Smoker50LabelValue = _mainCfg.Smoker.K50Std,
                Smoker50TestValue = _k50,
                Smoker50Ratio = _k50Err,
                StartTime = _startTime,
                EndTime = DateTime.Now,
                Result = lblResult.Text
            };
            if (!_dynoCmd.SaveSmokerPreheatSelfCheckDataCmd(cmdParams, out string errMsg)) {
                MessageBox.Show("执行保存烟度计预热数据命令失败", "执行命令出错", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            PreheatingDoneEventArgs args = new PreheatingDoneEventArgs {
                Result = _dicResults[this]
            };
            PreheatingDone?.Invoke(this, args);
        }
    }
}
