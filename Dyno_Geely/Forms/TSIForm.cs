﻿using DevComponents.Instrumentation;
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
    public partial class TSIForm : Form {
        private float _lastHeight;
        private readonly string _VIN;
        private readonly DynoCmd _dynoCmd;
        private readonly MainSetting _mainCfg;
        private readonly ModelLocal _db;
        private readonly EnvironmentData _envData;
        private readonly Logger _log;
        private readonly System.Timers.Timer _timer;
        private readonly DataTable _dtRealTime;
        private readonly TSIResultData _resultData;
        private readonly TSIResultForm f_result;
        private DateTime _startTime;
        private readonly int _RatedRPM;

        public TSIForm(string VIN, DynoCmd dynoCmd, MainSetting mainCfg, ModelLocal db, EnvironmentData envData, Logger log) {
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

            _dtRealTime = new DataTable("TSIRealTime");
            _dtRealTime.Columns.Add("VIN");
            _dtRealTime.Columns.Add("StartTime");
            _dtRealTime.Columns.Add("TimeSN");
            _dtRealTime.Columns.Add("Step");
            _dtRealTime.Columns.Add("RPM");
            _dtRealTime.Columns.Add("CurrentStageTime");
            _dtRealTime.Columns.Add("Lambda");
            _dtRealTime.Columns.Add("OilTemp");
            _dtRealTime.Columns.Add("HResult");
            _dtRealTime.Columns.Add("LResult");

            _resultData = new TSIResultData();

            _timer = new System.Timers.Timer(_mainCfg.RealtimeInterval);
            _timer.Elapsed += OnTimer;
            _timer.AutoReset = true;

            f_result = new TSIResultForm();

        }

        private void OnTimer(object source, System.Timers.ElapsedEventArgs e) {
            GetTsiRealTimeDataParams cmdParams = new GetTsiRealTimeDataParams {
                ClientID = _dynoCmd.ClientID,
                ignoreRpm70 = false,
                ignoreRpmHigh = false,
                ignoreRpmLow = false,
                ignoreOil = false,
            };
            GetTsiRealTimeDataAckParams ackParams = new GetTsiRealTimeDataAckParams();
            if (_dynoCmd.GetTSIRealTimeDataCmd(cmdParams, ref ackParams, out string errMsg) && ackParams != null) {
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
                        dr["RPM"] = ackParams.RPM;
                        dr["CurrentStageTime"] = ackParams.CurrentStageTime;
                        dr["Lambda"] = ackParams.lmd;
                        dr["OilTemp"] = ackParams.oilTemp;
                        dr["HResult"] = ackParams.Hresult == "合格" ? 1 : 0;
                        dr["LResult"] = ackParams.Lresult == "合格" ? 1 : 0;
                        _dtRealTime.Rows.Add(dr);

                        Invoke((EventHandler)delegate {
                            if (ackParams.msg != null && ackParams.msg.Length > 0) {
                                lblMsg.Text = ackParams.msg;
                            }
                            lblCurrentStageTime.Text = ackParams.CurrentStageTime.ToString();
                            lblLambda.Text = ackParams.lmd.ToString("F3");
                            lblOilTemp.Text = ackParams.oilTemp.ToString("F");
                            chart1.DataBind();
                            gaugeRPM.CircularScales["Scale1"].Pointers["Pointer1"].Value = ackParams.RPM / 1000.0;
                            if (gaugeRPM.GaugeItems["Indicator1"] is NumericIndicator ind) {
                                ind.Value = ackParams.RPM / 1000.0;
                            }
                        });
                        if (ackParams.step == 8 && !f_result.Visible) {
                            GetTsiCheckResultAckParams ackParams2 = new GetTsiCheckResultAckParams();
                            if (_dynoCmd.GetTSICheckResultDataCmd(ref ackParams2, out errMsg)) {
                                _resultData.HighCOLimit = ackParams2.HighCOLimit;
                                _resultData.HighHCLimit = ackParams2.HighHCLimit;
                                _resultData.HighCO = ackParams2.HighCO;
                                _resultData.HighHC = ackParams2.HighHC;
                                _resultData.HighIdleResult = ackParams2.HighIdeResult;
                                _resultData.LowCOLimit = ackParams2.LowCOLimit;
                                _resultData.LowHCLimit = ackParams2.LowHCLimit;
                                _resultData.LowCO = ackParams2.LowCO;
                                _resultData.LowHC = ackParams2.LowHC;
                                _resultData.LowIdleResult = ackParams2.LowIdeResult;
                                _resultData.LambdaLimit = ackParams2.LumdaLimit;
                                _resultData.Lambda = ackParams2.Lumda;
                                _resultData.LambdaResult = ackParams2.LumdaResult;
                                _resultData.Result = ackParams2.TsiCheckeResult;
                                Invoke((EventHandler)delegate {
                                    f_result.ShowResult(_resultData);
                                    f_result.ShowDialog();
                                });
                            } else {
                                _log.TraceError("GetTSICheckResultDataCmd() return false");
                                MessageBox.Show("执行获取双怠速检测结果数据命令失败", "执行命令出错", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                        if (ackParams.step == 9) {
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

        public void StartTest(bool bStart, bool bRetry) {
            if (bStart) {
                if (!_dynoCmd.StartTSICheckCmd(false, bRetry, out string errMsg)) {
                    MessageBox.Show("执行开始双怠速检测命令失败", "执行命令出错", MessageBoxButtons.OK, MessageBoxIcon.Error);
                } else {
                    _dtRealTime.Rows.Clear();
                    _timer.Enabled = true;
                    _startTime = DateTime.Now;
                }
            } else {
                _timer.Enabled = false;
                Thread.Sleep(_mainCfg.RealtimeInterval);
                if (!_dynoCmd.StartTSICheckCmd(true, bRetry, out string errMsg) && errMsg != "ati >= 0") {
                    MessageBox.Show("执行停止双怠速检测命令失败", "执行命令出错", MessageBoxButtons.OK, MessageBoxIcon.Error);
                } else if (errMsg.Length > 0) {
                    if (errMsg == "ati >= 0") {
                        lblMsg.Text = "已停止双怠速检测";
                    } else if (errMsg != "OK") {
                        lblMsg.Text = errMsg;
                    }
                }
            }
        }

        public void StopCheck() {
            StartTest(false, false);
        }

        private void SaveDBData() {
            _db.SetTestQTYInVehicleInfo(_VIN);
            _db.InsertRecords(_dtRealTime);
            _db.SaveTSIResult(_VIN, _startTime, Convert.ToDouble(_dtRealTime.Rows[_dtRealTime.Rows.Count - 1]["TimeSN"]), _envData, _resultData);
        }

        private void TSIForm_Load(object sender, EventArgs e) {
            lblMsg.Text = "双怠速法测试";
            gaugeRPM.CircularScales["Scale1"].Sections["Section2"].StartValue = _RatedRPM * 0.7;
            gaugeRPM.CircularScales["Scale1"].Sections["Section2"].EndValue = gaugeRPM.CircularScales["Scale1"].MaxValue;
            chart1.Series[0].Name = "过量空气系数λ";
            chart1.Series[0].Color = Color.DodgerBlue;
            chart1.Series[0].ChartType = SeriesChartType.FastLine;
            chart1.Series[0].BorderWidth = 2;
            chart1.Series[0].XValueMember = "TimeSN";
            chart1.Series[0].YValueMembers = "Lambda";
            chart1.ChartAreas[0].AxisX.IsStartedFromZero = true;
            chart1.ChartAreas[0].AxisX.Minimum = 0;
            chart1.ChartAreas[0].AxisX.IntervalAutoMode = IntervalAutoMode.VariableCount;
            chart1.ChartAreas[0].AxisX.MajorGrid.LineDashStyle = ChartDashStyle.Dash;
            chart1.ChartAreas[0].AxisX.MajorGrid.LineColor = Color.Gray;
            chart1.ChartAreas[0].AxisX.Title = "时间（秒）";
            chart1.ChartAreas[0].AxisY.MajorGrid.LineDashStyle = ChartDashStyle.Dash;
            chart1.ChartAreas[0].AxisY.MajorGrid.LineColor = Color.Gray;
            chart1.ChartAreas[0].AxisY.Title = "过量空气系数λ";
            chart1.DataSource = _dtRealTime;
            chart1.DataBind();
        }

        private void TSIForm_FormClosing(object sender, FormClosingEventArgs e) {
            _timer.Elapsed -= OnTimer;
            _timer.Enabled = false;
        }

        private void TSIForm_Shown(object sender, EventArgs e) {
            lblCurrentStageTime.Text = "--";
            lblLambda.Text = "--";
            lblOilTemp.Text = "--";
            StartTest(true, false);
        }

        private void TSIForm_Resize(object sender, EventArgs e) {
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
            lblCurrentStageTime.Text = "--";
            lblLambda.Text = "--";
            lblOilTemp.Text = "--";
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
