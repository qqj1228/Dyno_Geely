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
    public partial class LDResultForm : Form {
        public LDResultForm() {
            InitializeComponent();
        }

        public void ShowResult(LDResultData result) {
            lblRatedRPM.Text = result.RatedRPM.ToString();
            lblMaxRPM.Text = result.MaxRPM.ToString();
            lblVelMaxHP.Text = result.VelMaxHP.ToString("F");
            lblRealMaxPowerLimit.Text = result.RealMaxPowerLimit.ToString("F");
            lblRealMaxPower.Text = result.RealMaxPower.ToString("F");
            lblKLimit.Text = result.KLimit.ToString("F");
            lblK100.Text = result.K100.ToString("F");
            lblK80.Text = result.K80.ToString("F");
            lblNOx80Limit.Text = result.NOx80Limit.ToString("F");
            lblNOx80.Text = result.NOx80.ToString("F");
            lblResult.Text = result.Result;
            if (result.Result != "合格") {
                lblResult.BackColor = Color.Red;
            }
        }
    }
}
