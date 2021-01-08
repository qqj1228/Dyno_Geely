
namespace Dyno_Geely {
    partial class OilTempPreheatingSubForm {
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
            DevComponents.DotNetBar.Layout.BorderPattern borderPattern1 = new DevComponents.DotNetBar.Layout.BorderPattern();
            DevComponents.DotNetBar.Layout.BorderPattern borderPattern2 = new DevComponents.DotNetBar.Layout.BorderPattern();
            this.layoutMain = new DevComponents.DotNetBar.Layout.LayoutControl();
            this.btnDone = new System.Windows.Forms.Button();
            this.txtBoxTempStd = new System.Windows.Forms.TextBox();
            this.lblMsg = new System.Windows.Forms.Label();
            this.lblGasTemp = new System.Windows.Forms.Label();
            this.lblDieselTemp = new System.Windows.Forms.Label();
            this.txtBoxErrStd = new System.Windows.Forms.TextBox();
            this.lblErrAbs = new System.Windows.Forms.Label();
            this.lblErrRel = new System.Windows.Forms.Label();
            this.lblResult = new System.Windows.Forms.Label();
            this.layoutControlItem1 = new DevComponents.DotNetBar.Layout.LayoutControlItem();
            this.layoutGroup1 = new DevComponents.DotNetBar.Layout.LayoutGroup();
            this.layoutSpacerItem1 = new DevComponents.DotNetBar.Layout.LayoutSpacerItem();
            this.layoutControlItem2 = new DevComponents.DotNetBar.Layout.LayoutControlItem();
            this.layoutSpacerItem2 = new DevComponents.DotNetBar.Layout.LayoutSpacerItem();
            this.layoutControlItem3 = new DevComponents.DotNetBar.Layout.LayoutControlItem();
            this.layoutControlItem5 = new DevComponents.DotNetBar.Layout.LayoutControlItem();
            this.layoutGroup2 = new DevComponents.DotNetBar.Layout.LayoutGroup();
            this.layoutSpacerItem3 = new DevComponents.DotNetBar.Layout.LayoutSpacerItem();
            this.layoutControlItem6 = new DevComponents.DotNetBar.Layout.LayoutControlItem();
            this.layoutSpacerItem4 = new DevComponents.DotNetBar.Layout.LayoutSpacerItem();
            this.layoutControlItem8 = new DevComponents.DotNetBar.Layout.LayoutControlItem();
            this.layoutControlItem10 = new DevComponents.DotNetBar.Layout.LayoutControlItem();
            this.layoutControlItem12 = new DevComponents.DotNetBar.Layout.LayoutControlItem();
            this.layoutControlItem13 = new DevComponents.DotNetBar.Layout.LayoutControlItem();
            this.layoutControlItem4 = new DevComponents.DotNetBar.Layout.LayoutControlItem();
            this.layoutControlItem7 = new DevComponents.DotNetBar.Layout.LayoutControlItem();
            this.layoutControlItem9 = new DevComponents.DotNetBar.Layout.LayoutControlItem();
            this.layoutControlItem11 = new DevComponents.DotNetBar.Layout.LayoutControlItem();
            this.layoutMain.SuspendLayout();
            this.SuspendLayout();
            // 
            // layoutMain
            // 
            this.layoutMain.BackColor = System.Drawing.Color.Transparent;
            this.layoutMain.Controls.Add(this.btnDone);
            this.layoutMain.Controls.Add(this.txtBoxTempStd);
            this.layoutMain.Controls.Add(this.lblMsg);
            this.layoutMain.Controls.Add(this.lblGasTemp);
            this.layoutMain.Controls.Add(this.lblDieselTemp);
            this.layoutMain.Controls.Add(this.txtBoxErrStd);
            this.layoutMain.Controls.Add(this.lblErrAbs);
            this.layoutMain.Controls.Add(this.lblErrRel);
            this.layoutMain.Controls.Add(this.lblResult);
            this.layoutMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.layoutMain.Font = new System.Drawing.Font("宋体", 16F);
            this.layoutMain.Location = new System.Drawing.Point(0, 0);
            this.layoutMain.Name = "layoutMain";
            // 
            // 
            // 
            this.layoutMain.RootGroup.Items.AddRange(new DevComponents.DotNetBar.Layout.LayoutItemBase[] {
            this.layoutControlItem1,
            this.layoutGroup1,
            this.layoutGroup2,
            this.layoutControlItem12,
            this.layoutControlItem13});
            this.layoutMain.Size = new System.Drawing.Size(784, 411);
            this.layoutMain.TabIndex = 0;
            // 
            // btnDone
            // 
            this.btnDone.Location = new System.Drawing.Point(396, 360);
            this.btnDone.Margin = new System.Windows.Forms.Padding(0);
            this.btnDone.Name = "btnDone";
            this.btnDone.Size = new System.Drawing.Size(384, 45);
            this.btnDone.TabIndex = 14;
            this.btnDone.Text = "完成";
            this.btnDone.UseVisualStyleBackColor = true;
            this.btnDone.Click += new System.EventHandler(this.BtnDone_Click);
            // 
            // txtBoxTempStd
            // 
            this.txtBoxTempStd.Location = new System.Drawing.Point(145, 134);
            this.txtBoxTempStd.Margin = new System.Windows.Forms.Padding(0);
            this.txtBoxTempStd.Name = "txtBoxTempStd";
            this.txtBoxTempStd.Size = new System.Drawing.Size(233, 32);
            this.txtBoxTempStd.TabIndex = 2;
            this.txtBoxTempStd.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtBoxTempStd.TextChanged += new System.EventHandler(this.TxtBox_TextChanged);
            this.txtBoxTempStd.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.TextBox_KeyPress);
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
            // lblGasTemp
            // 
            this.lblGasTemp.AutoSize = true;
            this.lblGasTemp.BackColor = System.Drawing.Color.Black;
            this.lblGasTemp.ForeColor = System.Drawing.Color.Gold;
            this.lblGasTemp.Location = new System.Drawing.Point(145, 201);
            this.lblGasTemp.Margin = new System.Windows.Forms.Padding(0);
            this.lblGasTemp.Name = "lblGasTemp";
            this.lblGasTemp.Size = new System.Drawing.Size(233, 59);
            this.lblGasTemp.TabIndex = 4;
            this.lblGasTemp.Text = "--";
            this.lblGasTemp.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblDieselTemp
            // 
            this.lblDieselTemp.AutoSize = true;
            this.lblDieselTemp.BackColor = System.Drawing.Color.Black;
            this.lblDieselTemp.ForeColor = System.Drawing.Color.Gold;
            this.lblDieselTemp.Location = new System.Drawing.Point(145, 280);
            this.lblDieselTemp.Margin = new System.Windows.Forms.Padding(0);
            this.lblDieselTemp.Name = "lblDieselTemp";
            this.lblDieselTemp.Size = new System.Drawing.Size(233, 59);
            this.lblDieselTemp.TabIndex = 5;
            this.lblDieselTemp.Text = "--";
            this.lblDieselTemp.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // txtBoxErrStd
            // 
            this.txtBoxErrStd.Location = new System.Drawing.Point(537, 134);
            this.txtBoxErrStd.Margin = new System.Windows.Forms.Padding(0);
            this.txtBoxErrStd.Name = "txtBoxErrStd";
            this.txtBoxErrStd.Size = new System.Drawing.Size(233, 32);
            this.txtBoxErrStd.TabIndex = 8;
            this.txtBoxErrStd.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtBoxErrStd.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.TextBox_KeyPress);
            // 
            // lblErrAbs
            // 
            this.lblErrAbs.AutoSize = true;
            this.lblErrAbs.BackColor = System.Drawing.Color.Black;
            this.lblErrAbs.ForeColor = System.Drawing.Color.Gold;
            this.lblErrAbs.Location = new System.Drawing.Point(537, 201);
            this.lblErrAbs.Margin = new System.Windows.Forms.Padding(0);
            this.lblErrAbs.Name = "lblErrAbs";
            this.lblErrAbs.Size = new System.Drawing.Size(233, 59);
            this.lblErrAbs.TabIndex = 10;
            this.lblErrAbs.Text = "--";
            this.lblErrAbs.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblErrRel
            // 
            this.lblErrRel.AutoSize = true;
            this.lblErrRel.BackColor = System.Drawing.Color.Black;
            this.lblErrRel.ForeColor = System.Drawing.Color.Gold;
            this.lblErrRel.Location = new System.Drawing.Point(537, 280);
            this.lblErrRel.Margin = new System.Windows.Forms.Padding(0);
            this.lblErrRel.Name = "lblErrRel";
            this.lblErrRel.Size = new System.Drawing.Size(233, 59);
            this.lblErrRel.TabIndex = 11;
            this.lblErrRel.Text = "--";
            this.lblErrRel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblResult
            // 
            this.lblResult.AutoSize = true;
            this.lblResult.BackColor = System.Drawing.Color.Black;
            this.lblResult.ForeColor = System.Drawing.Color.Gold;
            this.lblResult.Location = new System.Drawing.Point(135, 360);
            this.lblResult.Margin = new System.Windows.Forms.Padding(0);
            this.lblResult.Name = "lblResult";
            this.lblResult.Size = new System.Drawing.Size(253, 45);
            this.lblResult.TabIndex = 13;
            this.lblResult.Text = "--";
            this.lblResult.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
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
            // layoutGroup1
            // 
            this.layoutGroup1.Height = 70;
            this.layoutGroup1.HeightType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            this.layoutGroup1.Items.AddRange(new DevComponents.DotNetBar.Layout.LayoutItemBase[] {
            this.layoutSpacerItem1,
            this.layoutControlItem2,
            this.layoutSpacerItem2,
            this.layoutControlItem3,
            this.layoutControlItem5});
            this.layoutGroup1.Name = "layoutGroup1";
            this.layoutGroup1.Style.BorderColors = new DevComponents.DotNetBar.Layout.BorderColors(System.Drawing.SystemColors.ControlDark, System.Drawing.SystemColors.ControlDark, System.Drawing.SystemColors.ControlDark, System.Drawing.SystemColors.ControlDark);
            borderPattern1.Bottom = DevComponents.DotNetBar.Layout.LinePattern.Solid;
            borderPattern1.Left = DevComponents.DotNetBar.Layout.LinePattern.Solid;
            borderPattern1.Right = DevComponents.DotNetBar.Layout.LinePattern.Solid;
            borderPattern1.Top = DevComponents.DotNetBar.Layout.LinePattern.Solid;
            this.layoutGroup1.Style.BorderPattern = borderPattern1;
            this.layoutGroup1.Style.BorderThickness = new DevComponents.DotNetBar.Layout.Thickness(1D, 1D, 1D, 1D);
            this.layoutGroup1.Text = "油温";
            this.layoutGroup1.TextAlignment = DevComponents.DotNetBar.Layout.eTextAlignment.Center;
            this.layoutGroup1.TextLineAlignment = DevComponents.DotNetBar.Layout.eTextLineAlignment.Middle;
            this.layoutGroup1.TextPadding = new System.Windows.Forms.Padding(0, 10, 0, 0);
            this.layoutGroup1.TextPosition = DevComponents.DotNetBar.Layout.eLayoutPosition.Top;
            this.layoutGroup1.Width = 50;
            this.layoutGroup1.WidthType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            // 
            // layoutSpacerItem1
            // 
            this.layoutSpacerItem1.Height = 6;
            this.layoutSpacerItem1.HeightType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            this.layoutSpacerItem1.Name = "layoutSpacerItem1";
            this.layoutSpacerItem1.Width = 100;
            this.layoutSpacerItem1.WidthType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            // 
            // layoutControlItem2
            // 
            this.layoutControlItem2.Control = this.txtBoxTempStd;
            this.layoutControlItem2.Height = 22;
            this.layoutControlItem2.HeightType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            this.layoutControlItem2.Name = "layoutControlItem2";
            this.layoutControlItem2.Padding = new System.Windows.Forms.Padding(10);
            this.layoutControlItem2.Text = "参考值(℃):";
            this.layoutControlItem2.TextAlignment = DevComponents.DotNetBar.Layout.eTextAlignment.Center;
            this.layoutControlItem2.TextLineAlignment = DevComponents.DotNetBar.Layout.eTextLineAlignment.Middle;
            this.layoutControlItem2.Width = 100;
            this.layoutControlItem2.WidthType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            // 
            // layoutSpacerItem2
            // 
            this.layoutSpacerItem2.Height = 6;
            this.layoutSpacerItem2.HeightType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            this.layoutSpacerItem2.Name = "layoutSpacerItem2";
            this.layoutSpacerItem2.Width = 100;
            this.layoutSpacerItem2.WidthType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            // 
            // layoutControlItem3
            // 
            this.layoutControlItem3.Control = this.lblGasTemp;
            this.layoutControlItem3.Height = 33;
            this.layoutControlItem3.HeightType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            this.layoutControlItem3.Name = "layoutControlItem3";
            this.layoutControlItem3.Padding = new System.Windows.Forms.Padding(10);
            this.layoutControlItem3.Text = "汽油值(℃):";
            this.layoutControlItem3.TextAlignment = DevComponents.DotNetBar.Layout.eTextAlignment.Center;
            this.layoutControlItem3.TextLineAlignment = DevComponents.DotNetBar.Layout.eTextLineAlignment.Middle;
            this.layoutControlItem3.Width = 100;
            this.layoutControlItem3.WidthType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            // 
            // layoutControlItem5
            // 
            this.layoutControlItem5.Control = this.lblDieselTemp;
            this.layoutControlItem5.Height = 33;
            this.layoutControlItem5.HeightType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            this.layoutControlItem5.Name = "layoutControlItem5";
            this.layoutControlItem5.Padding = new System.Windows.Forms.Padding(10);
            this.layoutControlItem5.Text = "柴油值(℃):";
            this.layoutControlItem5.TextAlignment = DevComponents.DotNetBar.Layout.eTextAlignment.Center;
            this.layoutControlItem5.TextLineAlignment = DevComponents.DotNetBar.Layout.eTextLineAlignment.Middle;
            this.layoutControlItem5.Width = 100;
            this.layoutControlItem5.WidthType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            // 
            // layoutGroup2
            // 
            this.layoutGroup2.Height = 70;
            this.layoutGroup2.HeightType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            this.layoutGroup2.Items.AddRange(new DevComponents.DotNetBar.Layout.LayoutItemBase[] {
            this.layoutSpacerItem3,
            this.layoutControlItem6,
            this.layoutSpacerItem4,
            this.layoutControlItem8,
            this.layoutControlItem10});
            this.layoutGroup2.Name = "layoutGroup2";
            this.layoutGroup2.Style.BorderColors = new DevComponents.DotNetBar.Layout.BorderColors(System.Drawing.SystemColors.ControlDark, System.Drawing.SystemColors.ControlDark, System.Drawing.SystemColors.ControlDark, System.Drawing.SystemColors.ControlDark);
            borderPattern2.Bottom = DevComponents.DotNetBar.Layout.LinePattern.Solid;
            borderPattern2.Left = DevComponents.DotNetBar.Layout.LinePattern.Solid;
            borderPattern2.Right = DevComponents.DotNetBar.Layout.LinePattern.Solid;
            borderPattern2.Top = DevComponents.DotNetBar.Layout.LinePattern.Solid;
            this.layoutGroup2.Style.BorderPattern = borderPattern2;
            this.layoutGroup2.Style.BorderThickness = new DevComponents.DotNetBar.Layout.Thickness(0D, 1D, 1D, 1D);
            this.layoutGroup2.Text = "误差";
            this.layoutGroup2.TextAlignment = DevComponents.DotNetBar.Layout.eTextAlignment.Center;
            this.layoutGroup2.TextLineAlignment = DevComponents.DotNetBar.Layout.eTextLineAlignment.Middle;
            this.layoutGroup2.TextPadding = new System.Windows.Forms.Padding(0, 10, 0, 0);
            this.layoutGroup2.TextPosition = DevComponents.DotNetBar.Layout.eLayoutPosition.Top;
            this.layoutGroup2.Width = 50;
            this.layoutGroup2.WidthType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            // 
            // layoutSpacerItem3
            // 
            this.layoutSpacerItem3.Height = 6;
            this.layoutSpacerItem3.HeightType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            this.layoutSpacerItem3.Name = "layoutSpacerItem3";
            this.layoutSpacerItem3.Width = 100;
            this.layoutSpacerItem3.WidthType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            // 
            // layoutControlItem6
            // 
            this.layoutControlItem6.Control = this.txtBoxErrStd;
            this.layoutControlItem6.Height = 22;
            this.layoutControlItem6.HeightType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            this.layoutControlItem6.Name = "layoutControlItem6";
            this.layoutControlItem6.Padding = new System.Windows.Forms.Padding(10);
            this.layoutControlItem6.Text = "参考值(℃):";
            this.layoutControlItem6.TextAlignment = DevComponents.DotNetBar.Layout.eTextAlignment.Center;
            this.layoutControlItem6.TextLineAlignment = DevComponents.DotNetBar.Layout.eTextLineAlignment.Middle;
            this.layoutControlItem6.Width = 100;
            this.layoutControlItem6.WidthType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            // 
            // layoutSpacerItem4
            // 
            this.layoutSpacerItem4.Height = 6;
            this.layoutSpacerItem4.HeightType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            this.layoutSpacerItem4.Name = "layoutSpacerItem4";
            this.layoutSpacerItem4.Width = 100;
            this.layoutSpacerItem4.WidthType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            // 
            // layoutControlItem8
            // 
            this.layoutControlItem8.Control = this.lblErrAbs;
            this.layoutControlItem8.Height = 33;
            this.layoutControlItem8.HeightType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            this.layoutControlItem8.Name = "layoutControlItem8";
            this.layoutControlItem8.Padding = new System.Windows.Forms.Padding(10);
            this.layoutControlItem8.Text = "绝对值(℃):";
            this.layoutControlItem8.TextAlignment = DevComponents.DotNetBar.Layout.eTextAlignment.Center;
            this.layoutControlItem8.TextLineAlignment = DevComponents.DotNetBar.Layout.eTextLineAlignment.Middle;
            this.layoutControlItem8.Width = 100;
            this.layoutControlItem8.WidthType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            // 
            // layoutControlItem10
            // 
            this.layoutControlItem10.Control = this.lblErrRel;
            this.layoutControlItem10.Height = 33;
            this.layoutControlItem10.HeightType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            this.layoutControlItem10.Name = "layoutControlItem10";
            this.layoutControlItem10.Padding = new System.Windows.Forms.Padding(10);
            this.layoutControlItem10.Text = "相对值(%):";
            this.layoutControlItem10.TextAlignment = DevComponents.DotNetBar.Layout.eTextAlignment.Center;
            this.layoutControlItem10.TextLineAlignment = DevComponents.DotNetBar.Layout.eTextLineAlignment.Middle;
            this.layoutControlItem10.Width = 100;
            this.layoutControlItem10.WidthType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            // 
            // layoutControlItem12
            // 
            this.layoutControlItem12.Control = this.lblResult;
            this.layoutControlItem12.Height = 13;
            this.layoutControlItem12.HeightType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            this.layoutControlItem12.Name = "layoutControlItem12";
            this.layoutControlItem12.Text = "预热总结果:";
            this.layoutControlItem12.TextAlignment = DevComponents.DotNetBar.Layout.eTextAlignment.Center;
            this.layoutControlItem12.TextLineAlignment = DevComponents.DotNetBar.Layout.eTextLineAlignment.Middle;
            this.layoutControlItem12.Width = 50;
            this.layoutControlItem12.WidthType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            // 
            // layoutControlItem13
            // 
            this.layoutControlItem13.Control = this.btnDone;
            this.layoutControlItem13.Height = 13;
            this.layoutControlItem13.HeightType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            this.layoutControlItem13.Name = "layoutControlItem13";
            this.layoutControlItem13.Width = 50;
            this.layoutControlItem13.WidthType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            // 
            // layoutControlItem4
            // 
            this.layoutControlItem4.Control = this.lblDieselTemp;
            this.layoutControlItem4.Height = 33;
            this.layoutControlItem4.HeightType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            this.layoutControlItem4.Name = "layoutControlItem4";
            this.layoutControlItem4.Padding = new System.Windows.Forms.Padding(10);
            this.layoutControlItem4.Text = "汽油值:";
            this.layoutControlItem4.TextAlignment = DevComponents.DotNetBar.Layout.eTextAlignment.Center;
            this.layoutControlItem4.TextLineAlignment = DevComponents.DotNetBar.Layout.eTextLineAlignment.Middle;
            this.layoutControlItem4.Width = 100;
            this.layoutControlItem4.WidthType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            // 
            // layoutControlItem7
            // 
            this.layoutControlItem7.Control = this.lblErrAbs;
            this.layoutControlItem7.Height = 33;
            this.layoutControlItem7.HeightType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            this.layoutControlItem7.Name = "layoutControlItem7";
            this.layoutControlItem7.Padding = new System.Windows.Forms.Padding(10);
            this.layoutControlItem7.Text = "汽油值:";
            this.layoutControlItem7.TextAlignment = DevComponents.DotNetBar.Layout.eTextAlignment.Center;
            this.layoutControlItem7.TextLineAlignment = DevComponents.DotNetBar.Layout.eTextLineAlignment.Middle;
            this.layoutControlItem7.Width = 100;
            this.layoutControlItem7.WidthType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            // 
            // layoutControlItem9
            // 
            this.layoutControlItem9.Control = this.lblErrRel;
            this.layoutControlItem9.Height = 33;
            this.layoutControlItem9.HeightType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            this.layoutControlItem9.Name = "layoutControlItem9";
            this.layoutControlItem9.Padding = new System.Windows.Forms.Padding(10);
            this.layoutControlItem9.Text = "绝对值:";
            this.layoutControlItem9.TextAlignment = DevComponents.DotNetBar.Layout.eTextAlignment.Center;
            this.layoutControlItem9.TextLineAlignment = DevComponents.DotNetBar.Layout.eTextLineAlignment.Middle;
            this.layoutControlItem9.Width = 100;
            this.layoutControlItem9.WidthType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            // 
            // layoutControlItem11
            // 
            this.layoutControlItem11.Control = this.lblResult;
            this.layoutControlItem11.Height = 33;
            this.layoutControlItem11.HeightType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            this.layoutControlItem11.Name = "layoutControlItem11";
            this.layoutControlItem11.Padding = new System.Windows.Forms.Padding(10);
            this.layoutControlItem11.Text = "柴油值:";
            this.layoutControlItem11.TextAlignment = DevComponents.DotNetBar.Layout.eTextAlignment.Center;
            this.layoutControlItem11.TextLineAlignment = DevComponents.DotNetBar.Layout.eTextLineAlignment.Middle;
            this.layoutControlItem11.Width = 100;
            this.layoutControlItem11.WidthType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            // 
            // OilTempPreheatingSubForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(784, 411);
            this.Controls.Add(this.layoutMain);
            this.Name = "OilTempPreheatingSubForm";
            this.Text = "OilTempPreheatingSubForm";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.OilTempPreheatingSubForm_FormClosing);
            this.Load += new System.EventHandler(this.OilTempPreheatingSubForm_Load);
            this.VisibleChanged += new System.EventHandler(this.OilTempPreheatingSubForm_VisibleChanged);
            this.Resize += new System.EventHandler(this.OilTempPreheatingSubForm_Resize);
            this.layoutMain.ResumeLayout(false);
            this.layoutMain.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private DevComponents.DotNetBar.Layout.LayoutControl layoutMain;
        private System.Windows.Forms.Label lblMsg;
        private DevComponents.DotNetBar.Layout.LayoutControlItem layoutControlItem1;
        private DevComponents.DotNetBar.Layout.LayoutGroup layoutGroup1;
        private DevComponents.DotNetBar.Layout.LayoutGroup layoutGroup2;
        private System.Windows.Forms.TextBox txtBoxTempStd;
        private DevComponents.DotNetBar.Layout.LayoutControlItem layoutControlItem2;
        private System.Windows.Forms.Label lblGasTemp;
        private DevComponents.DotNetBar.Layout.LayoutControlItem layoutControlItem3;
        private System.Windows.Forms.Label lblDieselTemp;
        private DevComponents.DotNetBar.Layout.LayoutSpacerItem layoutSpacerItem1;
        private DevComponents.DotNetBar.Layout.LayoutSpacerItem layoutSpacerItem2;
        private DevComponents.DotNetBar.Layout.LayoutControlItem layoutControlItem5;
        private DevComponents.DotNetBar.Layout.LayoutControlItem layoutControlItem4;
        private System.Windows.Forms.TextBox txtBoxErrStd;
        private System.Windows.Forms.Label lblErrAbs;
        private System.Windows.Forms.Label lblErrRel;
        private DevComponents.DotNetBar.Layout.LayoutSpacerItem layoutSpacerItem3;
        private DevComponents.DotNetBar.Layout.LayoutControlItem layoutControlItem6;
        private DevComponents.DotNetBar.Layout.LayoutSpacerItem layoutSpacerItem4;
        private DevComponents.DotNetBar.Layout.LayoutControlItem layoutControlItem8;
        private DevComponents.DotNetBar.Layout.LayoutControlItem layoutControlItem10;
        private DevComponents.DotNetBar.Layout.LayoutControlItem layoutControlItem7;
        private DevComponents.DotNetBar.Layout.LayoutControlItem layoutControlItem9;
        private System.Windows.Forms.Label lblResult;
        private DevComponents.DotNetBar.Layout.LayoutControlItem layoutControlItem12;
        private DevComponents.DotNetBar.Layout.LayoutControlItem layoutControlItem11;
        private System.Windows.Forms.Button btnDone;
        private DevComponents.DotNetBar.Layout.LayoutControlItem layoutControlItem13;
    }
}