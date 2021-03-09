using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO.Ports;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace Dyno_Geely {
    public partial class VehicleLoginForm : Form {
        private readonly ModelLocal _db;
        private readonly DynoCmd _dynoCmd;
        private readonly MainSetting _mainCfg;
        private readonly Logger _log;
        private readonly SerialPortClass _sp;
        private readonly DynoParamRecv _dynoParamRecver;
        private readonly ManualResetEvent _dynoParamRecvFlag;
        private string _serialRecvBuf;
        private float _lastHeight;
        private int _carID;
        public VehicleInfo VI { get; set; }
        public EmissionInfo EI { get; set; }

        public VehicleLoginForm(ModelLocal db, DynoCmd dynoCmd, MainSetting mainCfg, Logger log) {
            InitializeComponent();
            _lastHeight = Height;
            _db = db;
            _dynoCmd = dynoCmd;
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
                    _sp.DataReceived += SerialDataReceived;
                } catch (Exception ex) {
                    _log.TraceError("Open serial port error: " + ex.Message);
                    MessageBox.Show("打开串口扫码枪出错", "初始化错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            _serialRecvBuf = string.Empty;
            if (_mainCfg.DynoParamIP.Length > 0) {
                try {
                    _dynoParamRecver = new DynoParamRecv(_mainCfg.DynoParamIP, _mainCfg.DynoParamPort);
                    _dynoParamRecver.DynoParamRecvEvent += OnDynoParamRecv;
                    _dynoParamRecvFlag = new ManualResetEvent(true);
                } catch (Exception ex) {
                    _log.TraceError("Connect to dyno parameter server error: " + ex.Message);
                    MessageBox.Show("无法连接到测功机参数服务端软件", "初始化错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            VI = new VehicleInfo();
            EI = new EmissionInfo();
            _carID = -1;
        }

        void SerialDataReceived(object sender, SerialDataReceivedEventArgs e, byte[] bits) {
            Control con = ActiveControl;
            if (con is TextBox tb) {
                _serialRecvBuf += Encoding.Default.GetString(bits);
                if (_serialRecvBuf.Contains("\n")) {
                    _serialRecvBuf = _serialRecvBuf.Trim();
                    Invoke((EventHandler)delegate {
                        txtBoxVIN.Text = _serialRecvBuf;
                    });
                    if (_serialRecvBuf.Length == 17) {
                        VI.VIN = _serialRecvBuf;
                        if (_mainCfg.DynoParamIP.Length > 0) {
                            if (_dynoParamRecver != null) {
                                _dynoParamRecver.SendVIN(VI.VIN);
                                _dynoParamRecvFlag.Reset();
                                LoadingForm frmLoading = new LoadingForm();
                                frmLoading.BackgroundWorkAction = () => {
                                    frmLoading.CurrentMsg = new KeyValuePair<int, string>(50, "正在从MES获取测功机参数。。。");
                                    _dynoParamRecvFlag.WaitOne(_mainCfg.RecvTimeout, false);
                                    frmLoading.CurrentMsg = new KeyValuePair<int, string>(50, "测功机参数获取结束");
                                };
                                frmLoading.ShowDialog();
                            } else {
                                _log.TraceError("_dynoParamRecver is null");
                                MessageBox.Show("测功机参数服务端软件接口未实例化", "初始化错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        } else {
                            _carID = _db.GetEmissionInfoFromVIN(VI.VIN, EI);
                            FillInputTextBox();
                        }
                        _serialRecvBuf = string.Empty;
                        if (chkBoxAutoStart.Checked) {
                            btnStart.PerformClick();
                        }
                    }
                }
            }
        }

        private void OnDynoParamRecv(object sender, DynoParamRecvEventArgs e) {
            _dynoParamRecvFlag.Set();
            if (e.Code == "200") {
                EI = JsonConvert.DeserializeObject<EmissionInfo>(e.Msg);
                VI.VehicleModel = EI.VehicleModel;
                EI.OpenInfoSN = string.Empty;
                VI.OpenInfoSN = string.Empty;
                EI.EngineSN = string.Empty;
                EI.Name = string.Empty;
            } else {
                _log.TraceError(string.Format("Get dyno parameter from MES error, Code: {0}, Error: {1}", e.Code, e.Msg));
                MessageBox.Show(string.Format("从MES获取测功机参数失败, Code: {0}, Error: {1}", e.Code, e.Msg),
                    "获取测功机参数", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            Invoke((EventHandler)delegate {
                FillInputTextBox();
            });
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
            if (_mainCfg.DynoParamIP.Length <= 0) {
                cmbBoxSelectVehicleModel.SelectedItem = EI.VehicleModel;
            }
            txtBoxVehicleModel.Text = EI.VehicleModel;
            txtBoxOpenInfoSN.Text = EI.OpenInfoSN;
            txtBoxVehicleMfr.Text = EI.VehicleMfr;
            txtBoxEngineModel.Text = EI.EngineModel;
            txtBoxEngineSN.Text = EI.EngineSN;
            txtBoxEngineMfr.Text = EI.EngineMfr;
            txtBoxEngineVolume.Text = EI.EngineVolume.ToString("F");
            txtBoxCylinderQTY.Text = EI.CylinderQTY.ToString("");
            cmbBoxFuelSupply.SelectedIndex = EI.FuelSupply;
            txtBoxRatedPower.Text = EI.RatedPower.ToString("F");
            txtBoxRatedRPM.Text = EI.RatedRPM.ToString();
            cmbBoxEmissionStage.SelectedIndex = EI.EmissionStage;
            cmbBoxTransmission.SelectedIndex = EI.Transmission;
            txtBoxCatConverter.Text = EI.CatConverter;
            txtBoxRefMass.Text = EI.RefMass.ToString("");
            txtBoxMaxMass.Text = EI.MaxMass.ToString("");
            txtBoxOBDLocation.Text = EI.OBDLocation;
            txtBoxPostProcessorType.Text = EI.PostProcessorType;
            txtBoxPostProcessorModel.Text = EI.PostProcessorModel;
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
            EI.EngineVolume = Convert.ToDouble(txtBoxEngineVolume.Text);
            EI.CylinderQTY = Convert.ToInt32(txtBoxCylinderQTY.Text);
            EI.FuelSupply = cmbBoxFuelSupply.SelectedIndex;
            EI.RatedPower = Convert.ToDouble(txtBoxRatedPower.Text);
            EI.RatedRPM = Convert.ToInt32(txtBoxRatedRPM.Text);
            EI.EmissionStage = cmbBoxEmissionStage.SelectedIndex;
            EI.Transmission = cmbBoxTransmission.SelectedIndex;
            EI.CatConverter = txtBoxCatConverter.Text;
            EI.RefMass = Convert.ToInt32(txtBoxRefMass.Text);
            EI.MaxMass = Convert.ToInt32(txtBoxMaxMass.Text);
            EI.OBDLocation = txtBoxOBDLocation.Text;
            EI.PostProcessorType = txtBoxPostProcessorType.Text;
            EI.PostProcessorModel = txtBoxPostProcessorModel.Text;
            EI.MotorModel = txtBoxMotorModel.Text;
            EI.EnergyStorage = txtBoxEnergyStorage.Text;
            EI.BatteryCap = txtBoxBatteryCap.Text;
            EI.TestMethod = cmbBoxTestMethod.SelectedIndex;
            EI.Name = cmbBoxName.Text;
        }

        private void SetNewVehicle(NewVehicle newVehicle) {
            newVehicle.CarId = _db.GetLastNewVehicleID() + 1;
            newVehicle.VIN = VI.VIN;
            newVehicle.CLXH = EI.VehicleModel;
            newVehicle.ZZL = EI.MaxMass.ToString();
            newVehicle.JZZL = EI.RefMass.ToString();
            newVehicle.FDJXH = EI.EngineModel;
            newVehicle.FDJH = EI.EngineSN;
            newVehicle.GYFS = EI.GetFuelSupplyString();
            newVehicle.EDGL = EI.RatedPower.ToString("F");
            newVehicle.EDZS = EI.RatedRPM.ToString();
            newVehicle.FDJSCQY = EI.EngineMfr;
            newVehicle.FDJPL = EI.EngineVolume.ToString("F");
            newVehicle.PFSP = EI.GetEmissionStageString();
            newVehicle.QDDJXH = EI.MotorModel;
            newVehicle.CNZZXH = EI.EnergyStorage;
            newVehicle.DCRL = EI.BatteryCap;
            newVehicle.HasOBD = EI.OBDLocation.Length <= 0 || EI.OBDLocation.Contains("无") || EI.OBDLocation.Contains("没有") ? "无" : "有";
            newVehicle.CHQXH = EI.CatConverter;
            string[] PostProcessings = EI.PostProcessorType.Split(new char[] { '+' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (string item in PostProcessings) {
                if (item.Contains("DPF")) {
                    newVehicle.DPF = item;
                }
                if (item.Contains("SCR")) {
                    newVehicle.SCR = item;
                }
            }
            string[] PostProcessors = EI.PostProcessorModel.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (string item in PostProcessors) {
                if (item.Contains("DPF")) {
                    int index = item.IndexOf(':');
                    if (index < 0) {
                        newVehicle.DPFXH = item;
                    } else {
                        newVehicle.DPFXH = item.Substring(index + 1);
                    }
                }
                if (item.Contains("SCR")) {
                    int index = item.IndexOf(':');
                    if (index < 0) {
                        newVehicle.SRCXH = item;
                    } else {
                        newVehicle.SRCXH = item.Substring(index + 1);
                    }
                }
            }
            switch (EI.TestMethod) {
            case 0:
                newVehicle.CheckMethod = "免检";
                break;
            case 1:
                newVehicle.CheckMethod = "双怠速";
                break;
            case 2:
                newVehicle.CheckMethod = "稳态工况";
                break;
            case 3:
                newVehicle.CheckMethod = "简易瞬态";
                break;
            case 4:
                newVehicle.CheckMethod = "加载减速";
                break;
            case 6:
                newVehicle.CheckMethod = "自由加速";
                break;
            case 7:
                newVehicle.CheckMethod = "林格曼黑度";
                break;
            case 8:
                newVehicle.CheckMethod = "瞬态工况";
                break;
            case 9:
                newVehicle.CheckMethod = "其他";
                break;
            }
            newVehicle.CheckType = _db.GetVINCountFromNewVehicle(VI.VIN) <= 0 ? "初检" : "复检";
        }

        private void SetUseVehicle(UseVehicle useVehicle) {
            useVehicle.VIN = VI.VIN;
            useVehicle.CarId = _db.GetLastNewVehicleID() + 1;
            useVehicle.HPHM = string.Empty;
            useVehicle.HPYS = string.Empty;
            useVehicle.CLXH = EI.VehicleModel;
            useVehicle.JZZL = EI.RefMass.ToString();
            useVehicle.ZDZZL = EI.MaxMass.ToString();
            useVehicle.FDJXH = EI.EngineModel;
            useVehicle.FDJHM = EI.EngineSN;
            useVehicle.FDJPL = EI.EngineVolume.ToString("F");
            useVehicle.EDZS = EI.RatedRPM.ToString();
            useVehicle.FDJEDGL = EI.RatedPower.ToString("F");

            string[] PostProcessorTypes = EI.PostProcessorType.Split(new char[] { '+' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (string item in PostProcessorTypes) {
                if (item.Contains("DPF")) {
                    useVehicle.DPF = item;
                }
                if (item.Contains("SCR")) {
                    useVehicle.SRC = item;
                }
            }
            string[] PostProcessorModels = EI.PostProcessorModel.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (string item in PostProcessorModels) {
                if (item.Contains("DPF")) {
                    int index = item.IndexOf(':');
                    if (index < 0) {
                        useVehicle.DPFXH = item;
                    } else {
                        useVehicle.DPFXH = item.Substring(index + 1);
                    }
                }
                if (item.Contains("SCR")) {
                    int index = item.IndexOf(':');
                    if (index < 0) {
                        useVehicle.SRCXH = item;
                    } else {
                        useVehicle.SRCXH = item.Substring(index + 1);
                    }
                }
            }

            useVehicle.QGS = EI.CylinderQTY.ToString();
            useVehicle.QDDJXH = EI.MotorModel;
            useVehicle.CNZZXH = EI.EnergyStorage;
            useVehicle.DCRL = EI.BatteryCap;
            useVehicle.CLSCQY = EI.VehicleMfr;
            useVehicle.CLCCRQ = string.Empty;
            useVehicle.LJXSLC = string.Empty;
            useVehicle.CZXMorDW = string.Empty;
            useVehicle.LXDH = string.Empty;

            switch (EI.TestMethod) {
            case 4:
            case 6:
            case 7:
                useVehicle.RYLX = "柴油";
                break;
            default:
                useVehicle.RYLX = "汽油";
                break;
            }

            useVehicle.GYFS = EI.GetFuelSupplyString();
            useVehicle.QDFS = string.Empty;
            useVehicle.PPorXH = string.Empty;
            useVehicle.BSQXS = EI.GetTransmissionString();
            useVehicle.SYXZ = string.Empty;
            useVehicle.CCDJRI = string.Empty;
            useVehicle.HasOBD = EI.OBDLocation.Length <= 0 || EI.OBDLocation.Contains("无") || EI.OBDLocation.Contains("没有") ? "无" : "有";

            switch (EI.TestMethod) {
            case 0:
                useVehicle.CheckMethod = "免检";
                break;
            case 1:
                useVehicle.CheckMethod = "双怠速";
                break;
            case 2:
                useVehicle.CheckMethod = "稳态工况";
                break;
            case 3:
                useVehicle.CheckMethod = "简易瞬态";
                break;
            case 4:
                useVehicle.CheckMethod = "加载减速";
                break;
            case 6:
                useVehicle.CheckMethod = "自由加速";
                break;
            case 7:
                useVehicle.CheckMethod = "林格曼黑度";
                break;
            case 8:
                useVehicle.CheckMethod = "瞬态工况";
                break;
            case 9:
                useVehicle.CheckMethod = "其他";
                break;
            }

            useVehicle.CheckType = _db.GetVINCountFromNewVehicle(VI.VIN) <= 0 ? "初检" : "复检";
            useVehicle.CLLX = string.Empty;
            useVehicle.CXXL = string.Empty;
            useVehicle.CSYS = string.Empty;
            useVehicle.ZKRS = string.Empty;
            useVehicle.DPH = string.Empty;
            useVehicle.LTQY = string.Empty;
            useVehicle.SYZT = string.Empty;
            useVehicle.DWS = string.Empty;
            useVehicle.LCBDS = string.Empty;
            useVehicle.DCZZ = string.Empty;
            useVehicle.JCZQ = string.Empty;
            useVehicle.SFSYGYYB = string.Empty;
            useVehicle.PFSP = EI.GetEmissionStageString();
            useVehicle.CLS = string.Empty;
            useVehicle.JQFS = string.Empty;
            useVehicle.HPZL = string.Empty;
            useVehicle.CCS = string.Empty;
            useVehicle.FDJZZCJ = EI.EngineMfr;
            useVehicle.EDNJ = string.Empty;
            useVehicle.EDNJZS = string.Empty;
            useVehicle.PQCLZZ = string.Empty;
            useVehicle.PQGSL = string.Empty;
            useVehicle.SFSYSRL = string.Empty;
            useVehicle.CHQXH = EI.CatConverter;
            useVehicle.ZRCL = string.Empty;
            useVehicle.JRCZ = string.Empty;
            useVehicle.YQBF = string.Empty;
            useVehicle.CZLX = string.Empty;
            useVehicle.CLLB = string.Empty;
            useVehicle.CZDZ = string.Empty;
            useVehicle.XQBH = string.Empty;
            useVehicle.SFYSYCHQ = string.Empty;
            useVehicle.ZBZL = string.Empty;
            useVehicle.CheckStatus = "待检";
        }

        private void TxtBoxRatedRPM_KeyPress(object sender, KeyPressEventArgs e) {
            if (e.KeyChar != (char)Keys.Back && !char.IsDigit(e.KeyChar)) {
                e.Handled = true;
            }
        }

        private void VehicleLoginForm_Load(object sender, EventArgs e) {
            cmbBoxName.Items.Add(_mainCfg.Name);
            cmbBoxName.SelectedIndex = 0;

            if (_mainCfg.DynoParamIP.Length > 0) {
                lblDynoParamMode.Text += "MES";
                cmbBoxSelectVehicleModel.Enabled = false;
                chkBoxNewVehicleModel.Enabled = false;
            } else {
                cmbBoxSelectVehicleModel.Items.AddRange(_db.GetVehicleModels());
                lblDynoParamMode.Text += "本地";
            }

            foreach (string item in EI.GetFuelSupplyStrings()) {
                cmbBoxFuelSupply.Items.Add(item);
            }
            cmbBoxFuelSupply.SelectedIndex = 0;

            foreach (string item in EI.GetEmissionStageStrings()) {
                cmbBoxEmissionStage.Items.Add(item);
            }
            cmbBoxEmissionStage.SelectedIndex = 0;

            foreach (string item in EI.GetTransmissionStrings()) {
                cmbBoxTransmission.Items.Add(item);
            }
            cmbBoxTransmission.SelectedIndex = 0;

            foreach (string item in EI.GetTestMethodStrings()) {
                cmbBoxTestMethod.Items.Add(item);
            }
            cmbBoxTestMethod.SelectedIndex = 5;

            chkBoxNewVehicleModel.Checked = false;
            UpdateVehicleModelUI();
            chkBoxAutoStart.Checked = true;
        }

        private void UpdateVehicleModelUI() {
            txtBoxVehicleModel.Enabled = chkBoxNewVehicleModel.Checked;
            txtBoxOpenInfoSN.Enabled = chkBoxNewVehicleModel.Checked;
            txtBoxVehicleMfr.Enabled = chkBoxNewVehicleModel.Checked;
            txtBoxEngineModel.Enabled = chkBoxNewVehicleModel.Checked;
            txtBoxEngineSN.Enabled = chkBoxNewVehicleModel.Checked;
            txtBoxEngineMfr.Enabled = chkBoxNewVehicleModel.Checked;
            txtBoxEngineVolume.Enabled = chkBoxNewVehicleModel.Checked;
            txtBoxCylinderQTY.Enabled = chkBoxNewVehicleModel.Checked;
            cmbBoxFuelSupply.Enabled = chkBoxNewVehicleModel.Checked;
            txtBoxRatedPower.Enabled = chkBoxNewVehicleModel.Checked;
            txtBoxRatedRPM.Enabled = chkBoxNewVehicleModel.Checked;
            cmbBoxEmissionStage.Enabled = chkBoxNewVehicleModel.Checked;
            cmbBoxTransmission.Enabled = chkBoxNewVehicleModel.Checked;
            txtBoxCatConverter.Enabled = chkBoxNewVehicleModel.Checked;
            txtBoxRefMass.Enabled = chkBoxNewVehicleModel.Checked;
            txtBoxMaxMass.Enabled = chkBoxNewVehicleModel.Checked;
            txtBoxOBDLocation.Enabled = chkBoxNewVehicleModel.Checked;
            txtBoxPostProcessorType.Enabled = chkBoxNewVehicleModel.Checked;
            txtBoxPostProcessorModel.Enabled = chkBoxNewVehicleModel.Checked;
            txtBoxMotorModel.Enabled = chkBoxNewVehicleModel.Checked;
            txtBoxEnergyStorage.Enabled = chkBoxNewVehicleModel.Checked;
            txtBoxBatteryCap.Enabled = chkBoxNewVehicleModel.Checked;
            cmbBoxTestMethod.Enabled = chkBoxNewVehicleModel.Checked;
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
                    if (_mainCfg.DynoParamIP.Length > 0) {
                        if (_dynoParamRecver != null) {
                            _dynoParamRecver.SendVIN(VI.VIN);
                            _dynoParamRecvFlag.Reset();
                            LoadingForm frmLoading = new LoadingForm();
                            frmLoading.BackgroundWorkAction = () => {
                                frmLoading.CurrentMsg = new KeyValuePair<int, string>(50, "正在从MES获取测功机参数。。。");
                                _dynoParamRecvFlag.WaitOne(_mainCfg.RecvTimeout, false);
                                frmLoading.CurrentMsg = new KeyValuePair<int, string>(50, "测功机参数获取结束");
                            };
                            frmLoading.ShowDialog();
                        } else {
                            _log.TraceError("_dynoParamRecver is null");
                            MessageBox.Show("测功机参数服务端软件接口未实例化", "初始化错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    } else {
                        _carID = _db.GetEmissionInfoFromVIN(VI.VIN, EI);
                        FillInputTextBox();
                    }
                    txtBoxVIN.Clear();
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
            if (_dynoParamRecver != null) {
                _dynoParamRecver.DynoParamRecvEvent -= OnDynoParamRecv;
            }
        }

        private void BtnSave_Click(object sender, EventArgs e) {
            if (_mainCfg.DynoParamIP.Length > 0) {
                MessageBox.Show("已开启自动从MES获取测功机参数功能，测功机参数保存至本地后，对登录操作没有实质效果",
                    "车辆登录", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            FillEmissionInfo();
            _db.SaveEmissionInfo(txtBoxGettedVIN.Text, EI);
        }

        private void BtnStart_Click(object sender, EventArgs e) {
            double ratedPower = Convert.ToDouble(txtBoxRatedPower.Text.Length > 0 ? txtBoxRatedPower.Text : null);
            int ratedRPM = Convert.ToInt32(txtBoxRatedRPM.Text.Length > 0 ? txtBoxRatedRPM.Text : null);
            int refMass = Convert.ToInt32(txtBoxRefMass.Text.Length > 0 ? txtBoxRefMass.Text : null);
            int maxMass = Convert.ToInt32(txtBoxMaxMass.Text.Length > 0 ? txtBoxMaxMass.Text : null);
            bool bCanTest = true;
            bCanTest = bCanTest && (txtBoxGettedVIN.TextLength > 0);
            bCanTest = bCanTest && (txtBoxVehicleModel.TextLength > 0);
            bCanTest = bCanTest && (ratedPower > 0);
            bCanTest = bCanTest && (ratedRPM > 0);
            bCanTest = bCanTest && (cmbBoxEmissionStage.SelectedIndex > 0);
            bCanTest = bCanTest && (refMass > 0);
            bCanTest = bCanTest && (maxMass > 0);
            bCanTest = bCanTest && (cmbBoxTestMethod.SelectedIndex != 5);
            if (bCanTest) {
                // 目前欧润特软件只能使用“在用车”登录接口
                //NewVehicle newVehicle = new NewVehicle();
                //SetNewVehicle(newVehicle);
                //SaveNewVehicleInfoParams cmdParams = new SaveNewVehicleInfoParams() {
                //    ClientID = _dynoCmd.ClientID,
                //    Newvehicle = newVehicle
                //};
                //if (_dynoCmd.SaveNewVehicleInfoCmd(cmdParams, out string errMsg)) {
                //    DialogResult = DialogResult.OK;
                //    Close();
                //} else {
                //    MessageBox.Show("保存车辆登录表单至服务器时出错", "车辆登录", MessageBoxButtons.OK, MessageBoxIcon.Error);
                //}

                DateTime fromDate = DateTime.Now;
                UseVehicle useVehicle = new UseVehicle();
                SetUseVehicle(useVehicle);
                SaveInUseVehicleInfoParams cmdParams = new SaveInUseVehicleInfoParams() {
                    ClientID = _dynoCmd.ClientID,
                    Usevehicle = useVehicle
                };
                if (_dynoCmd.SaveInUseVehicleInfoCmd(cmdParams, out string errMsg)) {
                    // 调用“GetWaitCheckQueueInfo”接口获取 WJBGBH
                    string WJBGBH = string.Empty;
                    GetWaitCheckQueueInfoParams cmdParams2 = new GetWaitCheckQueueInfoParams {
                        ClientID = _dynoCmd.ClientID,
                        FromDate = fromDate.ToLocalTime().ToString("yyyy-M-d HH:mm:ss"),
                        ToDate = DateTime.Now.ToLocalTime().ToString("yyyy-M-d HH:mm:ss"),
                    };
                    GetWaitCheckQueueInfoAckParams ackParams2 = new GetWaitCheckQueueInfoAckParams();
                    if (_dynoCmd.GetWaitCheckQueueInfoCmd(cmdParams2, ref ackParams2, out errMsg) && ackParams2 != null) {
                        for (int i = 0; i < ackParams2.waitCheckQueueInfo.Rows.Count; i++) {
                            if(ackParams2.waitCheckQueueInfo.Rows[i]["VIN"].ToString() == VI.VIN) {
                                WJBGBH = ackParams2.waitCheckQueueInfo.Rows[i]["WJBGBH"].ToString();
                            }
                        }
                        // 使用上一步得到的 WJBGBH 调用“GetOneWaitVehicleInfo”接口填充服务器端 vehicleInfo 变量
                        GetOneWaitVehicleInfoParams cmdParams3 = new GetOneWaitVehicleInfoParams {
                            ClientID = _dynoCmd.ClientID,
                            WJBGBH = WJBGBH,
                            DLY = EI.Name,
                            DLSJ = DateTime.Now.ToLocalTime().ToString("yyyy-M-d HH:mm:ss"),
                            HPHM = string.Empty,
                            HPYS = string.Empty
                        };
                        GetOneWaitVehicleInfoAckParams ackParams3 = new GetOneWaitVehicleInfoAckParams();
                        if (_dynoCmd.GetOneWaitVehicleInfoCmd(cmdParams3, ref ackParams3, out errMsg) && ackParams3 != null) {
                            DialogResult = DialogResult.OK;
                            Close();
                        } else {
                            MessageBox.Show("获取待检车辆信息时出错", "车辆登录", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    } else {
                        MessageBox.Show("获取待检队列时出错", "车辆登录", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                } else {
                    MessageBox.Show("保存车辆登录表单至服务器时出错", "车辆登录", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

            } else {
                if (txtBoxGettedVIN.TextLength <= 0) {
                    txtBoxGettedVIN.BackColor = Color.Yellow;
                }
                if (txtBoxVehicleModel.TextLength <= 0) {
                    txtBoxVehicleModel.BackColor = Color.Yellow;
                }
                if (ratedPower <= 0) {
                    txtBoxRatedPower.BackColor = Color.Yellow;
                }
                if (ratedRPM <= 0) {
                    txtBoxRatedRPM.BackColor = Color.Yellow;
                }
                if (cmbBoxEmissionStage.SelectedIndex <= 0) {
                    cmbBoxEmissionStage.BackColor = Color.Yellow;
                }
                if (refMass <= 0) {
                    txtBoxRefMass.BackColor = Color.Yellow;
                }
                if (maxMass <= 0) {
                    txtBoxMaxMass.BackColor = Color.Yellow;
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
            float scale = Height / _lastHeight;
            layoutMain.Font = new Font(layoutMain.Font.FontFamily, layoutMain.Font.Size * scale, layoutMain.Font.Style);
            txtBoxVIN.Font = new Font(txtBoxVIN.Font.FontFamily, txtBoxVIN.Font.Size * scale, txtBoxVIN.Font.Style);
            layoutControlItem31.Style.Font = new Font(layoutControlItem31.Style.Font.FontFamily, layoutControlItem31.Style.Font.Size * scale, layoutControlItem31.Style.Font.Style);
            layoutControlItem31.TextSize = new Size(layoutControlItem31.TextSize.Width, layoutControlItem31.TextSize.Height * Convert.ToInt32(scale));
            _lastHeight = Height;
        }

        private void ChkBoxNewVehicleModel_CheckedChanged(object sender, EventArgs e) {
            UpdateVehicleModelUI();
        }

        private void CmbBoxSelectVehicleModel_SelectedIndexChanged(object sender, EventArgs e) {
            if (_mainCfg.DynoParamIP.Length <= 0) {
                if (_db.GetEmissionInfoFromVehicleModel(cmbBoxSelectVehicleModel.Text, EI)) {
                    FillInputTextBox();
                }
            }
        }
    }
}
