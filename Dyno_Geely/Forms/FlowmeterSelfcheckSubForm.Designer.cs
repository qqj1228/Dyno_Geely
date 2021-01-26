namespace Dyno_Geely {
    partial class FlowmeterSelfcheckSubForm {
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
            this.lblFlow = new System.Windows.Forms.Label();
            this.lblStep = new System.Windows.Forms.Label();
            this.lblMsg = new System.Windows.Forms.Label();
            this.lblO2 = new System.Windows.Forms.Label();
            this.lblRestTime = new System.Windows.Forms.Label();
            this.lblZero = new System.Windows.Forms.Label();
            this.lblFlowCheck = new System.Windows.Forms.Label();
            this.lblO2SpanCheck = new System.Windows.Forms.Label();
            this.lblResult = new System.Windows.Forms.Label();
            this.layoutControlItem1 = new DevComponents.DotNetBar.Layout.LayoutControlItem();
            this.layoutGroup1 = new DevComponents.DotNetBar.Layout.LayoutGroup();
            this.layoutControlItem2 = new DevComponents.DotNetBar.Layout.LayoutControlItem();
            this.layoutControlItem3 = new DevComponents.DotNetBar.Layout.LayoutControlItem();
            this.layoutControlItem4 = new DevComponents.DotNetBar.Layout.LayoutControlItem();
            this.layoutControlItem5 = new DevComponents.DotNetBar.Layout.LayoutControlItem();
            this.layoutGroup2 = new DevComponents.DotNetBar.Layout.LayoutGroup();
            this.layoutControlItem6 = new DevComponents.DotNetBar.Layout.LayoutControlItem();
            this.layoutControlItem7 = new DevComponents.DotNetBar.Layout.LayoutControlItem();
            this.layoutControlItem8 = new DevComponents.DotNetBar.Layout.LayoutControlItem();
            this.layoutControlItem9 = new DevComponents.DotNetBar.Layout.LayoutControlItem();
            this.layoutControlItem10 = new DevComponents.DotNetBar.Layout.LayoutControlItem();
            this.layoutControlItem11 = new DevComponents.DotNetBar.Layout.LayoutControlItem();
            this.layoutMain.SuspendLayout();
            this.SuspendLayout();
            // 
            // layoutMain
            // 
            this.layoutMain.BackColor = System.Drawing.Color.Transparent;
            this.layoutMain.Controls.Add(this.btnStop);
            this.layoutMain.Controls.Add(this.btnStart);
            this.layoutMain.Controls.Add(this.lblFlow);
            this.layoutMain.Controls.Add(this.lblStep);
            this.layoutMain.Controls.Add(this.lblMsg);
            this.layoutMain.Controls.Add(this.lblO2);
            this.layoutMain.Controls.Add(this.lblRestTime);
            this.layoutMain.Controls.Add(this.lblZero);
            this.layoutMain.Controls.Add(this.lblFlowCheck);
            this.layoutMain.Controls.Add(this.lblO2SpanCheck);
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
            this.layoutControlItem10,
            this.layoutControlItem11});
            this.layoutMain.Size = new System.Drawing.Size(784, 411);
            this.layoutMain.TabIndex = 0;
            // 
            // btnStop
            // 
            this.btnStop.Location = new System.Drawing.Point(396, 360);
            this.btnStop.Margin = new System.Windows.Forms.Padding(0);
            this.btnStop.Name = "btnStop";
            this.btnStop.Size = new System.Drawing.Size(384, 45);
            this.btnStop.TabIndex = 12;
            this.btnStop.Text = "手动停止";
            this.btnStop.UseVisualStyleBackColor = true;
            this.btnStop.Click += new System.EventHandler(this.BtnStop_Click);
            // 
            // btnStart
            // 
            this.btnStart.Location = new System.Drawing.Point(4, 360);
            this.btnStart.Margin = new System.Windows.Forms.Padding(0);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(384, 45);
            this.btnStart.TabIndex = 11;
            this.btnStart.Text = "开始自检";
            this.btnStart.UseVisualStyleBackColor = true;
            this.btnStart.Click += new System.EventHandler(this.BtnStart_Click);
            // 
            // lblFlow
            // 
            this.lblFlow.AutoSize = true;
            this.lblFlow.BackColor = System.Drawing.Color.Black;
            this.lblFlow.ForeColor = System.Drawing.Color.Gold;
            this.lblFlow.Location = new System.Drawing.Point(161, 146);
            this.lblFlow.Margin = new System.Windows.Forms.Padding(0);
            this.lblFlow.Name = "lblFlow";
            this.lblFlow.Size = new System.Drawing.Size(223, 61);
            this.lblFlow.TabIndex = 2;
            this.lblFlow.Text = "--";
            this.lblFlow.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblStep
            // 
            this.lblStep.AutoSize = true;
            this.lblStep.BackColor = System.Drawing.Color.Black;
            this.lblStep.ForeColor = System.Drawing.Color.Gold;
            this.lblStep.Location = new System.Drawing.Point(161, 77);
            this.lblStep.Margin = new System.Windows.Forms.Padding(0);
            this.lblStep.Name = "lblStep";
            this.lblStep.Size = new System.Drawing.Size(223, 61);
            this.lblStep.TabIndex = 1;
            this.lblStep.Text = "--";
            this.lblStep.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
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
            // lblO2
            // 
            this.lblO2.AutoSize = true;
            this.lblO2.BackColor = System.Drawing.Color.Black;
            this.lblO2.ForeColor = System.Drawing.Color.Gold;
            this.lblO2.Location = new System.Drawing.Point(161, 215);
            this.lblO2.Margin = new System.Windows.Forms.Padding(0);
            this.lblO2.Name = "lblO2";
            this.lblO2.Size = new System.Drawing.Size(223, 61);
            this.lblO2.TabIndex = 3;
            this.lblO2.Text = "--";
            this.lblO2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblRestTime
            // 
            this.lblRestTime.AutoSize = true;
            this.lblRestTime.BackColor = System.Drawing.Color.Black;
            this.lblRestTime.ForeColor = System.Drawing.Color.Gold;
            this.lblRestTime.Location = new System.Drawing.Point(161, 284);
            this.lblRestTime.Margin = new System.Windows.Forms.Padding(0);
            this.lblRestTime.Name = "lblRestTime";
            this.lblRestTime.Size = new System.Drawing.Size(223, 61);
            this.lblRestTime.TabIndex = 4;
            this.lblRestTime.Text = "--";
            this.lblRestTime.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblZero
            // 
            this.lblZero.AutoSize = true;
            this.lblZero.BackColor = System.Drawing.Color.Black;
            this.lblZero.ForeColor = System.Drawing.Color.Gold;
            this.lblZero.Location = new System.Drawing.Point(575, 77);
            this.lblZero.Margin = new System.Windows.Forms.Padding(0);
            this.lblZero.Name = "lblZero";
            this.lblZero.Size = new System.Drawing.Size(201, 61);
            this.lblZero.TabIndex = 6;
            this.lblZero.Text = "--";
            this.lblZero.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblFlowCheck
            // 
            this.lblFlowCheck.AutoSize = true;
            this.lblFlowCheck.BackColor = System.Drawing.Color.Black;
            this.lblFlowCheck.ForeColor = System.Drawing.Color.Gold;
            this.lblFlowCheck.Location = new System.Drawing.Point(575, 215);
            this.lblFlowCheck.Margin = new System.Windows.Forms.Padding(0);
            this.lblFlowCheck.Name = "lblFlowCheck";
            this.lblFlowCheck.Size = new System.Drawing.Size(201, 61);
            this.lblFlowCheck.TabIndex = 8;
            this.lblFlowCheck.Text = "--";
            this.lblFlowCheck.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblO2SpanCheck
            // 
            this.lblO2SpanCheck.AutoSize = true;
            this.lblO2SpanCheck.BackColor = System.Drawing.Color.Black;
            this.lblO2SpanCheck.ForeColor = System.Drawing.Color.Gold;
            this.lblO2SpanCheck.Location = new System.Drawing.Point(575, 146);
            this.lblO2SpanCheck.Margin = new System.Windows.Forms.Padding(0);
            this.lblO2SpanCheck.Name = "lblO2SpanCheck";
            this.lblO2SpanCheck.Size = new System.Drawing.Size(201, 61);
            this.lblO2SpanCheck.TabIndex = 7;
            this.lblO2SpanCheck.Text = "--";
            this.lblO2SpanCheck.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblResult
            // 
            this.lblResult.AutoSize = true;
            this.lblResult.BackColor = System.Drawing.Color.Black;
            this.lblResult.ForeColor = System.Drawing.Color.Gold;
            this.lblResult.Location = new System.Drawing.Point(575, 284);
            this.lblResult.Margin = new System.Windows.Forms.Padding(0);
            this.lblResult.Name = "lblResult";
            this.lblResult.Size = new System.Drawing.Size(201, 61);
            this.lblResult.TabIndex = 9;
            this.lblResult.Text = "--";
            this.lblResult.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
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
            // layoutGroup1
            // 
            this.layoutGroup1.Height = 70;
            this.layoutGroup1.HeightType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            this.layoutGroup1.Items.AddRange(new DevComponents.DotNetBar.Layout.LayoutItemBase[] {
            this.layoutControlItem2,
            this.layoutControlItem3,
            this.layoutControlItem4,
            this.layoutControlItem5});
            this.layoutGroup1.Name = "layoutGroup1";
            this.layoutGroup1.TextPosition = DevComponents.DotNetBar.Layout.eLayoutPosition.Top;
            this.layoutGroup1.Width = 50;
            this.layoutGroup1.WidthType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            // 
            // layoutControlItem2
            // 
            this.layoutControlItem2.Control = this.lblStep;
            this.layoutControlItem2.Height = 25;
            this.layoutControlItem2.HeightType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            this.layoutControlItem2.Name = "layoutControlItem2";
            this.layoutControlItem2.Text = "当前运行阶段:";
            this.layoutControlItem2.TextAlignment = DevComponents.DotNetBar.Layout.eTextAlignment.Center;
            this.layoutControlItem2.TextLineAlignment = DevComponents.DotNetBar.Layout.eTextLineAlignment.Middle;
            this.layoutControlItem2.Width = 100;
            this.layoutControlItem2.WidthType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            // 
            // layoutControlItem3
            // 
            this.layoutControlItem3.Control = this.lblFlow;
            this.layoutControlItem3.Height = 25;
            this.layoutControlItem3.HeightType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            this.layoutControlItem3.Name = "layoutControlItem3";
            this.layoutControlItem3.Text = "流量(L/s):";
            this.layoutControlItem3.TextAlignment = DevComponents.DotNetBar.Layout.eTextAlignment.Center;
            this.layoutControlItem3.TextLineAlignment = DevComponents.DotNetBar.Layout.eTextLineAlignment.Middle;
            this.layoutControlItem3.Width = 100;
            this.layoutControlItem3.WidthType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            // 
            // layoutControlItem4
            // 
            this.layoutControlItem4.Control = this.lblO2;
            this.layoutControlItem4.Height = 25;
            this.layoutControlItem4.HeightType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            this.layoutControlItem4.Name = "layoutControlItem4";
            this.layoutControlItem4.Text = "O2值(%):";
            this.layoutControlItem4.TextAlignment = DevComponents.DotNetBar.Layout.eTextAlignment.Center;
            this.layoutControlItem4.TextLineAlignment = DevComponents.DotNetBar.Layout.eTextLineAlignment.Middle;
            this.layoutControlItem4.Width = 100;
            this.layoutControlItem4.WidthType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            // 
            // layoutControlItem5
            // 
            this.layoutControlItem5.Control = this.lblRestTime;
            this.layoutControlItem5.Height = 25;
            this.layoutControlItem5.HeightType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            this.layoutControlItem5.Name = "layoutControlItem5";
            this.layoutControlItem5.Text = "剩余时间(s):";
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
            this.layoutControlItem6,
            this.layoutControlItem8,
            this.layoutControlItem7,
            this.layoutControlItem9});
            this.layoutGroup2.Name = "layoutGroup2";
            this.layoutGroup2.TextPosition = DevComponents.DotNetBar.Layout.eLayoutPosition.Top;
            this.layoutGroup2.Width = 50;
            this.layoutGroup2.WidthType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            // 
            // layoutControlItem6
            // 
            this.layoutControlItem6.Control = this.lblZero;
            this.layoutControlItem6.Height = 25;
            this.layoutControlItem6.HeightType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            this.layoutControlItem6.MinSize = new System.Drawing.Size(64, 18);
            this.layoutControlItem6.Name = "layoutControlItem6";
            this.layoutControlItem6.Text = "清零结果:";
            this.layoutControlItem6.TextAlignment = DevComponents.DotNetBar.Layout.eTextAlignment.Center;
            this.layoutControlItem6.TextLineAlignment = DevComponents.DotNetBar.Layout.eTextLineAlignment.Middle;
            this.layoutControlItem6.Width = 100;
            this.layoutControlItem6.WidthType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            // 
            // layoutControlItem7
            // 
            this.layoutControlItem7.Control = this.lblFlowCheck;
            this.layoutControlItem7.Height = 25;
            this.layoutControlItem7.HeightType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            this.layoutControlItem7.Name = "layoutControlItem7";
            this.layoutControlItem7.Text = "流量检查结果:";
            this.layoutControlItem7.TextAlignment = DevComponents.DotNetBar.Layout.eTextAlignment.Center;
            this.layoutControlItem7.TextLineAlignment = DevComponents.DotNetBar.Layout.eTextLineAlignment.Middle;
            this.layoutControlItem7.Width = 100;
            this.layoutControlItem7.WidthType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            // 
            // layoutControlItem8
            // 
            this.layoutControlItem8.Control = this.lblO2SpanCheck;
            this.layoutControlItem8.Height = 25;
            this.layoutControlItem8.HeightType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            this.layoutControlItem8.Name = "layoutControlItem8";
            this.layoutControlItem8.Text = "氧量程检查结果:";
            this.layoutControlItem8.TextAlignment = DevComponents.DotNetBar.Layout.eTextAlignment.Center;
            this.layoutControlItem8.TextLineAlignment = DevComponents.DotNetBar.Layout.eTextLineAlignment.Middle;
            this.layoutControlItem8.Width = 100;
            this.layoutControlItem8.WidthType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            // 
            // layoutControlItem9
            // 
            this.layoutControlItem9.Control = this.lblResult;
            this.layoutControlItem9.Height = 25;
            this.layoutControlItem9.HeightType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            this.layoutControlItem9.Name = "layoutControlItem9";
            this.layoutControlItem9.Text = "自检总结果:";
            this.layoutControlItem9.TextAlignment = DevComponents.DotNetBar.Layout.eTextAlignment.Center;
            this.layoutControlItem9.TextLineAlignment = DevComponents.DotNetBar.Layout.eTextLineAlignment.Middle;
            this.layoutControlItem9.Width = 100;
            this.layoutControlItem9.WidthType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            // 
            // layoutControlItem10
            // 
            this.layoutControlItem10.Control = this.btnStart;
            this.layoutControlItem10.Height = 13;
            this.layoutControlItem10.HeightType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            this.layoutControlItem10.Name = "layoutControlItem10";
            this.layoutControlItem10.Width = 50;
            this.layoutControlItem10.WidthType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            // 
            // layoutControlItem11
            // 
            this.layoutControlItem11.Control = this.btnStop;
            this.layoutControlItem11.Height = 13;
            this.layoutControlItem11.HeightType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            this.layoutControlItem11.Name = "layoutControlItem11";
            this.layoutControlItem11.Width = 50;
            this.layoutControlItem11.WidthType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            // 
            // FlowmeterSelfcheckSubForm
            // 
            this.AcceptButton = this.btnStart;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(784, 411);
            this.Controls.Add(this.layoutMain);
            this.Name = "FlowmeterSelfcheckSubForm";
            this.Text = "FlowmeterSelfcheckSubForm";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FlowmeterSelfcheckSubForm_FormClosing);
            this.Load += new System.EventHandler(this.FlowmeterSelfcheckSubForm_Load);
            this.Resize += new System.EventHandler(this.FlowmeterSelfcheckSubForm_Resize);
            this.layoutMain.ResumeLayout(false);
            this.layoutMain.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private DevComponents.DotNetBar.Layout.LayoutControl layoutMain;
        private System.Windows.Forms.Label lblMsg;
        private DevComponents.DotNetBar.Layout.LayoutControlItem layoutControlItem1;
        private System.Windows.Forms.Button btnStop;
        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.Label lblFlow;
        private System.Windows.Forms.Label lblStep;
        private System.Windows.Forms.Label lblO2;
        private System.Windows.Forms.Label lblRestTime;
        private System.Windows.Forms.Label lblZero;
        private System.Windows.Forms.Label lblFlowCheck;
        private System.Windows.Forms.Label lblO2SpanCheck;
        private System.Windows.Forms.Label lblResult;
        private DevComponents.DotNetBar.Layout.LayoutGroup layoutGroup1;
        private DevComponents.DotNetBar.Layout.LayoutControlItem layoutControlItem2;
        private DevComponents.DotNetBar.Layout.LayoutControlItem layoutControlItem3;
        private DevComponents.DotNetBar.Layout.LayoutControlItem layoutControlItem4;
        private DevComponents.DotNetBar.Layout.LayoutControlItem layoutControlItem5;
        private DevComponents.DotNetBar.Layout.LayoutGroup layoutGroup2;
        private DevComponents.DotNetBar.Layout.LayoutControlItem layoutControlItem6;
        private DevComponents.DotNetBar.Layout.LayoutControlItem layoutControlItem7;
        private DevComponents.DotNetBar.Layout.LayoutControlItem layoutControlItem8;
        private DevComponents.DotNetBar.Layout.LayoutControlItem layoutControlItem9;
        private DevComponents.DotNetBar.Layout.LayoutControlItem layoutControlItem10;
        private DevComponents.DotNetBar.Layout.LayoutControlItem layoutControlItem11;
    }
}