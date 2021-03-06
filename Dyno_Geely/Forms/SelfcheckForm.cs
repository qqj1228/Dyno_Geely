﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Dyno_Geely {
    public partial class SelfcheckForm : Form {
        private float _lastHeight;
        private readonly DynoCmd _dynoCmd;
        private readonly Dictionary<Form, bool> _dicResults;
        private readonly Dictionary<Form, bool> _dicStops;
        private readonly Dictionary<Button, Form> _dicSubForms;
        private readonly Button[] _buttonsOrder;
        private readonly System.Timers.Timer _timer;
        private readonly GasBoxSelfcheckSubForm f_gasBoxSelfcheck;
        private readonly FlowmeterSelfcheckSubForm f_flowmeterSelfcheck;
        private readonly SmokerSelfcheckSubForm f_smokerSelfcheck;
        private readonly OilTempSelfcheckSubForm f_oilTempSelfcheck;
        private readonly TachometerSelfcheckSubForm f_tachometerSelfcheck;
        private readonly WeatherSelfcheckSubForm f_weatherSelfcheck;
        private readonly EnvironmentData _envData;

        public SelfcheckForm(DynoCmd dynoCmd, MainSetting mainCfg, EnvironmentData envData, bool bDiesel) {
            InitializeComponent();
            _lastHeight = Height;
            _dynoCmd = dynoCmd;
            _dicResults = new Dictionary<Form, bool>();
            _dicStops = new Dictionary<Form, bool>();
            _envData = envData;

            _timer = new System.Timers.Timer(mainCfg.RealtimeInterval);
            _timer.Elapsed += OnTimer;
            _timer.AutoReset = false;

            f_gasBoxSelfcheck = new GasBoxSelfcheckSubForm(_dynoCmd, mainCfg, _dicResults, _dicStops, bDiesel);
            f_flowmeterSelfcheck = new FlowmeterSelfcheckSubForm(_dynoCmd, mainCfg, _dicResults, _dicStops);
            f_smokerSelfcheck = new SmokerSelfcheckSubForm(_dynoCmd, mainCfg, _dicResults, _dicStops);
            f_oilTempSelfcheck = new OilTempSelfcheckSubForm(_dynoCmd, mainCfg, _dicResults, _dicStops);
            f_tachometerSelfcheck = new TachometerSelfcheckSubForm(_dynoCmd, mainCfg, _dicResults, _dicStops);
            f_weatherSelfcheck = new WeatherSelfcheckSubForm(_dynoCmd, mainCfg, _dicResults, _dicStops, _envData);

            f_gasBoxSelfcheck.SelfcheckDone += OnSelfcheckDone;
            f_flowmeterSelfcheck.SelfcheckDone += OnSelfcheckDone;
            f_smokerSelfcheck.SelfcheckDone += OnSelfcheckDone;
            f_oilTempSelfcheck.SelfcheckDone += OnSelfcheckDone;
            f_tachometerSelfcheck.SelfcheckDone += OnSelfcheckDone;
            f_weatherSelfcheck.SelfcheckDone += OnSelfcheckDone;

            _dicResults.Add(f_gasBoxSelfcheck, false);
            _dicResults.Add(f_flowmeterSelfcheck, false);
            _dicResults.Add(f_smokerSelfcheck, false);
            _dicResults.Add(f_oilTempSelfcheck, false);
            _dicResults.Add(f_tachometerSelfcheck, false);
            _dicResults.Add(f_weatherSelfcheck, false);

            _dicStops.Add(f_gasBoxSelfcheck, false);
            _dicStops.Add(f_flowmeterSelfcheck, false);
            _dicStops.Add(f_smokerSelfcheck, false);
            _dicStops.Add(f_oilTempSelfcheck, false);
            _dicStops.Add(f_tachometerSelfcheck, false);
            _dicStops.Add(f_weatherSelfcheck, false);

            _buttonsOrder = new Button[] { btn1GasBox, btn2Flowmeter, btn3Smoker, btn4OilTemp, btn5Tachometer, btn6Weather };

            _dicSubForms = new Dictionary<Button, Form> {
                { btn1GasBox, f_gasBoxSelfcheck },
                { btn2Flowmeter, f_flowmeterSelfcheck },
                { btn3Smoker, f_smokerSelfcheck },
                { btn4OilTemp, f_oilTempSelfcheck },
                { btn5Tachometer, f_tachometerSelfcheck },
                { btn6Weather, f_weatherSelfcheck }
            };
        }

        private void OnSelfcheckDone(object sender, SelfcheckDoneEventArgs e) {
            if (e.Result) {
                if (sender is Form form) {
                    var keys = _dicSubForms.Where(kv => kv.Value == form).Select(kv => kv.Key);
                    int index = 0;
                    for (int i = 0; i < _buttonsOrder.Length; i++) {
                        if (_buttonsOrder[i] == keys.FirstOrDefault()) {
                            index = i + 1;
                        }
                    }
                    if (index < _buttonsOrder.Length) {
                        Invoke((EventHandler)delegate {
                            _buttonsOrder[index - 1].BackColor = Color.Lime;
                            _buttonsOrder[index].Enabled = true;
                            _buttonsOrder[index].PerformClick();
                            _dicSubForms[_buttonsOrder[index]].AcceptButton.PerformClick();
                        });
                    } else {
                        Close();
                    }
                }
            }
        }

        private void OnTimer(object source, System.Timers.ElapsedEventArgs e) {
            f_gasBoxSelfcheck.StartSelfcheck(true);
        }

        private void SelfcheckForm_Load(object sender, EventArgs e) {
            foreach (Form form in _dicSubForms.Values) {
                form.TopLevel = false;
                form.FormBorderStyle = FormBorderStyle.None;
                form.Size = pnlForm.Size;
                pnlForm.Controls.Add(form);
            }
            PnlForm_Resize(pnlForm, null);
            btn1GasBox.PerformClick();
#if DEBUG
            btn2Flowmeter.Enabled = true;
            btn3Smoker.Enabled = true;
            btn4OilTemp.Enabled = true;
            btn5Tachometer.Enabled = true;
            btn6Weather.Enabled = true;
#else
            btn2Flowmeter.Enabled = false;
            btn3Smoker.Enabled = false;
            btn4OilTemp.Enabled = false;
            btn5Tachometer.Enabled = false;
            btn6Weather.Enabled = false;
#endif
            _timer.Enabled = true;
        }

        private void PnlForm_Resize(object sender, EventArgs e) {
            if (pnlForm.Controls.Count > 0) {
                foreach (var item in pnlForm.Controls) {
                    if (item is Form form && form != null) {
                        form.Size = pnlForm.Size;
                    }
                }
            }
        }

        private void Button_Click(object sender, EventArgs e) {
            if (sender is Button activebtn && _dicSubForms.ContainsKey(activebtn)) {
                foreach (Button btn in _dicSubForms.Keys) {
                    btn.Font = btnCancel.Font;
                    btn.ForeColor = Color.Black;
                    _dicSubForms[btn].Hide();
                }
                activebtn.Font = new Font(btnCancel.Font, FontStyle.Bold);
                activebtn.ForeColor = Color.Red;
                _dicSubForms[activebtn].Show();
            }
        }

        private void SelfcheckForm_Resize(object sender, EventArgs e) {
            if (_lastHeight == 0) {
                return;
            }
            float scale = Height / _lastHeight;
            layoutMain.Font = new Font(layoutMain.Font.FontFamily, layoutMain.Font.Size * scale, layoutMain.Font.Style);
            btn1GasBox.Font = new Font(btn1GasBox.Font.FontFamily, btn1GasBox.Font.Size * scale, btn1GasBox.Font.Style);
            btn2Flowmeter.Font = new Font(btn2Flowmeter.Font.FontFamily, btn2Flowmeter.Font.Size * scale, btn2Flowmeter.Font.Style);
            btn3Smoker.Font = new Font(btn3Smoker.Font.FontFamily, btn3Smoker.Font.Size * scale, btn3Smoker.Font.Style);
            btn4OilTemp.Font = new Font(btn4OilTemp.Font.FontFamily, btn4OilTemp.Font.Size * scale, btn4OilTemp.Font.Style);
            btn5Tachometer.Font = new Font(btn5Tachometer.Font.FontFamily, btn5Tachometer.Font.Size * scale, btn5Tachometer.Font.Style);
            btn6Weather.Font = new Font(btn6Weather.Font.FontFamily, btn6Weather.Font.Size * scale, btn6Weather.Font.Style);
            pnlForm.Font = new Font(pnlForm.Font.FontFamily, pnlForm.Font.Size * scale, pnlForm.Font.Style);
            _lastHeight = Height;
        }

        private void SelfcheckForm_FormClosing(object sender, FormClosingEventArgs e) {
            foreach (Form form in _dicSubForms.Values) {
                form.Close();
            }
            bool btemp = true;
            foreach (bool item in _dicResults.Values) {
                btemp = btemp && item;
            }
            if (btemp) {
                DialogResult = DialogResult.Yes;
            } else {
                DialogResult = DialogResult.No;
            }
            f_gasBoxSelfcheck.SelfcheckDone -= OnSelfcheckDone;
        }

        private void BtnSkip_Click(object sender, EventArgs e) {
            int index = 0;
            foreach (Button activebtn in _dicSubForms.Keys) {
                if (activebtn.ForeColor == Color.Red) {
                    _dicStops[_dicSubForms[activebtn]] = true;
                    for (int i = 0; i < _buttonsOrder.Length; i++) {
                        if (_buttonsOrder[i] == activebtn) {
                            index = i + 1;
                        }
                    }
                    break;
                }
            }
            if (index < _buttonsOrder.Length) {
                _dynoCmd.ReconnectServer();
                _buttonsOrder[index].Enabled = true;
                _buttonsOrder[index].PerformClick();
                _dicSubForms[_buttonsOrder[index]].AcceptButton.PerformClick();
            }
        }

        private void BtnCancel_Click(object sender, EventArgs e) {
            Close();
        }
    }

    public class SelfcheckDoneEventArgs : EventArgs {
        public bool Result { get; set; }
    }
}
