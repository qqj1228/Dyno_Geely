namespace Dyno_Geely {
    partial class SelfcheckForm {
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SelfcheckForm));
            this.layoutMain = new DevComponents.DotNetBar.Layout.LayoutControl();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnSkip = new System.Windows.Forms.Button();
            this.pnlForm = new System.Windows.Forms.Panel();
            this.btn6Weather = new System.Windows.Forms.Button();
            this.btn5Tachometer = new System.Windows.Forms.Button();
            this.btn4OilTemp = new System.Windows.Forms.Button();
            this.btn3Smoker = new System.Windows.Forms.Button();
            this.btn2Flowmeter = new System.Windows.Forms.Button();
            this.btn1GasBox = new System.Windows.Forms.Button();
            this.layoutControlItem1 = new DevComponents.DotNetBar.Layout.LayoutControlItem();
            this.layoutControlItem2 = new DevComponents.DotNetBar.Layout.LayoutControlItem();
            this.layoutControlItem3 = new DevComponents.DotNetBar.Layout.LayoutControlItem();
            this.layoutControlItem4 = new DevComponents.DotNetBar.Layout.LayoutControlItem();
            this.layoutControlItem5 = new DevComponents.DotNetBar.Layout.LayoutControlItem();
            this.layoutControlItem6 = new DevComponents.DotNetBar.Layout.LayoutControlItem();
            this.layoutControlItem7 = new DevComponents.DotNetBar.Layout.LayoutControlItem();
            this.layoutControlItem8 = new DevComponents.DotNetBar.Layout.LayoutControlItem();
            this.layoutControlItem9 = new DevComponents.DotNetBar.Layout.LayoutControlItem();
            this.layoutMain.SuspendLayout();
            this.SuspendLayout();
            // 
            // layoutMain
            // 
            this.layoutMain.BackColor = System.Drawing.Color.Transparent;
            this.layoutMain.Controls.Add(this.btnCancel);
            this.layoutMain.Controls.Add(this.btnSkip);
            this.layoutMain.Controls.Add(this.pnlForm);
            this.layoutMain.Controls.Add(this.btn6Weather);
            this.layoutMain.Controls.Add(this.btn5Tachometer);
            this.layoutMain.Controls.Add(this.btn4OilTemp);
            this.layoutMain.Controls.Add(this.btn3Smoker);
            this.layoutMain.Controls.Add(this.btn2Flowmeter);
            this.layoutMain.Controls.Add(this.btn1GasBox);
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
            this.layoutControlItem9});
            this.layoutMain.Size = new System.Drawing.Size(784, 561);
            this.layoutMain.TabIndex = 0;
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(396, 508);
            this.btnCancel.Margin = new System.Windows.Forms.Padding(0);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(384, 48);
            this.btnCancel.TabIndex = 8;
            this.btnCancel.Text = "取消自检";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.BtnCancel_Click);
            // 
            // btnSkip
            // 
            this.btnSkip.Location = new System.Drawing.Point(4, 508);
            this.btnSkip.Margin = new System.Windows.Forms.Padding(0);
            this.btnSkip.Name = "btnSkip";
            this.btnSkip.Size = new System.Drawing.Size(384, 48);
            this.btnSkip.TabIndex = 7;
            this.btnSkip.Text = "跳过当前步骤";
            this.btnSkip.UseVisualStyleBackColor = true;
            this.btnSkip.Click += new System.EventHandler(this.BtnSkip_Click);
            // 
            // pnlForm
            // 
            this.pnlForm.Location = new System.Drawing.Point(4, 60);
            this.pnlForm.Margin = new System.Windows.Forms.Padding(0);
            this.pnlForm.Name = "pnlForm";
            this.pnlForm.Size = new System.Drawing.Size(776, 440);
            this.pnlForm.TabIndex = 6;
            this.pnlForm.Resize += new System.EventHandler(this.PnlForm_Resize);
            // 
            // btn6Weather
            // 
            this.btn6Weather.Location = new System.Drawing.Point(661, 4);
            this.btn6Weather.Margin = new System.Windows.Forms.Padding(0);
            this.btn6Weather.Name = "btn6Weather";
            this.btn6Weather.Size = new System.Drawing.Size(119, 48);
            this.btn6Weather.TabIndex = 5;
            this.btn6Weather.Text = "6.气象站";
            this.btn6Weather.UseVisualStyleBackColor = true;
            this.btn6Weather.Click += new System.EventHandler(this.Button_Click);
            // 
            // btn5Tachometer
            // 
            this.btn5Tachometer.Location = new System.Drawing.Point(536, 4);
            this.btn5Tachometer.Margin = new System.Windows.Forms.Padding(0);
            this.btn5Tachometer.Name = "btn5Tachometer";
            this.btn5Tachometer.Size = new System.Drawing.Size(117, 48);
            this.btn5Tachometer.TabIndex = 4;
            this.btn5Tachometer.Text = "5.转速计";
            this.btn5Tachometer.UseVisualStyleBackColor = true;
            this.btn5Tachometer.Click += new System.EventHandler(this.Button_Click);
            // 
            // btn4OilTemp
            // 
            this.btn4OilTemp.Location = new System.Drawing.Point(403, 4);
            this.btn4OilTemp.Margin = new System.Windows.Forms.Padding(0);
            this.btn4OilTemp.Name = "btn4OilTemp";
            this.btn4OilTemp.Size = new System.Drawing.Size(125, 48);
            this.btn4OilTemp.TabIndex = 3;
            this.btn4OilTemp.Text = "4.油温计";
            this.btn4OilTemp.UseVisualStyleBackColor = true;
            this.btn4OilTemp.Click += new System.EventHandler(this.Button_Click);
            // 
            // btn3Smoker
            // 
            this.btn3Smoker.Location = new System.Drawing.Point(270, 4);
            this.btn3Smoker.Margin = new System.Windows.Forms.Padding(0);
            this.btn3Smoker.Name = "btn3Smoker";
            this.btn3Smoker.Size = new System.Drawing.Size(125, 48);
            this.btn3Smoker.TabIndex = 2;
            this.btn3Smoker.Text = "3.烟度计";
            this.btn3Smoker.UseVisualStyleBackColor = true;
            this.btn3Smoker.Click += new System.EventHandler(this.Button_Click);
            // 
            // btn2Flowmeter
            // 
            this.btn2Flowmeter.Location = new System.Drawing.Point(137, 4);
            this.btn2Flowmeter.Margin = new System.Windows.Forms.Padding(0);
            this.btn2Flowmeter.Name = "btn2Flowmeter";
            this.btn2Flowmeter.Size = new System.Drawing.Size(125, 48);
            this.btn2Flowmeter.TabIndex = 1;
            this.btn2Flowmeter.Text = "2.流量计";
            this.btn2Flowmeter.UseVisualStyleBackColor = true;
            this.btn2Flowmeter.Click += new System.EventHandler(this.Button_Click);
            // 
            // btn1GasBox
            // 
            this.btn1GasBox.Location = new System.Drawing.Point(4, 4);
            this.btn1GasBox.Margin = new System.Windows.Forms.Padding(0);
            this.btn1GasBox.Name = "btn1GasBox";
            this.btn1GasBox.Size = new System.Drawing.Size(125, 48);
            this.btn1GasBox.TabIndex = 0;
            this.btn1GasBox.Text = "1.分析仪";
            this.btn1GasBox.UseVisualStyleBackColor = true;
            this.btn1GasBox.Click += new System.EventHandler(this.Button_Click);
            // 
            // layoutControlItem1
            // 
            this.layoutControlItem1.Control = this.btn1GasBox;
            this.layoutControlItem1.Height = 10;
            this.layoutControlItem1.HeightType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            this.layoutControlItem1.MinSize = new System.Drawing.Size(32, 20);
            this.layoutControlItem1.Name = "layoutControlItem1";
            this.layoutControlItem1.Width = 17;
            this.layoutControlItem1.WidthType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            // 
            // layoutControlItem2
            // 
            this.layoutControlItem2.Control = this.btn2Flowmeter;
            this.layoutControlItem2.Height = 10;
            this.layoutControlItem2.HeightType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            this.layoutControlItem2.MinSize = new System.Drawing.Size(32, 20);
            this.layoutControlItem2.Name = "layoutControlItem2";
            this.layoutControlItem2.Width = 17;
            this.layoutControlItem2.WidthType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            // 
            // layoutControlItem3
            // 
            this.layoutControlItem3.Control = this.btn3Smoker;
            this.layoutControlItem3.Height = 10;
            this.layoutControlItem3.HeightType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            this.layoutControlItem3.MinSize = new System.Drawing.Size(32, 20);
            this.layoutControlItem3.Name = "layoutControlItem3";
            this.layoutControlItem3.Width = 17;
            this.layoutControlItem3.WidthType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            // 
            // layoutControlItem4
            // 
            this.layoutControlItem4.Control = this.btn4OilTemp;
            this.layoutControlItem4.Height = 10;
            this.layoutControlItem4.HeightType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            this.layoutControlItem4.MinSize = new System.Drawing.Size(32, 20);
            this.layoutControlItem4.Name = "layoutControlItem4";
            this.layoutControlItem4.Width = 17;
            this.layoutControlItem4.WidthType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            // 
            // layoutControlItem5
            // 
            this.layoutControlItem5.Control = this.btn5Tachometer;
            this.layoutControlItem5.Height = 10;
            this.layoutControlItem5.HeightType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            this.layoutControlItem5.MinSize = new System.Drawing.Size(32, 20);
            this.layoutControlItem5.Name = "layoutControlItem5";
            this.layoutControlItem5.Width = 16;
            this.layoutControlItem5.WidthType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            // 
            // layoutControlItem6
            // 
            this.layoutControlItem6.Control = this.btn6Weather;
            this.layoutControlItem6.Height = 10;
            this.layoutControlItem6.HeightType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            this.layoutControlItem6.MinSize = new System.Drawing.Size(32, 20);
            this.layoutControlItem6.Name = "layoutControlItem6";
            this.layoutControlItem6.Width = 16;
            this.layoutControlItem6.WidthType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            // 
            // layoutControlItem7
            // 
            this.layoutControlItem7.Control = this.pnlForm;
            this.layoutControlItem7.Height = 80;
            this.layoutControlItem7.HeightType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            this.layoutControlItem7.MinSize = new System.Drawing.Size(64, 18);
            this.layoutControlItem7.Name = "layoutControlItem7";
            this.layoutControlItem7.Text = "Label:";
            this.layoutControlItem7.TextVisible = false;
            this.layoutControlItem7.Width = 100;
            this.layoutControlItem7.WidthType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            // 
            // layoutControlItem8
            // 
            this.layoutControlItem8.Control = this.btnSkip;
            this.layoutControlItem8.Height = 10;
            this.layoutControlItem8.HeightType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            this.layoutControlItem8.MinSize = new System.Drawing.Size(32, 20);
            this.layoutControlItem8.Name = "layoutControlItem8";
            this.layoutControlItem8.Width = 50;
            this.layoutControlItem8.WidthType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            // 
            // layoutControlItem9
            // 
            this.layoutControlItem9.Control = this.btnCancel;
            this.layoutControlItem9.Height = 10;
            this.layoutControlItem9.HeightType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            this.layoutControlItem9.MinSize = new System.Drawing.Size(32, 20);
            this.layoutControlItem9.Name = "layoutControlItem9";
            this.layoutControlItem9.Width = 50;
            this.layoutControlItem9.WidthType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            // 
            // SelfcheckForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(784, 561);
            this.Controls.Add(this.layoutMain);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "SelfcheckForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "仪器自检";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.SelfcheckForm_FormClosing);
            this.Load += new System.EventHandler(this.SelfcheckForm_Load);
            this.Resize += new System.EventHandler(this.SelfcheckForm_Resize);
            this.layoutMain.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private DevComponents.DotNetBar.Layout.LayoutControl layoutMain;
        private System.Windows.Forms.Button btn2Flowmeter;
        private System.Windows.Forms.Button btn1GasBox;
        private DevComponents.DotNetBar.Layout.LayoutControlItem layoutControlItem1;
        private DevComponents.DotNetBar.Layout.LayoutControlItem layoutControlItem2;
        private System.Windows.Forms.Button btn6Weather;
        private System.Windows.Forms.Button btn5Tachometer;
        private System.Windows.Forms.Button btn4OilTemp;
        private System.Windows.Forms.Button btn3Smoker;
        private DevComponents.DotNetBar.Layout.LayoutControlItem layoutControlItem3;
        private DevComponents.DotNetBar.Layout.LayoutControlItem layoutControlItem4;
        private DevComponents.DotNetBar.Layout.LayoutControlItem layoutControlItem5;
        private DevComponents.DotNetBar.Layout.LayoutControlItem layoutControlItem6;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnSkip;
        private System.Windows.Forms.Panel pnlForm;
        private DevComponents.DotNetBar.Layout.LayoutControlItem layoutControlItem7;
        private DevComponents.DotNetBar.Layout.LayoutControlItem layoutControlItem8;
        private DevComponents.DotNetBar.Layout.LayoutControlItem layoutControlItem9;
    }
}