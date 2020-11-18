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
    public partial class ASMResultForm : Form {
        public ASMResultForm() {
            InitializeComponent();
        }

        public void ShowResult(ASMResultData result) {
            lblHC5025Limit.Text = result.HC5025Limit;
            lblCO5025Limit.Text = result.CO5025Limit;
            lblNO5025Limit.Text = result.NO5025Limit;
            lblHC5025.Text = result.HC5025;
            lblCO5025.Text = result.CO5025;
            lblNO5025.Text = result.NO5025;
            lblHC5025Evl.Text = result.HC5025Evl;
            lblCO5025Evl.Text = result.CO5025Evl;
            lblNO5025Evl.Text = result.NO5025Evl;

            lblHC2540Limit.Text = result.HC2540Limit;
            lblCO2540Limit.Text = result.CO2540Limit;
            lblNO2540Limit.Text = result.NO2540Limit;
            lblHC2540.Text = result.HC2540;
            lblCO2540.Text = result.CO2540;
            lblNO2540.Text = result.NO2540;
            lblHC2540Evl.Text = result.HC2540Evl;
            lblCO2540Evl.Text = result.CO2540Evl;
            lblNO2540Evl.Text = result.NO2540Evl;

            lblResult.Text = result.Result;

            if (result.HC5025Evl != "合格") {
                lblHC5025Evl.BackColor = Color.Red;
            }
            if (result.CO5025Evl != "合格") {
                lblCO5025Evl.BackColor = Color.Red;
            }
            if (result.NO5025Evl != "合格") {
                lblNO5025Evl.BackColor = Color.Red;
            }
            if (result.HC2540Evl != "合格") {
                lblHC2540Evl.BackColor = Color.Red;
            }
            if (result.CO2540Evl != "合格") {
                lblCO2540Evl.BackColor = Color.Red;
            }
            if (result.NO2540Evl != "合格") {
                lblNO2540Evl.BackColor = Color.Red;
            }
            if (result.Result != "合格") {
                lblResult.BackColor = Color.Red;
            }
        }

    }
}
