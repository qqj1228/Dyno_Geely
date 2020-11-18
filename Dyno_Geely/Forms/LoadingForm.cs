using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace Dyno_Geely {
    public partial class LoadingForm : Form {
        [DllImport("user32.dll")]
        private static extern bool ReleaseCapture();
        [DllImport("user32.dll")]
        private static extern bool SendMessage(IntPtr hwnd, int wMsg, int wParam, int lParam);

        private const int VM_NCLBUTTONDOWN = 0XA1; // 定义鼠标左键按下
        private const int HTCAPTION = 2;

        private int _second = 0;
        private readonly System.Timers.Timer _timer = new System.Timers.Timer(1000);
        private readonly BackgroundWorker _updateBGWorker = new BackgroundWorker();

        public Action BackgroundWorkAction { get; set; }

        public KeyValuePair<int, string> CurrentMsg {
            set {
                _updateBGWorker.ReportProgress(value.Key, value.Value);
            }
        }

        public LoadingForm() {
            InitializeComponent();
            _updateBGWorker.WorkerReportsProgress = true;
            _updateBGWorker.WorkerSupportsCancellation = true;
            _updateBGWorker.DoWork += new DoWorkEventHandler(this.BackgroundWorker_DoWork);
            _updateBGWorker.ProgressChanged += new ProgressChangedEventHandler(this.BackgroundWorker_ProgressChanged);
            _updateBGWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(this.BackgroundWorker_RunWorkerCompleted);

            _timer.Elapsed += new System.Timers.ElapsedEventHandler(OnTimer);
            _timer.AutoReset = true;
            _timer.Enabled = true;
        }

        private void OnTimer(object source, System.Timers.ElapsedEventArgs e) {
            ShowSecond(++_second);
        }

        private void ShowSecond(int second) {
            if (lblTimer.InvokeRequired) {
                lblTimer.BeginInvoke((EventHandler)delegate { ShowSecond(second); });
            } else {
                lblTimer.Text = "用时" + second.ToString() + "秒";
            }
        }

        private void ShowLog(string strLog, int intValue) {
            if (lblLog.InvokeRequired) {
                lblLog.BeginInvoke((EventHandler)delegate { ShowLog(strLog, intValue); });
            } else {
                lblLog.Text = strLog;
                prgBar.Value = intValue;
            }
        }

        private void LoadingForm_Load(object sender, EventArgs e) {
            _updateBGWorker.RunWorkerAsync();
            lblTimer.Text = "用时0秒";
        }

        private void BackgroundWorker_DoWork(object sender, DoWorkEventArgs e) {
            this.BackgroundWorkAction?.Invoke();
            Thread.Sleep(100);
            if (InvokeRequired) {
                BeginInvoke((EventHandler)delegate {
                    Close();
                });
            } else {
                Close();
            }
        }

        private void BackgroundWorker_ProgressChanged(object sender, ProgressChangedEventArgs e) {
            ShowLog((e.UserState == null) ? "" : e.UserState.ToString(), e.ProgressPercentage);
        }

        private void BackgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e) {
        }

        private void OnMouseDown(object sender, MouseEventArgs e) {
            // 为当前应用程序释放鼠标捕获
            ReleaseCapture();
            // 发送消息 模拟在标题栏上按下鼠标
            SendMessage((IntPtr)this.Handle, VM_NCLBUTTONDOWN, HTCAPTION, 0);
        }
    }
}
