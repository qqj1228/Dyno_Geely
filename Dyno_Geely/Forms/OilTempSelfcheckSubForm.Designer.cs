namespace Dyno_Geely {
    partial class OilTempSelfcheckSubForm {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            this.layoutMain = new DevComponents.DotNetBar.Layout.LayoutControl();
            this.btnStop = new System.Windows.Forms.Button();
            this.btnStart = new System.Windows.Forms.Button();
            this.lblOilTemp = new System.Windows.Forms.Label();
            this.lblMsg = new System.Windows.Forms.Label();
            this.layoutControlItem1 = new DevComponents.DotNetBar.Layout.LayoutControlItem();
            this.layoutControlItem2 = new DevComponents.DotNetBar.Layout.LayoutControlItem();
            this.layoutControlItem3 = new DevComponents.DotNetBar.Layout.LayoutControlItem();
            this.layoutControlItem4 = new DevComponents.DotNetBar.Layout.LayoutControlItem();
            this.layoutControlItem5 = new DevComponents.DotNetBar.Layout.LayoutControlItem();
            this.lblOilTempCY = new System.Windows.Forms.Label();
            this.layoutControlItem6 = new DevComponents.DotNetBar.Layout.LayoutControlItem();
            this.layoutControlItem7 = new DevComponents.DotNetBar.Layout.LayoutControlItem();
            this.lblOilTempOBD = new System.Windows.Forms.Label();
            this.layoutControlItem8 = new DevComponents.DotNetBar.Layout.LayoutControlItem();
            this.layoutControlItem9 = new DevComponents.DotNetBar.Layout.LayoutControlItem();
            this.lblLQYTempOBD = new System.Windows.Forms.Label();
            this.layoutControlItem10 = new DevComponents.DotNetBar.Layout.LayoutControlItem();
            this.layoutMain.SuspendLayout();
            this.SuspendLayout();
            // 
            // layoutMain
            // 
            this.layoutMain.BackColor = System.Drawing.Color.Transparent;
            this.layoutMain.Controls.Add(this.btnStop);
            this.layoutMain.Controls.Add(this.btnStart);
            this.layoutMain.Controls.Add(this.lblOilTemp);
            this.layoutMain.Controls.Add(this.lblMsg);
            this.layoutMain.Controls.Add(this.lblOilTempCY);
            this.layoutMain.Controls.Add(this.lblOilTempOBD);
            this.layoutMain.Controls.Add(this.lblLQYTempOBD);
            this.layoutMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.layoutMain.Font = new System.Drawing.Font("宋体", 18F);
            this.layoutMain.Location = new System.Drawing.Point(0, 0);
            this.layoutMain.Name = "layoutMain";
            // 
            // 
            // 
            this.layoutMain.RootGroup.Items.AddRange(new DevComponents.DotNetBar.Layout.LayoutItemBase[] {
            this.layoutControlItem1,
            this.layoutControlItem2,
            this.layoutControlItem6,
            this.layoutControlItem8,
            this.layoutControlItem10,
            this.layoutControlItem3,
            this.layoutControlItem4});
            this.layoutMain.Size = new System.Drawing.Size(784, 411);
            this.layoutMain.TabIndex = 0;
            // 
            // btnStop
            // 
            this.btnStop.Location = new System.Drawing.Point(396, 357);
            this.btnStop.Margin = new System.Windows.Forms.Padding(0);
            this.btnStop.Name = "btnStop";
            this.btnStop.Size = new System.Drawing.Size(384, 45);
            this.btnStop.TabIndex = 6;
            this.btnStop.Text = "手动停止";
            this.btnStop.UseVisualStyleBackColor = true;
            this.btnStop.Click += new System.EventHandler(this.BtnStop_Click);
            // 
            // btnStart
            // 
            this.btnStart.Location = new System.Drawing.Point(4, 357);
            this.btnStart.Margin = new System.Windows.Forms.Padding(0);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(384, 45);
            this.btnStart.TabIndex = 5;
            this.btnStart.Text = "开始自检";
            this.btnStart.UseVisualStyleBackColor = true;
            this.btnStart.Click += new System.EventHandler(this.BtnStart_Click);
            // 
            // lblOilTemp
            // 
            this.lblOilTemp.AutoSize = true;
            this.lblOilTemp.BackColor = System.Drawing.Color.Black;
            this.lblOilTemp.ForeColor = System.Drawing.Color.Gold;
            this.lblOilTemp.Location = new System.Drawing.Point(238, 73);
            this.lblOilTemp.Margin = new System.Windows.Forms.Padding(0);
            this.lblOilTemp.Name = "lblOilTemp";
            this.lblOilTemp.Size = new System.Drawing.Size(542, 65);
            this.lblOilTemp.TabIndex = 1;
            this.lblOilTemp.Text = "--";
            this.lblOilTemp.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblMsg
            // 
            this.lblMsg.AutoSize = true;
            this.lblMsg.Font = new System.Drawing.Font("黑体", 20F, System.Drawing.FontStyle.Bold);
            this.lblMsg.ForeColor = System.Drawing.Color.Red;
            this.lblMsg.Location = new System.Drawing.Point(4, 4);
            this.lblMsg.Margin = new System.Windows.Forms.Padding(0);
            this.lblMsg.Name = "lblMsg";
            this.lblMsg.Size = new System.Drawing.Size(776, 61);
            this.lblMsg.TabIndex = 0;
            this.lblMsg.Text = "Msg";
            this.lblMsg.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // layoutControlItem1
            // 
            this.layoutControlItem1.Control = this.lblMsg;
            this.layoutControlItem1.Height = 17;
            this.layoutControlItem1.HeightType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            this.layoutControlItem1.Name = "layoutControlItem1";
            this.layoutControlItem1.Text = "Label:";
            this.layoutControlItem1.TextVisible = false;
            this.layoutControlItem1.Width = 100;
            this.layoutControlItem1.WidthType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            // 
            // layoutControlItem2
            // 
            this.layoutControlItem2.Control = this.lblOilTemp;
            this.layoutControlItem2.Height = 18;
            this.layoutControlItem2.HeightType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            this.layoutControlItem2.Name = "layoutControlItem2";
            this.layoutControlItem2.Text = "汽油油温(℃):";
            this.layoutControlItem2.TextAlignment = DevComponents.DotNetBar.Layout.eTextAlignment.Center;
            this.layoutControlItem2.TextLineAlignment = DevComponents.DotNetBar.Layout.eTextLineAlignment.Middle;
            this.layoutControlItem2.Width = 100;
            this.layoutControlItem2.WidthType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            // 
            // layoutControlItem3
            // 
            this.layoutControlItem3.Control = this.btnStart;
            this.layoutControlItem3.Height = 13;
            this.layoutControlItem3.HeightType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            this.layoutControlItem3.Name = "layoutControlItem3";
            this.layoutControlItem3.Width = 50;
            this.layoutControlItem3.WidthType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            // 
            // layoutControlItem4
            // 
            this.layoutControlItem4.Control = this.btnStop;
            this.layoutControlItem4.Height = 13;
            this.layoutControlItem4.HeightType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            this.layoutControlItem4.Name = "layoutControlItem4";
            this.layoutControlItem4.Width = 50;
            this.layoutControlItem4.WidthType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            // 
            // layoutControlItem5
            // 
            this.layoutControlItem5.Control = this.lblOilTempCY;
            this.layoutControlItem5.Height = 18;
            this.layoutControlItem5.HeightType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            this.layoutControlItem5.Name = "layoutControlItem5";
            this.layoutControlItem5.Text = "汽油油温(℃):";
            this.layoutControlItem5.TextAlignment = DevComponents.DotNetBar.Layout.eTextAlignment.Center;
            this.layoutControlItem5.TextLineAlignment = DevComponents.DotNetBar.Layout.eTextLineAlignment.Middle;
            this.layoutControlItem5.Width = 100;
            this.layoutControlItem5.WidthType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            // 
            // lblOilTempCY
            // 
            this.lblOilTempCY.AutoSize = true;
            this.lblOilTempCY.BackColor = System.Drawing.Color.Black;
            this.lblOilTempCY.ForeColor = System.Drawing.Color.Gold;
            this.lblOilTempCY.Location = new System.Drawing.Point(238, 146);
            this.lblOilTempCY.Margin = new System.Windows.Forms.Padding(0);
            this.lblOilTempCY.Name = "lblOilTempCY";
            this.lblOilTempCY.Size = new System.Drawing.Size(542, 65);
            this.lblOilTempCY.TabIndex = 2;
            this.lblOilTempCY.Text = "--";
            this.lblOilTempCY.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // layoutControlItem6
            // 
            this.layoutControlItem6.Control = this.lblOilTempCY;
            this.layoutControlItem6.Height = 18;
            this.layoutControlItem6.HeightType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            this.layoutControlItem6.MinSize = new System.Drawing.Size(64, 18);
            this.layoutControlItem6.Name = "layoutControlItem6";
            this.layoutControlItem6.Text = "柴油油温(℃):";
            this.layoutControlItem6.TextAlignment = DevComponents.DotNetBar.Layout.eTextAlignment.Center;
            this.layoutControlItem6.TextLineAlignment = DevComponents.DotNetBar.Layout.eTextLineAlignment.Middle;
            this.layoutControlItem6.Width = 100;
            this.layoutControlItem6.WidthType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            // 
            // layoutControlItem7
            // 
            this.layoutControlItem7.Control = this.lblOilTempOBD;
            this.layoutControlItem7.Height = 17;
            this.layoutControlItem7.HeightType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            this.layoutControlItem7.MinSize = new System.Drawing.Size(64, 18);
            this.layoutControlItem7.Name = "layoutControlItem7";
            this.layoutControlItem7.Text = "柴油油温(℃):";
            this.layoutControlItem7.TextAlignment = DevComponents.DotNetBar.Layout.eTextAlignment.Center;
            this.layoutControlItem7.TextLineAlignment = DevComponents.DotNetBar.Layout.eTextLineAlignment.Middle;
            this.layoutControlItem7.Width = 100;
            this.layoutControlItem7.WidthType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            // 
            // lblOilTempOBD
            // 
            this.lblOilTempOBD.AutoSize = true;
            this.lblOilTempOBD.BackColor = System.Drawing.Color.Black;
            this.lblOilTempOBD.ForeColor = System.Drawing.Color.Gold;
            this.lblOilTempOBD.Location = new System.Drawing.Point(238, 219);
            this.lblOilTempOBD.Margin = new System.Windows.Forms.Padding(0);
            this.lblOilTempOBD.Name = "lblOilTempOBD";
            this.lblOilTempOBD.Size = new System.Drawing.Size(542, 61);
            this.lblOilTempOBD.TabIndex = 3;
            this.lblOilTempOBD.Text = "--";
            this.lblOilTempOBD.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // layoutControlItem8
            // 
            this.layoutControlItem8.Control = this.lblOilTempOBD;
            this.layoutControlItem8.Height = 17;
            this.layoutControlItem8.HeightType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            this.layoutControlItem8.MinSize = new System.Drawing.Size(64, 18);
            this.layoutControlItem8.Name = "layoutControlItem8";
            this.layoutControlItem8.Text = "OBD油温(℃):";
            this.layoutControlItem8.TextAlignment = DevComponents.DotNetBar.Layout.eTextAlignment.Center;
            this.layoutControlItem8.TextLineAlignment = DevComponents.DotNetBar.Layout.eTextLineAlignment.Middle;
            this.layoutControlItem8.Width = 100;
            this.layoutControlItem8.WidthType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            // 
            // layoutControlItem9
            // 
            this.layoutControlItem9.Control = this.lblLQYTempOBD;
            this.layoutControlItem9.Height = 17;
            this.layoutControlItem9.HeightType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            this.layoutControlItem9.MinSize = new System.Drawing.Size(64, 18);
            this.layoutControlItem9.Name = "layoutControlItem9";
            this.layoutControlItem9.Text = "OBD油温(℃):";
            this.layoutControlItem9.TextAlignment = DevComponents.DotNetBar.Layout.eTextAlignment.Center;
            this.layoutControlItem9.TextLineAlignment = DevComponents.DotNetBar.Layout.eTextLineAlignment.Middle;
            this.layoutControlItem9.Width = 100;
            this.layoutControlItem9.WidthType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            // 
            // lblLQYTempOBD
            // 
            this.lblLQYTempOBD.AutoSize = true;
            this.lblLQYTempOBD.BackColor = System.Drawing.Color.Black;
            this.lblLQYTempOBD.ForeColor = System.Drawing.Color.Gold;
            this.lblLQYTempOBD.Location = new System.Drawing.Point(238, 288);
            this.lblLQYTempOBD.Margin = new System.Windows.Forms.Padding(0);
            this.lblLQYTempOBD.Name = "lblLQYTempOBD";
            this.lblLQYTempOBD.Size = new System.Drawing.Size(542, 61);
            this.lblLQYTempOBD.TabIndex = 4;
            this.lblLQYTempOBD.Text = "--";
            this.lblLQYTempOBD.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // layoutControlItem10
            // 
            this.layoutControlItem10.Control = this.lblLQYTempOBD;
            this.layoutControlItem10.Height = 17;
            this.layoutControlItem10.HeightType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            this.layoutControlItem10.MinSize = new System.Drawing.Size(64, 18);
            this.layoutControlItem10.Name = "layoutControlItem10";
            this.layoutControlItem10.Text = "OBD冷却液温度(℃):";
            this.layoutControlItem10.TextAlignment = DevComponents.DotNetBar.Layout.eTextAlignment.Center;
            this.layoutControlItem10.TextLineAlignment = DevComponents.DotNetBar.Layout.eTextLineAlignment.Middle;
            this.layoutControlItem10.Width = 100;
            this.layoutControlItem10.WidthType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            // 
            // OilTempSelfcheckSubForm
            // 
            this.AcceptButton = this.btnStart;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(784, 411);
            this.Controls.Add(this.layoutMain);
            this.Name = "OilTempSelfcheckSubForm";
            this.Text = "OilTempSelfcheckSubForm";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.OilTempSelfcheckSubForm_FormClosing);
            this.Load += new System.EventHandler(this.OilTempSelfcheckSubForm_Load);
            this.Resize += new System.EventHandler(this.OilTempSelfcheckSubForm_Resize);
            this.layoutMain.ResumeLayout(false);
            this.layoutMain.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private DevComponents.DotNetBar.Layout.LayoutControl layoutMain;
        private System.Windows.Forms.Label lblOilTemp;
        private System.Windows.Forms.Label lblMsg;
        private DevComponents.DotNetBar.Layout.LayoutControlItem layoutControlItem1;
        private DevComponents.DotNetBar.Layout.LayoutControlItem layoutControlItem2;
        private System.Windows.Forms.Button btnStart;
        private DevComponents.DotNetBar.Layout.LayoutControlItem layoutControlItem3;
        private System.Windows.Forms.Button btnStop;
        private DevComponents.DotNetBar.Layout.LayoutControlItem layoutControlItem4;
        private System.Windows.Forms.Label lblOilTempCY;
        private System.Windows.Forms.Label lblOilTempOBD;
        private System.Windows.Forms.Label lblLQYTempOBD;
        private DevComponents.DotNetBar.Layout.LayoutControlItem layoutControlItem6;
        private DevComponents.DotNetBar.Layout.LayoutControlItem layoutControlItem8;
        private DevComponents.DotNetBar.Layout.LayoutControlItem layoutControlItem10;
        private DevComponents.DotNetBar.Layout.LayoutControlItem layoutControlItem5;
        private DevComponents.DotNetBar.Layout.LayoutControlItem layoutControlItem7;
        private DevComponents.DotNetBar.Layout.LayoutControlItem layoutControlItem9;
    }
}