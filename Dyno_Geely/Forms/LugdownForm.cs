using DevComponents.Instrumentation;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace Dyno_Geely {
    public partial class LugdownForm : Form {
        private float _lastHeight;
        private readonly string _VIN;
        private readonly DynoCmd _dynoCmd;
        private readonly MainSetting _mainCfg;
        private readonly ModelLocal _db;
        private readonly EnvironmentData _envData;
        private readonly Logger _log;
        private readonly System.Timers.Timer _timer;
        private readonly DataTable _dtRealTime;
        private readonly LDResultData _resultData;
        private readonly LDResultForm f_result;
        private DateTime _startTime;
        private readonly int _RatedRPM;
        private int _MaxRPM;

        public LugdownForm(string VIN, DynoCmd dynoCmd, MainSetting mainCfg, ModelLocal db, EnvironmentData envData, Logger log) {
            InitializeComponent();
            _lastHeight = Height;
            _VIN = VIN;
            _dynoCmd = dynoCmd;
            _mainCfg = mainCfg;
            _db = db;
            _envData = envData;
            _log = log;
            EmissionInfo ei = new EmissionInfo();
            _db.GetEmissionInfoFromVIN(_VIN, ei);
            _RatedRPM = ei.RatedRPM;
            _MaxRPM = -1;

            _dtRealTime = new DataTable("LugdownRealTime");
            _dtRealTime.Columns.Add("VIN");
            _dtRealTime.Columns.Add("StartTime");
            _dtRealTime.Columns.Add("TimeSN");
            _dtRealTime.Columns.Add("RPM");
            _dtRealTime.Columns.Add("Speed");
            _dtRealTime.Columns.Add("Power");
            _dtRealTime.Columns.Add("Torque");
            _dtRealTime.Columns.Add("K");
            _dtRealTime.Columns.Add("CO2");
            _dtRealTime.Columns.Add("NOx");

            _resultData = new LDResultData();

            _timer = new System.Timers.Timer(_mainCfg.RealtimeInterval);
            _timer.Elapsed += OnTimer;
            _timer.AutoReset = true;

            f_result = new LDResultForm();
        }

        private void OnTimer(object source, System.Timers.ElapsedEventArgs e) {
            GetLdRealTimeDataAckParams ackParams = new GetLdRealTimeDataAckParams();
            if (_dynoCmd.GetLdRealTimeDataCmd(false, ref ackParams, out string errMsg) && ackParams != null) {
                if (_timer != null && _timer.Enabled) {
                    try {
                        DataRow dr = _dtRealTime.NewRow();
                        TimeSpan interval = DateTime.Now - _startTime;
                        if (interval.TotalSeconds < 0.001) {
                            interval = TimeSpan.Zero;
                        }
                        dr["VIN"] = _VIN;
                        dr["StartTime"] = _startTime;
                        dr["TimeSN"] = interval.TotalSeconds.ToString("F1");
                        dr["RPM"] = ackParams.RPM;
                        dr["Speed"] = ackParams.Speed;
                        dr["Power"] = ackParams.Power;
                        dr["Torque"] = ackParams.RPM == 0 ? 0 : 9550 * ackParams.Power / ackParams.RPM;
                        dr["K"] = ackParams.K;
                        dr["NOx"] = ackParams.NOx;
                        dr["CO2"] = ackParams.CO2;
                        _dtRealTime.Rows.Add(dr);

                        Invoke((EventHandler)delegate {
                            if (ackParams.msg != null && ackParams.msg.Length > 0) {
                                lblMsg.Text = ackParams.msg;
                            }
                            lblK.Text = ackParams.K.ToString("F");
                            lblNOx.Text = ackParams.NOx.ToString("F");
                            lblCO2.Text = ackParams.CO2.ToString("F");
                            chart1.DataBind();
                            gaugeSpeed.CircularScales["Scale1"].Pointers["Pointer1"].Value = ackParams.Speed;
                            if (gaugeSpeed.GaugeItems["Indicator1"] is NumericIndicator ind) {
                                ind.Value = ackParams.Speed;
                            }
                        });
                        if (ackParams.step == 0) {
                            if (_MaxRPM < ackParams.RPM) {
                                _MaxRPM = ackParams.RPM;
                            }
                        }
                        if (ackParams.step == 11) {
                            GetLdCheckResultAckParams ackParams2 = new GetLdCheckResultAckParams();
                            if (_dynoCmd.GetLdCheckResultDataCmd(ref ackParams2, out errMsg)) {
                                _resultData.RatedRPM = _RatedRPM;
                                _resultData.MaxRPM = _MaxRPM;
                                _resultData.VelMaxHP = ackParams2.VelMaxHP;
                                _resultData.RealMaxPowerLimit = ackParams2.RealMaxPowerLimit;
                                _resultData.RealMaxPower = ackParams2.RealMaxPower;
                                _resultData.KLimit = ackParams2.KLimit;
                                _resultData.K100 = ackParams2.K100;
                                _resultData.K80 = ackParams2.K80;
                                _resultData.NOx80Limit = ackParams2.NOx80Limit;
                                _resultData.NOx80 = ackParams2.NOx80;
                                _resultData.Result = ackParams2.LdCheckeResult;
                                if (!f_result.Visible) {
                                    Invoke((EventHandler)delegate {
                                        f_result.ShowResult(_resultData);
                                        f_result.ShowDialog();
                                    });
                                }
                            } else {
                                _log.TraceError("GetLdCheckResultDataCmd() return false");
                                MessageBox.Show("执行获取加载减速检测结果数据命令失败", "执行命令出错", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                        if (ackParams.step == 12) {
                            if (f_result.Visible) {
                                Invoke((EventHandler)delegate {
                                    f_result.Close();
                                });
                            }
                            StopCheck();
                            //SaveDBData();
                            if (_resultData.Result == "合格") {
                                Invoke((EventHandler)delegate {
                                    Close();
                                });
                            }
                        }
                    } catch (ObjectDisposedException) {
                        // 关闭窗口后仍有一定几率会进入主UI线程，此时访问界面元素会引发此异常，直接忽略即可
                    }
                }
            }
        }

        public void StartTest(bool bStart) {
            if (bStart) {
                if (!_dynoCmd.StartLdCheckCmd(false, 1, out string errMsg)) {
                    MessageBox.Show("执行开始加载减速检测命令失败", "执行命令出错", MessageBoxButtons.OK, MessageBoxIcon.Error);
                } else {
                    _dtRealTime.Rows.Clear();
                    _timer.Enabled = true;
                    _startTime = DateTime.Now;
                }
            } else {
                _timer.Enabled = false;
                Thread.Sleep(_mainCfg.RealtimeInterval);
                if (!_dynoCmd.StartLdCheckCmd(true, 1, out string errMsg) && errMsg != "ati >= 0") {
                    MessageBox.Show("执行停止加载减速检测命令失败", "执行命令出错", MessageBoxButtons.OK, MessageBoxIcon.Error);
                } else if (errMsg.Length > 0) {
                    if (errMsg == "ati >= 0") {
                        lblMsg.Text = "已停止加载减速检测";
                    } else if (errMsg != "OK") {
                        lblMsg.Text = errMsg;
                    }
                }
            }
        }

        public void StopCheck() {
            //_dynoCmd.ReconnectServer();
            StartTest(false);
        }

        private void SaveDBData() {
            _db.SetTestQTYInVehicleInfo(_VIN);
            _db.InsertRecords(_dtRealTime);
            _db.SaveLDResult(_VIN, _startTime, Convert.ToDouble(_dtRealTime.Rows[_dtRealTime.Rows.Count - 1]["TimeSN"]), _envData, _resultData);
        }

        private void LugdownForm_Load(object sender, EventArgs e) {
            lblMsg.Text = "加载减速法测试";
            chart1.Series[0].Name = "转速(r/min)";
            chart1.Series.Add("车速(km/h)");
            chart1.Series.Add("功率(kW)");
            chart1.Series[0].Color = Color.DodgerBlue;
            chart1.Series[1].Color = Color.SeaGreen;
            chart1.Series[2].Color = Color.Red;
            chart1.Series[0].ChartType = SeriesChartType.FastLine;
            chart1.Series[1].ChartType = SeriesChartType.FastLine;
            chart1.Series[2].ChartType = SeriesChartType.FastLine;
            chart1.Series[0].BorderWidth = 2;
            chart1.Series[1].BorderWidth = 2;
            chart1.Series[2].BorderWidth = 2;
            chart1.Series[0].XValueMember = "TimeSN";
            chart1.Series[0].YValueMembers = "RPM";
            chart1.Series[0].YAxisType = AxisType.Secondary;
            chart1.Series[1].XValueMember = "TimeSN";
            chart1.Series[1].YValueMembers = "Speed";
            chart1.Series[2].XValueMember = "TimeSN";
            chart1.Series[2].YValueMembers = "Power";
            chart1.ChartAreas[0].AxisX.IsStartedFromZero = true;
            chart1.ChartAreas[0].AxisX.Minimum = 0;
            chart1.ChartAreas[0].AxisX.IntervalAutoMode = IntervalAutoMode.VariableCount;
            chart1.ChartAreas[0].AxisX.MajorGrid.LineDashStyle = ChartDashStyle.Dash;
            chart1.ChartAreas[0].AxisX.MajorGrid.LineColor = Color.Gray;
            chart1.ChartAreas[0].AxisX.Title = "时间（秒）";
            chart1.ChartAreas[0].AxisY.MajorGrid.LineDashStyle = ChartDashStyle.Dash;
            chart1.ChartAreas[0].AxisY.MajorGrid.LineColor = Color.Gray;
            chart1.ChartAreas[0].AxisY.Title = "车速(km/h) / 功率(kW)";
            chart1.ChartAreas[0].AxisY2.Enabled = AxisEnabled.True;
            chart1.ChartAreas[0].AxisY2.MajorGrid.LineDashStyle = ChartDashStyle.Dash;
            chart1.ChartAreas[0].AxisY2.MajorGrid.LineColor = Color.Gray;
            chart1.ChartAreas[0].AxisY2.Title = "转速(r/min)";
            chart1.ChartAreas[0].AxisY2.TitleForeColor = Color.DodgerBlue;
            chart1.DataSource = _dtRealTime;
            chart1.DataBind();
        }

        private void LugdownForm_FormClosing(object sender, FormClosingEventArgs e) {
            _timer.Elapsed -= OnTimer;
            _timer.Enabled = false;
        }

        private void LugdownForm_Shown(object sender, EventArgs e) {
            string strTips = "1、车辆是否预检通过？车辆是否预热通过？" + Environment.NewLine;
            strTips += "2、驱动轮是否干燥清洁？" + Environment.NewLine;
            strTips += "3、系好安全带。" + Environment.NewLine;
            strTips += "4、前驱车辆是否已拉手刹？" + Environment.NewLine;
            strTips += "5、打开冷却风扇，安装安全限位。" + Environment.NewLine;
            strTips += "6、驾驶车辆使车轮与试验台吻合。" + Environment.NewLine;
            strTips += "7、测试全程禁止刹车。" + Environment.NewLine;
            strTips += "8、双后桥驱动的车辆，请连接双后桥差速锁。" + Environment.NewLine;
            strTips += "9、请做好安全措施后进行车辆测试。" + Environment.NewLine;
            TipsForm f_tips = new TipsForm(strTips, 30);
            f_tips.ShowDialog();
            lblK.Text = "--";
            lblNOx.Text = "--";
            lblCO2.Text = "--";
            StartTest(true);
        }

        private void LugdownForm_Resize(object sender, EventArgs e) {
            if (_lastHeight == 0) {
                return;
            }
            float scale = Height / _lastHeight;
            layoutMain.Font = new Font(layoutMain.Font.FontFamily, layoutMain.Font.Size * scale, layoutMain.Font.Style);
            lblMsg.Font = new Font(lblMsg.Font.FontFamily, lblMsg.Font.Size * scale, lblMsg.Font.Style);
            _lastHeight = Height;
        }

        private void BtnRestart_Click(object sender, EventArgs e) {
            StopCheck();
            lblK.Text = "--";
            lblNOx.Text = "--";
            lblCO2.Text = "--";
            StartTest(true);
        }

        private void BtnStop_Click(object sender, EventArgs e) {
            StopCheck();
        }

        private void BtnCancel_Click(object sender, EventArgs e) {
            StopCheck();
            Close();
        }
    }
}
