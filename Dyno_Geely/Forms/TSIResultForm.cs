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
    public partial class TSIResultForm : Form {
        public TSIResultForm() {
            InitializeComponent();
        }

        public void ShowResult(TSIResultData result) {
            lblHighCOLimit.Text = result.HighCOLimit.ToString("F");
            lblHighHCLimit.Text = result.HighHCLimit.ToString("F");
            lblHighCO.Text = result.HighCO.ToString("F");
            lblHighHC.Text = result.HighHC.ToString("F");
            lblHighIdleResult.Text = result.HighIdleResult;
            if (result.HighIdleResult != "合格") {
                lblHighIdleResult.BackColor = Color.Red;
            }
            lblLowCOLimit.Text = result.LowCOLimit.ToString("F");
            lblLowHCLimit.Text = result.LowHCLimit.ToString("F");
            lblLowCO.Text = result.LowCO.ToString("F");
            lblLowHC.Text = result.LowHC.ToString("F");
            lblLowIdleResult.Text = result.LowIdleResult;
            if (result.LowIdleResult != "合格") {
                lblLowIdleResult.BackColor = Color.Red;
            }
            lblLambdaLimit.Text = result.LambdaLimit.ToString("F3");
            lblLambda.Text = result.Lambda.ToString("F3");
            lblLambdaResult.Text = result.LambdaResult;
            if (result.LambdaResult != "合格") {
                lblLambdaResult.BackColor = Color.Red;
            }
            lblResult.Text = result.Result;
            if (result.Result != "合格") {
                lblResult.BackColor = Color.Red;
            }
        }
    }
}
