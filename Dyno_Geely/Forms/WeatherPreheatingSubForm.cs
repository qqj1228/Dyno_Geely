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
    public partial class WeatherPreheatingSubForm : Form {
        private float _lastHeight;
        private readonly DynoCmd _dynoCmd;
        private readonly MainSetting _mainCfg;
        private readonly Dictionary<Form, bool> _dicResults;
        private readonly System.Timers.Timer _timer;
        public event EventHandler<PreheatingDoneEventArgs> PreheatingDone;
        private DateTime _startTime;
        // 存放检测结果，[0]: 温度; [1]: 湿度; [2]: 气压
        private readonly bool[] _bResults;
        private bool _bCommResult; // 通讯结果

        public WeatherPreheatingSubForm(DynoCmd dynoCmd, MainSetting mainCfg, Dictionary<Form, bool> dicResults) {
            InitializeComponent();
            _lastHeight = Height;
            _dynoCmd = dynoCmd;
            _mainCfg = mainCfg;
            _dicResults = dicResults;
            _timer = new System.Timers.Timer(_mainCfg.RealtimeInterval);
            _timer.Elapsed += OnTimer;
            _timer.AutoReset = true;
            _bResults = new bool[] { false, false, false };
            _bCommResult = true;
        }

        private void OnTimer(object source, System.Timers.ElapsedEventArgs e) {
            GetWeatherCheckRealTimeDataAckParams ackParams = new GetWeatherCheckRealTimeDataAckParams();
            if (_dynoCmd.GetWeatherCheckRealTimeDataCmd(ref ackParams, out string errMsg) && ackParams != null) {
                if (_timer != null && _timer.Enabled) {
                    try {
                        Invoke((EventHandler)delegate {
                            if (ackParams.msg != null && ackParams.msg.Length > 0) {
                                lblMsg.Text = ackParams.msg;
                            }
                            lblTempe.Text = ackParams.EnvTemperature.ToString("F");
                            lblHumidity.Text = ackParams.EnvHumidity.ToString("F");
                            lblPressure.Text = ackParams.EnvPressure.ToString("F");
                            double errTempe = Math.Round(Math.Abs(ackParams.EnvTemperature - _mainCfg.Weather.EnvTempe), 2);
                            lblErrTempe.Text = errTempe.ToString();
                            double errHumidity = Math.Round(Math.Abs(ackParams.EnvHumidity - _mainCfg.Weather.EnvHumidity), 2);
                            lblErrHumidity.Text = errHumidity.ToString();
                            double errPressure = Math.Round(Math.Abs(ackParams.EnvPressure - _mainCfg.Weather.EnvPressure), 2);
                            lblErrPressure.Text = errPressure.ToString();
                        });
                    } catch (ObjectDisposedException) {
                        // 关闭窗口后仍有一定几率会进入主UI线程，此时访问界面元素会引发此异常，直接忽略即可
                    }
                }
            } else {
                _bCommResult = false;
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

        private void WeatherPreheatingSubForm_Load(object sender, EventArgs e) {
            lblMsg.Text = "气象站预热";
            txtBoxErrTempeStd.Text = _mainCfg.Weather.ErrTempeStd.ToString();
            txtBoxErrHumidityStd.Text = _mainCfg.Weather.ErrHumidityStd.ToString();
            txtBoxErrPressureStd.Text = _mainCfg.Weather.ErrPressureStd.ToString();
            txtBoxEnvTempe.Text = _mainCfg.Weather.EnvTempe.ToString();
            txtBoxEnvHumidity.Text = _mainCfg.Weather.EnvHumidity.ToString();
            txtBoxEnvPressure.Text = _mainCfg.Weather.EnvPressure.ToString();
        }

        private void BtnTempe_Click(object sender, EventArgs e) {
            if (_mainCfg.Weather.ErrTempeStd < Convert.ToDouble(lblErrTempe.Text)) {
                _bResults[0] = false;
                lblTempeResult.Text = "失败";
            } else {
                _bResults[0] = true;
                lblTempeResult.Text = "合格";
            }
        }

        private void BtnHumidity_Click(object sender, EventArgs e) {
            if (_mainCfg.Weather.ErrHumidityStd < Convert.ToDouble(lblErrHumidity.Text)) {
                _bResults[1] = false;
                lblHumidityResult.Text = "失败";
            } else {
                _bResults[1] = true;
                lblHumidityResult.Text = "合格";
            }
        }

        private void BtnPressure_Click(object sender, EventArgs e) {
            if (_mainCfg.Weather.ErrPressureStd < Convert.ToDouble(lblErrPressure.Text)) {
                _bResults[2] = false;
                lblPressureResult.Text = "失败";
            } else {
                _bResults[2] = true;
                lblPressureResult.Text = "合格";
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
                case "txtBoxErrTempeStd":
                    _mainCfg.Weather.ErrTempeStd = value;
                    break;
                case "txtBoxErrHumidityStd":
                    _mainCfg.Weather.ErrHumidityStd = value;
                    break;
                case "txtBoxErrPressureStd":
                    _mainCfg.Weather.ErrPressureStd = value;
                    break;
                case "txtBoxEnvTempe":
                    _mainCfg.Weather.EnvTempe = value;
                    break;
                case "txtBoxEnvHumidity":
                    _mainCfg.Weather.EnvHumidity = value;
                    break;
                case "txtBoxEnvPressure":
                    _mainCfg.Weather.EnvPressure = value;
                    break;
                }
            }
        }

        private void WeatherPreheatingSubForm_VisibleChanged(object sender, EventArgs e) {
            if (Visible) {
                _timer.Enabled = true;
                _startTime = DateTime.Now;
            } else {
                _timer.Enabled = false;
                lblTempe.Text = "--";
                lblHumidity.Text = "--";
                lblPressure.Text = "--";
            }
        }

        private void WeatherPreheatingSubForm_Resize(object sender, EventArgs e) {
            if (_lastHeight == 0) {
                return;
            }
            float scale = Height / _lastHeight;
            layoutMain.Font = new Font(layoutMain.Font.FontFamily, layoutMain.Font.Size * scale, layoutMain.Font.Style);
            lblMsg.Font = new Font(lblMsg.Font.FontFamily, lblMsg.Font.Size * scale, lblMsg.Font.Style);
            _lastHeight = Height;
        }

        private void WeatherPreheatingSubForm_FormClosing(object sender, FormClosingEventArgs e) {
            _timer.Enabled = false;
            _timer.Elapsed -= OnTimer;
            _bCommResult = true;
        }

        private void BtnDone_Click(object sender, EventArgs e) {
            _timer.Enabled = false;
            _dicResults[this] = _bResults[0] && _bResults[1] && _bResults[2];
            lblResult.Text = _dicResults[this] ? "合格" : "不合格";
            SaveWeatherCheckDataParams cmdParams = new SaveWeatherCheckDataParams {
                ClientID = _dynoCmd.ClientID,
                StartTime = _startTime,
                EndTime = DateTime.Now,
                CommCheck = _bCommResult ? "1" : "2",
                EnvTemperature = _mainCfg.Weather.EnvTempe,
                InstrumentTemperature = Convert.ToDouble(lblTempe.Text),
                EnvHumidity = _mainCfg.Weather.EnvHumidity,
                InstrumentHumidity = Convert.ToDouble(lblHumidity.Text),
                EnvPressure = _mainCfg.Weather.EnvPressure,
                InstrumentPressure = Convert.ToDouble(lblPressure.Text),
                Result = lblResult.Text
            };
            if (!_dynoCmd.SaveWeatherCheckDataCmd(cmdParams, out string errMsg)) {
                MessageBox.Show("执行保存气象站预热数据命令失败", "执行命令出错", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            PreheatingDoneEventArgs args = new PreheatingDoneEventArgs {
                Result = _dicResults[this]
            };
            PreheatingDone?.Invoke(this, args);
        }
    }
}
