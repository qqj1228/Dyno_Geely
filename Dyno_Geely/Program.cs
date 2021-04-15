using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Dyno_Geely {
    static class Program {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main() {
            System.Threading.Mutex run = new System.Threading.Mutex(true, "Dyno_Geely", out bool runone);
            if (runone) {
                run.ReleaseMutex();
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);

                Logger log = new Logger("Main", ".\\log", EnumLogLevel.LogLevelAll, true, 100);
                Config cfg = null;
                ModelLocal db = null;
                DynoCmd dynoCmd = null;

                // 显示加载窗口
                LoadingForm frmLoading = new LoadingForm();
                frmLoading.BackgroundWorkAction = () => {
                    try {
                        //设置连接字符串
                        frmLoading.CurrentMsg = new KeyValuePair<int, string>(1, "正在加载配置...");
                        cfg = new Config(log);

                        frmLoading.CurrentMsg = new KeyValuePair<int, string>(20, "正在初始化本地数据库...");
                        db = new ModelLocal(cfg.Main.Data.Native, LibBase.DataBaseType.SQLServer, log);
                        try {
                            db.TestConnect();
                        } catch (Exception ex) {
                            log.TraceError("Can't connect with dyno database: " + ex.Message);
                            MessageBox.Show("无法与本地数据库通讯，请检查设置\n" + ex.Message, "初始化错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }

                        frmLoading.CurrentMsg = new KeyValuePair<int, string>(40, "正在初始化测功机客户端...");
                        dynoCmd = new DynoCmd(cfg);
                        if (!dynoCmd.ConnectServer()) {
                            log.TraceError("Can't connect to Dyno server");
                            MessageBox.Show("无法与测功机服务器建立连接，请检查设置", "初始化错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        dynoCmd.SafeClose();

                        frmLoading.CurrentMsg = new KeyValuePair<int, string>(60, "正在登录测功机服务器...");
                        if (!dynoCmd.LoginCmd(out string errMsg)) {
                            log.TraceError("Can't login Dyno server");
                            MessageBox.Show("无法登录测功机服务器，请检查设置，" + errMsg, "初始化错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }

                        frmLoading.CurrentMsg = new KeyValuePair<int, string>(80, "正在设置测功机服务器配置...");
                        SetDataBaseInitInfoParams cmdParams = new SetDataBaseInitInfoParams() {
                            ClientID = dynoCmd.ClientID,
                            ServerName = cfg.Main.Data.Native.IP + "," + cfg.Main.Data.Native.Port,
                            DataBaseName = cfg.Main.Data.Native.DBName,
                            UID = cfg.Main.Data.Native.UserName,
                            PWD = cfg.Main.Data.Native.PassWord
                        };
                        bool bChange = false;
                        GetDataBaseInitInfoAckParams ackParams = new GetDataBaseInitInfoAckParams();
                        if (dynoCmd.GetDataBaseInitInfoCmd(ref ackParams, out errMsg)) {
                            if (ackParams.ServerName != cmdParams.ServerName) {
                                bChange |= true;
                            }
                            if (ackParams.DataBaseName != cmdParams.DataBaseName) {
                                bChange |= true;
                            }
                            if (ackParams.UID != cmdParams.UID) {
                                bChange |= true;
                            }
                            if (ackParams.PWD != cmdParams.PWD) {
                                bChange |= true;
                            }
                            if (bChange) {
                                if(!dynoCmd.SetDataBaseInitInfoCmd(cmdParams, out errMsg)) {
                                    MessageBox.Show("无法获取测功机服务器数据库配置，" + errMsg, "初始化错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                }
                            }
                        } else {
                            MessageBox.Show("无法获取测功机服务器数据库配置，" + errMsg, "初始化错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }

                        frmLoading.CurrentMsg = new KeyValuePair<int, string>(100, "初始化完成");
                    } catch (Exception ex) {
                        MessageBox.Show(ex.Message, "初始化错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        log.TraceError("Loading error: " + ex.Message);
                        Application.Exit();
                        Process.GetCurrentProcess().Kill();
                    }
                };
                // 必须在BackgroundWorkAction设置之后调用ShowDialog()，否则无效果
                frmLoading.ShowDialog();

                Application.Run(new MainForm(log, cfg, db, dynoCmd));
            } else {
                MessageBox.Show("已经有一个相同的程序在运行了！");
            }
        }
    }
}
