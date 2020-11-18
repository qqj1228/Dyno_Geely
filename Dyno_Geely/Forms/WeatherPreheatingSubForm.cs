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
        private double _tempe, _humidity, _pressure;
        // 存放检测结果，[0]: 温度; [1]: 湿度; [2]: 气压
        private readonly bool[] _bResults;
        private bool _bCommResult; // 通讯结果

        public WeatherPreheatingSubForm(DynoCmd dynoCmd, MainSetting mainCfg, Dictionary<Form, bool> dicResults) {
            InitializeComponent();
            _lastHeight = this.Height;
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
            if (_dynoCmd.GetWeatherCheckRealTimeDataCmd(ref ackParams) && ackParams != null) {
                if (_timer != null && _timer.Enabled) {
                    try {
                        Invoke((EventHandler)delegate {
                            if (ackParams.msg != null && ackParams.msg.Length > 0) {
                                lblMsg.Text = ackParams.msg;
                            }
                            lblTempe.Text = ackParams.EnvTemperature.ToString("F");
                            lblHumidity.Text = ackParams.EnvHumidity.ToString("F");
                            lblPressure.Text = ackParams.EnvPressure.ToString("F");
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
            try {
                _tempe = Convert.ToDouble(lblTempe.Text);
                double errTempe = Math.Round(Math.Abs(_tempe - _mainCfg.Weather.EnvTempe), 2);
                lblErrTempe.Text = errTempe.ToString();
                if (_mainCfg.Weather.ErrTempeStd < errTempe) {
                    _bResults[0] = false;
                    lblTempeResult.Text = "失败";
                } else {
                    _bResults[0] = true;
                    lblTempeResult.Text = "合格";
                }
            } catch (Exception ex) {
                lblErrTempe.Text = "--";
                lblTempeResult.Text = "--";
                MessageBox.Show(ex.Message, "温度误差值计算出错", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnHumidity_Click(object sender, EventArgs e) {
            try {
                _humidity = Convert.ToDouble(lblHumidity.Text);
                double errHumidity = Math.Round(Math.Abs(_humidity - _mainCfg.Weather.EnvHumidity), 2);
                lblErrHumidity.Text = errHumidity.ToString();
                if (_mainCfg.Weather.ErrHumidityStd < errHumidity) {
                    _bResults[1] = false;
                    lblHumidityResult.Text = "失败";
                } else {
                    _bResults[1] = true;
                    lblHumidityResult.Text = "合格";
                }
            } catch (Exception ex) {
                lblErrHumidity.Text = "--";
                lblHumidityResult.Text = "--";
                MessageBox.Show(ex.Message, "湿度误差值计算出错", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnPressure_Click(object sender, EventArgs e) {
            try {
                _pressure = Convert.ToDouble(lblPressure.Text);
                double errPressure = Math.Round(Math.Abs(_pressure - _mainCfg.Weather.EnvPressure), 2);
                lblErrPressure.Text = errPressure.ToString();
                if (_mainCfg.Weather.ErrPressureStd < errPressure) {
                    _bResults[2] = false;
                    lblPressureResult.Text = "失败";
                } else {
                    _bResults[2] = true;
                    lblPressureResult.Text = "合格";
                }
            } catch (Exception ex) {
                lblErrPressure.Text = "--";
                lblPressureResult.Text = "--";
                MessageBox.Show(ex.Message, "气压误差值计算出错", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void TxtBoxErrTempeStd_Leave(object sender, EventArgs e) {
            try {
                _mainCfg.Weather.ErrTempeStd = Convert.ToDouble(txtBoxErrTempeStd.Text);
            } catch (Exception ex) {
                MessageBox.Show(ex.Message, "标准温度误差值输入出错", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void TxtBoxErrHumidityStd_Leave(object sender, EventArgs e) {
            try {
                _mainCfg.Weather.ErrHumidityStd = Convert.ToDouble(txtBoxErrHumidityStd.Text);
            } catch (Exception ex) {
                MessageBox.Show(ex.Message, "标准湿度误差值输入出错", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void TxtBoxErrPressureStd_Leave(object sender, EventArgs e) {
            try {
                _mainCfg.Weather.ErrPressureStd = Convert.ToDouble(txtBoxErrPressureStd.Text);
            } catch (Exception ex) {
                MessageBox.Show(ex.Message, "标准气压误差值输入出错", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void TxtBoxEnvTempe_Leave(object sender, EventArgs e) {
            try {
                _mainCfg.Weather.EnvTempe = Convert.ToDouble(txtBoxEnvTempe.Text);
            } catch (Exception ex) {
                MessageBox.Show(ex.Message, "环境温度输入出错", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void TxtBoxEnvHumidity_Leave(object sender, EventArgs e) {
            try {
                _mainCfg.Weather.EnvHumidity = Convert.ToDouble(txtBoxEnvHumidity.Text);
            } catch (Exception ex) {
                MessageBox.Show(ex.Message, "环境湿度输入出错", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void TxtBoxEnvPressure_Leave(object sender, EventArgs e) {
            try {
                _mainCfg.Weather.EnvPressure = Convert.ToDouble(txtBoxEnvPressure.Text);
            } catch (Exception ex) {
                MessageBox.Show(ex.Message, "环境气压入出错", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void WeatherPreheatingSubForm_VisibleChanged(object sender, EventArgs e) {
            if (this.Visible) {
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
            float scale = this.Height / _lastHeight;
            layoutMain.Font = new Font(layoutMain.Font.FontFamily, layoutMain.Font.Size * scale, layoutMain.Font.Style);
            lblMsg.Font = new Font(lblMsg.Font.FontFamily, lblMsg.Font.Size * scale, lblMsg.Font.Style);
            _lastHeight = this.Height;
        }

        private void WeatherPreheatingSubForm_FormClosing(object sender, FormClosingEventArgs e) {
            _timer.Enabled = false;
            _timer.Elapsed -= OnTimer;
            _bCommResult = true;
        }

        private void BtnDone_Click(object sender, EventArgs e) {
            _timer.Enabled = false;
            _dicResults[this] = _bResults[0] && _bResults[1] && _bResults[2];
            lblResult.Text = _dicResults[this] ? "成功" : "失败";
            SaveWeatherCheckDataParams cmdParams = new SaveWeatherCheckDataParams {
                ClientID = _dynoCmd.ClientID,
                StartTime = _startTime,
                EndTime = DateTime.Now,
                CommCheck = _bCommResult ? "成功" : "失败",
                EnvTemperature = _mainCfg.Weather.EnvTempe,
                InstrumentTemperature = _tempe,
                EnvHumidity = _mainCfg.Weather.EnvHumidity,
                InstrumentHumidity = _humidity,
                EnvPressure = _mainCfg.Weather.EnvPressure,
                InstrumentPressure = _pressure,
                Result = lblResult.Text
            };
            if (!_dynoCmd.SaveWeatherCheckDataCmd(cmdParams)) {
                MessageBox.Show("执行保存气象站预热数据命令失败", "执行命令出错", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            PreheatingDoneEventArgs args = new PreheatingDoneEventArgs {
                Result = _dicResults[this]
            };
            PreheatingDone?.Invoke(this, args);
        }
    }
}
