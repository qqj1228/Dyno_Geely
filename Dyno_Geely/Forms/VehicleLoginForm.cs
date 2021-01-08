using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO.Ports;
using System.Linq;
using System.Management;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Dyno_Geely {
    public partial class VehicleLoginForm : Form {
        private readonly ModelLocal _db;
        private readonly MainSetting _mainCfg;
        private readonly Logger _log;
        private readonly SerialPortClass _sp;
        private string _serialRecvBuf;
        private float _lastHeight;
        public VehicleInfo VI { get; set; }
        public EmissionInfo EI { get; set; }

        public VehicleLoginForm(ModelLocal db, MainSetting mainCfg, Logger log) {
            InitializeComponent();
            _lastHeight = this.Height;
            _db = db;
            _mainCfg = mainCfg;
            _log = log;
            if (_mainCfg.ScannerPort.Length > 0) {
                _sp = new SerialPortClass(
                    _mainCfg.ScannerPort,
                    _mainCfg.ScannerBaud,
                    Parity.None,
                    8,
                    StopBits.One
                );
                try {
                    _sp.OpenPort();
                    _sp.DataReceived += new SerialPortClass.SerialPortDataReceiveEventArgs(SerialDataReceived);
                } catch (Exception ex) {
                    _log.TraceError("Open serial port error: " + ex.Message);
                    MessageBox.Show("打开串口扫码枪出错", "初始化错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            _serialRecvBuf = string.Empty;
            VI = new VehicleInfo();
            EI = new EmissionInfo();
        }

        void SerialDataReceived(object sender, SerialDataReceivedEventArgs e, byte[] bits) {
            Control con = this.ActiveControl;
            if (con is TextBox tb) {
                _serialRecvBuf += Encoding.Default.GetString(bits);
                if (_serialRecvBuf.Contains("\n")) {
                    _serialRecvBuf = _serialRecvBuf.Trim();
                    this.Invoke((EventHandler)delegate {
                        this.txtBoxVIN.Text = _serialRecvBuf;
                    });
                    if (_serialRecvBuf.Length == 17) {
                        VI.VIN = _serialRecvBuf;
                        _db.GetEmissionInfo(VI.VIN, EI);
                        FillInputTextBox();
                        _serialRecvBuf = string.Empty;
                        if (chkBoxAutoStart.Checked) {
                            btnStart.PerformClick();
                        }
                    }
                }
            }
        }

        private void FillInputTextBox() {
            foreach (Control item in layoutMain.Controls) {
                if (item is TextBox txtBox && txtBox.Name != "txtBoxVIN") {
                    txtBox.Text = string.Empty;
                    txtBox.BackColor = txtBoxVIN.BackColor;
                }
                if (item is ComboBox cmbBox && item.Name == "cmbBoxTestMethod") {
                    cmbBox.SelectedIndex = 5;
                    cmbBox.BackColor = txtBoxVIN.BackColor;
                }
            }
            txtBoxGettedVIN.Text = VI.VIN;
            txtBoxVehicleModel.Text = EI.VehicleModel;
            txtBoxOpenInfoSN.Text = EI.OpenInfoSN;
            txtBoxVehicleMfr.Text = EI.VehicleMfr;
            txtBoxEngineModel.Text = EI.EngineModel;
            txtBoxEngineSN.Text = EI.EngineSN;
            txtBoxEngineMfr.Text = EI.EngineMfr;
            txtBoxEngineVolume.Text = EI.EngineVolume;
            txtBoxCylinderQTY.Text = EI.CylinderQTY;
            txtBoxFuelSupply.Text = EI.FuelSupply;
            txtBoxRatedPower.Text = EI.RatedPower;
            txtBoxRatedRPM.Text = EI.RatedRPM.ToString();
            txtBoxEmissionStage.Text = EI.EmissionStage;
            txtBoxTransmission.Text = EI.Transmission;
            txtBoxCatConverter.Text = EI.CatConverter;
            txtBoxRefMass.Text = EI.RefMass;
            txtBoxMaxMass.Text = EI.MaxMass;
            txtBoxOBDLocation.Text = EI.OBDLocation;
            txtBoxPostProcessing.Text = EI.PostProcessing;
            txtBoxPostProcessor.Text = EI.PostProcessor;
            txtBoxMotorModel.Text = EI.MotorModel;
            txtBoxEnergyStorage.Text = EI.EnergyStorage;
            txtBoxBatteryCap.Text = EI.BatteryCap;
            cmbBoxTestMethod.SelectedIndex = EI.TestMethod;
            if (EI.Name != null && EI.Name.Length > 0) {
                cmbBoxName.Text = EI.Name;
            } else {
                EI.Name = cmbBoxName.Text;
            }
        }

        private void FillEmissionInfo() {
            VI.VehicleModel = txtBoxVehicleModel.Text;
            VI.OpenInfoSN = txtBoxOpenInfoSN.Text;
            EI.VehicleModel = txtBoxVehicleModel.Text;
            EI.OpenInfoSN = txtBoxOpenInfoSN.Text;
            EI.VehicleMfr = txtBoxVehicleMfr.Text;
            EI.EngineModel = txtBoxEngineModel.Text;
            EI.EngineSN = txtBoxEngineSN.Text;
            EI.EngineMfr = txtBoxEngineMfr.Text;
            EI.EngineVolume = txtBoxEngineVolume.Text;
            EI.CylinderQTY = txtBoxCylinderQTY.Text;
            EI.FuelSupply = txtBoxFuelSupply.Text;
            EI.RatedPower = txtBoxRatedPower.Text;
            EI.RatedRPM = Convert.ToInt32(txtBoxRatedRPM.Text);
            EI.EmissionStage = txtBoxEmissionStage.Text;
            EI.Transmission = txtBoxTransmission.Text;
            EI.CatConverter = txtBoxCatConverter.Text;
            EI.RefMass = txtBoxRefMass.Text;
            EI.MaxMass = txtBoxMaxMass.Text;
            EI.OBDLocation = txtBoxOBDLocation.Text;
            EI.PostProcessing = txtBoxPostProcessing.Text;
            EI.PostProcessor = txtBoxPostProcessor.Text;
            EI.MotorModel = txtBoxMotorModel.Text;
            EI.EnergyStorage = txtBoxEnergyStorage.Text;
            EI.BatteryCap = txtBoxBatteryCap.Text;
            EI.TestMethod = cmbBoxTestMethod.SelectedIndex;
            EI.Name = cmbBoxName.Text;
        }

        private void TxtBoxRatedRPM_KeyPress(object sender, KeyPressEventArgs e) {
            if (e.KeyChar != (char)Keys.Back && !char.IsDigit(e.KeyChar)) {
                e.Handled = true;
            }
        }

        private void VehicleLoginForm_Load(object sender, EventArgs e) {
            cmbBoxTestMethod.Items.Add("免检");
            cmbBoxTestMethod.Items.Add("✓双怠速法✓");
            cmbBoxTestMethod.Items.Add("✓稳态工况法✓");
            cmbBoxTestMethod.Items.Add("✓简易瞬态工况法✓");
            cmbBoxTestMethod.Items.Add("✓加载减速法✓");
            cmbBoxTestMethod.Items.Add("---------");
            cmbBoxTestMethod.Items.Add("✓自由加速法✓");
            cmbBoxTestMethod.Items.Add("林格曼黑度法");
            cmbBoxTestMethod.Items.Add("瞬态工况法");
            cmbBoxTestMethod.Items.Add("其它");
            cmbBoxTestMethod.SelectedIndex = 5;
            cmbBoxName.Items.Add(_mainCfg.Name);
            cmbBoxName.SelectedIndex = 0;
            chkBoxAutoStart.Checked = true;
        }

        private void VehicleLoginForm_Activated(object sender, EventArgs e) {
            // 若要设置某个控件默认获取焦点，应该在窗体的Activated事件中执行Focus()方法。而不是写在Load()事件中
            txtBoxVIN.Focus();
        }

        private void TxtBoxVIN_KeyPress(object sender, KeyPressEventArgs e) {
            if (e.KeyChar == (char)Keys.Enter) {
                TextBox tb = sender as TextBox;
                if (tb.TextLength == 17) {
                    txtBoxRatedRPM.BackColor = txtBoxGettedVIN.BackColor;
                    cmbBoxTestMethod.BackColor = txtBoxGettedVIN.BackColor;
                    VI.VIN = tb.Text;
                    _db.GetEmissionInfo(VI.VIN, EI);
                    FillInputTextBox();
                    this.txtBoxVIN.Clear();
                    if (chkBoxAutoStart.Checked) {
                        btnStart.PerformClick();
                    }
                }
            }
        }

        private void VehicleLoginForm_FormClosing(object sender, FormClosingEventArgs e) {
            if (_sp != null) {
                _sp.ClosePort();
                _sp.DataReceived -= new SerialPortClass.SerialPortDataReceiveEventArgs(SerialDataReceived);
            }
        }

        private void BtnSave_Click(object sender, EventArgs e) {
            FillEmissionInfo();
            _db.SaveEmissionInfo(txtBoxGettedVIN.Text, EI);
        }

        private void BtnStart_Click(object sender, EventArgs e) {
            bool bCanTest = true;
            bCanTest = bCanTest && txtBoxGettedVIN.TextLength > 0;
            bCanTest = bCanTest && txtBoxVehicleModel.TextLength > 0;
            bCanTest = bCanTest && txtBoxOpenInfoSN.TextLength > 0;
            bCanTest = bCanTest && txtBoxRatedRPM.TextLength > 0;
            bCanTest = bCanTest && cmbBoxTestMethod.SelectedIndex != 5;
            if (bCanTest) {
                DialogResult = DialogResult.OK;
                Close();
            } else {
                if (txtBoxGettedVIN.TextLength <= 0) {
                    txtBoxGettedVIN.BackColor = Color.Yellow;
                }
                if (txtBoxVehicleModel.TextLength <= 0) {
                    txtBoxVehicleModel.BackColor = Color.Yellow;
                }
                if (txtBoxOpenInfoSN.TextLength <= 0) {
                    txtBoxOpenInfoSN.BackColor = Color.Yellow;
                }
                if (txtBoxRatedRPM.TextLength <= 0) {
                    txtBoxRatedRPM.BackColor = Color.Yellow;
                }
                if (cmbBoxTestMethod.SelectedIndex == 5) {
                    cmbBoxTestMethod.BackColor = Color.Yellow;
                }
                MessageBox.Show("车辆登录表单有必填项未填写", "车辆登录", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void VehicleLoginForm_Resize(object sender, EventArgs e) {
            if (_lastHeight == 0) {
                return;
            }
            float scale = this.Height / _lastHeight;
            layoutMain.Font = new Font(layoutMain.Font.FontFamily, layoutMain.Font.Size * scale, layoutMain.Font.Style);
            txtBoxVIN.Font = new Font(txtBoxVIN.Font.FontFamily, txtBoxVIN.Font.Size * scale, txtBoxVIN.Font.Style);
            layoutControlItem31.Style.Font = new Font(layoutControlItem31.Style.Font.FontFamily, layoutControlItem31.Style.Font.Size * scale, layoutControlItem31.Style.Font.Style);
            layoutControlItem31.TextSize = new Size(layoutControlItem31.TextSize.Width, layoutControlItem31.TextSize.Height * Convert.ToInt32(scale));
            _lastHeight = this.Height;
        }
    }
}
