namespace Dyno_Geely {
    partial class PreheatingForm {
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PreheatingForm));
            this.btn5Weather = new System.Windows.Forms.Button();
            this.btn4Smoker = new System.Windows.Forms.Button();
            this.btn3Flowmeter = new System.Windows.Forms.Button();
            this.btn2GasBox = new System.Windows.Forms.Button();
            this.btn1Dyno = new System.Windows.Forms.Button();
            this.pnlForm = new System.Windows.Forms.Panel();
            this.btnCancel = new System.Windows.Forms.Button();
            this.layoutMain = new DevComponents.DotNetBar.Layout.LayoutControl();
            this.btn6Tacho = new System.Windows.Forms.Button();
            this.btn7Oil = new System.Windows.Forms.Button();
            this.layoutControlItem1 = new DevComponents.DotNetBar.Layout.LayoutControlItem();
            this.layoutControlItem2 = new DevComponents.DotNetBar.Layout.LayoutControlItem();
            this.layoutControlItem3 = new DevComponents.DotNetBar.Layout.LayoutControlItem();
            this.layoutControlItem4 = new DevComponents.DotNetBar.Layout.LayoutControlItem();
            this.layoutControlItem5 = new DevComponents.DotNetBar.Layout.LayoutControlItem();
            this.layoutControlItem8 = new DevComponents.DotNetBar.Layout.LayoutControlItem();
            this.layoutControlItem9 = new DevComponents.DotNetBar.Layout.LayoutControlItem();
            this.layoutControlItem6 = new DevComponents.DotNetBar.Layout.LayoutControlItem();
            this.layoutControlItem7 = new DevComponents.DotNetBar.Layout.LayoutControlItem();
            this.layoutMain.SuspendLayout();
            this.SuspendLayout();
            // 
            // btn5Weather
            // 
            this.btn5Weather.Location = new System.Drawing.Point(440, 4);
            this.btn5Weather.Margin = new System.Windows.Forms.Padding(0);
            this.btn5Weather.Name = "btn5Weather";
            this.btn5Weather.Size = new System.Drawing.Size(101, 48);
            this.btn5Weather.TabIndex = 4;
            this.btn5Weather.Text = "5.气象站";
            this.btn5Weather.UseVisualStyleBackColor = true;
            this.btn5Weather.Click += new System.EventHandler(this.Button_Click);
            // 
            // btn4Smoker
            // 
            this.btn4Smoker.Location = new System.Drawing.Point(331, 4);
            this.btn4Smoker.Margin = new System.Windows.Forms.Padding(0);
            this.btn4Smoker.Name = "btn4Smoker";
            this.btn4Smoker.Size = new System.Drawing.Size(101, 48);
            this.btn4Smoker.TabIndex = 3;
            this.btn4Smoker.Text = "4.烟度计";
            this.btn4Smoker.UseVisualStyleBackColor = true;
            this.btn4Smoker.Click += new System.EventHandler(this.Button_Click);
            // 
            // btn3Flowmeter
            // 
            this.btn3Flowmeter.Location = new System.Drawing.Point(222, 4);
            this.btn3Flowmeter.Margin = new System.Windows.Forms.Padding(0);
            this.btn3Flowmeter.Name = "btn3Flowmeter";
            this.btn3Flowmeter.Size = new System.Drawing.Size(101, 48);
            this.btn3Flowmeter.TabIndex = 2;
            this.btn3Flowmeter.Text = "3.流量计";
            this.btn3Flowmeter.UseVisualStyleBackColor = true;
            this.btn3Flowmeter.Click += new System.EventHandler(this.Button_Click);
            // 
            // btn2GasBox
            // 
            this.btn2GasBox.Location = new System.Drawing.Point(113, 4);
            this.btn2GasBox.Margin = new System.Windows.Forms.Padding(0);
            this.btn2GasBox.Name = "btn2GasBox";
            this.btn2GasBox.Size = new System.Drawing.Size(101, 48);
            this.btn2GasBox.TabIndex = 1;
            this.btn2GasBox.Text = "2.分析仪";
            this.btn2GasBox.UseVisualStyleBackColor = true;
            this.btn2GasBox.Click += new System.EventHandler(this.Button_Click);
            // 
            // btn1Dyno
            // 
            this.btn1Dyno.Location = new System.Drawing.Point(4, 4);
            this.btn1Dyno.Margin = new System.Windows.Forms.Padding(0);
            this.btn1Dyno.Name = "btn1Dyno";
            this.btn1Dyno.Size = new System.Drawing.Size(101, 48);
            this.btn1Dyno.TabIndex = 0;
            this.btn1Dyno.Text = "1.测功机";
            this.btn1Dyno.UseVisualStyleBackColor = true;
            this.btn1Dyno.Click += new System.EventHandler(this.Button_Click);
            // 
            // pnlForm
            // 
            this.pnlForm.Location = new System.Drawing.Point(4, 60);
            this.pnlForm.Margin = new System.Windows.Forms.Padding(0);
            this.pnlForm.Name = "pnlForm";
            this.pnlForm.Size = new System.Drawing.Size(776, 440);
            this.pnlForm.TabIndex = 7;
            this.pnlForm.Resize += new System.EventHandler(this.PnlForm_Resize);
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(4, 508);
            this.btnCancel.Margin = new System.Windows.Forms.Padding(0);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(776, 48);
            this.btnCancel.TabIndex = 8;
            this.btnCancel.Text = "退出预热";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // layoutMain
            // 
            this.layoutMain.BackColor = System.Drawing.Color.Transparent;
            this.layoutMain.Controls.Add(this.btn1Dyno);
            this.layoutMain.Controls.Add(this.btn2GasBox);
            this.layoutMain.Controls.Add(this.btn3Flowmeter);
            this.layoutMain.Controls.Add(this.btn4Smoker);
            this.layoutMain.Controls.Add(this.btn5Weather);
            this.layoutMain.Controls.Add(this.pnlForm);
            this.layoutMain.Controls.Add(this.btnCancel);
            this.layoutMain.Controls.Add(this.btn6Tacho);
            this.layoutMain.Controls.Add(this.btn7Oil);
            this.layoutMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.layoutMain.Font = new System.Drawing.Font("宋体", 14F);
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
            this.layoutControlItem8,
            this.layoutControlItem9,
            this.layoutControlItem6,
            this.layoutControlItem7});
            this.layoutMain.Size = new System.Drawing.Size(784, 561);
            this.layoutMain.TabIndex = 1;
            // 
            // btn6Tacho
            // 
            this.btn6Tacho.Location = new System.Drawing.Point(549, 4);
            this.btn6Tacho.Margin = new System.Windows.Forms.Padding(0);
            this.btn6Tacho.Name = "btn6Tacho";
            this.btn6Tacho.Size = new System.Drawing.Size(109, 48);
            this.btn6Tacho.TabIndex = 5;
            this.btn6Tacho.Text = "6.转速计";
            this.btn6Tacho.UseVisualStyleBackColor = true;
            this.btn6Tacho.Click += new System.EventHandler(this.Button_Click);
            // 
            // btn7Oil
            // 
            this.btn7Oil.Location = new System.Drawing.Point(666, 4);
            this.btn7Oil.Margin = new System.Windows.Forms.Padding(0);
            this.btn7Oil.Name = "btn7Oil";
            this.btn7Oil.Size = new System.Drawing.Size(114, 48);
            this.btn7Oil.TabIndex = 6;
            this.btn7Oil.Text = "7.油温计";
            this.btn7Oil.UseVisualStyleBackColor = true;
            this.btn7Oil.Click += new System.EventHandler(this.Button_Click);
            // 
            // layoutControlItem1
            // 
            this.layoutControlItem1.Control = this.btn1Dyno;
            this.layoutControlItem1.Height = 10;
            this.layoutControlItem1.HeightType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            this.layoutControlItem1.MinSize = new System.Drawing.Size(32, 20);
            this.layoutControlItem1.Name = "layoutControlItem1";
            this.layoutControlItem1.Width = 14;
            this.layoutControlItem1.WidthType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            // 
            // layoutControlItem2
            // 
            this.layoutControlItem2.Control = this.btn2GasBox;
            this.layoutControlItem2.Height = 10;
            this.layoutControlItem2.HeightType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            this.layoutControlItem2.MinSize = new System.Drawing.Size(32, 20);
            this.layoutControlItem2.Name = "layoutControlItem2";
            this.layoutControlItem2.Width = 14;
            this.layoutControlItem2.WidthType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            // 
            // layoutControlItem3
            // 
            this.layoutControlItem3.Control = this.btn3Flowmeter;
            this.layoutControlItem3.Height = 10;
            this.layoutControlItem3.HeightType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            this.layoutControlItem3.MinSize = new System.Drawing.Size(32, 20);
            this.layoutControlItem3.Name = "layoutControlItem3";
            this.layoutControlItem3.Width = 14;
            this.layoutControlItem3.WidthType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            // 
            // layoutControlItem4
            // 
            this.layoutControlItem4.Control = this.btn4Smoker;
            this.layoutControlItem4.Height = 10;
            this.layoutControlItem4.HeightType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            this.layoutControlItem4.MinSize = new System.Drawing.Size(32, 20);
            this.layoutControlItem4.Name = "layoutControlItem4";
            this.layoutControlItem4.Width = 14;
            this.layoutControlItem4.WidthType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            // 
            // layoutControlItem5
            // 
            this.layoutControlItem5.Control = this.btn5Weather;
            this.layoutControlItem5.Height = 10;
            this.layoutControlItem5.HeightType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            this.layoutControlItem5.MinSize = new System.Drawing.Size(32, 20);
            this.layoutControlItem5.Name = "layoutControlItem5";
            this.layoutControlItem5.Width = 14;
            this.layoutControlItem5.WidthType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            // 
            // layoutControlItem8
            // 
            this.layoutControlItem8.Control = this.btn6Tacho;
            this.layoutControlItem8.Height = 10;
            this.layoutControlItem8.HeightType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            this.layoutControlItem8.MinSize = new System.Drawing.Size(32, 20);
            this.layoutControlItem8.Name = "layoutControlItem8";
            this.layoutControlItem8.Width = 15;
            this.layoutControlItem8.WidthType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            // 
            // layoutControlItem9
            // 
            this.layoutControlItem9.Control = this.btn7Oil;
            this.layoutControlItem9.Height = 10;
            this.layoutControlItem9.HeightType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            this.layoutControlItem9.MinSize = new System.Drawing.Size(32, 20);
            this.layoutControlItem9.Name = "layoutControlItem9";
            this.layoutControlItem9.Width = 15;
            this.layoutControlItem9.WidthType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            // 
            // layoutControlItem6
            // 
            this.layoutControlItem6.Control = this.pnlForm;
            this.layoutControlItem6.Height = 80;
            this.layoutControlItem6.HeightType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            this.layoutControlItem6.MinSize = new System.Drawing.Size(64, 18);
            this.layoutControlItem6.Name = "layoutControlItem6";
            this.layoutControlItem6.Text = "Label:";
            this.layoutControlItem6.TextVisible = false;
            this.layoutControlItem6.Width = 100;
            this.layoutControlItem6.WidthType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            // 
            // layoutControlItem7
            // 
            this.layoutControlItem7.Control = this.btnCancel;
            this.layoutControlItem7.Height = 10;
            this.layoutControlItem7.HeightType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            this.layoutControlItem7.MinSize = new System.Drawing.Size(32, 20);
            this.layoutControlItem7.Name = "layoutControlItem7";
            this.layoutControlItem7.Width = 100;
            this.layoutControlItem7.WidthType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            // 
            // PreheatingForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(784, 561);
            this.Controls.Add(this.layoutMain);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "PreheatingForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "设备预热";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.PreheatingForm_FormClosing);
            this.Load += new System.EventHandler(this.PreheatingForm_Load);
            this.Resize += new System.EventHandler(this.PreheatingForm_Resize);
            this.layoutMain.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Button btn1Dyno;
        private System.Windows.Forms.Button btn5Weather;
        private System.Windows.Forms.Button btn4Smoker;
        private System.Windows.Forms.Button btn3Flowmeter;
        private System.Windows.Forms.Button btn2GasBox;
        private System.Windows.Forms.Panel pnlForm;
        private System.Windows.Forms.Button btnCancel;
        private DevComponents.DotNetBar.Layout.LayoutControl layoutMain;
        private DevComponents.DotNetBar.Layout.LayoutControlItem layoutControlItem1;
        private DevComponents.DotNetBar.Layout.LayoutControlItem layoutControlItem2;
        private DevComponents.DotNetBar.Layout.LayoutControlItem layoutControlItem3;
        private DevComponents.DotNetBar.Layout.LayoutControlItem layoutControlItem4;
        private DevComponents.DotNetBar.Layout.LayoutControlItem layoutControlItem5;
        private DevComponents.DotNetBar.Layout.LayoutControlItem layoutControlItem6;
        private DevComponents.DotNetBar.Layout.LayoutControlItem layoutControlItem7;
        private System.Windows.Forms.Button btn6Tacho;
        private System.Windows.Forms.Button btn7Oil;
        private DevComponents.DotNetBar.Layout.LayoutControlItem layoutControlItem8;
        private DevComponents.DotNetBar.Layout.LayoutControlItem layoutControlItem9;
    }
}