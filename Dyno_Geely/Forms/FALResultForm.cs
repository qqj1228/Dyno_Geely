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
    public partial class FALResultForm : Form {
        public FALResultForm() {
            InitializeComponent();
        }

        public void ShowResult(FALResultData result) {
            lblRatedRPM.Text = result.RatedRPM.ToString();
            lblMaxRPM.Text = result.MaxRPM.ToString();
            lblKLimit.Text = result.KLimit.ToString("F");
            lblKAvg.Text = result.KAvg.ToString("F");
            lblK1.Text = result.K1.ToString("F");
            lblK2.Text = result.K2.ToString("F");
            lblK3.Text = result.K3.ToString("F");
            lblResult.Text = result.Result;
            if (result.Result != "合格") {
                lblResult.BackColor = Color.Red;
            }
        }
    }
}
