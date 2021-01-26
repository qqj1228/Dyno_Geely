using DevComponents.Instrumentation;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace Dyno_Geely {
    public partial class FALForm : Form {
        private float _lastHeight;
        private readonly string _VIN;
        private readonly DynoCmd _dynoCmd;
        private readonly MainSetting _mainCfg;
        private readonly ModelLocal _db;
        private readonly EnvironmentData _envData;
        private readonly Logger _log;
        private readonly System.Timers.Timer _timer;
        private readonly DataTable _dtRealTime;
        private readonly FALResultData _resultData;
        private readonly FALResultForm f_result;
        private DateTime _startTime;
        private readonly int _RatedRPM;
        private int _MaxRPM;

        public FALForm(string VIN, DynoCmd dynoCmd, MainSetting mainCfg, ModelLocal db, EnvironmentData envData, Logger log) {
            InitializeComponent();
            _lastHeight = this.Height;
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

            _dtRealTime = new DataTable("FALRealTime");
            _dtRealTime.Columns.Add("VIN");
            _dtRealTime.Columns.Add("StartTime");
            _dtRealTime.Columns.Add("TimeSN");
            _dtRealTime.Columns.Add("Step");
            _dtRealTime.Columns.Add("CurrentStageTime");
            _dtRealTime.Columns.Add("RPM");
            _dtRealTime.Columns.Add("K");

            _resultData = new FALResultData();

            _timer = new System.Timers.Timer(_mainCfg.RealtimeInterval);
            _timer.Elapsed += OnTimer;
            _timer.AutoReset = true;

            f_result = new FALResultForm();

        }

        private void OnTimer(object source, System.Timers.ElapsedEventArgs e) {
            GetFalRealTimeDataAckParams ackParams = new GetFalRealTimeDataAckParams();
            if (_dynoCmd.GetFALRealTimeDataCmd(false, ref ackParams) && ackParams != null) {
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
                        dr["Step"] = ackParams.step;
                        dr["CurrentStageTime"] = ackParams.CurrentStageTime;
                        dr["RPM"] = ackParams.RPM;
                        dr["K"] = ackParams.K;
                        _dtRealTime.Rows.Add(dr);

                        Invoke((EventHandler)delegate {
                            if (ackParams.msg != null && ackParams.msg.Length > 0) {
                                lblMsg.Text = ackParams.msg;
                            }
                            lblCurrentStageTime.Text = ackParams.CurrentStageTime.ToString();
                            lblK.Text = ackParams.K.ToString("F");
                            if (ackParams.K1 > 0) {
                                lblK1.Text = ackParams.K1.ToString("F");
                            }
                            if (ackParams.K2 > 0) {
                                lblK2.Text = ackParams.K1.ToString("F");
                            }
                            if (ackParams.K3 > 0) {
                                lblK3.Text = ackParams.K1.ToString("F");
                            }
                            this.chart1.DataBind();
                            gaugeRPM.CircularScales["Scale1"].Pointers["Pointer1"].Value = ackParams.RPM / 1000.0;
                            if (gaugeRPM.GaugeItems["Indicator1"] is NumericIndicator ind) {
                                ind.Value = ackParams.RPM / 1000.0;
                            }
                        });
                        if (_MaxRPM < ackParams.RPM) {
                            _MaxRPM = ackParams.RPM;
                        }
                        if (ackParams.step == 14) {
                            GetFalCheckResultAckParams ackParams2 = new GetFalCheckResultAckParams();
                            if (_dynoCmd.GetFALCheckResultDataCmd(ref ackParams2)) {
                                _resultData.RatedRPM = _RatedRPM;
                                _resultData.MaxRPM = _MaxRPM;
                                _resultData.KLimit = ackParams2.KLimit;
                                _resultData.KAvg = ackParams2.KAvg;
                                _resultData.K1 = ackParams2.K1;
                                _resultData.K2 = ackParams2.K2;
                                _resultData.K3 = ackParams2.K3;
                                _resultData.Result = ackParams2.FalCheckeResult;
                                Invoke((EventHandler)delegate {
                                    lblK1.Text = ackParams2.K1.ToString("F");
                                    lblK2.Text = ackParams2.K2.ToString("F");
                                    lblK3.Text = ackParams2.K3.ToString("F");
                                });
                                if (!f_result.Visible) {
                                    Invoke((EventHandler)delegate {
                                        f_result.ShowResult(_resultData);
                                        f_result.ShowDialog();
                                    });
                                }
                            } else {
                                _log.TraceError("GetFALCheckResultDataCmd() return false");
                                MessageBox.Show("执行获取自由加速不透光检测结果数据命令失败", "执行命令出错", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                        if (ackParams.step == 15) {
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

        public void StartTest(bool bStart) {
            if (bStart) {
                if (!_dynoCmd.StartFALCheckCmd(false)) {
                    MessageBox.Show("执行开始自由加速不透光检测命令失败", "执行命令出错", MessageBoxButtons.OK, MessageBoxIcon.Error);
                } else {
                    _dtRealTime.Rows.Clear();
                    _timer.Enabled = true;
                    _startTime = DateTime.Now;
                }
            } else {
                if (!_dynoCmd.StartFALCheckCmd(true)) {
                    MessageBox.Show("执行停止自由加速不透光检测命令失败", "执行命令出错", MessageBoxButtons.OK, MessageBoxIcon.Error);
                } else {
                    _timer.Enabled = false;
                    lblMsg.Text = "已停止自由加速不透光检测";
                }
            }
        }

        public void StopCheck() {
            _dynoCmd.ReconnectServer();
            GetFalRealTimeDataAckParams ackParams = new GetFalRealTimeDataAckParams();
            if (_dynoCmd.GetFALRealTimeDataCmd(true, ref ackParams)) {
                _timer.Enabled = false;
                Invoke((EventHandler)delegate {
                    StartTest(false);
                });
            } else {
                _log.TraceError("GetFALRealTimeDataCmd() return false");
                MessageBox.Show("执行停止获取自由加速不透光实时数据命令失败", "执行命令出错", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void SaveDBData() {
            _db.SetTestQTYInVehicleInfo(_VIN);
            _db.InsertRecords(_dtRealTime);
            _db.SaveFALResult(_VIN, _startTime, Convert.ToDouble(_dtRealTime.Rows[_dtRealTime.Rows.Count - 1]["TimeSN"]), _envData, _resultData);
        }

        private void FALForm_Load(object sender, EventArgs e) {
            lblMsg.Text = "自由加速不透光法测试";
            gaugeRPM.CircularScales["Scale1"].Sections["Section2"].StartValue = _RatedRPM * 2 / 3000.0;
            gaugeRPM.CircularScales["Scale1"].Sections["Section2"].EndValue = gaugeRPM.CircularScales["Scale1"].MaxValue;
            this.chart1.Series[0].Name = "光吸收系数(K值)(m^-1)";
            this.chart1.Series[0].Color = Color.DodgerBlue;
            this.chart1.Series[0].ChartType = SeriesChartType.FastLine;
            this.chart1.Series[0].BorderWidth = 2;
            this.chart1.Series[0].XValueMember = "TimeSN";
            this.chart1.Series[0].YValueMembers = "K";
            this.chart1.ChartAreas[0].AxisX.IsStartedFromZero = true;
            this.chart1.ChartAreas[0].AxisX.Minimum = 0;
            this.chart1.ChartAreas[0].AxisX.IntervalAutoMode = IntervalAutoMode.VariableCount;
            this.chart1.ChartAreas[0].AxisX.MajorGrid.LineDashStyle = ChartDashStyle.Dash;
            this.chart1.ChartAreas[0].AxisX.MajorGrid.LineColor = Color.Gray;
            this.chart1.ChartAreas[0].AxisX.Title = "时间（秒）";
            this.chart1.ChartAreas[0].AxisY.MajorGrid.LineDashStyle = ChartDashStyle.Dash;
            this.chart1.ChartAreas[0].AxisY.MajorGrid.LineColor = Color.Gray;
            this.chart1.ChartAreas[0].AxisY.Title = "光吸收系数(K值)(m^-1)";
            this.chart1.DataSource = _dtRealTime;
            this.chart1.DataBind();
        }

        private void FALForm_FormClosing(object sender, FormClosingEventArgs e) {
            _timer.Elapsed -= OnTimer;
            _timer.Enabled = false;
        }

        private void FALForm_Shown(object sender, EventArgs e) {
            lblCurrentStageTime.Text = "--";
            lblK.Text = "--";
            lblK1.Text = "--";
            lblK2.Text = "--";
            lblK3.Text = "--";
            StartTest(true);
        }

        private void FALForm_Resize(object sender, EventArgs e) {
            if (_lastHeight == 0) {
                return;
            }
            float scale = this.Height / _lastHeight;
            layoutMain.Font = new Font(layoutMain.Font.FontFamily, layoutMain.Font.Size * scale, layoutMain.Font.Style);
            lblMsg.Font = new Font(lblMsg.Font.FontFamily, lblMsg.Font.Size * scale, lblMsg.Font.Style);
            _lastHeight = this.Height;
        }

        private void BtnRestart_Click(object sender, EventArgs e) {
            StopCheck();
            lblCurrentStageTime.Text = "--";
            lblK.Text = "--";
            lblK1.Text = "--";
            lblK2.Text = "--";
            lblK3.Text = "--";
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
