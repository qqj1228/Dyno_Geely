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
    public partial class WeatherSelfcheckSubForm : Form {
        private float _lastHeight;
        private readonly DynoCmd _dynoCmd;
        private readonly MainSetting _mainCfg;
        private readonly Dictionary<Form, bool> _dicResults;
        private readonly Dictionary<Form, bool> _dicStops;
        private readonly EnvironmentData _envData;
        private readonly System.Timers.Timer _timer;
        private const int OK_COUNTER = 3;
        private int _counter;
        public event EventHandler<SelfcheckDoneEventArgs> SelfcheckDone;

        public WeatherSelfcheckSubForm(DynoCmd dynoCmd, MainSetting mainCfg, Dictionary<Form, bool> dicResults, Dictionary<Form, bool> dicStops, EnvironmentData envData) {
            InitializeComponent();
            _lastHeight = Height;
            _dynoCmd = dynoCmd;
            _mainCfg = mainCfg;
            _dicResults = dicResults;
            _dicStops = dicStops;
            _envData = envData;
            _timer = new System.Timers.Timer(_mainCfg.RealtimeInterval);
            _timer.Elapsed += OnTimer;
            _timer.AutoReset = true;
        }

        private void OnTimer(object source, System.Timers.ElapsedEventArgs e) {
            GetWeatherPrepareRealTimeDataAckParams ackParams = new GetWeatherPrepareRealTimeDataAckParams();
            if (_dynoCmd.GetWeatherPrepareRealTimeDataCmd(true, false, ref ackParams, out string errMsg) && ackParams != null) {
                if (_timer != null && _timer.Enabled) {
                    try {
                        Invoke((EventHandler)delegate {
                            lblTemperature.Text = ackParams.temperature.ToString("F");
                            lblHumidity.Text = ackParams.humidity.ToString("F");
                            lblPressure.Text = ackParams.amibientPressure.ToString("F");
                            if ((/*ackParams.temperature > 0 && */ackParams.humidity > 0 && ackParams.amibientPressure > 0) || _dicStops[this]) {
                                if (++_counter >= OK_COUNTER || _dicStops[this]) {
                                    _envData.Temperature = ackParams.temperature;
                                    _envData.Humidity = ackParams.humidity;
                                    _envData.Pressure = ackParams.amibientPressure;
                                    _timer.Enabled = false;
                                    _dicResults[this] = true;
                                    ackParams = new GetWeatherPrepareRealTimeDataAckParams();
                                    _dynoCmd.GetWeatherPrepareRealTimeDataCmd(false, true, ref ackParams, out errMsg);
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
            GetWeatherPrepareRealTimeDataAckParams ackParams = new GetWeatherPrepareRealTimeDataAckParams();
            if (bStart) {
                if (!_dynoCmd.GetWeatherPrepareRealTimeDataCmd(true, false, ref ackParams, out string errMsg)) {
                    MessageBox.Show("执行开始获取气象站实时数据命令失败", "执行命令出错", MessageBoxButtons.OK, MessageBoxIcon.Error);
                } else {
                    _timer.Enabled = true;
                    _counter = 0;
                }
            } else {
                if (!_dynoCmd.GetWeatherPrepareRealTimeDataCmd(false, true, ref ackParams, out string errMsg)) {
                    MessageBox.Show("执行停止获取气象站实时数据命令失败", "执行命令出错", MessageBoxButtons.OK, MessageBoxIcon.Error);
                } else {
                    _timer.Enabled = false;
                    lblMsg.Text = "已手动停止气象站自检";
                }
            }
        }

        private void WeatherSelfcheckSubForm_Load(object sender, EventArgs e) {
            lblMsg.Text = "气象站自检";
        }

        private void WeatherSelfcheckSubForm_FormClosing(object sender, FormClosingEventArgs e) {
            _timer.Elapsed -= OnTimer;
            _timer.Enabled = false;
        }

        private void WeatherSelfcheckSubForm_Resize(object sender, EventArgs e) {
            if (_lastHeight == 0) {
                return;
            }
            float scale = Height / _lastHeight;
            layoutMain.Font = new Font(layoutMain.Font.FontFamily, layoutMain.Font.Size * scale, layoutMain.Font.Style);
            lblMsg.Font = new Font(lblMsg.Font.FontFamily, lblMsg.Font.Size * scale, lblMsg.Font.Style);
            _lastHeight = Height;
        }

        private void BtnStart_Click(object sender, EventArgs e) {
            lblTemperature.Text = "--";
            lblHumidity.Text = "--";
            lblPressure.Text = "--";
            StartSelfcheck(true);
        }

        private void BtnStop_Click(object sender, EventArgs e) {
            _dynoCmd.ReconnectServer();
            StartSelfcheck(false);
        }
    }
}
