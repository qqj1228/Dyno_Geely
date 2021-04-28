using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Dyno_Geely {
    public partial class MainForm : Form {
        private readonly Logger _log;
        private readonly Config _cfg;
        private readonly ModelLocal _db;
        private readonly DynoCmd _dynoCmd;
        private float _lastHeight;
        private List<bool> _selfChecks;

        public MainForm(Logger log, Config cfg, ModelLocal db, DynoCmd dynoCmd) {
            InitializeComponent();
            _lastHeight = Height;
            _log = log;
            _cfg = cfg;
            _db = db;
            _dynoCmd = dynoCmd;
            _selfChecks = new List<bool>();
        }

        private void ResizeContrlFont(Control control, float scale) {
            foreach (Control subControl in control.Controls) {
                if (subControl.Controls.Count == 0) {
                    subControl.Font = new Font(subControl.Font.FontFamily, subControl.Font.Size * scale, subControl.Font.Style);
                } else {
                    ResizeContrlFont(subControl, scale);
                }
            }
        }

        private void MainForm_Load(object sender, EventArgs e) {
            Text += " Ver: " + MainFileVersion.AssemblyVersion;
            lblInfo.Text = string.Empty;
            if (DateTime.Compare(DateTime.Now.AddDays(-1), _db.GetPreheating()) > 0) {
                lblInfo.Text = "请先进行设备预热";
                lblInfo.ForeColor = Color.Red;
#if DEBUG
                btnLogin.Enabled = true;
#else
                btnLogin.Enabled = false;
#endif
            } else {
                lblInfo.Text = "设备就绪";
                lblInfo.ForeColor = lblLogo.ForeColor;
                btnLogin.Enabled = true;
            }
        }

        private void MainForm_Resize(object sender, EventArgs e) {
            if (_lastHeight == 0) {
                return;
            }
            float scale = Height / _lastHeight;
            foreach (Control control in Controls) {
                ResizeContrlFont(control, scale);
            }
            _lastHeight = Height;
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e) {
            LoadingForm frmLoading = new LoadingForm();
            frmLoading.BackgroundWorkAction = () => {
                frmLoading.CurrentMsg = new KeyValuePair<int, string>(50, "退出测功机服务器中。。。");
                try {
                    _dynoCmd.LogoutCmd(out string errMsg);
                    frmLoading.CurrentMsg = new KeyValuePair<int, string>(100, "已安全退出测功机服务器");
                } catch (Exception ex) {
                    _log.TraceError("Logout error: " + ex.Message);
                    frmLoading.CurrentMsg = new KeyValuePair<int, string>(100, "退出测功机服务器时发生错误");
                }
            };
            frmLoading.ShowDialog();
        }

        private void BtnPreheating_Click(object sender, EventArgs e) {
            PreheatingForm f_preheating = new PreheatingForm(_dynoCmd, _cfg.Main.Data);
            if (f_preheating.ShowDialog() == DialogResult.Yes) {
                _db.UpdatePreheating();
                lblInfo.Text = "设备就绪";
                lblInfo.ForeColor = lblLogo.ForeColor;
                btnLogin.Enabled = true;
            }
            _cfg.SaveConfig(_cfg.Main);
        }

        private void BtnLogin_Click(object sender, EventArgs e) {
            VehicleLoginForm f_vehicleLogin = new VehicleLoginForm(_db, _dynoCmd, _cfg.Main.Data, _log);
            f_vehicleLogin.ShowDialog();
            if (f_vehicleLogin.DialogResult == DialogResult.OK) {
                EnvironmentData envData = new EnvironmentData();

                bool bDiesel = false;
                switch (f_vehicleLogin.EI.TestMethod) {
                case 1:
                    _selfChecks = _cfg.SelfCheck.Data.TSI;
                    break;
                case 2:
                    _selfChecks = _cfg.SelfCheck.Data.ASM;
                    break;
                case 3:
                    _selfChecks = _cfg.SelfCheck.Data.VMAS;
                    break;
                case 4:
                    _selfChecks = _cfg.SelfCheck.Data.LD;
                    bDiesel = true;
                    break;
                case 6:
                    _selfChecks = _cfg.SelfCheck.Data.FAL;
                    bDiesel = true;
                    break;
                case 7:
                    _selfChecks = _cfg.SelfCheck.Data.Default;
                    bDiesel = true;
                    break;
                default:
                    _selfChecks = _cfg.SelfCheck.Data.Default;
                    break;
                }
                if (_selfChecks.Count != 6) {
                    _selfChecks = _cfg.SelfCheck.Data.Default;
                    MessageBox.Show("仪器准备配置出错，将使用默认配置！", "仪器准备配置出错", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                SelfcheckForm f_prepare = new SelfcheckForm(_dynoCmd, _cfg.Main.Data, _selfChecks, envData, bDiesel);
                f_prepare.ShowDialog();
#if DEBUG
                LugdownForm f_Lugdown = new LugdownForm(f_vehicleLogin.VI.VIN, _dynoCmd, _cfg.Main.Data, _db, envData, _log);
                f_Lugdown.ShowDialog();
                //ASMForm f_ASM = new ASMForm(f_vehicleLogin.VI.VIN, _dynoCmd, _cfg.Main.Data, _db, envData, _log);
                //f_ASM.ShowDialog();
                //FALForm f_FAL = new FALForm(f_vehicleLogin.VI.VIN, _dynoCmd, _cfg.Main.Data, _db, envData, _log);
                //f_FAL.ShowDialog();
                //TSIForm f_TSI = new TSIForm(f_vehicleLogin.VI.VIN, _dynoCmd, _cfg.Main.Data, _db, envData, _log);
                //f_TSI.ShowDialog();
                //VMASForm f_VMAS = new VMASForm(f_vehicleLogin.VI.VIN, _dynoCmd, _cfg.Main.Data, _db, envData, _log);
                //f_VMAS.ShowDialog();
#else
                if (f_prepare.DialogResult == DialogResult.Yes) {
                    switch (f_vehicleLogin.EI.TestMethod) {
                    case 0:
                        lblInfo.Text = "[" + f_vehicleLogin.VI.VIN + "]该辆车免检";
                        lblInfo.ForeColor = Color.Red;
                        break;
                    case 1:
                        TSIForm f_TSI = new TSIForm(f_vehicleLogin.VI.VIN, _dynoCmd, _cfg.Main.Data, _db, envData, _log);
                        f_TSI.ShowDialog();
                        break;
                    case 2:
                        ASMForm f_ASM = new ASMForm(f_vehicleLogin.VI.VIN, _dynoCmd, _cfg.Main.Data, _db, envData, _log);
                        f_ASM.ShowDialog();
                        break;
                    case 3:
                        VMASForm f_VMAS = new VMASForm(f_vehicleLogin.VI.VIN, _dynoCmd, _cfg.Main.Data, _db, envData, _log);
                        f_VMAS.ShowDialog();
                        break;
                    case 4:
                        LugdownForm f_Lugdown = new LugdownForm(f_vehicleLogin.VI.VIN, _dynoCmd, _cfg.Main.Data, _db, envData, _log);
                        f_Lugdown.ShowDialog();
                        break;
                    case 6:
                        FALForm f_FAL = new FALForm(f_vehicleLogin.VI.VIN, _dynoCmd, _cfg.Main.Data, _db, envData, _log);
                        f_FAL.ShowDialog();
                        break;
                    default:
                        lblInfo.Text = "本系统暂不支持[" + f_vehicleLogin.EI.TestMethod + "]检测方法";
                        lblInfo.ForeColor = Color.Red;
                        break;
                    }
                } else {
                    lblInfo.Text = "仪器准备过程不合格，请先排除仪器故障";
                    lblInfo.ForeColor = Color.Red;
                }
#endif
            }
        }

        private void BtnGasGlideCheck_Click(object sender, EventArgs e) {
            DynoGlideCheckForm f_dynoGlideCheck = new DynoGlideCheckForm(_dynoCmd, _cfg.Main.Data, _log, false);
            f_dynoGlideCheck.ShowDialog();
        }

        private void BtnDieselGlideCheck_Click(object sender, EventArgs e) {
            DynoGlideCheckForm f_dynoGlideCheck = new DynoGlideCheckForm(_dynoCmd, _cfg.Main.Data, _log, true);
            f_dynoGlideCheck.ShowDialog();
        }
    }
}
