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
    public partial class VMASResultForm : Form {
        public VMASResultForm() {
            InitializeComponent();
        }

        public void ShowResult(VMASResultData result) {
            lblHCLimit.Text = result.HCLimit;
            lblCOLimit.Text = result.COLimit;
            lblNOLimit.Text = result.NOLimit;
            lblHC.Text = result.HC;
            lblCO.Text = result.CO;
            lblNO.Text = result.NO;
            lblHCEvl.Text = result.HCEvl;
            if (result.HCEvl != "合格") {
                lblHCEvl.BackColor = Color.Red;
            }
            lblCOEvl.Text = result.COEvl;
            if (result.COEvl != "合格") {
                lblCOEvl.BackColor = Color.Red;
            }
            lblNOEvl.Text = result.NOEvl;
            if (result.NOEvl != "合格") {
                lblNOEvl.BackColor = Color.Red;
            }
            lblHCNO.Text = result.HCNO;
            lblResult.Text = result.Result;
            if (result.Result != "合格") {
                lblResult.BackColor = Color.Red;
            }
        }

    }
}
