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
    public partial class DynoGlideCheckForm : Form {
        private float _lastHeight;
        private readonly DynoCmd _dynoCmd;
        private readonly MainSetting _mainCfg;
        private readonly Logger _log;
        private readonly bool _bDiesel;
        private readonly DataTable _dtRTData;
        private readonly DataTable _dtResult;
        private readonly System.Timers.Timer _timer;
        private DateTime _startTime;
        private DieselIHP _dieselIHP;

        private enum DieselIHP {
            IHP30kW,
            IHP20kW,
            IHP10kW
        }

        public DynoGlideCheckForm(DynoCmd dynoCmd, MainSetting mainCfg, Logger log, bool bDiesel) {
            InitializeComponent();
            _lastHeight = Height;
            _dynoCmd = dynoCmd;
            _mainCfg = mainCfg;
            _log = log;
            _bDiesel = bDiesel;
            _dtRTData = new DataTable("RTData");
            _dtResult = new DataTable("Result");
            _timer = new System.Timers.Timer(_mainCfg.RealtimeInterval);
            _timer.Elapsed += OnTimer;
            _timer.AutoReset = true;
            _dieselIHP = DieselIHP.IHP30kW;
        }

        private void FillDieselResult(GetCYDynoLoadGlideCheckRealTimeDataAckParams ackParams) {
            _dtResult.Rows[0]["PLHP"] = ackParams.PLHP90_30kw;
            _dtResult.Rows[0]["CCDT"] = ackParams.CCDT90_30kw;
            _dtResult.Rows[0]["ACDT"] = ackParams.ACDT90_30kw;
            _dtResult.Rows[0]["滑行误差(%)"] = ackParams.Error90_30kw;
            _dtResult.Rows[1]["PLHP"] = ackParams.PLHP80_30kw;
            _dtResult.Rows[1]["CCDT"] = ackParams.CCDT80_30kw;
            _dtResult.Rows[1]["ACDT"] = ackParams.ACDT80_30kw;
            _dtResult.Rows[1]["滑行误差(%)"] = ackParams.Error80_30kw;
            _dtResult.Rows[2]["PLHP"] = ackParams.PLHP70_30kw;
            _dtResult.Rows[2]["CCDT"] = ackParams.CCDT70_30kw;
            _dtResult.Rows[2]["ACDT"] = ackParams.ACDT70_30kw;
            _dtResult.Rows[2]["滑行误差(%)"] = ackParams.Error70_30kw;
            _dtResult.Rows[3]["PLHP"] = ackParams.PLHP60_30kw;
            _dtResult.Rows[3]["CCDT"] = ackParams.CCDT60_30kw;
            _dtResult.Rows[3]["ACDT"] = ackParams.ACDT60_30kw;
            _dtResult.Rows[3]["滑行误差(%)"] = ackParams.Error60_30kw;
            _dtResult.Rows[4]["PLHP"] = ackParams.PLHP50_30kw;
            _dtResult.Rows[4]["CCDT"] = ackParams.CCDT50_30kw;
            _dtResult.Rows[4]["ACDT"] = ackParams.ACDT50_30kw;
            _dtResult.Rows[4]["滑行误差(%)"] = ackParams.Error50_30kw;
            _dtResult.Rows[5]["PLHP"] = ackParams.PLHP40_30kw;
            _dtResult.Rows[5]["CCDT"] = ackParams.CCDT40_30kw;
            _dtResult.Rows[5]["ACDT"] = ackParams.ACDT40_30kw;
            _dtResult.Rows[5]["滑行误差(%)"] = ackParams.Error40_30kw;
            _dtResult.Rows[6]["PLHP"] = ackParams.PLHP30_30kw;
            _dtResult.Rows[6]["CCDT"] = ackParams.CCDT30_30kw;
            _dtResult.Rows[6]["ACDT"] = ackParams.ACDT30_30kw;
            _dtResult.Rows[6]["滑行误差(%)"] = ackParams.Error30_30kw;
            _dtResult.Rows[7]["PLHP"] = ackParams.PLHP20_30kw;
            _dtResult.Rows[7]["CCDT"] = ackParams.CCDT20_30kw;
            _dtResult.Rows[7]["ACDT"] = ackParams.ACDT20_30kw;
            _dtResult.Rows[7]["滑行误差(%)"] = ackParams.Error20_30kw;

            _dtResult.Rows[8]["PLHP"] = ackParams.PLHP90_20kw;
            _dtResult.Rows[8]["CCDT"] = ackParams.CCDT90_20kw;
            _dtResult.Rows[8]["ACDT"] = ackParams.ACDT90_20kw;
            _dtResult.Rows[8]["滑行误差(%)"] = ackParams.Error90_20kw;
            _dtResult.Rows[9]["PLHP"] = ackParams.PLHP80_20kw;
            _dtResult.Rows[9]["CCDT"] = ackParams.CCDT80_20kw;
            _dtResult.Rows[9]["ACDT"] = ackParams.ACDT80_20kw;
            _dtResult.Rows[9]["滑行误差(%)"] = ackParams.Error80_20kw;
            _dtResult.Rows[10]["PLHP"] = ackParams.PLHP70_20kw;
            _dtResult.Rows[10]["CCDT"] = ackParams.CCDT70_20kw;
            _dtResult.Rows[10]["ACDT"] = ackParams.ACDT70_20kw;
            _dtResult.Rows[10]["滑行误差(%)"] = ackParams.Error70_20kw;
            _dtResult.Rows[11]["PLHP"] = ackParams.PLHP60_20kw;
            _dtResult.Rows[11]["CCDT"] = ackParams.CCDT60_20kw;
            _dtResult.Rows[11]["ACDT"] = ackParams.ACDT60_20kw;
            _dtResult.Rows[11]["滑行误差(%)"] = ackParams.Error60_20kw;
            _dtResult.Rows[12]["PLHP"] = ackParams.PLHP50_20kw;
            _dtResult.Rows[12]["CCDT"] = ackParams.CCDT50_20kw;
            _dtResult.Rows[12]["ACDT"] = ackParams.ACDT50_20kw;
            _dtResult.Rows[12]["滑行误差(%)"] = ackParams.Error50_20kw;
            _dtResult.Rows[13]["PLHP"] = ackParams.PLHP40_20kw;
            _dtResult.Rows[13]["CCDT"] = ackParams.CCDT40_20kw;
            _dtResult.Rows[13]["ACDT"] = ackParams.ACDT40_20kw;
            _dtResult.Rows[13]["滑行误差(%)"] = ackParams.Error40_20kw;
            _dtResult.Rows[14]["PLHP"] = ackParams.PLHP30_20kw;
            _dtResult.Rows[14]["CCDT"] = ackParams.CCDT30_20kw;
            _dtResult.Rows[14]["ACDT"] = ackParams.ACDT30_20kw;
            _dtResult.Rows[14]["滑行误差(%)"] = ackParams.Error30_20kw;
            _dtResult.Rows[15]["PLHP"] = ackParams.PLHP20_20kw;
            _dtResult.Rows[15]["CCDT"] = ackParams.CCDT20_20kw;
            _dtResult.Rows[15]["ACDT"] = ackParams.ACDT20_20kw;
            _dtResult.Rows[15]["滑行误差(%)"] = ackParams.Error20_20kw;

            _dtResult.Rows[16]["PLHP"] = ackParams.PLHP90_10kw;
            _dtResult.Rows[16]["CCDT"] = ackParams.CCDT90_10kw;
            _dtResult.Rows[16]["ACDT"] = ackParams.ACDT90_10kw;
            _dtResult.Rows[16]["滑行误差(%)"] = ackParams.Error90_10kw;
            _dtResult.Rows[17]["PLHP"] = ackParams.PLHP80_10kw;
            _dtResult.Rows[17]["CCDT"] = ackParams.CCDT80_10kw;
            _dtResult.Rows[17]["ACDT"] = ackParams.ACDT80_10kw;
            _dtResult.Rows[17]["滑行误差(%)"] = ackParams.Error80_10kw;
            _dtResult.Rows[18]["PLHP"] = ackParams.PLHP70_10kw;
            _dtResult.Rows[18]["CCDT"] = ackParams.CCDT70_10kw;
            _dtResult.Rows[18]["ACDT"] = ackParams.ACDT70_10kw;
            _dtResult.Rows[18]["滑行误差(%)"] = ackParams.Error70_10kw;
            _dtResult.Rows[19]["PLHP"] = ackParams.PLHP60_10kw;
            _dtResult.Rows[19]["CCDT"] = ackParams.CCDT60_10kw;
            _dtResult.Rows[19]["ACDT"] = ackParams.ACDT60_10kw;
            _dtResult.Rows[19]["滑行误差(%)"] = ackParams.Error60_10kw;
            _dtResult.Rows[20]["PLHP"] = ackParams.PLHP50_10kw;
            _dtResult.Rows[20]["CCDT"] = ackParams.CCDT50_10kw;
            _dtResult.Rows[20]["ACDT"] = ackParams.ACDT50_10kw;
            _dtResult.Rows[20]["滑行误差(%)"] = ackParams.Error50_10kw;
            _dtResult.Rows[21]["PLHP"] = ackParams.PLHP40_10kw;
            _dtResult.Rows[21]["CCDT"] = ackParams.CCDT40_10kw;
            _dtResult.Rows[21]["ACDT"] = ackParams.ACDT40_10kw;
            _dtResult.Rows[21]["滑行误差(%)"] = ackParams.Error40_10kw;
            _dtResult.Rows[22]["PLHP"] = ackParams.PLHP30_10kw;
            _dtResult.Rows[22]["CCDT"] = ackParams.CCDT30_10kw;
            _dtResult.Rows[22]["ACDT"] = ackParams.ACDT30_10kw;
            _dtResult.Rows[22]["滑行误差(%)"] = ackParams.Error30_10kw;
            _dtResult.Rows[23]["PLHP"] = ackParams.PLHP20_10kw;
            _dtResult.Rows[23]["CCDT"] = ackParams.CCDT20_10kw;
            _dtResult.Rows[23]["ACDT"] = ackParams.ACDT20_10kw;
            _dtResult.Rows[23]["滑行误差(%)"] = ackParams.Error20_10kw;

        }

        private void OnTimer(object source, System.Timers.ElapsedEventArgs e) {
            if (_bDiesel) {
                double IHP = 0;
                switch (_dieselIHP) {
                case DieselIHP.IHP30kW:
                    IHP = 30;
                    break;
                case DieselIHP.IHP20kW:
                    IHP = 20;
                    break;
                case DieselIHP.IHP10kW:
                    IHP = 10;
                    break;
                }
                GetCYDynoLoadGlideCheckRealTimeDataAckParams ackParams = new GetCYDynoLoadGlideCheckRealTimeDataAckParams();
                if (_dynoCmd.GetCYDynoLoadGlideCheckRealTimeDataCmd(IHP, ref ackParams, out string errMsg) && ackParams != null) {
                    if (_timer != null && _timer.Enabled) {
                        try {
                            DataRow dr = _dtRTData.NewRow();
                            TimeSpan interval = DateTime.Now - _startTime;
                            if (interval.TotalSeconds < 0.001) {
                                interval = TimeSpan.Zero;
                            }
                            dr["TimeSN"] = interval.TotalSeconds.ToString("F1");
                            dr["Power"] = ackParams.power;
                            dr["Speed"] = ackParams.speed;
                            dr["TorqueF"] = ackParams.torquef;
                            _dtRTData.Rows.Add(dr);

                            FillDieselResult(ackParams);
                            if (ackParams.step == 7) {
                                _dieselIHP = DieselIHP.IHP20kW;
                            } else if (ackParams.step == 15) {
                                _dieselIHP = DieselIHP.IHP10kW;
                            }
                            Invoke((EventHandler)delegate {
                                lblPower.Text = ackParams.power.ToString("F");
                                lblSpeed.Text = ackParams.speed.ToString("F");
                                lblTorqueF.Text = ackParams.torquef.ToString("F");
                                chart1.DataBind();
                                switch (_dieselIHP) {
                                case DieselIHP.IHP30kW:
                                    txtBoxIHP.Text = "30";
                                    break;
                                case DieselIHP.IHP20kW:
                                    txtBoxIHP.Text = "20";
                                    break;
                                case DieselIHP.IHP10kW:
                                    txtBoxIHP.Text = "10";
                                    break;
                                }
                                // 柴油无法仅用step来判断滑行检查是否已结束
                                if (ackParams.step >= 23 && ackParams.result != null && ackParams.result.Length > 0) {
                                    lblResult.Text = ackParams.result;
                                    if (ackParams.result.Contains("不")) {
                                        lblResult.BackColor = Color.Red;
                                    }
                                    _timer.Enabled = false;
                                }
                            });
                        } catch (ObjectDisposedException) {
                            // 关闭窗口后仍有一定几率会进入主UI线程，此时访问界面元素会引发此异常，直接忽略即可
                        }
                    }
                }
            } else {
                GetQYDynoLoadGlideCheckRealTimeDataAckParams ackParams = new GetQYDynoLoadGlideCheckRealTimeDataAckParams();
                if (_dynoCmd.GetQYDynoLoadGlideCheckRealTimeDataCmd(ref ackParams, out string errMsg) && ackParams != null) {
                    if (_timer != null && _timer.Enabled) {
                        try {
                            DataRow dr = _dtRTData.NewRow();
                            TimeSpan interval = DateTime.Now - _startTime;
                            if (interval.TotalSeconds < 0.001) {
                                interval = TimeSpan.Zero;
                            }
                            dr["TimeSN"] = interval.TotalSeconds.ToString("F1");
                            dr["Power"] = ackParams.power;
                            dr["Speed"] = ackParams.speed;
                            dr["TorqueF"] = ackParams.torquef;
                            _dtRTData.Rows.Add(dr);

                            _dtResult.Rows[0]["ACDT"] = ackParams.ACDT25_11kw;
                            _dtResult.Rows[0]["滑行误差(%)"] = ackParams.Error25_11kw;
                            _dtResult.Rows[1]["ACDT"] = ackParams.ACDT40_11kw;
                            _dtResult.Rows[1]["滑行误差(%)"] = ackParams.Error40_11kw;
                            Invoke((EventHandler)delegate {
                                lblPower.Text = ackParams.power.ToString("F");
                                lblSpeed.Text = ackParams.speed.ToString("F");
                                lblTorqueF.Text = ackParams.torquef.ToString("F");
                                chart1.DataBind();
                                if (ackParams.result != null && ackParams.result.Length > 0) {
                                    lblResult.Text = ackParams.result;
                                    if (ackParams.result.Contains("不")) {
                                        lblResult.BackColor = Color.Red;
                                    }
                                }
                            });
                            // 汽油可以用step来判断滑行检查是否已结束
                            if (ackParams.step < 0) {
                                _timer.Enabled = false;
                            }
                        } catch (ObjectDisposedException) {
                            // 关闭窗口后仍有一定几率会进入主UI线程，此时访问界面元素会引发此异常，直接忽略即可
                        }
                    }
                }
            }
        }

        private void StartGasCheck(bool bStart) {
            double IHP = double.Parse(txtBoxIHP.Text);
            if (bStart) {
                if (IHP < 6 || IHP > 13) {
                    MessageBox.Show("加载功率范围必须为：6 ~ 13（kW）", "加载功率输入出错", MessageBoxButtons.OK, MessageBoxIcon.Error);
                } else {
                    StartQYDynoLoadGlideCheckParams cmdParams = new StartQYDynoLoadGlideCheckParams {
                        ClientID = _dynoCmd.ClientID,
                        stopCheck = !bStart,
                        FlagPrepareHeat = chkBoxPreheat.Checked,
                        LoadPower = IHP,
                        TargetSpeed = 60
                    };
                    StartQYDynoLoadGlideCheckAckParams ackParams = new StartQYDynoLoadGlideCheckAckParams();
                    if (!_dynoCmd.StartQYDynoLoadGlideCheckCmd(cmdParams, ref ackParams, out string errMsg)) {
                        MessageBox.Show("执行开始汽油测功机加载滑行检查命令失败", "执行命令出错", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    } else {
                        lblMsg.Text = "测功机滑行检查 - 汽油";
                        _dtRTData.Rows.Clear();
                        _timer.Enabled = true;
                        _startTime = DateTime.Now;
                        _dtResult.Rows[0]["PLHP"] = ackParams.PLHP25_11kw;
                        _dtResult.Rows[0]["CCDT"] = ackParams.CCDT25_11kw;
                        _dtResult.Rows[1]["PLHP"] = ackParams.PLHP40_11kw;
                        _dtResult.Rows[1]["CCDT"] = ackParams.CCDT40_11kw;
                        Invoke((EventHandler)delegate {
                            btnBeamDown.Enabled = false;
                            btnBeamUp.Enabled = false;
                            btnStart.Enabled = false;
                            btnStop.Enabled = true;
                        });
                    }
                }
            } else {
                _timer.Enabled = false;
                Thread.Sleep(_mainCfg.RealtimeInterval);
                StartQYDynoLoadGlideCheckParams cmdParams = new StartQYDynoLoadGlideCheckParams {
                    ClientID = _dynoCmd.ClientID,
                    stopCheck = !bStart,
                    FlagPrepareHeat = chkBoxPreheat.Checked,
                    LoadPower = IHP,
                    TargetSpeed = 60
                };
                StartQYDynoLoadGlideCheckAckParams ackParams = new StartQYDynoLoadGlideCheckAckParams();
                if (!_dynoCmd.StartQYDynoLoadGlideCheckCmd(cmdParams, ref ackParams, out string errMsg) && errMsg != "ati >= 0") {
                    MessageBox.Show("执行停止汽油测功机加载滑行检查命令失败", "执行命令出错", MessageBoxButtons.OK, MessageBoxIcon.Error);
                } else if (errMsg.Length > 0) {
                    Invoke((EventHandler)delegate {
                        if (errMsg == "ati >= 0") {
                            lblMsg.Text = "已停止汽油测功机加载滑行检查";
                        } else if (errMsg != "OK") {
                            lblMsg.Text = errMsg;
                        }
                        btnBeamDown.Enabled = false;
                        btnBeamUp.Enabled = true;
                        btnStart.Enabled = true;
                        btnStop.Enabled = false;
                    });
                }
            }
        }

        private void StartDieselCheck(bool bStart) {
            double IHP = double.Parse(txtBoxIHP.Text);
            if (bStart) {
                StartCYDynoLoadGlideCheckParams cmdParams = new StartCYDynoLoadGlideCheckParams {
                    ClientID = _dynoCmd.ClientID,
                    stopCheck = !bStart,
                    FlagPrepareHeat = chkBoxPreheat.Checked,
                    LoadPower = IHP,
                };
                if (!_dynoCmd.StartCYDynoLoadGlideCheckCmd(cmdParams, out string errMsg)) {
                    MessageBox.Show("执行开始汽油测功机加载滑行检查命令失败", "执行命令出错", MessageBoxButtons.OK, MessageBoxIcon.Error);
                } else {
                    lblMsg.Text = "测功机滑行检查 - 柴油";
                    _dtRTData.Rows.Clear();
                    _timer.Enabled = true;
                    _startTime = DateTime.Now;
                    Invoke((EventHandler)delegate {
                        btnBeamDown.Enabled = false;
                        btnBeamUp.Enabled = false;
                        btnStart.Enabled = false;
                        btnStop.Enabled = true;
                    });
                }
            } else {
                _timer.Enabled = false;
                StartCYDynoLoadGlideCheckParams cmdParams = new StartCYDynoLoadGlideCheckParams {
                    ClientID = _dynoCmd.ClientID,
                    stopCheck = !bStart,
                    FlagPrepareHeat = chkBoxPreheat.Checked,
                    LoadPower = IHP,
                };
                if (!_dynoCmd.StartCYDynoLoadGlideCheckCmd(cmdParams, out string errMsg) && errMsg != "ati >= 0") {
                    MessageBox.Show("执行停止柴油测功机加载滑行检查命令失败", "执行命令出错", MessageBoxButtons.OK, MessageBoxIcon.Error);
                } else if (errMsg.Length > 0) {
                    Invoke((EventHandler)delegate {
                        if (errMsg == "ati >= 0") {
                            lblMsg.Text = "已停止柴油测功机加载滑行检查";
                        } else if (errMsg != "OK") {
                            lblMsg.Text = errMsg;
                        }
                        btnBeamDown.Enabled = false;
                        btnBeamUp.Enabled = true;
                        btnStart.Enabled = true;
                        btnStop.Enabled = false;
                    });
                }
            }
        }

        private void SetDataGridViewColumnsSortMode(DataGridView dgv, DataGridViewColumnSortMode sortMode) {
            for (int i = 0; i < dgv.Columns.Count; i++) {
                dgv.Columns[i].SortMode = sortMode;
            }
        }

        private void DynoGlideCheckForm_Load(object sender, EventArgs e) {
            _dtRTData.Columns.Add("TimeSN");
            _dtRTData.Columns.Add("Power");
            _dtRTData.Columns.Add("Speed");
            _dtRTData.Columns.Add("TorqueF");
            DataRow dr = _dtRTData.NewRow();
            dr["TimeSN"] = TimeSpan.Zero.TotalSeconds.ToString("F1");
            dr["Power"] = 0;
            dr["Speed"] = 0;
            dr["TorqueF"] = 0;
            _dtRTData.Rows.Add(dr);

            _dtResult.Columns.Add("项目");
            _dtResult.Columns.Add("PLHP");
            _dtResult.Columns.Add("CCDT");
            _dtResult.Columns.Add("ACDT");
            _dtResult.Columns.Add("滑行误差(%)");
            if (_bDiesel) {
                txtBoxIHP.Enabled = false;
                switch (_dieselIHP) {
                case DieselIHP.IHP30kW:
                    txtBoxIHP.Text = "30";
                    break;
                case DieselIHP.IHP20kW:
                    txtBoxIHP.Text = "20";
                    break;
                case DieselIHP.IHP10kW:
                    txtBoxIHP.Text = "10";
                    break;
                }
                chkBoxPreheat.Checked = true;
                lblMsg.Text = "测功机滑行检查 - 柴油";
                lblNote.Text += "δ[30kW]≤4%，δ[20kW]≤2%，δ[10kW]≤2%";

                for (int i = 3; i >= 1; i--) {
                    for (int j = 9; j >= 2; j--) {
                        DataRow dr2 = _dtResult.NewRow();
                        dr2["项目"] = string.Format("{0}km/h {1}kW", j * 10, i * 10);
                        dr2["PLHP"] = 0;
                        dr2["CCDT"] = 0;
                        dr2["ACDT"] = 0;
                        dr2["滑行误差(%)"] = 0;
                        _dtResult.Rows.Add(dr2);
                    }
                }
                dgvResult.DataSource = _dtResult;
                SetDataGridViewColumnsSortMode(dgvResult, DataGridViewColumnSortMode.Programmatic);
                dgvResult.Columns[0].Width = 180;
            } else {
                txtBoxIHP.Text = "11";
                chkBoxPreheat.Checked = true;
                lblMsg.Text = "测功机滑行检查 - 汽油";
                lblNote.Text += "δ[40km/h]≤7%，δ[25km/h]≤7%";

                for (int i = 0; i < 2; i++) {
                    DataRow dr2 = _dtResult.NewRow();
                    dr2["项目"] = string.Empty;
                    dr2["PLHP"] = 0;
                    dr2["CCDT"] = 0;
                    dr2["ACDT"] = 0;
                    dr2["滑行误差(%)"] = 0;
                    _dtResult.Rows.Add(dr2);
                }
                _dtResult.Rows[0]["项目"] = "25km/h";
                _dtResult.Rows[1]["项目"] = "40km/h";
                dgvResult.DataSource = _dtResult;
                SetDataGridViewColumnsSortMode(dgvResult, DataGridViewColumnSortMode.Programmatic);
            }

            chart1.Series[0].Name = "功率(kW)";
            chart1.Series.Add("速度(km/h)");
            chart1.Series.Add("扭力(N)");
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
            chart1.Series[0].YValueMembers = "Power";
            chart1.Series[1].XValueMember = "TimeSN";
            chart1.Series[1].YValueMembers = "Speed";
            chart1.Series[2].XValueMember = "TimeSN";
            chart1.Series[2].YValueMembers = "TorqueF";
            chart1.Series[2].YAxisType = AxisType.Secondary;
            chart1.ChartAreas[0].AxisX.IsStartedFromZero = true;
            chart1.ChartAreas[0].AxisX.Minimum = 0;
            chart1.ChartAreas[0].AxisX.IntervalAutoMode = IntervalAutoMode.VariableCount;
            chart1.ChartAreas[0].AxisX.MajorGrid.LineDashStyle = ChartDashStyle.Dash;
            chart1.ChartAreas[0].AxisX.MajorGrid.LineColor = Color.Gray;
            chart1.ChartAreas[0].AxisX.Title = "时间(秒)";
            chart1.ChartAreas[0].AxisY.MajorGrid.LineDashStyle = ChartDashStyle.Dash;
            chart1.ChartAreas[0].AxisY.MajorGrid.LineColor = Color.Gray;
            chart1.ChartAreas[0].AxisY.Title = "功率(kW) / 速度(km/h)";
            chart1.ChartAreas[0].AxisY2.Enabled = AxisEnabled.True;
            chart1.ChartAreas[0].AxisY2.MajorGrid.LineDashStyle = ChartDashStyle.Dash;
            chart1.ChartAreas[0].AxisY2.MajorGrid.LineColor = Color.Gray;
            chart1.ChartAreas[0].AxisY2.Title = "扭力(N)";
            chart1.ChartAreas[0].AxisY2.TitleForeColor = Color.Red;
            chart1.DataSource = _dtRTData;
            chart1.DataBind();

            btnBeamDown.Enabled = true;
            btnBeamUp.Enabled = false;
            btnStart.Enabled = false;
            btnStop.Enabled = false;
        }

        private void DynoGlideCheckForm_Resize(object sender, EventArgs e) {
            if (_lastHeight == 0) {
                return;
            }
            float scale = Height / _lastHeight;
            layoutMain.Font = new Font(layoutMain.Font.FontFamily, layoutMain.Font.Size * scale, layoutMain.Font.Style);
            lblMsg.Font = new Font(lblMsg.Font.FontFamily, lblMsg.Font.Size * scale, lblMsg.Font.Style);
            _lastHeight = Height;
        }

        private void DynoGlideCheckForm_FormClosing(object sender, FormClosingEventArgs e) {
            _timer.Elapsed -= OnTimer;
            _timer.Enabled = false;
        }

        private void BtnBeamDown_Click(object sender, EventArgs e) {
            btnBeamDown.Enabled = false;
            btnBeamUp.Enabled = true;
            btnStart.Enabled = true;
            btnStop.Enabled = false;
            if (!_dynoCmd.DynoBeamDownCmd(out string errMsg)) {
                MessageBox.Show("执行举升下降命令失败", "执行命令出错", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnBeamUp_Click(object sender, EventArgs e) {
            btnBeamDown.Enabled = true;
            btnBeamUp.Enabled = false;
            btnStart.Enabled = false;
            btnStop.Enabled = false;
            if (!_dynoCmd.DynoBeamUpCmd(out string errMsg)) {
                MessageBox.Show("执行举升上升命令失败", "执行命令出错", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnStart_Click(object sender, EventArgs e) {
            if (_bDiesel) {
                StartDieselCheck(true);
            } else {
                StartGasCheck(true);
            }
        }

        private void BtnStop_Click(object sender, EventArgs e) {
            if (_bDiesel) {
                StartDieselCheck(false);
            } else {
                StartGasCheck(false);
            }
        }

        private void TxtBoxIHP_KeyPress(object sender, KeyPressEventArgs e) {
            if (sender is TextBox txtBox) {
                if (e.KeyChar != '\b' && e.KeyChar != '.' && !char.IsDigit(e.KeyChar)) {
                    // 允许退格、删除、小数点和数字输入
                    e.Handled = true;
                } else if (e.KeyChar == '.' && txtBox.Text.Contains(".")) {
                    // 只允许输入一个小数点
                    e.Handled = true;
                }
            }
        }
    }
}
