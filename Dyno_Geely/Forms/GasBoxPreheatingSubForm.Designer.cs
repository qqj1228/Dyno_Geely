namespace Dyno_Geely.Forms {
    partial class GasBoxPreheatingSubForm {
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
            this.lblResult = new System.Windows.Forms.Label();
            this.lblO2Span = new System.Windows.Forms.Label();
            this.lblLowFlow = new System.Windows.Forms.Label();
            this.lblLeak = new System.Windows.Forms.Label();
            this.lblZero = new System.Windows.Forms.Label();
            this.btnStart = new System.Windows.Forms.Button();
            this.btnStop = new System.Windows.Forms.Button();
            this.lblHC = new System.Windows.Forms.Label();
            this.lblNO = new System.Windows.Forms.Label();
            this.lblCO = new System.Windows.Forms.Label();
            this.lblCO2 = new System.Windows.Forms.Label();
            this.lblO2 = new System.Windows.Forms.Label();
            this.lblPEF = new System.Windows.Forms.Label();
            this.lblMsg = new System.Windows.Forms.Label();
            this.lblWarmUp = new System.Windows.Forms.Label();
            this.layoutMain = new DevComponents.DotNetBar.Layout.LayoutControl();
            this.layoutControlItem1 = new DevComponents.DotNetBar.Layout.LayoutControlItem();
            this.layoutGroup2 = new DevComponents.DotNetBar.Layout.LayoutGroup();
            this.layoutControlItem2 = new DevComponents.DotNetBar.Layout.LayoutControlItem();
            this.layoutControlItem3 = new DevComponents.DotNetBar.Layout.LayoutControlItem();
            this.layoutControlItem4 = new DevComponents.DotNetBar.Layout.LayoutControlItem();
            this.layoutControlItem5 = new DevComponents.DotNetBar.Layout.LayoutControlItem();
            this.layoutControlItem6 = new DevComponents.DotNetBar.Layout.LayoutControlItem();
            this.layoutControlItem7 = new DevComponents.DotNetBar.Layout.LayoutControlItem();
            this.layoutControlItem8 = new DevComponents.DotNetBar.Layout.LayoutControlItem();
            this.layoutControlItem9 = new DevComponents.DotNetBar.Layout.LayoutControlItem();
            this.layoutControlItem10 = new DevComponents.DotNetBar.Layout.LayoutControlItem();
            this.layoutControlItem11 = new DevComponents.DotNetBar.Layout.LayoutControlItem();
            this.layoutControlItem12 = new DevComponents.DotNetBar.Layout.LayoutControlItem();
            this.layoutControlItem13 = new DevComponents.DotNetBar.Layout.LayoutControlItem();
            this.layoutControlItem14 = new DevComponents.DotNetBar.Layout.LayoutControlItem();
            this.layoutControlItem15 = new DevComponents.DotNetBar.Layout.LayoutControlItem();
            this.layoutGroup1 = new DevComponents.DotNetBar.Layout.LayoutGroup();
            this.layoutMain.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblResult
            // 
            this.lblResult.BackColor = System.Drawing.Color.Black;
            this.lblResult.ForeColor = System.Drawing.Color.Gold;
            this.lblResult.Location = new System.Drawing.Point(553, 309);
            this.lblResult.Margin = new System.Windows.Forms.Padding(0);
            this.lblResult.Name = "lblResult";
            this.lblResult.Size = new System.Drawing.Size(223, 36);
            this.lblResult.TabIndex = 13;
            this.lblResult.Text = "--";
            this.lblResult.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblO2Span
            // 
            this.lblO2Span.BackColor = System.Drawing.Color.Black;
            this.lblO2Span.ForeColor = System.Drawing.Color.Gold;
            this.lblO2Span.Location = new System.Drawing.Point(553, 265);
            this.lblO2Span.Margin = new System.Windows.Forms.Padding(0);
            this.lblO2Span.Name = "lblO2Span";
            this.lblO2Span.Size = new System.Drawing.Size(223, 36);
            this.lblO2Span.TabIndex = 12;
            this.lblO2Span.Text = "--";
            this.lblO2Span.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblLowFlow
            // 
            this.lblLowFlow.BackColor = System.Drawing.Color.Black;
            this.lblLowFlow.ForeColor = System.Drawing.Color.Gold;
            this.lblLowFlow.Location = new System.Drawing.Point(553, 218);
            this.lblLowFlow.Margin = new System.Windows.Forms.Padding(0);
            this.lblLowFlow.Name = "lblLowFlow";
            this.lblLowFlow.Size = new System.Drawing.Size(223, 39);
            this.lblLowFlow.TabIndex = 11;
            this.lblLowFlow.Text = "--";
            this.lblLowFlow.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblLeak
            // 
            this.lblLeak.BackColor = System.Drawing.Color.Black;
            this.lblLeak.ForeColor = System.Drawing.Color.Gold;
            this.lblLeak.Location = new System.Drawing.Point(553, 171);
            this.lblLeak.Margin = new System.Windows.Forms.Padding(0);
            this.lblLeak.Name = "lblLeak";
            this.lblLeak.Size = new System.Drawing.Size(223, 39);
            this.lblLeak.TabIndex = 10;
            this.lblLeak.Text = "--";
            this.lblLeak.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblZero
            // 
            this.lblZero.BackColor = System.Drawing.Color.Black;
            this.lblZero.ForeColor = System.Drawing.Color.Gold;
            this.lblZero.Location = new System.Drawing.Point(553, 124);
            this.lblZero.Margin = new System.Windows.Forms.Padding(0);
            this.lblZero.Name = "lblZero";
            this.lblZero.Size = new System.Drawing.Size(223, 39);
            this.lblZero.TabIndex = 9;
            this.lblZero.Text = "--";
            this.lblZero.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnStart
            // 
            this.btnStart.Location = new System.Drawing.Point(4, 360);
            this.btnStart.Margin = new System.Windows.Forms.Padding(0);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(384, 45);
            this.btnStart.TabIndex = 15;
            this.btnStart.Text = "开始预热";
            this.btnStart.UseVisualStyleBackColor = true;
            this.btnStart.Click += new System.EventHandler(this.BtnStart_Click);
            // 
            // btnStop
            // 
            this.btnStop.Location = new System.Drawing.Point(396, 360);
            this.btnStop.Margin = new System.Windows.Forms.Padding(0);
            this.btnStop.Name = "btnStop";
            this.btnStop.Size = new System.Drawing.Size(384, 45);
            this.btnStop.TabIndex = 16;
            this.btnStop.Text = "手动停止";
            this.btnStop.UseVisualStyleBackColor = true;
            this.btnStop.Click += new System.EventHandler(this.BtnStop_Click);
            // 
            // lblHC
            // 
            this.lblHC.BackColor = System.Drawing.Color.Black;
            this.lblHC.ForeColor = System.Drawing.Color.Gold;
            this.lblHC.Location = new System.Drawing.Point(107, 77);
            this.lblHC.Margin = new System.Windows.Forms.Padding(0);
            this.lblHC.Name = "lblHC";
            this.lblHC.Size = new System.Drawing.Size(277, 39);
            this.lblHC.TabIndex = 1;
            this.lblHC.Text = "--";
            this.lblHC.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblNO
            // 
            this.lblNO.BackColor = System.Drawing.Color.Black;
            this.lblNO.ForeColor = System.Drawing.Color.Gold;
            this.lblNO.Location = new System.Drawing.Point(107, 124);
            this.lblNO.Margin = new System.Windows.Forms.Padding(0);
            this.lblNO.Name = "lblNO";
            this.lblNO.Size = new System.Drawing.Size(277, 39);
            this.lblNO.TabIndex = 2;
            this.lblNO.Text = "--";
            this.lblNO.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblCO
            // 
            this.lblCO.BackColor = System.Drawing.Color.Black;
            this.lblCO.ForeColor = System.Drawing.Color.Gold;
            this.lblCO.Location = new System.Drawing.Point(107, 171);
            this.lblCO.Margin = new System.Windows.Forms.Padding(0);
            this.lblCO.Name = "lblCO";
            this.lblCO.Size = new System.Drawing.Size(277, 39);
            this.lblCO.TabIndex = 3;
            this.lblCO.Text = "--";
            this.lblCO.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblCO2
            // 
            this.lblCO2.BackColor = System.Drawing.Color.Black;
            this.lblCO2.ForeColor = System.Drawing.Color.Gold;
            this.lblCO2.Location = new System.Drawing.Point(107, 218);
            this.lblCO2.Margin = new System.Windows.Forms.Padding(0);
            this.lblCO2.Name = "lblCO2";
            this.lblCO2.Size = new System.Drawing.Size(277, 39);
            this.lblCO2.TabIndex = 4;
            this.lblCO2.Text = "--";
            this.lblCO2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblO2
            // 
            this.lblO2.BackColor = System.Drawing.Color.Black;
            this.lblO2.ForeColor = System.Drawing.Color.Gold;
            this.lblO2.Location = new System.Drawing.Point(107, 265);
            this.lblO2.Margin = new System.Windows.Forms.Padding(0);
            this.lblO2.Name = "lblO2";
            this.lblO2.Size = new System.Drawing.Size(277, 36);
            this.lblO2.TabIndex = 5;
            this.lblO2.Text = "--";
            this.lblO2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblPEF
            // 
            this.lblPEF.BackColor = System.Drawing.Color.Black;
            this.lblPEF.ForeColor = System.Drawing.Color.Gold;
            this.lblPEF.Location = new System.Drawing.Point(107, 309);
            this.lblPEF.Margin = new System.Windows.Forms.Padding(0);
            this.lblPEF.Name = "lblPEF";
            this.lblPEF.Size = new System.Drawing.Size(277, 36);
            this.lblPEF.TabIndex = 6;
            this.lblPEF.Text = "--";
            this.lblPEF.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblMsg
            // 
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
            // lblWarmUp
            // 
            this.lblWarmUp.BackColor = System.Drawing.Color.Black;
            this.lblWarmUp.ForeColor = System.Drawing.Color.Gold;
            this.lblWarmUp.Location = new System.Drawing.Point(553, 77);
            this.lblWarmUp.Margin = new System.Windows.Forms.Padding(0);
            this.lblWarmUp.Name = "lblWarmUp";
            this.lblWarmUp.Size = new System.Drawing.Size(223, 39);
            this.lblWarmUp.TabIndex = 8;
            this.lblWarmUp.Text = "--";
            this.lblWarmUp.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // layoutMain
            // 
            this.layoutMain.BackColor = System.Drawing.Color.Transparent;
            this.layoutMain.Controls.Add(this.lblMsg);
            this.layoutMain.Controls.Add(this.lblHC);
            this.layoutMain.Controls.Add(this.lblNO);
            this.layoutMain.Controls.Add(this.lblCO);
            this.layoutMain.Controls.Add(this.lblCO2);
            this.layoutMain.Controls.Add(this.lblO2);
            this.layoutMain.Controls.Add(this.lblPEF);
            this.layoutMain.Controls.Add(this.lblWarmUp);
            this.layoutMain.Controls.Add(this.lblZero);
            this.layoutMain.Controls.Add(this.lblLeak);
            this.layoutMain.Controls.Add(this.lblLowFlow);
            this.layoutMain.Controls.Add(this.lblO2Span);
            this.layoutMain.Controls.Add(this.lblResult);
            this.layoutMain.Controls.Add(this.btnStart);
            this.layoutMain.Controls.Add(this.btnStop);
            this.layoutMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.layoutMain.Font = new System.Drawing.Font("宋体", 16F);
            this.layoutMain.Location = new System.Drawing.Point(0, 0);
            this.layoutMain.Name = "layoutMain";
            // 
            // 
            // 
            this.layoutMain.RootGroup.Items.AddRange(new DevComponents.DotNetBar.Layout.LayoutItemBase[] {
            this.layoutControlItem1,
            this.layoutGroup2,
            this.layoutGroup1,
            this.layoutControlItem14,
            this.layoutControlItem15});
            this.layoutMain.Size = new System.Drawing.Size(784, 411);
            this.layoutMain.TabIndex = 1;
            // 
            // layoutControlItem1
            // 
            this.layoutControlItem1.Control = this.lblMsg;
            this.layoutControlItem1.Height = 17;
            this.layoutControlItem1.HeightType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            this.layoutControlItem1.MinSize = new System.Drawing.Size(64, 18);
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
            this.layoutControlItem2,
            this.layoutControlItem3,
            this.layoutControlItem4,
            this.layoutControlItem5,
            this.layoutControlItem6,
            this.layoutControlItem7});
            this.layoutGroup2.MinSize = new System.Drawing.Size(120, 32);
            this.layoutGroup2.Name = "layoutGroup2";
            this.layoutGroup2.TextPosition = DevComponents.DotNetBar.Layout.eLayoutPosition.Top;
            this.layoutGroup2.Width = 50;
            this.layoutGroup2.WidthType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            // 
            // layoutControlItem2
            // 
            this.layoutControlItem2.Control = this.lblHC;
            this.layoutControlItem2.Height = 17;
            this.layoutControlItem2.HeightType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            this.layoutControlItem2.Name = "layoutControlItem2";
            this.layoutControlItem2.Text = "HC(ppm):";
            this.layoutControlItem2.TextAlignment = DevComponents.DotNetBar.Layout.eTextAlignment.Center;
            this.layoutControlItem2.TextLineAlignment = DevComponents.DotNetBar.Layout.eTextLineAlignment.Middle;
            this.layoutControlItem2.Width = 100;
            this.layoutControlItem2.WidthType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            // 
            // layoutControlItem3
            // 
            this.layoutControlItem3.Control = this.lblNO;
            this.layoutControlItem3.Height = 17;
            this.layoutControlItem3.HeightType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            this.layoutControlItem3.Name = "layoutControlItem3";
            this.layoutControlItem3.Text = "NO(ppm):";
            this.layoutControlItem3.TextAlignment = DevComponents.DotNetBar.Layout.eTextAlignment.Center;
            this.layoutControlItem3.TextLineAlignment = DevComponents.DotNetBar.Layout.eTextLineAlignment.Middle;
            this.layoutControlItem3.Width = 100;
            this.layoutControlItem3.WidthType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            // 
            // layoutControlItem4
            // 
            this.layoutControlItem4.Control = this.lblCO;
            this.layoutControlItem4.Height = 17;
            this.layoutControlItem4.HeightType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            this.layoutControlItem4.Name = "layoutControlItem4";
            this.layoutControlItem4.Text = "CO(%):";
            this.layoutControlItem4.TextAlignment = DevComponents.DotNetBar.Layout.eTextAlignment.Center;
            this.layoutControlItem4.TextLineAlignment = DevComponents.DotNetBar.Layout.eTextLineAlignment.Middle;
            this.layoutControlItem4.Width = 100;
            this.layoutControlItem4.WidthType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            // 
            // layoutControlItem5
            // 
            this.layoutControlItem5.Control = this.lblCO2;
            this.layoutControlItem5.Height = 17;
            this.layoutControlItem5.HeightType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            this.layoutControlItem5.Name = "layoutControlItem5";
            this.layoutControlItem5.Text = "CO2(%):";
            this.layoutControlItem5.TextAlignment = DevComponents.DotNetBar.Layout.eTextAlignment.Center;
            this.layoutControlItem5.TextLineAlignment = DevComponents.DotNetBar.Layout.eTextLineAlignment.Middle;
            this.layoutControlItem5.Width = 100;
            this.layoutControlItem5.WidthType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            // 
            // layoutControlItem6
            // 
            this.layoutControlItem6.Control = this.lblO2;
            this.layoutControlItem6.Height = 16;
            this.layoutControlItem6.HeightType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            this.layoutControlItem6.Name = "layoutControlItem6";
            this.layoutControlItem6.Text = "O2(%):";
            this.layoutControlItem6.TextAlignment = DevComponents.DotNetBar.Layout.eTextAlignment.Center;
            this.layoutControlItem6.TextLineAlignment = DevComponents.DotNetBar.Layout.eTextLineAlignment.Middle;
            this.layoutControlItem6.Width = 100;
            this.layoutControlItem6.WidthType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            // 
            // layoutControlItem7
            // 
            this.layoutControlItem7.Control = this.lblPEF;
            this.layoutControlItem7.Height = 16;
            this.layoutControlItem7.HeightType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            this.layoutControlItem7.Name = "layoutControlItem7";
            this.layoutControlItem7.Text = "PEF:";
            this.layoutControlItem7.TextAlignment = DevComponents.DotNetBar.Layout.eTextAlignment.Center;
            this.layoutControlItem7.TextLineAlignment = DevComponents.DotNetBar.Layout.eTextLineAlignment.Middle;
            this.layoutControlItem7.Width = 100;
            this.layoutControlItem7.WidthType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            // 
            // layoutControlItem8
            // 
            this.layoutControlItem8.Control = this.lblWarmUp;
            this.layoutControlItem8.Height = 17;
            this.layoutControlItem8.HeightType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            this.layoutControlItem8.Name = "layoutControlItem8";
            this.layoutControlItem8.Text = "1.预热检查:";
            this.layoutControlItem8.TextAlignment = DevComponents.DotNetBar.Layout.eTextAlignment.Center;
            this.layoutControlItem8.TextLineAlignment = DevComponents.DotNetBar.Layout.eTextLineAlignment.Middle;
            this.layoutControlItem8.Width = 100;
            this.layoutControlItem8.WidthType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            // 
            // layoutControlItem9
            // 
            this.layoutControlItem9.Control = this.lblZero;
            this.layoutControlItem9.Height = 17;
            this.layoutControlItem9.HeightType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            this.layoutControlItem9.Name = "layoutControlItem9";
            this.layoutControlItem9.Text = "2.清零检查:";
            this.layoutControlItem9.TextAlignment = DevComponents.DotNetBar.Layout.eTextAlignment.Center;
            this.layoutControlItem9.TextLineAlignment = DevComponents.DotNetBar.Layout.eTextLineAlignment.Middle;
            this.layoutControlItem9.Width = 100;
            this.layoutControlItem9.WidthType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            // 
            // layoutControlItem10
            // 
            this.layoutControlItem10.Control = this.lblLeak;
            this.layoutControlItem10.Height = 17;
            this.layoutControlItem10.HeightType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            this.layoutControlItem10.Name = "layoutControlItem10";
            this.layoutControlItem10.Text = "3.泄漏检查:";
            this.layoutControlItem10.TextAlignment = DevComponents.DotNetBar.Layout.eTextAlignment.Center;
            this.layoutControlItem10.TextLineAlignment = DevComponents.DotNetBar.Layout.eTextLineAlignment.Middle;
            this.layoutControlItem10.Width = 100;
            this.layoutControlItem10.WidthType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            // 
            // layoutControlItem11
            // 
            this.layoutControlItem11.Control = this.lblLowFlow;
            this.layoutControlItem11.Height = 17;
            this.layoutControlItem11.HeightType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            this.layoutControlItem11.Name = "layoutControlItem11";
            this.layoutControlItem11.Text = "4.低流量检查:";
            this.layoutControlItem11.TextAlignment = DevComponents.DotNetBar.Layout.eTextAlignment.Center;
            this.layoutControlItem11.TextLineAlignment = DevComponents.DotNetBar.Layout.eTextLineAlignment.Middle;
            this.layoutControlItem11.Width = 100;
            this.layoutControlItem11.WidthType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            // 
            // layoutControlItem12
            // 
            this.layoutControlItem12.Control = this.lblO2Span;
            this.layoutControlItem12.Height = 16;
            this.layoutControlItem12.HeightType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            this.layoutControlItem12.Name = "layoutControlItem12";
            this.layoutControlItem12.Text = "5.氧量程检查:";
            this.layoutControlItem12.TextAlignment = DevComponents.DotNetBar.Layout.eTextAlignment.Center;
            this.layoutControlItem12.TextLineAlignment = DevComponents.DotNetBar.Layout.eTextLineAlignment.Middle;
            this.layoutControlItem12.Width = 100;
            this.layoutControlItem12.WidthType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            // 
            // layoutControlItem13
            // 
            this.layoutControlItem13.Control = this.lblResult;
            this.layoutControlItem13.Height = 16;
            this.layoutControlItem13.HeightType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            this.layoutControlItem13.Name = "layoutControlItem13";
            this.layoutControlItem13.Text = "6.预热总结果:";
            this.layoutControlItem13.TextAlignment = DevComponents.DotNetBar.Layout.eTextAlignment.Center;
            this.layoutControlItem13.TextLineAlignment = DevComponents.DotNetBar.Layout.eTextLineAlignment.Middle;
            this.layoutControlItem13.Width = 16;
            this.layoutControlItem13.WidthType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            // 
            // layoutControlItem14
            // 
            this.layoutControlItem14.Control = this.btnStart;
            this.layoutControlItem14.Height = 13;
            this.layoutControlItem14.HeightType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            this.layoutControlItem14.MinSize = new System.Drawing.Size(32, 20);
            this.layoutControlItem14.Name = "layoutControlItem14";
            this.layoutControlItem14.Width = 50;
            this.layoutControlItem14.WidthType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            // 
            // layoutControlItem15
            // 
            this.layoutControlItem15.Control = this.btnStop;
            this.layoutControlItem15.Height = 13;
            this.layoutControlItem15.HeightType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            this.layoutControlItem15.MinSize = new System.Drawing.Size(32, 20);
            this.layoutControlItem15.Name = "layoutControlItem15";
            this.layoutControlItem15.Width = 50;
            this.layoutControlItem15.WidthType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            // 
            // layoutGroup1
            // 
            this.layoutGroup1.Height = 70;
            this.layoutGroup1.HeightType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            this.layoutGroup1.Items.AddRange(new DevComponents.DotNetBar.Layout.LayoutItemBase[] {
            this.layoutControlItem8,
            this.layoutControlItem9,
            this.layoutControlItem10,
            this.layoutControlItem11,
            this.layoutControlItem12,
            this.layoutControlItem13});
            this.layoutGroup1.MinSize = new System.Drawing.Size(120, 32);
            this.layoutGroup1.Name = "layoutGroup1";
            this.layoutGroup1.TextPosition = DevComponents.DotNetBar.Layout.eLayoutPosition.Top;
            this.layoutGroup1.Width = 50;
            this.layoutGroup1.WidthType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            // 
            // GasBoxPreheatingSubForm
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            this.ClientSize = new System.Drawing.Size(784, 411);
            this.Controls.Add(this.layoutMain);
            this.DoubleBuffered = true;
            this.Name = "GasBoxPreheatingSubForm";
            this.Text = "GasBoxPreheatingForm";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.GasBoxPreheatingSubForm_FormClosing);
            this.Load += new System.EventHandler(this.GasBoxPreheatingSubForm_Load);
            this.Resize += new System.EventHandler(this.GasBoxPreheatingSubForm_Resize);
            this.layoutMain.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.Button btnStop;
        private System.Windows.Forms.Label lblHC;
        private System.Windows.Forms.Label lblNO;
        private System.Windows.Forms.Label lblCO;
        private System.Windows.Forms.Label lblCO2;
        private System.Windows.Forms.Label lblO2;
        private System.Windows.Forms.Label lblPEF;
        private System.Windows.Forms.Label lblMsg;
        private System.Windows.Forms.Label lblWarmUp;
        private System.Windows.Forms.Label lblResult;
        private System.Windows.Forms.Label lblO2Span;
        private System.Windows.Forms.Label lblLowFlow;
        private System.Windows.Forms.Label lblLeak;
        private System.Windows.Forms.Label lblZero;
        private DevComponents.DotNetBar.Layout.LayoutControl layoutMain;
        private DevComponents.DotNetBar.Layout.LayoutControlItem layoutControlItem1;
        private DevComponents.DotNetBar.Layout.LayoutControlItem layoutControlItem2;
        private DevComponents.DotNetBar.Layout.LayoutControlItem layoutControlItem3;
        private DevComponents.DotNetBar.Layout.LayoutControlItem layoutControlItem4;
        private DevComponents.DotNetBar.Layout.LayoutControlItem layoutControlItem5;
        private DevComponents.DotNetBar.Layout.LayoutControlItem layoutControlItem6;
        private DevComponents.DotNetBar.Layout.LayoutControlItem layoutControlItem7;
        private DevComponents.DotNetBar.Layout.LayoutControlItem layoutControlItem8;
        private DevComponents.DotNetBar.Layout.LayoutControlItem layoutControlItem9;
        private DevComponents.DotNetBar.Layout.LayoutControlItem layoutControlItem10;
        private DevComponents.DotNetBar.Layout.LayoutControlItem layoutControlItem11;
        private DevComponents.DotNetBar.Layout.LayoutControlItem layoutControlItem12;
        private DevComponents.DotNetBar.Layout.LayoutControlItem layoutControlItem13;
        private DevComponents.DotNetBar.Layout.LayoutControlItem layoutControlItem14;
        private DevComponents.DotNetBar.Layout.LayoutControlItem layoutControlItem15;
        private DevComponents.DotNetBar.Layout.LayoutGroup layoutGroup2;
        private DevComponents.DotNetBar.Layout.LayoutGroup layoutGroup1;
    }
}