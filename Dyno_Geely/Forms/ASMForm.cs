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

namespace Dyno_Geely {
    public partial class ASMForm : Form {
        private float _lastHeight;
        private readonly string _VIN;
        private readonly DynoCmd _dynoCmd;
        private readonly MainSetting _mainCfg;
        private readonly ModelLocal _db;
        private readonly EnvironmentData _envData;
        private readonly Logger _log;
        private readonly System.Timers.Timer _timer;
        private readonly DataTable _dtRealTime;
        private readonly ASMResultData _resultData;
        private readonly ASMResultForm f_result;
        private DateTime _startTime;

        public ASMForm(string VIN, DynoCmd dynoCmd, MainSetting mainCfg, ModelLocal db, EnvironmentData envData, Logger log) {
            InitializeComponent();
            _lastHeight = Height;
            _VIN = VIN;
            _dynoCmd = dynoCmd;
            _mainCfg = mainCfg;
            _db = db;
            _envData = envData;
            _log = log;

            _dtRealTime = new DataTable("ASMRealTime");
            _dtRealTime.Columns.Add("VIN");
            _dtRealTime.Columns.Add("StartTime");
            _dtRealTime.Columns.Add("TimeSN");
            _dtRealTime.Columns.Add("Step");
            _dtRealTime.Columns.Add("TestTime");
            _dtRealTime.Columns.Add("WorkingTime");
            _dtRealTime.Columns.Add("RPM");
            _dtRealTime.Columns.Add("Speed");
            _dtRealTime.Columns.Add("Power");
            _dtRealTime.Columns.Add("HC");
            _dtRealTime.Columns.Add("CO");
            _dtRealTime.Columns.Add("NO");
            _dtRealTime.Columns.Add("CO2");
            _dtRealTime.Columns.Add("O2");
            _dtRealTime.Columns.Add("lambda");
            _dtRealTime.Columns.Add("KH");
            _dtRealTime.Columns.Add("DF");
            _dtRealTime.Columns.Add("HCNor");
            _dtRealTime.Columns.Add("CONor");
            _dtRealTime.Columns.Add("NONor");

            _resultData = new ASMResultData();

            _timer = new System.Timers.Timer(_mainCfg.RealtimeInterval);
            _timer.Elapsed += OnTimer;
            _timer.AutoReset = true;

            f_result = new ASMResultForm();
        }

        private void OnTimer(object source, System.Timers.ElapsedEventArgs e) {
            GetASMRealTimeDataAckParams ackParams = new GetASMRealTimeDataAckParams();
            if (_dynoCmd.GetASMRealTimeDataCmd(ref ackParams, out string errMsg) && ackParams != null) {
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
                        dr["TestTime"] = ackParams.testTime;
                        dr["WorkingTime"] = ackParams.singleWorkingCondition;
                        dr["RPM"] = ackParams.RPM;
                        dr["Speed"] = ackParams.speed;
                        dr["lambda"] = ackParams.lmd;
                        _dtRealTime.Rows.Add(dr);

                        Invoke((EventHandler)delegate {
                            if (ackParams.msg != null && ackParams.msg.Length > 0) {
                                lblMsg.Text = ackParams.msg;
                            }
                            lblTestTime.Text = ackParams.testTime.ToString("F");
                            lblWorkingTime.Text = ackParams.singleWorkingCondition.ToString("F");
                            lblAccTime.Text = ackParams.accTime.ToString("F");
                            lblStableTime.Text = ackParams.stableTime.ToString("F");
                            gaugeSpeed.CircularScales["Scale1"].Pointers["Pointer1"].Value = ackParams.speed;
                            if (gaugeSpeed.GaugeItems["Indicator1"] is NumericIndicator indSpeed) {
                                indSpeed.Value = ackParams.speed;
                            }
                            if (ackParams.SpeedMax > 0) {
                                gaugeSpeed.CircularScales["Scale1"].MaxValue = ackParams.SpeedMax;
                                gaugeSpeed.CircularScales["Scale1"].MinValue = ackParams.SpeedMin;
                                gaugeSpeed.CircularScales["Scale1"].MajorTickMarks.Interval = (ackParams.SpeedMax - ackParams.SpeedMin) / 5;
                                gaugeSpeed.CircularScales["Scale1"].MinorTickMarks.Interval = gaugeSpeed.CircularScales["Scale1"].MajorTickMarks.Interval / 5;
                            }
                            if (ackParams.PaintOrange > 0 && ackParams.PaintGreen > 0) {
                                if (ackParams.step <= 6) {
                                    // 5025
                                    gaugeSpeed.CircularScales["Scale1"].Sections["Section2"].StartValue = ackParams.PaintOrange;
                                    gaugeSpeed.CircularScales["Scale1"].Sections["Section2"].EndValue = 25 + (25 - ackParams.PaintOrange);
                                    gaugeSpeed.CircularScales["Scale1"].Sections["Section3"].StartValue = ackParams.PaintGreen;
                                    gaugeSpeed.CircularScales["Scale1"].Sections["Section3"].EndValue = 25 + (25 - ackParams.PaintGreen);
                                } else {
                                    // 2540
                                    gaugeSpeed.CircularScales["Scale1"].Sections["Section2"].StartValue = ackParams.PaintOrange;
                                    gaugeSpeed.CircularScales["Scale1"].Sections["Section2"].EndValue = 40 + (40 - ackParams.PaintOrange);
                                    gaugeSpeed.CircularScales["Scale1"].Sections["Section3"].StartValue = ackParams.PaintGreen;
                                    gaugeSpeed.CircularScales["Scale1"].Sections["Section3"].EndValue = 40 + (40 - ackParams.PaintGreen);
                                }
                            }
                            gaugeRPM.CircularScales["Scale1"].Pointers["Pointer1"].Value = ackParams.RPM / 1000.0;
                            if (gaugeRPM.GaugeItems["Indicator1"] is NumericIndicator indRPM) {
                                indRPM.Value = ackParams.RPM / 1000.0;
                            }
                        });
                        if (ackParams.step == 14 && !f_result.Visible) {
                            GetASMCheckResultAckParams ackParams2 = new GetASMCheckResultAckParams();
                            if (_dynoCmd.GetASMCheckResultDataCmd(ref ackParams2, out errMsg)) {
                                _resultData.HC5025Limit = ackParams2.HC5025Limit;
                                _resultData.CO5025Limit = ackParams2.CO5025Limit;
                                _resultData.NO5025Limit = ackParams2.NO5025Limit;
                                _resultData.HC5025 = ackParams2.HC5025;
                                _resultData.CO5025 = ackParams2.CO5025;
                                _resultData.NO5025 = ackParams2.NO5025;
                                _resultData.HC5025Evl = ackParams2.HC5025Evl;
                                _resultData.CO5025Evl = ackParams2.CO5025Evl;
                                _resultData.NO5025Evl = ackParams2.NO5025Evl;

                                _resultData.HC2540Limit = ackParams2.HC2540Limit;
                                _resultData.CO2540Limit = ackParams2.CO2540Limit;
                                _resultData.NO2540Limit = ackParams2.NO2540Limit;
                                _resultData.HC2540 = ackParams2.HC2540;
                                _resultData.CO2540 = ackParams2.CO2540;
                                _resultData.NO2540 = ackParams2.NO2540;
                                _resultData.HC2540Evl = ackParams2.HC2540Evl;
                                _resultData.CO2540Evl = ackParams2.CO2540Evl;
                                _resultData.NO2540Evl = ackParams2.NO2540Evl;

                                _resultData.Result = ackParams2.ASMCheckeResult;
                                Invoke((EventHandler)delegate {
                                    f_result.ShowResult(_resultData);
                                    f_result.ShowDialog();
                                });
                            } else {
                                _log.TraceError("GetASMCheckResultDataCmd() return false");
                                MessageBox.Show("执行获取稳态工况检测结果数据命令失败", "执行命令出错", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                        if (ackParams.step == 15) {
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
                if (!_dynoCmd.StartASMCheckCmd(false, bRetry, out string errMsg)) {
                    MessageBox.Show("执行开始稳态工况检测命令失败", "执行命令出错", MessageBoxButtons.OK, MessageBoxIcon.Error);
                } else {
                    _dtRealTime.Rows.Clear();
                    _timer.Enabled = true;
                    _startTime = DateTime.Now;
                }
            } else {
                _timer.Enabled = false;
                Thread.Sleep(_mainCfg.RealtimeInterval);
                if (!_dynoCmd.StartASMCheckCmd(true, bRetry, out string errMsg) && errMsg != "ati >= 0") {
                    MessageBox.Show("执行停止稳态工况检测命令失败", "执行命令出错", MessageBoxButtons.OK, MessageBoxIcon.Error);
                } else if (errMsg.Length > 0) {
                    if (errMsg == "ati >= 0") {
                        lblMsg.Text = "已停止稳态工况检测";
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
            _db.SaveASMResult(_VIN, _startTime, Convert.ToDouble(_dtRealTime.Rows[_dtRealTime.Rows.Count - 1]["TimeSN"]), _envData, _resultData);
        }

        private void ASMForm_Load(object sender, EventArgs e) {
            lblMsg.Text = "稳态工况法测试";
        }

        private void ASMForm_FormClosing(object sender, FormClosingEventArgs e) {
            _timer.Elapsed -= OnTimer;
            _timer.Enabled = false;
        }

        private void ASMForm_Resize(object sender, EventArgs e) {
            if (_lastHeight == 0) {
                return;
            }
            float scale = Height / _lastHeight;
            layoutMain.Font = new Font(layoutMain.Font.FontFamily, layoutMain.Font.Size * scale, layoutMain.Font.Style);
            lblMsg.Font = new Font(lblMsg.Font.FontFamily, lblMsg.Font.Size * scale, lblMsg.Font.Style);
            _lastHeight = Height;
        }

        private void ASMForm_Shown(object sender, EventArgs e) {
            string strTips = "1、系好安全带，确认举升下降。" + Environment.NewLine;
            strTips += "2、安装安全限位。" + Environment.NewLine;
            strTips += "3、打开冷却风扇。" + Environment.NewLine;
            strTips += "4、车辆是否已经预热？" + Environment.NewLine;
            TipsForm f_tips = new TipsForm(strTips, 10);
            f_tips.ShowDialog();
            lblTestTime.Text = "--";
            lblWorkingTime.Text = "--";
            lblAccTime.Text = "--";
            lblStableTime.Text = "--";
            StartTest(true, false);
        }

        private void BtnRestart_Click(object sender, EventArgs e) {
            StopCheck();
            lblTestTime.Text = "--";
            lblWorkingTime.Text = "--";
            lblAccTime.Text = "--";
            lblStableTime.Text = "--";
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
