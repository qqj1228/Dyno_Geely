﻿namespace Dyno_Geely {
    partial class TachometerSelfcheckSubForm {
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
            DevComponents.DotNetBar.Layout.BorderPattern borderPattern3 = new DevComponents.DotNetBar.Layout.BorderPattern();
            this.layoutMain = new DevComponents.DotNetBar.Layout.LayoutControl();
            this.btnStop = new System.Windows.Forms.Button();
            this.btnStart = new System.Windows.Forms.Button();
            this.lblGasRPM = new System.Windows.Forms.Label();
            this.lblMsg = new System.Windows.Forms.Label();
            this.lblGasRPMLow = new System.Windows.Forms.Label();
            this.lblGasRPMHigh = new System.Windows.Forms.Label();
            this.lblDieselRPMLow = new System.Windows.Forms.Label();
            this.lblDieselRPMHigh = new System.Windows.Forms.Label();
            this.lblDieselRPM = new System.Windows.Forms.Label();
            this.lblOBDRPM = new System.Windows.Forms.Label();
            this.layoutControlItem1 = new DevComponents.DotNetBar.Layout.LayoutControlItem();
            this.layoutGroup2 = new DevComponents.DotNetBar.Layout.LayoutGroup();
            this.layoutControlItem6 = new DevComponents.DotNetBar.Layout.LayoutControlItem();
            this.layoutControlItem8 = new DevComponents.DotNetBar.Layout.LayoutControlItem();
            this.layoutControlItem2 = new DevComponents.DotNetBar.Layout.LayoutControlItem();
            this.layoutGroup3 = new DevComponents.DotNetBar.Layout.LayoutGroup();
            this.layoutControlItem10 = new DevComponents.DotNetBar.Layout.LayoutControlItem();
            this.layoutControlItem12 = new DevComponents.DotNetBar.Layout.LayoutControlItem();
            this.layoutControlItem14 = new DevComponents.DotNetBar.Layout.LayoutControlItem();
            this.layoutControlItem16 = new DevComponents.DotNetBar.Layout.LayoutControlItem();
            this.layoutControlItem3 = new DevComponents.DotNetBar.Layout.LayoutControlItem();
            this.layoutControlItem4 = new DevComponents.DotNetBar.Layout.LayoutControlItem();
            this.layoutGroup1 = new DevComponents.DotNetBar.Layout.LayoutGroup();
            this.layoutControlItem5 = new DevComponents.DotNetBar.Layout.LayoutControlItem();
            this.layoutControlItem7 = new DevComponents.DotNetBar.Layout.LayoutControlItem();
            this.layoutControlItem9 = new DevComponents.DotNetBar.Layout.LayoutControlItem();
            this.layoutControlItem11 = new DevComponents.DotNetBar.Layout.LayoutControlItem();
            this.layoutControlItem13 = new DevComponents.DotNetBar.Layout.LayoutControlItem();
            this.layoutControlItem15 = new DevComponents.DotNetBar.Layout.LayoutControlItem();
            this.layoutMain.SuspendLayout();
            this.SuspendLayout();
            // 
            // layoutMain
            // 
            this.layoutMain.BackColor = System.Drawing.Color.Transparent;
            this.layoutMain.Controls.Add(this.btnStop);
            this.layoutMain.Controls.Add(this.btnStart);
            this.layoutMain.Controls.Add(this.lblGasRPM);
            this.layoutMain.Controls.Add(this.lblMsg);
            this.layoutMain.Controls.Add(this.lblGasRPMLow);
            this.layoutMain.Controls.Add(this.lblGasRPMHigh);
            this.layoutMain.Controls.Add(this.lblDieselRPMLow);
            this.layoutMain.Controls.Add(this.lblDieselRPMHigh);
            this.layoutMain.Controls.Add(this.lblDieselRPM);
            this.layoutMain.Controls.Add(this.lblOBDRPM);
            this.layoutMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.layoutMain.Font = new System.Drawing.Font("宋体", 18F);
            this.layoutMain.Location = new System.Drawing.Point(0, 0);
            this.layoutMain.Name = "layoutMain";
            // 
            // 
            // 
            this.layoutMain.RootGroup.Items.AddRange(new DevComponents.DotNetBar.Layout.LayoutItemBase[] {
            this.layoutControlItem1,
            this.layoutGroup2,
            this.layoutGroup3,
            this.layoutControlItem16,
            this.layoutControlItem3,
            this.layoutControlItem4});
            this.layoutMain.Size = new System.Drawing.Size(784, 411);
            this.layoutMain.TabIndex = 0;
            // 
            // btnStop
            // 
            this.btnStop.Location = new System.Drawing.Point(592, 360);
            this.btnStop.Margin = new System.Windows.Forms.Padding(0);
            this.btnStop.Name = "btnStop";
            this.btnStop.Size = new System.Drawing.Size(188, 45);
            this.btnStop.TabIndex = 11;
            this.btnStop.Text = "手动停止";
            this.btnStop.UseVisualStyleBackColor = true;
            this.btnStop.Click += new System.EventHandler(this.BtnStop_Click);
            // 
            // btnStart
            // 
            this.btnStart.Location = new System.Drawing.Point(396, 360);
            this.btnStart.Margin = new System.Windows.Forms.Padding(0);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(188, 45);
            this.btnStart.TabIndex = 10;
            this.btnStart.Text = "开始自检";
            this.btnStart.UseVisualStyleBackColor = true;
            this.btnStart.Click += new System.EventHandler(this.BtnStart_Click);
            // 
            // lblGasRPM
            // 
            this.lblGasRPM.AutoSize = true;
            this.lblGasRPM.BackColor = System.Drawing.Color.Black;
            this.lblGasRPM.ForeColor = System.Drawing.Color.Gold;
            this.lblGasRPM.Location = new System.Drawing.Point(106, 274);
            this.lblGasRPM.Margin = new System.Windows.Forms.Padding(0);
            this.lblGasRPM.Name = "lblGasRPM";
            this.lblGasRPM.Size = new System.Drawing.Size(278, 72);
            this.lblGasRPM.TabIndex = 3;
            this.lblGasRPM.Text = "--";
            this.lblGasRPM.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
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
            // lblGasRPMLow
            // 
            this.lblGasRPMLow.BackColor = System.Drawing.Color.Black;
            this.lblGasRPMLow.ForeColor = System.Drawing.Color.Gold;
            this.lblGasRPMLow.Location = new System.Drawing.Point(106, 118);
            this.lblGasRPMLow.Margin = new System.Windows.Forms.Padding(0);
            this.lblGasRPMLow.Name = "lblGasRPMLow";
            this.lblGasRPMLow.Size = new System.Drawing.Size(278, 70);
            this.lblGasRPMLow.TabIndex = 1;
            this.lblGasRPMLow.Text = "--";
            this.lblGasRPMLow.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblGasRPMHigh
            // 
            this.lblGasRPMHigh.BackColor = System.Drawing.Color.Black;
            this.lblGasRPMHigh.ForeColor = System.Drawing.Color.Gold;
            this.lblGasRPMHigh.Location = new System.Drawing.Point(106, 196);
            this.lblGasRPMHigh.Margin = new System.Windows.Forms.Padding(0);
            this.lblGasRPMHigh.Name = "lblGasRPMHigh";
            this.lblGasRPMHigh.Size = new System.Drawing.Size(278, 70);
            this.lblGasRPMHigh.TabIndex = 2;
            this.lblGasRPMHigh.Text = "--";
            this.lblGasRPMHigh.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblDieselRPMLow
            // 
            this.lblDieselRPMLow.BackColor = System.Drawing.Color.Black;
            this.lblDieselRPMLow.ForeColor = System.Drawing.Color.Gold;
            this.lblDieselRPMLow.Location = new System.Drawing.Point(498, 118);
            this.lblDieselRPMLow.Margin = new System.Windows.Forms.Padding(0);
            this.lblDieselRPMLow.Name = "lblDieselRPMLow";
            this.lblDieselRPMLow.Size = new System.Drawing.Size(278, 70);
            this.lblDieselRPMLow.TabIndex = 5;
            this.lblDieselRPMLow.Text = "--";
            this.lblDieselRPMLow.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblDieselRPMHigh
            // 
            this.lblDieselRPMHigh.BackColor = System.Drawing.Color.Black;
            this.lblDieselRPMHigh.ForeColor = System.Drawing.Color.Gold;
            this.lblDieselRPMHigh.Location = new System.Drawing.Point(498, 196);
            this.lblDieselRPMHigh.Margin = new System.Windows.Forms.Padding(0);
            this.lblDieselRPMHigh.Name = "lblDieselRPMHigh";
            this.lblDieselRPMHigh.Size = new System.Drawing.Size(278, 70);
            this.lblDieselRPMHigh.TabIndex = 6;
            this.lblDieselRPMHigh.Text = "--";
            this.lblDieselRPMHigh.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblDieselRPM
            // 
            this.lblDieselRPM.AutoSize = true;
            this.lblDieselRPM.BackColor = System.Drawing.Color.Black;
            this.lblDieselRPM.ForeColor = System.Drawing.Color.Gold;
            this.lblDieselRPM.Location = new System.Drawing.Point(498, 274);
            this.lblDieselRPM.Margin = new System.Windows.Forms.Padding(0);
            this.lblDieselRPM.Name = "lblDieselRPM";
            this.lblDieselRPM.Size = new System.Drawing.Size(278, 72);
            this.lblDieselRPM.TabIndex = 7;
            this.lblDieselRPM.Text = "--";
            this.lblDieselRPM.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblOBDRPM
            // 
            this.lblOBDRPM.AutoSize = true;
            this.lblOBDRPM.BackColor = System.Drawing.Color.Black;
            this.lblOBDRPM.ForeColor = System.Drawing.Color.Gold;
            this.lblOBDRPM.Location = new System.Drawing.Point(201, 360);
            this.lblOBDRPM.Margin = new System.Windows.Forms.Padding(0);
            this.lblOBDRPM.Name = "lblOBDRPM";
            this.lblOBDRPM.Size = new System.Drawing.Size(187, 45);
            this.lblOBDRPM.TabIndex = 9;
            this.lblOBDRPM.Text = "--";
            this.lblOBDRPM.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
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
            // layoutGroup2
            // 
            this.layoutGroup2.Height = 70;
            this.layoutGroup2.HeightType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            this.layoutGroup2.Items.AddRange(new DevComponents.DotNetBar.Layout.LayoutItemBase[] {
            this.layoutControlItem6,
            this.layoutControlItem8,
            this.layoutControlItem2});
            this.layoutGroup2.MinSize = new System.Drawing.Size(120, 32);
            this.layoutGroup2.Name = "layoutGroup2";
            this.layoutGroup2.Style.BorderColors = new DevComponents.DotNetBar.Layout.BorderColors(System.Drawing.SystemColors.ControlDark, System.Drawing.SystemColors.ControlDark, System.Drawing.SystemColors.ControlDark, System.Drawing.SystemColors.ControlDark);
            borderPattern1.Bottom = DevComponents.DotNetBar.Layout.LinePattern.Solid;
            borderPattern1.Left = DevComponents.DotNetBar.Layout.LinePattern.Solid;
            borderPattern1.Right = DevComponents.DotNetBar.Layout.LinePattern.Solid;
            borderPattern1.Top = DevComponents.DotNetBar.Layout.LinePattern.Solid;
            this.layoutGroup2.Style.BorderPattern = borderPattern1;
            this.layoutGroup2.Style.BorderThickness = new DevComponents.DotNetBar.Layout.Thickness(1D, 1D, 1D, 1D);
            this.layoutGroup2.Text = "汽油转速(r/min)";
            this.layoutGroup2.TextAlignment = DevComponents.DotNetBar.Layout.eTextAlignment.Center;
            this.layoutGroup2.TextLineAlignment = DevComponents.DotNetBar.Layout.eTextLineAlignment.Middle;
            this.layoutGroup2.TextPadding = new System.Windows.Forms.Padding(0, 10, 0, 0);
            this.layoutGroup2.TextPosition = DevComponents.DotNetBar.Layout.eLayoutPosition.Top;
            this.layoutGroup2.Width = 50;
            this.layoutGroup2.WidthType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            // 
            // layoutControlItem6
            // 
            this.layoutControlItem6.Control = this.lblGasRPMLow;
            this.layoutControlItem6.Height = 33;
            this.layoutControlItem6.HeightType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            this.layoutControlItem6.MinSize = new System.Drawing.Size(64, 18);
            this.layoutControlItem6.Name = "layoutControlItem6";
            this.layoutControlItem6.Text = "下限值:";
            this.layoutControlItem6.TextAlignment = DevComponents.DotNetBar.Layout.eTextAlignment.Center;
            this.layoutControlItem6.TextLineAlignment = DevComponents.DotNetBar.Layout.eTextLineAlignment.Middle;
            this.layoutControlItem6.Width = 100;
            this.layoutControlItem6.WidthType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            // 
            // layoutControlItem8
            // 
            this.layoutControlItem8.Control = this.lblGasRPMHigh;
            this.layoutControlItem8.Height = 33;
            this.layoutControlItem8.HeightType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            this.layoutControlItem8.MinSize = new System.Drawing.Size(64, 18);
            this.layoutControlItem8.Name = "layoutControlItem8";
            this.layoutControlItem8.Text = "上限值:";
            this.layoutControlItem8.TextAlignment = DevComponents.DotNetBar.Layout.eTextAlignment.Center;
            this.layoutControlItem8.TextLineAlignment = DevComponents.DotNetBar.Layout.eTextLineAlignment.Middle;
            this.layoutControlItem8.Width = 100;
            this.layoutControlItem8.WidthType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            // 
            // layoutControlItem2
            // 
            this.layoutControlItem2.Control = this.lblGasRPM;
            this.layoutControlItem2.Height = 34;
            this.layoutControlItem2.HeightType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            this.layoutControlItem2.Name = "layoutControlItem2";
            this.layoutControlItem2.Text = "实测值:";
            this.layoutControlItem2.TextAlignment = DevComponents.DotNetBar.Layout.eTextAlignment.Center;
            this.layoutControlItem2.TextLineAlignment = DevComponents.DotNetBar.Layout.eTextLineAlignment.Middle;
            this.layoutControlItem2.Width = 100;
            this.layoutControlItem2.WidthType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            // 
            // layoutGroup3
            // 
            this.layoutGroup3.Height = 70;
            this.layoutGroup3.HeightType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            this.layoutGroup3.Items.AddRange(new DevComponents.DotNetBar.Layout.LayoutItemBase[] {
            this.layoutControlItem10,
            this.layoutControlItem12,
            this.layoutControlItem14});
            this.layoutGroup3.MinSize = new System.Drawing.Size(120, 32);
            this.layoutGroup3.Name = "layoutGroup3";
            this.layoutGroup3.Style.BorderColors = new DevComponents.DotNetBar.Layout.BorderColors(System.Drawing.SystemColors.ControlDark, System.Drawing.SystemColors.ControlDark, System.Drawing.SystemColors.ControlDark, System.Drawing.SystemColors.ControlDark);
            borderPattern2.Bottom = DevComponents.DotNetBar.Layout.LinePattern.Solid;
            borderPattern2.Left = DevComponents.DotNetBar.Layout.LinePattern.Solid;
            borderPattern2.Right = DevComponents.DotNetBar.Layout.LinePattern.Solid;
            borderPattern2.Top = DevComponents.DotNetBar.Layout.LinePattern.Solid;
            this.layoutGroup3.Style.BorderPattern = borderPattern2;
            this.layoutGroup3.Style.BorderThickness = new DevComponents.DotNetBar.Layout.Thickness(0D, 1D, 1D, 1D);
            this.layoutGroup3.Text = "柴油转速(r/min)";
            this.layoutGroup3.TextAlignment = DevComponents.DotNetBar.Layout.eTextAlignment.Center;
            this.layoutGroup3.TextLineAlignment = DevComponents.DotNetBar.Layout.eTextLineAlignment.Middle;
            this.layoutGroup3.TextPadding = new System.Windows.Forms.Padding(0, 10, 0, 0);
            this.layoutGroup3.TextPosition = DevComponents.DotNetBar.Layout.eLayoutPosition.Top;
            this.layoutGroup3.Width = 50;
            this.layoutGroup3.WidthType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            // 
            // layoutControlItem10
            // 
            this.layoutControlItem10.Control = this.lblDieselRPMLow;
            this.layoutControlItem10.Height = 33;
            this.layoutControlItem10.HeightType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            this.layoutControlItem10.MinSize = new System.Drawing.Size(64, 18);
            this.layoutControlItem10.Name = "layoutControlItem10";
            this.layoutControlItem10.Text = "下限值:";
            this.layoutControlItem10.TextAlignment = DevComponents.DotNetBar.Layout.eTextAlignment.Center;
            this.layoutControlItem10.TextLineAlignment = DevComponents.DotNetBar.Layout.eTextLineAlignment.Middle;
            this.layoutControlItem10.Width = 100;
            this.layoutControlItem10.WidthType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            // 
            // layoutControlItem12
            // 
            this.layoutControlItem12.Control = this.lblDieselRPMHigh;
            this.layoutControlItem12.Height = 33;
            this.layoutControlItem12.HeightType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            this.layoutControlItem12.MinSize = new System.Drawing.Size(64, 18);
            this.layoutControlItem12.Name = "layoutControlItem12";
            this.layoutControlItem12.Text = "上限值:";
            this.layoutControlItem12.TextAlignment = DevComponents.DotNetBar.Layout.eTextAlignment.Center;
            this.layoutControlItem12.TextLineAlignment = DevComponents.DotNetBar.Layout.eTextLineAlignment.Middle;
            this.layoutControlItem12.Width = 100;
            this.layoutControlItem12.WidthType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            // 
            // layoutControlItem14
            // 
            this.layoutControlItem14.Control = this.lblDieselRPM;
            this.layoutControlItem14.Height = 34;
            this.layoutControlItem14.HeightType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            this.layoutControlItem14.MinSize = new System.Drawing.Size(64, 18);
            this.layoutControlItem14.Name = "layoutControlItem14";
            this.layoutControlItem14.Text = "实测值:";
            this.layoutControlItem14.TextAlignment = DevComponents.DotNetBar.Layout.eTextAlignment.Center;
            this.layoutControlItem14.TextLineAlignment = DevComponents.DotNetBar.Layout.eTextLineAlignment.Middle;
            this.layoutControlItem14.Width = 100;
            this.layoutControlItem14.WidthType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            // 
            // layoutControlItem16
            // 
            this.layoutControlItem16.Control = this.lblOBDRPM;
            this.layoutControlItem16.Height = 13;
            this.layoutControlItem16.HeightType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            this.layoutControlItem16.MinSize = new System.Drawing.Size(64, 18);
            this.layoutControlItem16.Name = "layoutControlItem16";
            this.layoutControlItem16.Text = "OBD转速(r/min):";
            this.layoutControlItem16.TextAlignment = DevComponents.DotNetBar.Layout.eTextAlignment.Center;
            this.layoutControlItem16.TextLineAlignment = DevComponents.DotNetBar.Layout.eTextLineAlignment.Middle;
            this.layoutControlItem16.Width = 50;
            this.layoutControlItem16.WidthType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            // 
            // layoutControlItem3
            // 
            this.layoutControlItem3.Control = this.btnStart;
            this.layoutControlItem3.Height = 13;
            this.layoutControlItem3.HeightType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            this.layoutControlItem3.Name = "layoutControlItem3";
            this.layoutControlItem3.Width = 25;
            this.layoutControlItem3.WidthType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            // 
            // layoutControlItem4
            // 
            this.layoutControlItem4.Control = this.btnStop;
            this.layoutControlItem4.Height = 13;
            this.layoutControlItem4.HeightType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            this.layoutControlItem4.Name = "layoutControlItem4";
            this.layoutControlItem4.Width = 25;
            this.layoutControlItem4.WidthType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            // 
            // layoutGroup1
            // 
            this.layoutGroup1.CaptionStyle.BorderThickness = new DevComponents.DotNetBar.Layout.Thickness(1D, 1D, 1D, 1D);
            this.layoutGroup1.Height = 70;
            this.layoutGroup1.HeightType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            this.layoutGroup1.Items.AddRange(new DevComponents.DotNetBar.Layout.LayoutItemBase[] {
            this.layoutControlItem5,
            this.layoutControlItem7,
            this.layoutControlItem9});
            this.layoutGroup1.Name = "layoutGroup1";
            this.layoutGroup1.Style.BorderColors = new DevComponents.DotNetBar.Layout.BorderColors(System.Drawing.SystemColors.ControlDark, System.Drawing.SystemColors.ControlDark, System.Drawing.SystemColors.ControlDark, System.Drawing.SystemColors.ControlDark);
            borderPattern3.Bottom = DevComponents.DotNetBar.Layout.LinePattern.Solid;
            borderPattern3.Left = DevComponents.DotNetBar.Layout.LinePattern.Solid;
            borderPattern3.Right = DevComponents.DotNetBar.Layout.LinePattern.Solid;
            borderPattern3.Top = DevComponents.DotNetBar.Layout.LinePattern.Solid;
            this.layoutGroup1.Style.BorderPattern = borderPattern3;
            this.layoutGroup1.Style.BorderThickness = new DevComponents.DotNetBar.Layout.Thickness(1D, 1D, 1D, 1D);
            this.layoutGroup1.Text = "汽油转速";
            this.layoutGroup1.TextAlignment = DevComponents.DotNetBar.Layout.eTextAlignment.Center;
            this.layoutGroup1.TextPadding = new System.Windows.Forms.Padding(0, 10, 0, 0);
            this.layoutGroup1.TextPosition = DevComponents.DotNetBar.Layout.eLayoutPosition.Top;
            this.layoutGroup1.Width = 50;
            this.layoutGroup1.WidthType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            // 
            // layoutControlItem5
            // 
            this.layoutControlItem5.Control = this.lblGasRPMLow;
            this.layoutControlItem5.Height = 33;
            this.layoutControlItem5.HeightType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            this.layoutControlItem5.Name = "layoutControlItem5";
            this.layoutControlItem5.Padding = new System.Windows.Forms.Padding(10);
            this.layoutControlItem5.Text = "转速下限:";
            this.layoutControlItem5.TextAlignment = DevComponents.DotNetBar.Layout.eTextAlignment.Center;
            this.layoutControlItem5.TextLineAlignment = DevComponents.DotNetBar.Layout.eTextLineAlignment.Middle;
            this.layoutControlItem5.Width = 100;
            this.layoutControlItem5.WidthType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            // 
            // layoutControlItem7
            // 
            this.layoutControlItem7.Control = this.lblGasRPMHigh;
            this.layoutControlItem7.Height = 33;
            this.layoutControlItem7.HeightType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            this.layoutControlItem7.Name = "layoutControlItem7";
            this.layoutControlItem7.Padding = new System.Windows.Forms.Padding(10);
            this.layoutControlItem7.Text = "转速上限:";
            this.layoutControlItem7.TextAlignment = DevComponents.DotNetBar.Layout.eTextAlignment.Center;
            this.layoutControlItem7.TextLineAlignment = DevComponents.DotNetBar.Layout.eTextLineAlignment.Middle;
            this.layoutControlItem7.Width = 100;
            this.layoutControlItem7.WidthType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            // 
            // layoutControlItem9
            // 
            this.layoutControlItem9.Control = this.lblDieselRPMLow;
            this.layoutControlItem9.Height = 34;
            this.layoutControlItem9.HeightType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            this.layoutControlItem9.Name = "layoutControlItem9";
            this.layoutControlItem9.Padding = new System.Windows.Forms.Padding(10);
            this.layoutControlItem9.Text = "实测值:";
            this.layoutControlItem9.TextAlignment = DevComponents.DotNetBar.Layout.eTextAlignment.Center;
            this.layoutControlItem9.TextLineAlignment = DevComponents.DotNetBar.Layout.eTextLineAlignment.Middle;
            this.layoutControlItem9.Width = 100;
            this.layoutControlItem9.WidthType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            // 
            // layoutControlItem11
            // 
            this.layoutControlItem11.Control = this.lblDieselRPMHigh;
            this.layoutControlItem11.Height = 33;
            this.layoutControlItem11.HeightType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            this.layoutControlItem11.MinSize = new System.Drawing.Size(64, 18);
            this.layoutControlItem11.Name = "layoutControlItem11";
            this.layoutControlItem11.Text = "下限值:";
            this.layoutControlItem11.TextAlignment = DevComponents.DotNetBar.Layout.eTextAlignment.Center;
            this.layoutControlItem11.TextLineAlignment = DevComponents.DotNetBar.Layout.eTextLineAlignment.Middle;
            this.layoutControlItem11.Width = 100;
            this.layoutControlItem11.WidthType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            // 
            // layoutControlItem13
            // 
            this.layoutControlItem13.Control = this.lblDieselRPM;
            this.layoutControlItem13.Height = 34;
            this.layoutControlItem13.HeightType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            this.layoutControlItem13.Name = "layoutControlItem13";
            this.layoutControlItem13.Text = "实测值:";
            this.layoutControlItem13.TextAlignment = DevComponents.DotNetBar.Layout.eTextAlignment.Center;
            this.layoutControlItem13.TextLineAlignment = DevComponents.DotNetBar.Layout.eTextLineAlignment.Middle;
            this.layoutControlItem13.Width = 100;
            this.layoutControlItem13.WidthType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            // 
            // layoutControlItem15
            // 
            this.layoutControlItem15.Control = this.lblOBDRPM;
            this.layoutControlItem15.Height = 34;
            this.layoutControlItem15.HeightType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            this.layoutControlItem15.Name = "layoutControlItem15";
            this.layoutControlItem15.Text = "实测值:";
            this.layoutControlItem15.TextAlignment = DevComponents.DotNetBar.Layout.eTextAlignment.Center;
            this.layoutControlItem15.TextLineAlignment = DevComponents.DotNetBar.Layout.eTextLineAlignment.Middle;
            this.layoutControlItem15.Width = 100;
            this.layoutControlItem15.WidthType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            // 
            // TachometerSelfcheckSubForm
            // 
            this.AcceptButton = this.btnStart;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(784, 411);
            this.Controls.Add(this.layoutMain);
            this.Name = "TachometerSelfcheckSubForm";
            this.Text = "TachometerSelfcheckSubForm";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.TachometerSelfcheckSubForm_FormClosing);
            this.Load += new System.EventHandler(this.TachometerSelfcheckSubForm_Load);
            this.Resize += new System.EventHandler(this.TachometerSelfcheckSubForm_Resize);
            this.layoutMain.ResumeLayout(false);
            this.layoutMain.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private DevComponents.DotNetBar.Layout.LayoutControl layoutMain;
        private System.Windows.Forms.Button btnStop;
        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.Label lblGasRPM;
        private System.Windows.Forms.Label lblMsg;
        private DevComponents.DotNetBar.Layout.LayoutControlItem layoutControlItem1;
        private DevComponents.DotNetBar.Layout.LayoutControlItem layoutControlItem2;
        private DevComponents.DotNetBar.Layout.LayoutControlItem layoutControlItem3;
        private DevComponents.DotNetBar.Layout.LayoutControlItem layoutControlItem4;
        private System.Windows.Forms.Label lblGasRPMLow;
        private System.Windows.Forms.Label lblGasRPMHigh;
        private System.Windows.Forms.Label lblDieselRPMLow;
        private DevComponents.DotNetBar.Layout.LayoutGroup layoutGroup2;
        private DevComponents.DotNetBar.Layout.LayoutControlItem layoutControlItem6;
        private DevComponents.DotNetBar.Layout.LayoutControlItem layoutControlItem8;
        private DevComponents.DotNetBar.Layout.LayoutControlItem layoutControlItem10;
        private DevComponents.DotNetBar.Layout.LayoutGroup layoutGroup1;
        private DevComponents.DotNetBar.Layout.LayoutControlItem layoutControlItem5;
        private DevComponents.DotNetBar.Layout.LayoutControlItem layoutControlItem7;
        private DevComponents.DotNetBar.Layout.LayoutControlItem layoutControlItem9;
        private DevComponents.DotNetBar.Layout.LayoutGroup layoutGroup3;
        private System.Windows.Forms.Label lblDieselRPMHigh;
        private System.Windows.Forms.Label lblDieselRPM;
        private DevComponents.DotNetBar.Layout.LayoutControlItem layoutControlItem12;
        private DevComponents.DotNetBar.Layout.LayoutControlItem layoutControlItem14;
        private DevComponents.DotNetBar.Layout.LayoutControlItem layoutControlItem11;
        private DevComponents.DotNetBar.Layout.LayoutControlItem layoutControlItem13;
        private System.Windows.Forms.Label lblOBDRPM;
        private DevComponents.DotNetBar.Layout.LayoutControlItem layoutControlItem16;
        private DevComponents.DotNetBar.Layout.LayoutControlItem layoutControlItem15;
    }
}