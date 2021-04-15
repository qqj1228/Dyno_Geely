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
    public partial class SelfcheckForm : Form {
        private float _lastHeight;
        private readonly DynoCmd _dynoCmd;
        private readonly Dictionary<Form, bool> _dicResults;
        private readonly Dictionary<Form, bool> _dicStops;
        private readonly Dictionary<Button, Form> _dicSubForms;
        private readonly Button[] _buttonsOrder;
        private readonly GasBoxSelfcheckSubForm f_gasBoxSelfcheck;
        private readonly FlowmeterSelfcheckSubForm f_flowmeterSelfcheck;
        private readonly SmokerSelfcheckSubForm f_smokerSelfcheck;
        private readonly OilTempSelfcheckSubForm f_oilTempSelfcheck;
        private readonly TachometerSelfcheckSubForm f_tachometerSelfcheck;
        private readonly WeatherSelfcheckSubForm f_weatherSelfcheck;
        private readonly EnvironmentData _envData;
        private readonly List<bool> _selfChecks;

        public SelfcheckForm(DynoCmd dynoCmd, MainSetting mainCfg, List<bool> selfChecks, EnvironmentData envData, bool bDiesel) {
            InitializeComponent();
            _lastHeight = Height;
            _dynoCmd = dynoCmd;
            _dicResults = new Dictionary<Form, bool>();
            _dicStops = new Dictionary<Form, bool>();
            _envData = envData;
            _selfChecks = selfChecks;

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

            _dicResults.Add(f_gasBoxSelfcheck, !selfChecks[0]);
            _dicResults.Add(f_flowmeterSelfcheck, !selfChecks[1]);
            _dicResults.Add(f_smokerSelfcheck, !selfChecks[2]);
            _dicResults.Add(f_oilTempSelfcheck, !selfChecks[3]);
            _dicResults.Add(f_tachometerSelfcheck, !selfChecks[4]);
            _dicResults.Add(f_weatherSelfcheck, !selfChecks[5]);

            _dicStops.Add(f_gasBoxSelfcheck, false);
            _dicStops.Add(f_flowmeterSelfcheck, false);
            _dicStops.Add(f_smokerSelfcheck, false);
            _dicStops.Add(f_oilTempSelfcheck, false);
            _dicStops.Add(f_tachometerSelfcheck, false);
            _dicStops.Add(f_weatherSelfcheck, false);

            _buttonsOrder = new Button[] { btn1GasBox, btn2Flowmeter, btn3Smoker, btn4OilTemp, btn5Tachometer, btn6Weather };
            for (int i = 0; i < selfChecks.Count; i++) {
                if (_buttonsOrder.Length > i) {
                    _buttonsOrder[i].Enabled = selfChecks[i];
                }
            }

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
                    int preIndex = 0;
                    for (int i = 0; i < _buttonsOrder.Length; i++) {
                        if (_buttonsOrder[i] == keys.FirstOrDefault()) {
                            index = i + 1;
                            preIndex = i;
                        }
                    }
                    for (int i = index; i < _selfChecks.Count; i++) {
                        if (!_selfChecks[i]) {
                            index++;
                        } else {
                            break;
                        }
                    }
                    if (index < _buttonsOrder.Length) {
                        Invoke((EventHandler)delegate {
                            _buttonsOrder[preIndex].BackColor = Color.Lime;
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

        private void SelfcheckForm_Load(object sender, EventArgs e) {
            foreach (Form form in _dicSubForms.Values) {
                form.TopLevel = false;
                form.FormBorderStyle = FormBorderStyle.None;
                form.Size = pnlForm.Size;
                pnlForm.Controls.Add(form);
            }
            PnlForm_Resize(pnlForm, null);
#if DEBUG
            btn1GasBox.Enabled = _selfChecks[0];
            btn2Flowmeter.Enabled = _selfChecks[1];
            btn3Smoker.Enabled = _selfChecks[2];
            btn4OilTemp.Enabled = _selfChecks[3];
            btn5Tachometer.Enabled = _selfChecks[4];
            btn6Weather.Enabled = _selfChecks[5];
#else
            btn1GasBox.Enabled = false;
            btn2Flowmeter.Enabled = false;
            btn3Smoker.Enabled = false;
            btn4OilTemp.Enabled = false;
            btn5Tachometer.Enabled = false;
            btn6Weather.Enabled = false;
#endif
            for (int i = 0; i < _selfChecks.Count; i++) {
                if (_selfChecks[i]) {
                    _buttonsOrder[i].Enabled = true;
                    _buttonsOrder[i].PerformClick();
                    _dicSubForms[_buttonsOrder[i]].AcceptButton.PerformClick();
                    break;
                }
            }
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

        private void BtnBeamDown_Click(object sender, EventArgs e) {
            btnBeamDown.Enabled = false;
            btnBeamUp.Enabled = true;
            if (!_dynoCmd.DynoBeamDownCmd(out string errMsg)) {
                MessageBox.Show("执行举升下降命令失败", "执行命令出错", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnBeamUp_Click(object sender, EventArgs e) {
            btnBeamDown.Enabled = true;
            btnBeamUp.Enabled = false;
            if (!_dynoCmd.DynoBeamUpCmd(out string errMsg)) {
                MessageBox.Show("执行举升上升命令失败", "执行命令出错", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
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
            for (int i = index; i < _selfChecks.Count; i++) {
                if (!_selfChecks[i]) {
                    index++;
                } else {
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
