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
    public partial class VMASForm : Form {
        private float _lastHeight;
        private readonly string _VIN;
        private readonly DynoCmd _dynoCmd;
        private readonly MainSetting _mainCfg;
        private readonly ModelLocal _db;
        private readonly EnvironmentData _envData;
        private readonly Logger _log;
        private readonly System.Timers.Timer _timer;
        private readonly DataTable _dtRealTime;
        private readonly DataTable _dtSpeeds;
        private readonly VMASResultData _resultData;
        private readonly VMASResultForm f_result;
        private DateTime _startTime;
        private double _speedRange;
        private double _speedOverTime;

        public VMASForm(string VIN, DynoCmd dynoCmd, MainSetting mainCfg, ModelLocal db, EnvironmentData envData, Logger log) {
            InitializeComponent();
            _lastHeight = Height;
            _VIN = VIN;
            _dynoCmd = dynoCmd;
            _mainCfg = mainCfg;
            _db = db;
            _envData = envData;
            _log = log;
            _speedRange = 3;

            _dtRealTime = new DataTable("VMASRealTime");
            _dtRealTime.Columns.Add("VIN");
            _dtRealTime.Columns.Add("StartTime");
            _dtRealTime.Columns.Add("TimeSN");
            _dtRealTime.Columns.Add("Speed");
            _dtRealTime.Columns.Add("RPM");
            _dtRealTime.Columns.Add("SpeedOverTime");
            _dtRealTime.Columns.Add("Power");
            _dtRealTime.Columns.Add("HC");
            _dtRealTime.Columns.Add("NO");
            _dtRealTime.Columns.Add("CO");
            _dtRealTime.Columns.Add("CO2");
            _dtRealTime.Columns.Add("O2");
            _dtRealTime.Columns.Add("DilutionO2");
            _dtRealTime.Columns.Add("EnvO2");
            _dtRealTime.Columns.Add("DilutionRatio");
            _dtRealTime.Columns.Add("Flow");

            _dtSpeeds = new DataTable();
            _dtSpeeds.Columns.Add(_mainCfg.VMASSpeed.Columns[0].ToString());
            _dtSpeeds.Columns.Add("Speed");
            _dtSpeeds.Columns.Add(_mainCfg.VMASSpeed.Columns[1].ToString());
            _dtSpeeds.Columns.Add(_mainCfg.VMASSpeed.Columns[2].ToString());
            _dtSpeeds.Columns.Add(_mainCfg.VMASSpeed.Columns[3].ToString());

            for (int i = 0; i < _mainCfg.VMASSpeed.Rows.Count; i++) {
                DataRow dr = _dtSpeeds.NewRow();
                if (_mainCfg.VMASSpeed.Rows[i][1].ToString().Length > 0) {
                    dr[0] = _mainCfg.VMASSpeed.Rows[i][0];
                    dr[2] = _mainCfg.VMASSpeed.Rows[i][1];
                    dr[3] = _mainCfg.VMASSpeed.Rows[i][2];
                    dr[4] = _mainCfg.VMASSpeed.Rows[i][3];
                }
                _dtSpeeds.Rows.Add(dr);
            }

            _resultData = new VMASResultData();

            _timer = new System.Timers.Timer(_mainCfg.RealtimeInterval);
            _timer.Elapsed += OnTimer;
            _timer.AutoReset = true;

            f_result = new VMASResultForm();
        }

        private void OnTimer(object source, System.Timers.ElapsedEventArgs e) {
            GetVmasRealTimeDataAckParams ackParams = new GetVmasRealTimeDataAckParams();
            if (_dynoCmd.GetVMASRealTimeDataCmd(_speedOverTime, ref ackParams, out string errMsg) && ackParams != null) {
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
                        dr["Speed"] = ackParams.speed;
                        dr["RPM"] = ackParams.RPM;
                        dr["SpeedOverTime"] = ackParams.speedContinuityOverProofTime;
                        dr["Power"] = ackParams.power;
                        dr["HC"] = ackParams.HC;
                        dr["NO"] = ackParams.NO;
                        dr["CO"] = ackParams.CO;
                        dr["CO2"] = ackParams.CO2;
                        dr["O2"] = ackParams.O2;
                        dr["DilutionO2"] = ackParams.dilutionO2;
                        dr["EnvO2"] = ackParams.environmentalO2;
                        dr["DilutionRatio"] = ackParams.dilutionRatio;
                        dr["Flow"] = ackParams.flow;
                        _dtRealTime.Rows.Add(dr);
                        _speedOverTime = ackParams.speedContinuityOverProofTime;

                        if (_dtRealTime.Rows.Count % (1000 / _mainCfg.RealtimeInterval) == 0) {
                            int index = _dtRealTime.Rows.Count / (1000 / _mainCfg.RealtimeInterval) - 1;
                            _dtSpeeds.Rows[index]["Speed"] = ackParams.speed;
                        }

                        Invoke((EventHandler)delegate {
                            if (ackParams.msg != null && ackParams.msg.Length > 0) {
                                lblMsg.Text = ackParams.msg;
                            }
                            lblRPM.Text = ackParams.RPM.ToString();
                            lblHC.Text = ackParams.HC.ToString("F");
                            lblNO.Text = ackParams.NO.ToString("F");
                            lblCO.Text = ackParams.CO.ToString("F");
                            chart1.DataBind();
                            gaugeSpeed.CircularScales["Scale1"].Pointers["Pointer1"].Value = ackParams.speed;
                            if (gaugeSpeed.GaugeItems["Indicator1"] is NumericIndicator ind) {
                                ind.Value = ackParams.speed;
                            }
                        });
                        // 判断“速度连续超差时间”是否大于2秒
                        if (ackParams.speedContinuityOverProofTime > 2) {
                            GetVmasCheckResultAckParams ackParams2 = new GetVmasCheckResultAckParams();
                            if (_dynoCmd.GetVMASCheckResultDataCmd(ref ackParams2, out errMsg)) {
                                _resultData.HCLimit = ackParams2.HCLimit;
                                _resultData.COLimit = ackParams2.COLimit;
                                _resultData.NOLimit = ackParams2.NOLimit;
                                _resultData.HC = ackParams2.HCTest;
                                _resultData.CO = ackParams2.COTest;
                                _resultData.NO = ackParams2.NOTest;
                                _resultData.HCNO = ackParams2.HCNOTest;
                                _resultData.HCEvl = ackParams2.HCEvl;
                                _resultData.COEvl = ackParams2.COEvl;
                                _resultData.NOEvl = ackParams2.NOEvl;
                                _resultData.Result = ackParams2.VmasCheckeResult;
                            } else {
                                _log.TraceError("GetVMASCheckResultDataCmd() return false");
                                MessageBox.Show("执行获取简易瞬态工况检测结果数据命令失败", "执行命令出错", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                            _timer.Enabled = false;
                            Invoke((EventHandler)delegate {
                                lblMsg.Text = "因“速度连续超差时间”大于2秒而停止简易瞬态工况检测";
                            });
                        }
                        if (ackParams.step == 3) {
                            GetVmasCheckResultAckParams ackParams2 = new GetVmasCheckResultAckParams();
                            if (_dynoCmd.GetVMASCheckResultDataCmd(ref ackParams2, out errMsg)) {
                                _resultData.HCLimit = ackParams2.HCLimit;
                                _resultData.COLimit = ackParams2.COLimit;
                                _resultData.NOLimit = ackParams2.NOLimit;
                                _resultData.HC = ackParams2.HCTest;
                                _resultData.CO = ackParams2.COTest;
                                _resultData.NO = ackParams2.NOTest;
                                _resultData.HCNO = ackParams2.HCNOTest;
                                _resultData.HCEvl = ackParams2.HCEvl;
                                _resultData.COEvl = ackParams2.COEvl;
                                _resultData.NOEvl = ackParams2.NOEvl;
                                _resultData.Result = ackParams2.VmasCheckeResult;
                                if (!f_result.Visible) {
                                    Invoke((EventHandler)delegate {
                                        f_result.ShowResult(_resultData);
                                        f_result.ShowDialog();
                                    });
                                }
                            } else {
                                _log.TraceError("GetVMASCheckResultDataCmd() return false");
                                MessageBox.Show("执行获取简易瞬态工况检测结果数据命令失败", "执行命令出错", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                        if (ackParams.step == 4) {
                            if (f_result.Visible) {
                                Invoke((EventHandler)delegate {
                                    f_result.Close();
                                });
                            }
                            StopCheck();
                            SaveDBData();
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

        public void StartTest(bool bStart, bool bRetry) {
            StartVmasCheckAckParams ackParams = new StartVmasCheckAckParams();
            if (bStart) {
                if (!_dynoCmd.StartVMASCheckCmd(false, bRetry, ref ackParams, out string errMsg)) {
                    MessageBox.Show("执行开始简易瞬态工况检测命令失败", "执行命令出错", MessageBoxButtons.OK, MessageBoxIcon.Error);
                } else {
                    _speedRange = ackParams.VmasCheckSpeedSpan;
                    _speedOverTime = 0;
                    for (int i = 0; i < _dtSpeeds.Rows.Count; i++) {
                        _dtSpeeds.Rows[i][3] = Convert.ToDouble(_dtSpeeds.Rows[i][3]) + (_speedRange - 2);
                        _dtSpeeds.Rows[i][4] = Convert.ToDouble(_dtSpeeds.Rows[i][4]) - (_speedRange - 2);
                    }
                    _dtRealTime.Rows.Clear();
                    _timer.Enabled = true;
                    _startTime = DateTime.Now;
                }
            } else {
                _timer.Enabled = false;
                Thread.Sleep(_mainCfg.RealtimeInterval);
                if (!_dynoCmd.StartVMASCheckCmd(true, bRetry, ref ackParams, out string errMsg) && errMsg != "ati >= 0") {
                    MessageBox.Show("执行停止简易瞬态工况检测命令失败", "执行命令出错", MessageBoxButtons.OK, MessageBoxIcon.Error);
                } else if (errMsg.Length > 0) {
                    if (errMsg == "ati >= 0") {
                        lblMsg.Text = "已停止简易瞬态工况检测";
                    } else if (errMsg != "OK") {
                        lblMsg.Text = errMsg;
                    }
                }
            }
        }

        public void StopCheck() {
            //_dynoCmd.ReconnectServer();
            StartTest(false, false);
        }

        private void SaveDBData() {
            _db.SetTestQTYInVehicleInfo(_VIN);
            _db.InsertRecords(_dtRealTime);
            _db.SaveVMASResult(_VIN, _startTime, Convert.ToDouble(_dtRealTime.Rows[_dtRealTime.Rows.Count - 1]["TimeSN"]), _envData, _resultData);
        }

        private void SetupRectangleAnnotation(RectangleAnnotation ra, string text, double AnchorX, double AnchorY, Color color) {
            ra.AxisX = chart1.ChartAreas[0].AxisX;
            ra.AxisY = chart1.ChartAreas[0].AxisY;
            ra.AnchorAlignment = ContentAlignment.BottomRight;
            ra.AnchorOffsetX = 2;
            ra.AnchorX = AnchorX;
            ra.AnchorY = AnchorY;
            ra.Text = text;
            ra.BackColor = Color.Transparent;
            ra.ForeColor = color;
            ra.LineColor = color;
        }

        private void VMASForm_Load(object sender, EventArgs e) {
            lblMsg.Text = "简易瞬态工况法测试";
            chart1.Series[0].Name = "实测车速(km/h)";
            chart1.Series.Add("标准车速(km/h)");
            chart1.Series.Add("上限车速(km/h)");
            chart1.Series.Add("下限车速(km/h)");
            chart1.Series[0].Color = Color.Black;
            chart1.Series[1].Color = Color.Red;
            chart1.Series[2].Color = Color.DodgerBlue;
            chart1.Series[3].Color = Color.SeaGreen;
            chart1.Series[0].ChartType = SeriesChartType.FastLine;
            chart1.Series[1].ChartType = SeriesChartType.FastLine;
            chart1.Series[2].ChartType = SeriesChartType.FastLine;
            chart1.Series[3].ChartType = SeriesChartType.FastLine;
            chart1.Series[0].BorderWidth = 2;
            chart1.Series[1].BorderWidth = 2;
            chart1.Series[2].BorderWidth = 2;
            chart1.Series[3].BorderWidth = 2;
            chart1.Series[0].XValueMember = _dtSpeeds.Columns[0].ToString();
            chart1.Series[0].YValueMembers = _dtSpeeds.Columns[1].ToString();
            chart1.Series[1].XValueMember = _dtSpeeds.Columns[0].ToString();
            chart1.Series[1].YValueMembers = _dtSpeeds.Columns[2].ToString();
            chart1.Series[2].XValueMember = _dtSpeeds.Columns[0].ToString();
            chart1.Series[2].YValueMembers = _dtSpeeds.Columns[3].ToString();
            chart1.Series[3].XValueMember = _dtSpeeds.Columns[0].ToString();
            chart1.Series[3].YValueMembers = _dtSpeeds.Columns[4].ToString();
            chart1.ChartAreas[0].AxisX.Minimum = 0;
            chart1.ChartAreas[0].AxisX.Maximum = 195;
            chart1.ChartAreas[0].AxisX.Interval = 15;
            chart1.ChartAreas[0].AxisX.IntervalAutoMode = IntervalAutoMode.VariableCount;
            chart1.ChartAreas[0].AxisX.MajorGrid.LineDashStyle = ChartDashStyle.Dash;
            chart1.ChartAreas[0].AxisX.MajorGrid.LineColor = Color.Gray;
            chart1.ChartAreas[0].AxisX.Title = "时间（秒）";
            chart1.ChartAreas[0].AxisY.Minimum = -10;
            chart1.ChartAreas[0].AxisY.Maximum = 60;
            chart1.ChartAreas[0].AxisY.Interval = 10;
            chart1.ChartAreas[0].AxisY.IntervalAutoMode = IntervalAutoMode.FixedCount;
            chart1.ChartAreas[0].AxisY.MajorGrid.LineDashStyle = ChartDashStyle.Dash;
            chart1.ChartAreas[0].AxisY.MajorGrid.LineColor = Color.Gray;
            chart1.ChartAreas[0].AxisY.Title = "车速(km/h)";
            chart1.DataSource = _dtSpeeds;
            chart1.DataBind();
            RectangleAnnotation speed50 = new RectangleAnnotation();
            SetupRectangleAnnotation(speed50, "50km/h", 144, 50, Color.Red);
            RectangleAnnotation speed35 = new RectangleAnnotation();
            SetupRectangleAnnotation(speed35, "35km/h", 164, 35, Color.Red);
            RectangleAnnotation speed32 = new RectangleAnnotation();
            SetupRectangleAnnotation(speed32, "32km/h", 62, 32, Color.Red);
            RectangleAnnotation speed15 = new RectangleAnnotation();
            SetupRectangleAnnotation(speed15, "15km/h", 16, 15, Color.Red);
            RectangleAnnotation shift1 = new RectangleAnnotation();
            SetupRectangleAnnotation(shift1, "一挡换二挡", 55, 15, Color.Red);
            RectangleAnnotation shift2 = new RectangleAnnotation();
            SetupRectangleAnnotation(shift2, "一挡换二挡", 123, 15, Color.Red);
            RectangleAnnotation shift3 = new RectangleAnnotation();
            SetupRectangleAnnotation(shift3, "二挡换三挡", 134, 35, Color.Red);
            RectangleAnnotation shift4 = new RectangleAnnotation();
            SetupRectangleAnnotation(shift4, "三挡换二挡", 177, 35, Color.Red);
            chart1.Annotations.Add(speed50);
            chart1.Annotations.Add(speed35);
            chart1.Annotations.Add(speed32);
            chart1.Annotations.Add(speed15);
            chart1.Annotations.Add(shift1);
            chart1.Annotations.Add(shift2);
            chart1.Annotations.Add(shift3);
            chart1.Annotations.Add(shift4);
        }

        private void VMASForm_FormClosing(object sender, FormClosingEventArgs e) {
            _timer.Elapsed -= OnTimer;
            _timer.Enabled = false;
        }

        private void VMASForm_Shown(object sender, EventArgs e) {
            lblRPM.Text = "--";
            lblHC.Text = "--";
            lblNO.Text = "--";
            lblCO.Text = "--";
            StartTest(true, false);
        }

        private void VMASForm_Resize(object sender, EventArgs e) {
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
            lblRPM.Text = "--";
            lblHC.Text = "--";
            lblNO.Text = "--";
            lblCO.Text = "--";
            StartTest(true, true);
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
