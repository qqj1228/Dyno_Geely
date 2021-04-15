namespace Dyno_Geely {
    partial class VMASForm {
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
            DevComponents.Instrumentation.GaugeCircularScale gaugeCircularScale1 = new DevComponents.Instrumentation.GaugeCircularScale();
            DevComponents.Instrumentation.GaugePointer gaugePointer1 = new DevComponents.Instrumentation.GaugePointer();
            DevComponents.Instrumentation.GaugeSection gaugeSection1 = new DevComponents.Instrumentation.GaugeSection();
            DevComponents.Instrumentation.GaugeSection gaugeSection2 = new DevComponents.Instrumentation.GaugeSection();
            DevComponents.Instrumentation.GradientFillColor gradientFillColor1 = new DevComponents.Instrumentation.GradientFillColor();
            DevComponents.Instrumentation.GradientFillColor gradientFillColor2 = new DevComponents.Instrumentation.GradientFillColor();
            DevComponents.Instrumentation.GaugeText gaugeText1 = new DevComponents.Instrumentation.GaugeText();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(VMASForm));
            DevComponents.Instrumentation.NumericIndicator numericIndicator1 = new DevComponents.Instrumentation.NumericIndicator();
            DevComponents.Instrumentation.NumericRange numericRange1 = new DevComponents.Instrumentation.NumericRange();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend1 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
            this.layoutMain = new DevComponents.DotNetBar.Layout.LayoutControl();
            this.lblRPM = new System.Windows.Forms.Label();
            this.lblMsg = new System.Windows.Forms.Label();
            this.gaugeSpeed = new DevComponents.Instrumentation.GaugeControl();
            this.chart1 = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.lblHC = new System.Windows.Forms.Label();
            this.lblNO = new System.Windows.Forms.Label();
            this.lblCO = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.btnRestart = new System.Windows.Forms.Button();
            this.btnStop = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.layoutControlItem1 = new DevComponents.DotNetBar.Layout.LayoutControlItem();
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
            this.layoutMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gaugeSpeed)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chart1)).BeginInit();
            this.SuspendLayout();
            // 
            // layoutMain
            // 
            this.layoutMain.BackColor = System.Drawing.Color.Transparent;
            this.layoutMain.Controls.Add(this.lblRPM);
            this.layoutMain.Controls.Add(this.lblMsg);
            this.layoutMain.Controls.Add(this.gaugeSpeed);
            this.layoutMain.Controls.Add(this.chart1);
            this.layoutMain.Controls.Add(this.lblHC);
            this.layoutMain.Controls.Add(this.lblNO);
            this.layoutMain.Controls.Add(this.lblCO);
            this.layoutMain.Controls.Add(this.label1);
            this.layoutMain.Controls.Add(this.btnRestart);
            this.layoutMain.Controls.Add(this.btnStop);
            this.layoutMain.Controls.Add(this.btnCancel);
            this.layoutMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.layoutMain.Font = new System.Drawing.Font("宋体", 16F);
            this.layoutMain.Location = new System.Drawing.Point(0, 0);
            this.layoutMain.Name = "layoutMain";
            // 
            // 
            // 
            this.layoutMain.RootGroup.Items.AddRange(new DevComponents.DotNetBar.Layout.LayoutItemBase[] {
            this.layoutControlItem1,
            this.layoutControlItem2,
            this.layoutControlItem3,
            this.layoutControlItem4,
            this.layoutControlItem5,
            this.layoutControlItem6,
            this.layoutControlItem7,
            this.layoutControlItem8,
            this.layoutControlItem9,
            this.layoutControlItem10,
            this.layoutControlItem11});
            this.layoutMain.Size = new System.Drawing.Size(1008, 729);
            this.layoutMain.TabIndex = 0;
            // 
            // lblRPM
            // 
            this.lblRPM.AutoSize = true;
            this.lblRPM.BackColor = System.Drawing.Color.Black;
            this.lblRPM.ForeColor = System.Drawing.Color.Gold;
            this.lblRPM.Location = new System.Drawing.Point(135, 637);
            this.lblRPM.Margin = new System.Windows.Forms.Padding(0);
            this.lblRPM.Name = "lblRPM";
            this.lblRPM.Size = new System.Drawing.Size(113, 35);
            this.lblRPM.TabIndex = 3;
            this.lblRPM.Text = "--";
            this.lblRPM.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblMsg
            // 
            this.lblMsg.AutoSize = true;
            this.lblMsg.Font = new System.Drawing.Font("黑体", 30F, System.Drawing.FontStyle.Bold);
            this.lblMsg.ForeColor = System.Drawing.Color.Red;
            this.lblMsg.Location = new System.Drawing.Point(4, 4);
            this.lblMsg.Margin = new System.Windows.Forms.Padding(0);
            this.lblMsg.Name = "lblMsg";
            this.lblMsg.Size = new System.Drawing.Size(1000, 115);
            this.lblMsg.TabIndex = 0;
            this.lblMsg.Text = "Msg";
            this.lblMsg.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // gaugeSpeed
            // 
            gaugeCircularScale1.Labels.Layout.RotateLabel = false;
            gaugeCircularScale1.MajorTickMarks.Interval = 2D;
            gaugeCircularScale1.MaxPin.Name = "MaxPin";
            gaugeCircularScale1.MaxValue = 8D;
            gaugeCircularScale1.MinorTickMarks.Interval = 1D;
            gaugeCircularScale1.MinPin.Name = "MinPin";
            gaugeCircularScale1.MinValue = -8D;
            gaugeCircularScale1.Name = "Scale1";
            gaugePointer1.CapFillColor.BorderColor = System.Drawing.Color.DimGray;
            gaugePointer1.CapFillColor.BorderWidth = 1;
            gaugePointer1.CapFillColor.Color1 = System.Drawing.Color.WhiteSmoke;
            gaugePointer1.CapFillColor.Color2 = System.Drawing.Color.DimGray;
            gaugePointer1.FillColor.BorderColor = System.Drawing.Color.DimGray;
            gaugePointer1.FillColor.BorderWidth = 1;
            gaugePointer1.FillColor.Color1 = System.Drawing.Color.WhiteSmoke;
            gaugePointer1.FillColor.Color2 = System.Drawing.Color.Red;
            gaugePointer1.Length = 0.358F;
            gaugePointer1.Name = "Pointer1";
            gaugePointer1.Style = DevComponents.Instrumentation.PointerStyle.Needle;
            gaugeCircularScale1.Pointers.AddRange(new DevComponents.Instrumentation.GaugePointer[] {
            gaugePointer1});
            gaugeSection1.FillColor.Color1 = System.Drawing.Color.CornflowerBlue;
            gaugeSection1.FillColor.Color2 = System.Drawing.Color.Purple;
            gaugeSection1.Name = "Section1";
            gaugeSection2.EndValue = 2D;
            gaugeSection2.FillColor.Color1 = System.Drawing.Color.Lime;
            gaugeSection2.Name = "Section2";
            gaugeSection2.StartValue = -2D;
            gaugeCircularScale1.Sections.AddRange(new DevComponents.Instrumentation.GaugeSection[] {
            gaugeSection1,
            gaugeSection2});
            gaugeCircularScale1.StartAngle = 160F;
            gaugeCircularScale1.SweepAngle = 220F;
            this.gaugeSpeed.CircularScales.AddRange(new DevComponents.Instrumentation.GaugeCircularScale[] {
            gaugeCircularScale1});
            gradientFillColor1.Color1 = System.Drawing.Color.Gainsboro;
            gradientFillColor1.Color2 = System.Drawing.Color.DarkGray;
            this.gaugeSpeed.Frame.BackColor = gradientFillColor1;
            gradientFillColor2.BorderColor = System.Drawing.Color.Gainsboro;
            gradientFillColor2.BorderWidth = 1;
            gradientFillColor2.Color1 = System.Drawing.Color.White;
            gradientFillColor2.Color2 = System.Drawing.Color.DimGray;
            this.gaugeSpeed.Frame.FrameColor = gradientFillColor2;
            this.gaugeSpeed.Frame.Style = DevComponents.Instrumentation.GaugeFrameStyle.Circular;
            gaugeText1.BackColor.BorderColor = System.Drawing.Color.Black;
            gaugeText1.Location = ((System.Drawing.PointF)(resources.GetObject("gaugeText1.Location")));
            gaugeText1.Name = "Text1";
            gaugeText1.Text = "车速(km/h)";
            numericIndicator1.BackColor.BorderColor = System.Drawing.Color.Gray;
            numericIndicator1.BackColor.BorderWidth = 3;
            numericIndicator1.BackColor.Color1 = System.Drawing.Color.Black;
            numericIndicator1.DecimalColor = System.Drawing.Color.Lime;
            numericIndicator1.DecimalDimColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(50)))), ((int)(((byte)(0)))));
            numericIndicator1.DigitColor = System.Drawing.Color.Red;
            numericIndicator1.DigitDimColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            numericIndicator1.Name = "Indicator1";
            numericIndicator1.NumberOfDigits = 5;
            numericRange1.DecimalColor = System.Drawing.Color.Red;
            numericRange1.DecimalDimColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            numericRange1.DigitColor = System.Drawing.Color.Red;
            numericRange1.DigitDimColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            numericRange1.EndValue = 1000D;
            numericRange1.Name = "Range1";
            numericRange1.StartValue = 500D;
            numericIndicator1.Ranges.AddRange(new DevComponents.Instrumentation.NumericRange[] {
            numericRange1});
            numericIndicator1.ShowDecimalPoint = true;
            numericIndicator1.ShowDimColonPoints = false;
            numericIndicator1.Size = new System.Drawing.SizeF(0.35F, 0.12F);
            numericIndicator1.Style = DevComponents.Instrumentation.NumericIndicatorStyle.Digital7Segment;
            this.gaugeSpeed.GaugeItems.AddRange(new DevComponents.Instrumentation.GaugeItem[] {
            gaugeText1,
            numericIndicator1});
            this.gaugeSpeed.Location = new System.Drawing.Point(4, 127);
            this.gaugeSpeed.Margin = new System.Windows.Forms.Padding(0);
            this.gaugeSpeed.Name = "gaugeSpeed";
            this.gaugeSpeed.Size = new System.Drawing.Size(294, 502);
            this.gaugeSpeed.TabIndex = 1;
            this.gaugeSpeed.Text = "gaugeSpeed";
            // 
            // chart1
            // 
            this.chart1.BackColor = System.Drawing.Color.Transparent;
            chartArea1.BackColor = System.Drawing.Color.Transparent;
            chartArea1.Name = "ChartArea1";
            this.chart1.ChartAreas.Add(chartArea1);
            legend1.BackColor = System.Drawing.Color.Transparent;
            legend1.DockedToChartArea = "ChartArea1";
            legend1.Name = "Legend1";
            this.chart1.Legends.Add(legend1);
            this.chart1.Location = new System.Drawing.Point(306, 127);
            this.chart1.Margin = new System.Windows.Forms.Padding(0);
            this.chart1.Name = "chart1";
            series1.ChartArea = "ChartArea1";
            series1.Legend = "Legend1";
            series1.Name = "Series1";
            this.chart1.Series.Add(series1);
            this.chart1.Size = new System.Drawing.Size(698, 502);
            this.chart1.TabIndex = 2;
            this.chart1.Text = "chart1";
            // 
            // lblHC
            // 
            this.lblHC.AutoSize = true;
            this.lblHC.BackColor = System.Drawing.Color.Black;
            this.lblHC.ForeColor = System.Drawing.Color.Gold;
            this.lblHC.Location = new System.Drawing.Point(387, 637);
            this.lblHC.Margin = new System.Windows.Forms.Padding(0);
            this.lblHC.Name = "lblHC";
            this.lblHC.Size = new System.Drawing.Size(113, 35);
            this.lblHC.TabIndex = 4;
            this.lblHC.Text = "--";
            this.lblHC.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblNO
            // 
            this.lblNO.AutoSize = true;
            this.lblNO.BackColor = System.Drawing.Color.Black;
            this.lblNO.ForeColor = System.Drawing.Color.Gold;
            this.lblNO.Location = new System.Drawing.Point(639, 637);
            this.lblNO.Margin = new System.Windows.Forms.Padding(0);
            this.lblNO.Name = "lblNO";
            this.lblNO.Size = new System.Drawing.Size(113, 35);
            this.lblNO.TabIndex = 5;
            this.lblNO.Text = "--";
            this.lblNO.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblCO
            // 
            this.lblCO.AutoSize = true;
            this.lblCO.BackColor = System.Drawing.Color.Black;
            this.lblCO.ForeColor = System.Drawing.Color.Gold;
            this.lblCO.Location = new System.Drawing.Point(891, 637);
            this.lblCO.Margin = new System.Windows.Forms.Padding(0);
            this.lblCO.Name = "lblCO";
            this.lblCO.Size = new System.Drawing.Size(113, 35);
            this.lblCO.TabIndex = 6;
            this.lblCO.Text = "--";
            this.lblCO.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Black;
            this.label1.ForeColor = System.Drawing.Color.Gold;
            this.label1.Location = new System.Drawing.Point(4, 680);
            this.label1.Margin = new System.Windows.Forms.Padding(0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(244, 43);
            this.label1.TabIndex = 7;
            this.label1.Text = "简易瞬态工况法";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnRestart
            // 
            this.btnRestart.Location = new System.Drawing.Point(256, 680);
            this.btnRestart.Margin = new System.Windows.Forms.Padding(0);
            this.btnRestart.Name = "btnRestart";
            this.btnRestart.Size = new System.Drawing.Size(244, 43);
            this.btnRestart.TabIndex = 8;
            this.btnRestart.Text = "重新测试";
            this.btnRestart.UseVisualStyleBackColor = true;
            this.btnRestart.Click += new System.EventHandler(this.BtnRestart_Click);
            // 
            // btnStop
            // 
            this.btnStop.Location = new System.Drawing.Point(508, 680);
            this.btnStop.Margin = new System.Windows.Forms.Padding(0);
            this.btnStop.Name = "btnStop";
            this.btnStop.Size = new System.Drawing.Size(244, 43);
            this.btnStop.TabIndex = 9;
            this.btnStop.Text = "手动停止";
            this.btnStop.UseVisualStyleBackColor = true;
            this.btnStop.Click += new System.EventHandler(this.BtnStop_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(760, 680);
            this.btnCancel.Margin = new System.Windows.Forms.Padding(0);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(244, 43);
            this.btnCancel.TabIndex = 10;
            this.btnCancel.Text = "取消测试";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.BtnCancel_Click);
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
            this.layoutControlItem2.Control = this.gaugeSpeed;
            this.layoutControlItem2.Height = 70;
            this.layoutControlItem2.HeightType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            this.layoutControlItem2.Name = "layoutControlItem2";
            this.layoutControlItem2.Text = "Label:";
            this.layoutControlItem2.TextVisible = false;
            this.layoutControlItem2.Width = 30;
            this.layoutControlItem2.WidthType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            // 
            // layoutControlItem3
            // 
            this.layoutControlItem3.Control = this.chart1;
            this.layoutControlItem3.Height = 70;
            this.layoutControlItem3.HeightType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            this.layoutControlItem3.Name = "layoutControlItem3";
            this.layoutControlItem3.Text = "Label:";
            this.layoutControlItem3.TextVisible = false;
            this.layoutControlItem3.Width = 70;
            this.layoutControlItem3.WidthType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            // 
            // layoutControlItem4
            // 
            this.layoutControlItem4.Control = this.lblRPM;
            this.layoutControlItem4.Height = 6;
            this.layoutControlItem4.HeightType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            this.layoutControlItem4.Name = "layoutControlItem4";
            this.layoutControlItem4.Text = "转速(km/h):";
            this.layoutControlItem4.TextAlignment = DevComponents.DotNetBar.Layout.eTextAlignment.Center;
            this.layoutControlItem4.TextLineAlignment = DevComponents.DotNetBar.Layout.eTextLineAlignment.Middle;
            this.layoutControlItem4.Width = 25;
            this.layoutControlItem4.WidthType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            // 
            // layoutControlItem5
            // 
            this.layoutControlItem5.Control = this.lblHC;
            this.layoutControlItem5.Height = 6;
            this.layoutControlItem5.HeightType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            this.layoutControlItem5.Name = "layoutControlItem5";
            this.layoutControlItem5.Text = "HC(ppm):";
            this.layoutControlItem5.TextAlignment = DevComponents.DotNetBar.Layout.eTextAlignment.Center;
            this.layoutControlItem5.TextLineAlignment = DevComponents.DotNetBar.Layout.eTextLineAlignment.Middle;
            this.layoutControlItem5.Width = 25;
            this.layoutControlItem5.WidthType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            // 
            // layoutControlItem6
            // 
            this.layoutControlItem6.Control = this.lblNO;
            this.layoutControlItem6.Height = 6;
            this.layoutControlItem6.HeightType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            this.layoutControlItem6.Name = "layoutControlItem6";
            this.layoutControlItem6.Text = "NO(ppm):";
            this.layoutControlItem6.TextAlignment = DevComponents.DotNetBar.Layout.eTextAlignment.Center;
            this.layoutControlItem6.TextLineAlignment = DevComponents.DotNetBar.Layout.eTextLineAlignment.Middle;
            this.layoutControlItem6.Width = 25;
            this.layoutControlItem6.WidthType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            // 
            // layoutControlItem7
            // 
            this.layoutControlItem7.Control = this.lblCO;
            this.layoutControlItem7.Height = 6;
            this.layoutControlItem7.HeightType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            this.layoutControlItem7.Name = "layoutControlItem7";
            this.layoutControlItem7.Text = "CO(%):";
            this.layoutControlItem7.TextAlignment = DevComponents.DotNetBar.Layout.eTextAlignment.Center;
            this.layoutControlItem7.TextLineAlignment = DevComponents.DotNetBar.Layout.eTextLineAlignment.Middle;
            this.layoutControlItem7.Width = 25;
            this.layoutControlItem7.WidthType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            // 
            // layoutControlItem8
            // 
            this.layoutControlItem8.Control = this.label1;
            this.layoutControlItem8.Height = 7;
            this.layoutControlItem8.HeightType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            this.layoutControlItem8.Name = "layoutControlItem8";
            this.layoutControlItem8.Text = "Label:";
            this.layoutControlItem8.TextVisible = false;
            this.layoutControlItem8.Width = 25;
            this.layoutControlItem8.WidthType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            // 
            // layoutControlItem9
            // 
            this.layoutControlItem9.Control = this.btnRestart;
            this.layoutControlItem9.Height = 7;
            this.layoutControlItem9.HeightType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            this.layoutControlItem9.Name = "layoutControlItem9";
            this.layoutControlItem9.Width = 25;
            this.layoutControlItem9.WidthType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            // 
            // layoutControlItem10
            // 
            this.layoutControlItem10.Control = this.btnStop;
            this.layoutControlItem10.Height = 7;
            this.layoutControlItem10.HeightType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            this.layoutControlItem10.Name = "layoutControlItem10";
            this.layoutControlItem10.Width = 25;
            this.layoutControlItem10.WidthType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            // 
            // layoutControlItem11
            // 
            this.layoutControlItem11.Control = this.btnCancel;
            this.layoutControlItem11.Height = 7;
            this.layoutControlItem11.HeightType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            this.layoutControlItem11.Name = "layoutControlItem11";
            this.layoutControlItem11.Width = 25;
            this.layoutControlItem11.WidthType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            // 
            // VMASForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1008, 729);
            this.Controls.Add(this.layoutMain);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "VMASForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "简易瞬态工况";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.VMASForm_FormClosing);
            this.Load += new System.EventHandler(this.VMASForm_Load);
            this.Shown += new System.EventHandler(this.VMASForm_Shown);
            this.Resize += new System.EventHandler(this.VMASForm_Resize);
            this.layoutMain.ResumeLayout(false);
            this.layoutMain.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gaugeSpeed)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chart1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevComponents.DotNetBar.Layout.LayoutControl layoutMain;
        private System.Windows.Forms.Label lblMsg;
        private DevComponents.DotNetBar.Layout.LayoutControlItem layoutControlItem1;
        private DevComponents.Instrumentation.GaugeControl gaugeSpeed;
        private DevComponents.DotNetBar.Layout.LayoutControlItem layoutControlItem2;
        private System.Windows.Forms.DataVisualization.Charting.Chart chart1;
        private DevComponents.DotNetBar.Layout.LayoutControlItem layoutControlItem3;
        private System.Windows.Forms.Label lblRPM;
        private DevComponents.DotNetBar.Layout.LayoutControlItem layoutControlItem4;
        private System.Windows.Forms.Label lblHC;
        private DevComponents.DotNetBar.Layout.LayoutControlItem layoutControlItem5;
        private System.Windows.Forms.Label lblNO;
        private System.Windows.Forms.Label lblCO;
        private System.Windows.Forms.Label label1;
        private DevComponents.DotNetBar.Layout.LayoutControlItem layoutControlItem6;
        private DevComponents.DotNetBar.Layout.LayoutControlItem layoutControlItem7;
        private DevComponents.DotNetBar.Layout.LayoutControlItem layoutControlItem8;
        private System.Windows.Forms.Button btnRestart;
        private DevComponents.DotNetBar.Layout.LayoutControlItem layoutControlItem9;
        private System.Windows.Forms.Button btnStop;
        private DevComponents.DotNetBar.Layout.LayoutControlItem layoutControlItem10;
        private System.Windows.Forms.Button btnCancel;
        private DevComponents.DotNetBar.Layout.LayoutControlItem layoutControlItem11;
    }
}