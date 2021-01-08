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
    public partial class FlowmeterPreheatingSubForm : Form {
        private float _lastHeight;
        private readonly DynoCmd _dynoCmd;
        private readonly MainSetting _mainCfg;
        private readonly Dictionary<Form, bool> _dicResults;
        private readonly System.Timers.Timer _timer;
        public event EventHandler<PreheatingDoneEventArgs> PreheatingDone;
        // -1: 停止检测; 0：流量检查；1：氧量程检查；2：温度检查；3：压力检查
        private int _step;
        // 结果数组, 存放每一步的结果, -1: 失败, 0: 无结果, 1: 成功
        // [0]：流量检查；[1]：氧量程检查；[2]：温度检查；[3]：压力检查
        private readonly int[] _iResults;

        public FlowmeterPreheatingSubForm(DynoCmd dynoCmd, MainSetting mainCfg, Dictionary<Form, bool> dicResults) {
            InitializeComponent();
            _lastHeight = this.Height;
            _dynoCmd = dynoCmd;
            _mainCfg = mainCfg;
            _dicResults = dicResults;
            _timer = new System.Timers.Timer(_mainCfg.RealtimeInterval);
            _timer.Elapsed += OnTimer;
            _timer.AutoReset = true;
            _step = -1;
            _iResults = new int[] { 0, 0, 0, 0 };
        }

        private void DoStep(string strResult, ref GetFlowmeterCheckRealTimeDataAckParams ackParams, ref int iResult) {
            if (strResult == "成功") {
                iResult = 1;
                _timer.Enabled = false;
                _dynoCmd.GetFlowmeterCheckRealTimeDataCmd(true, ref ackParams);
                _step++;
                StartFlowmeterCheckAckParams startAckParams = new StartFlowmeterCheckAckParams();
                if (!_dynoCmd.StartFlowmeterCheckCmd(false, _step, _mainCfg.Flowmeter.TargetTempe, _mainCfg.Flowmeter.TargetPressure, ref startAckParams)) {
                    MessageBox.Show("执行开始流量计预热命令失败", "执行命令出错", MessageBoxButtons.OK, MessageBoxIcon.Error);
                } else {
                    _timer.Enabled = true;
                    Invoke((EventHandler)delegate {
                        lblLowFlowSpan.Text = startAckParams.FlowmeterLowFlowSpan.ToString("F");
                        lblO2Span.Text = startAckParams.FlowmeterO2SpanLow.ToString("F");
                        lblO2Span.Text += "/" + startAckParams.FlowmeterO2SpanHight.ToString("F");
                    });
                }
            } else if (strResult == "失败") {
                iResult = -1;
            }
        }

        private void OnTimer(object source, System.Timers.ElapsedEventArgs e) {
            GetFlowmeterCheckRealTimeDataAckParams ackParams = new GetFlowmeterCheckRealTimeDataAckParams();
            if (_dynoCmd.GetFlowmeterCheckRealTimeDataCmd(false, ref ackParams) && ackParams != null) {
                if (_timer != null && _timer.Enabled) {
                    try {
                        Invoke((EventHandler)delegate {
                            if (ackParams.msg != null && ackParams.msg.Length > 0) {
                                lblMsg.Text = ackParams.msg;
                            }
                            lblFlow.Text = ackParams.Flow.ToString("F");
                            lblDiluteO2.Text = ackParams.Temperature.ToString("F");
                            lblTemperature.Text = ackParams.Pressure.ToString("F");
                            lblPressure.Text = ackParams.DiluteO2.ToString("F");
                            if (ackParams.FlowCheckResult != null && ackParams.FlowCheckResult.Length > 0) {
                                lblFlowCheck.Text = ackParams.FlowCheckResult;
                            }
                            if (ackParams.O2SpanCheckResult != null && ackParams.O2SpanCheckResult.Length > 0) {
                                lblO2SpanCheck.Text = ackParams.O2SpanCheckResult;
                            }
                            if (ackParams.TempeCheckResult != null && ackParams.TempeCheckResult.Length > 0) {
                                lblTempeCheck.Text = ackParams.TempeCheckResult;
                            }
                            if (ackParams.PressureCheckResult != null && ackParams.PressureCheckResult.Length > 0) {
                                lblPressureCheck.Text = ackParams.PressureCheckResult;
                            }
                            if (ackParams.FlowmeterCheckResult != null && ackParams.FlowmeterCheckResult.Length > 0) {
                                lblResult.Text = ackParams.FlowmeterCheckResult;
                            }
                        });
                    } catch (ObjectDisposedException) {
                        // 关闭窗口后仍有一定几率会进入主UI线程，此时访问界面元素会引发此异常，直接忽略即可
                    }
                    switch (_step) {
                    case 0:
                        DoStep(ackParams.FlowCheckResult, ref ackParams, ref _iResults[_step]);
                        break;
                    case 1:
                        DoStep(ackParams.O2SpanCheckResult, ref ackParams, ref _iResults[_step]);
                        break;
                    case 2:
                        DoStep(ackParams.TempeCheckResult, ref ackParams, ref _iResults[_step]);
                        break;
                    case 3:
                        if (ackParams.PressureCheckResult == "成功") {
                            _iResults[_step] = 1;
                            _step++;
                        } else if (ackParams.PressureCheckResult == "失败") {
                            _iResults[_step] = -1;
                        }
                        break;
                    }
                    foreach (int item in _iResults) {
                        if (item < 0) {
                            _timer.Enabled = false;
                            try {
                                Invoke((EventHandler)delegate {
                                    lblMsg.Text = "流量计预热失败!";
                                    lblFlow.Text = "--";
                                    lblDiluteO2.Text = "--";
                                    lblTemperature.Text = "--";
                                    lblPressure.Text = "--";
                                });
                            } catch (ObjectDisposedException) {
                                // 关闭窗口后仍有一定几率会进入主UI线程，此时访问界面元素会引发此异常，直接忽略即可
                            }
                            break;
                        }
                    }
                    int iResult = 0;
                    if (_step > 3) {
                        foreach (int item in _iResults) {
                            iResult += item;
                        }
                        if (iResult >= 4 && ackParams.FlowmeterCheckResult == "成功") {
                            _timer.Enabled = false;
                            _dicResults[this] = true;
                            try {
                                Invoke((EventHandler)delegate {
                                    lblMsg.Text = "流量计预热成功";
                                    lblFlow.Text = "--";
                                    lblDiluteO2.Text = "--";
                                    lblTemperature.Text = "--";
                                    lblPressure.Text = "--";
                                });
                            } catch (ObjectDisposedException) {
                                // 关闭窗口后仍有一定几率会进入主UI线程，此时访问界面元素会引发此异常，直接忽略即可
                            }
                            PreheatingDoneEventArgs args = new PreheatingDoneEventArgs {
                                Result = _dicResults[this]
                            };
                            PreheatingDone?.Invoke(this, args);
                        }
                    }
                }
            }
        }

        private void FlowmeterPreheatingSubForm_Load(object sender, EventArgs e) {
            lblMsg.Text = "流量计预热";
            txtBoxTargetTempe.Text = _mainCfg.Flowmeter.TargetTempe.ToString("F");
            txtBoxTargetPressure.Text = _mainCfg.Flowmeter.TargetPressure.ToString("F");
        }

        private void BtnStart_Click(object sender, EventArgs e) {
            _step = 0;
            for (int i = 0; i < _iResults.Length; i++) {
                _iResults[i] = 0;
            }
            StartFlowmeterCheckAckParams ackParams = new StartFlowmeterCheckAckParams();
            if (!_dynoCmd.StartFlowmeterCheckCmd(false, _step, _mainCfg.Flowmeter.TargetTempe, _mainCfg.Flowmeter.TargetPressure, ref ackParams)) {
                MessageBox.Show("执行开始流量计预热命令失败", "执行命令出错", MessageBoxButtons.OK, MessageBoxIcon.Error);
            } else {
                _timer.Enabled = true;
                lblLowFlowSpan.Text = ackParams.FlowmeterLowFlowSpan.ToString("F");
                lblO2Span.Text = ackParams.FlowmeterO2SpanLow.ToString("F");
                lblO2Span.Text += "/" + ackParams.FlowmeterO2SpanHight.ToString("F");
            }
        }

        private void BtnStop_Click(object sender, EventArgs e) {
            _dynoCmd.ReconnectServer();
            StartFlowmeterCheckAckParams ackParams = new StartFlowmeterCheckAckParams();
            if (!_dynoCmd.StartFlowmeterCheckCmd(true, _step, _mainCfg.Flowmeter.TargetTempe, _mainCfg.Flowmeter.TargetPressure, ref ackParams)) {
                MessageBox.Show("执行停止流量计预热命令失败", "执行命令出错", MessageBoxButtons.OK, MessageBoxIcon.Error);
            } else {
                _timer.Enabled = false;
                lblMsg.Text = "停止流量计预热";
                lblLowFlowSpan.Text = ackParams.FlowmeterLowFlowSpan.ToString("F");
                lblO2Span.Text = ackParams.FlowmeterO2SpanLow.ToString("F");
                lblO2Span.Text += "/" + ackParams.FlowmeterO2SpanHight.ToString("F");
                lblFlow.Text = "--";
                lblDiluteO2.Text = "--";
                lblTemperature.Text = "--";
                lblPressure.Text = "--";
            }
        }

        private void FlowmeterPreheatingSubForm_FormClosing(object sender, FormClosingEventArgs e) {
            _timer.Elapsed -= OnTimer;
            _timer.Enabled = false;
        }

        private void FlowmeterPreheatingSubForm_Resize(object sender, EventArgs e) {
            if (_lastHeight == 0) {
                return;
            }
            float scale = this.Height / _lastHeight;
            layoutMain.Font = new Font(layoutMain.Font.FontFamily, layoutMain.Font.Size * scale, layoutMain.Font.Style);
            lblMsg.Font = new Font(lblMsg.Font.FontFamily, lblMsg.Font.Size * scale, lblMsg.Font.Style);
            _lastHeight = this.Height;
        }
    }
}
