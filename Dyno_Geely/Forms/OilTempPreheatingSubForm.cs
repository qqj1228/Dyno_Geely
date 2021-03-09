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
    public partial class OilTempPreheatingSubForm : Form {
        private float _lastHeight;
        private readonly DynoCmd _dynoCmd;
        private readonly MainSetting _mainCfg;
        private readonly Dictionary<Form, bool> _dicResults;
        private readonly System.Timers.Timer _timer;
        public event EventHandler<PreheatingDoneEventArgs> PreheatingDone;

        public OilTempPreheatingSubForm(DynoCmd dynoCmd, MainSetting mainCfg, Dictionary<Form, bool> dicResults) {
            InitializeComponent();
            _lastHeight = Height;
            _dynoCmd = dynoCmd;
            _mainCfg = mainCfg;
            _dicResults = dicResults;
            _timer = new System.Timers.Timer(_mainCfg.RealtimeInterval);
            _timer.Elapsed += OnTimer;
            _timer.AutoReset = true;
        }

        private void OnTimer(object source, System.Timers.ElapsedEventArgs e) {
            GetOilThermometerPreheatSelfCheckRealTimeDataAckParams ackParams = new GetOilThermometerPreheatSelfCheckRealTimeDataAckParams();
            if (_dynoCmd.GetOilThermometerPreheatSelfCheckRealTimeDataCmd(ref ackParams, out string errMsg) && ackParams != null) {
                if (_timer != null && _timer.Enabled) {
                    try {
                        Invoke((EventHandler)delegate {
                            if (ackParams.msg != null && ackParams.msg.Length > 0) {
                                lblMsg.Text = ackParams.msg;
                            }
                            lblGasTemp.Text = ackParams.OilTemperature.ToString("F");
                            lblDieselTemp.Text = ackParams.CYOilTemperature.ToString("F");
                            double temp = Math.Max(ackParams.OilTemperature, ackParams.CYOilTemperature);
                            double errAbs = Math.Round(Math.Abs(temp - _mainCfg.OilTemp.TempStd), 2);
                            lblErrAbs.Text = errAbs.ToString();
                            double errRel = Math.Round(errAbs * 100 / _mainCfg.OilTemp.TempStd, 2);
                            lblErrRel.Text = errRel.ToString();
                        });
                    } catch (ObjectDisposedException) {
                        // 关闭窗口后仍有一定几率会进入主UI线程，此时访问界面元素会引发此异常，直接忽略即可
                    }
                }
            }
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

        private void TxtBox_TextChanged(object sender, EventArgs e) {
            if (sender is TextBox txtBox) {
                double value;
                try {
                    value = Convert.ToDouble(txtBox.Text);
                } catch (Exception) {
                    value = 0;
                }
                switch (txtBox.Name) {
                case "txtBoxTempStd":
                    _mainCfg.OilTemp.TempStd = value;
                    break;
                case "txtBoxErrStd":
                    _mainCfg.OilTemp.ErrStd = value;
                    break;
                }
            }
        }

        private void OilTempPreheatingSubForm_Resize(object sender, EventArgs e) {
            if (_lastHeight == 0) {
                return;
            }
            float scale = Height / _lastHeight;
            layoutMain.Font = new Font(layoutMain.Font.FontFamily, layoutMain.Font.Size * scale, layoutMain.Font.Style);
            lblMsg.Font = new Font(lblMsg.Font.FontFamily, lblMsg.Font.Size * scale, lblMsg.Font.Style);
            _lastHeight = Height;
        }

        private void OilTempPreheatingSubForm_VisibleChanged(object sender, EventArgs e) {
            if (Visible) {
                _timer.Enabled = true;
            } else {
                _timer.Enabled = false;
                lblGasTemp.Text = "--";
                lblDieselTemp.Text = "--";
            }
        }

        private void OilTempPreheatingSubForm_Load(object sender, EventArgs e) {
            lblMsg.Text = "油温计预热";
            txtBoxTempStd.Text = _mainCfg.OilTemp.TempStd.ToString();
            txtBoxErrStd.Text = _mainCfg.OilTemp.ErrStd.ToString();
        }

        private void OilTempPreheatingSubForm_FormClosing(object sender, FormClosingEventArgs e) {
            _timer.Enabled = false;
            _timer.Elapsed -= OnTimer;
        }

        private void BtnDone_Click(object sender, EventArgs e) {
            _timer.Enabled = false;
            if (_mainCfg.OilTemp.ErrStd < Convert.ToDouble(lblErrAbs.Text)) {
                _dicResults[this] = false;
                lblResult.Text = "失败";
            } else {
                _dicResults[this] = true;
                lblResult.Text = "成功";
            }
            double gasTemp = Convert.ToDouble(lblGasTemp.Text);
            double dieselTemp = Convert.ToDouble(lblDieselTemp.Text);
            double temp = Math.Max(gasTemp, dieselTemp);
            SaveOilThermometerPreheatSelfCheckParams cmdParams = new SaveOilThermometerPreheatSelfCheckParams {
                ClientID = _dynoCmd.ClientID,
                OilTemperatureData = Math.Max(gasTemp, dieselTemp).ToString(),
                TempDataIn = _mainCfg.OilTemp.TempStd.ToString(),
                AbsError = lblErrAbs.Text,
                ReError = lblErrRel.Text,
                Result = lblResult.Text
            };
            if (!_dynoCmd.SaveOilThermometerPreheatSelfCheckCmd(cmdParams, out string errMsg)) {
                MessageBox.Show("执行保存油温计预热数据命令失败", "执行命令出错", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            PreheatingDoneEventArgs args = new PreheatingDoneEventArgs {
                Result = _dicResults[this]
            };
            PreheatingDone?.Invoke(this, args);
        }

    }
}
