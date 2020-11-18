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
    public partial class TipsForm : Form {
        private readonly string _strInfo;
        private readonly System.Timers.Timer _timer;

        public TipsForm(string strInfo, int iWaittingTime) {
            InitializeComponent();
            _strInfo = strInfo;
            progressBar1.Maximum = iWaittingTime;
            progressBar1.Minimum = 0;
            progressBar1.Step = 1;
            progressBar1.Value = 0;

            _timer = new System.Timers.Timer(1000);
            _timer.Elapsed += OnTimer;
            _timer.AutoReset = true;

        }

        private void OnTimer(object source, System.Timers.ElapsedEventArgs e) {
            if (_timer != null && _timer.Enabled) {
                try {
                    Invoke((EventHandler)delegate {
                        if (progressBar1.Value == progressBar1.Maximum) {
                            Close();
                        } else {
                            progressBar1.PerformStep();
                        }
                    });
                } catch (ObjectDisposedException) {
                    // 关闭窗口后仍有一定几率会进入主UI线程，此时访问界面元素会引发此异常，直接忽略即可
                }
            }
        }

        private void TipsForm_Load(object sender, EventArgs e) {
            lblMsg.Text = _strInfo;
            _timer.Enabled = true;
        }

        private void TipsForm_FormClosing(object sender, FormClosingEventArgs e) {
            _timer.Elapsed -= OnTimer;
            _timer.Enabled = false;
        }
    }
}
