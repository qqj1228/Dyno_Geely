using Dyno_Geely.Forms;
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
    public partial class PreheatingForm : Form {
        private float _lastHeight;
        private readonly DynoCmd _dynoCmd;
        private readonly Dictionary<Form, bool> _dicResults;
        private readonly Dictionary<Button, Form> _dicSubForms;
        private readonly Button[] _buttonsOrder;
        private readonly DynoPreheatingSubForm f_dynoPreheating;
        private readonly GasBoxPreheatingSubForm f_gasBoxPreheating;
        private readonly FlowmeterPreheatingSubForm f_flowmeterPreheating;
        private readonly SmokerPreheatingSubForm f_smokerPreheating;
        private readonly WeatherPreheatingSubForm f_weatherPreheating;
        private readonly TachometerPreheatingSubForm f_tachometerPreheating;
        private readonly OilTempPreheatingSubForm f_oilTempPreheating;

        public PreheatingForm(DynoCmd dynoCmd, MainSetting mainCfg) {
            InitializeComponent();
            _lastHeight = Height;
            _dynoCmd = dynoCmd;
            _dicResults = new Dictionary<Form, bool>();
            f_dynoPreheating = new DynoPreheatingSubForm(_dynoCmd, mainCfg, _dicResults);
            f_gasBoxPreheating = new GasBoxPreheatingSubForm(_dynoCmd, mainCfg, _dicResults);
            f_flowmeterPreheating = new FlowmeterPreheatingSubForm(_dynoCmd, mainCfg, _dicResults);
            f_smokerPreheating = new SmokerPreheatingSubForm(_dynoCmd, mainCfg, _dicResults);
            f_weatherPreheating = new WeatherPreheatingSubForm(_dynoCmd, mainCfg, _dicResults);
            f_tachometerPreheating = new TachometerPreheatingSubForm(_dynoCmd, mainCfg, _dicResults);
            f_oilTempPreheating = new OilTempPreheatingSubForm(_dynoCmd, mainCfg, _dicResults);

            f_dynoPreheating.PreheatingDone += OnPreheatingDone;
            f_gasBoxPreheating.PreheatingDone += OnPreheatingDone;
            f_flowmeterPreheating.PreheatingDone += OnPreheatingDone;
            f_smokerPreheating.PreheatingDone += OnPreheatingDone;
            f_weatherPreheating.PreheatingDone += OnPreheatingDone;
            f_tachometerPreheating.PreheatingDone += OnPreheatingDone;
            f_oilTempPreheating.PreheatingDone += OnPreheatingDone;

            _dicResults.Add(f_dynoPreheating, false);
            _dicResults.Add(f_gasBoxPreheating, false);
            _dicResults.Add(f_flowmeterPreheating, false);
            _dicResults.Add(f_smokerPreheating, false);
            _dicResults.Add(f_weatherPreheating, false);
            _dicResults.Add(f_tachometerPreheating, false);
            _dicResults.Add(f_oilTempPreheating, false);

            _buttonsOrder = new Button[] { btn1Dyno, btn2GasBox, btn3Flowmeter, btn4Smoker, btn5Weather, btn6Tacho, btn7Oil };

            _dicSubForms = new Dictionary<Button, Form> {
                { btn1Dyno, f_dynoPreheating },
                { btn2GasBox, f_gasBoxPreheating },
                { btn3Flowmeter, f_flowmeterPreheating },
                { btn4Smoker, f_smokerPreheating },
                { btn5Weather, f_weatherPreheating },
                { btn6Tacho, f_tachometerPreheating },
                { btn7Oil, f_oilTempPreheating }
            };
        }

        private void OnPreheatingDone(object sender, PreheatingDoneEventArgs e) {
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
                            _buttonsOrder[index].PerformClick();
                        });
                    } else {
                        Close();
                    }
                }
            }
        }

        private void PreheatingForm_Load(object sender, EventArgs e) {
            foreach (Form form in _dicSubForms.Values) {
                form.TopLevel = false;
                form.FormBorderStyle = FormBorderStyle.None;
                form.Size = pnlForm.Size;
                pnlForm.Controls.Add(form);
            }
            PnlForm_Resize(pnlForm, null);
            btn1Dyno.PerformClick();
        }

        private void PreheatingForm_Resize(object sender, EventArgs e) {
            if (_lastHeight == 0) {
                return;
            }
            float scale = Height / _lastHeight;
            layoutMain.Font = new Font(layoutMain.Font.FontFamily, layoutMain.Font.Size * scale, layoutMain.Font.Style);
            btn1Dyno.Font = new Font(btn1Dyno.Font.FontFamily, btn1Dyno.Font.Size * scale, btn1Dyno.Font.Style);
            btn2GasBox.Font = new Font(btn2GasBox.Font.FontFamily, btn2GasBox.Font.Size * scale, btn2GasBox.Font.Style);
            btn3Flowmeter.Font = new Font(btn3Flowmeter.Font.FontFamily, btn3Flowmeter.Font.Size * scale, btn3Flowmeter.Font.Style);
            btn4Smoker.Font = new Font(btn4Smoker.Font.FontFamily, btn4Smoker.Font.Size * scale, btn4Smoker.Font.Style);
            btn5Weather.Font = new Font(btn5Weather.Font.FontFamily, btn5Weather.Font.Size * scale, btn5Weather.Font.Style);
            btn6Tacho.Font = new Font(btn6Tacho.Font.FontFamily, btn6Tacho.Font.Size * scale, btn6Tacho.Font.Style);
            btn7Oil.Font = new Font(btn7Oil.Font.FontFamily, btn7Oil.Font.Size * scale, btn7Oil.Font.Style);
            pnlForm.Font = new Font(pnlForm.Font.FontFamily, pnlForm.Font.Size * scale, pnlForm.Font.Style);
            _lastHeight = Height;
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

        private void PnlForm_Resize(object sender, EventArgs e) {
            if (pnlForm.Controls.Count > 0) {
                foreach (var item in pnlForm.Controls) {
                    if (item is Form form && form != null) {
                        form.Size = pnlForm.Size;
                    }
                }
            }
        }

        private void PreheatingForm_FormClosing(object sender, FormClosingEventArgs e) {
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
        }

    }

    public class PreheatingDoneEventArgs : EventArgs {
        public bool Result { get; set; }
    }

}
