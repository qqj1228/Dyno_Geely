
namespace Dyno_Geely {
    partial class DynoGlideCheckForm {
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
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea4 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend4 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series4 = new System.Windows.Forms.DataVisualization.Charting.Series();
            this.layoutMain = new DevComponents.DotNetBar.Layout.LayoutControl();
            this.chkBoxPreheat = new System.Windows.Forms.CheckBox();
            this.txtBoxIHP = new System.Windows.Forms.TextBox();
            this.lblNote = new System.Windows.Forms.Label();
            this.btnStop = new System.Windows.Forms.Button();
            this.btnStart = new System.Windows.Forms.Button();
            this.btnBeamUp = new System.Windows.Forms.Button();
            this.btnBeamDown = new System.Windows.Forms.Button();
            this.chart1 = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.dgvResult = new System.Windows.Forms.DataGridView();
            this.lblMsg = new System.Windows.Forms.Label();
            this.lblPower = new System.Windows.Forms.Label();
            this.lblSpeed = new System.Windows.Forms.Label();
            this.lblTorqueF = new System.Windows.Forms.Label();
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
            this.layoutGroup1 = new DevComponents.DotNetBar.Layout.LayoutGroup();
            this.layoutSpacerItem1 = new DevComponents.DotNetBar.Layout.LayoutSpacerItem();
            this.layoutControlItem12 = new DevComponents.DotNetBar.Layout.LayoutControlItem();
            this.layoutControlItem11 = new DevComponents.DotNetBar.Layout.LayoutControlItem();
            this.layoutControlItem13 = new DevComponents.DotNetBar.Layout.LayoutControlItem();
            this.layoutControlItem14 = new DevComponents.DotNetBar.Layout.LayoutControlItem();
            this.lblResult = new System.Windows.Forms.Label();
            this.layoutControlItem15 = new DevComponents.DotNetBar.Layout.LayoutControlItem();
            this.layoutMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chart1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvResult)).BeginInit();
            this.SuspendLayout();
            // 
            // layoutMain
            // 
            this.layoutMain.BackColor = System.Drawing.Color.Transparent;
            this.layoutMain.Controls.Add(this.chkBoxPreheat);
            this.layoutMain.Controls.Add(this.txtBoxIHP);
            this.layoutMain.Controls.Add(this.lblNote);
            this.layoutMain.Controls.Add(this.btnStop);
            this.layoutMain.Controls.Add(this.btnStart);
            this.layoutMain.Controls.Add(this.btnBeamUp);
            this.layoutMain.Controls.Add(this.btnBeamDown);
            this.layoutMain.Controls.Add(this.chart1);
            this.layoutMain.Controls.Add(this.dgvResult);
            this.layoutMain.Controls.Add(this.lblMsg);
            this.layoutMain.Controls.Add(this.lblPower);
            this.layoutMain.Controls.Add(this.lblSpeed);
            this.layoutMain.Controls.Add(this.lblTorqueF);
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
            this.layoutControlItem2,
            this.layoutControlItem3,
            this.layoutControlItem4,
            this.layoutControlItem5,
            this.layoutControlItem6,
            this.layoutControlItem15,
            this.layoutControlItem7,
            this.layoutControlItem8,
            this.layoutControlItem9,
            this.layoutControlItem10,
            this.layoutGroup1,
            this.layoutControlItem11,
            this.layoutControlItem13});
            this.layoutMain.Size = new System.Drawing.Size(1008, 729);
            this.layoutMain.TabIndex = 0;
            // 
            // chkBoxPreheat
            // 
            this.chkBoxPreheat.AutoSize = true;
            this.chkBoxPreheat.Location = new System.Drawing.Point(880, 688);
            this.chkBoxPreheat.Margin = new System.Windows.Forms.Padding(0);
            this.chkBoxPreheat.Name = "chkBoxPreheat";
            this.chkBoxPreheat.Size = new System.Drawing.Size(124, 35);
            this.chkBoxPreheat.TabIndex = 15;
            this.chkBoxPreheat.Text = "滑行预热";
            this.chkBoxPreheat.UseVisualStyleBackColor = true;
            // 
            // txtBoxIHP
            // 
            this.txtBoxIHP.Location = new System.Drawing.Point(925, 643);
            this.txtBoxIHP.Margin = new System.Windows.Forms.Padding(0);
            this.txtBoxIHP.Name = "txtBoxIHP";
            this.txtBoxIHP.Size = new System.Drawing.Size(75, 32);
            this.txtBoxIHP.TabIndex = 12;
            this.txtBoxIHP.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtBoxIHP.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.TxtBoxIHP_KeyPress);
            // 
            // lblNote
            // 
            this.lblNote.AutoSize = true;
            this.lblNote.Location = new System.Drawing.Point(4, 688);
            this.lblNote.Margin = new System.Windows.Forms.Padding(0);
            this.lblNote.Name = "lblNote";
            this.lblNote.Size = new System.Drawing.Size(868, 35);
            this.lblNote.TabIndex = 14;
            this.lblNote.Text = "滑行时间误差范围需满足：";
            this.lblNote.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnStop
            // 
            this.btnStop.Location = new System.Drawing.Point(577, 637);
            this.btnStop.Margin = new System.Windows.Forms.Padding(0);
            this.btnStop.Name = "btnStop";
            this.btnStop.Size = new System.Drawing.Size(183, 43);
            this.btnStop.TabIndex = 10;
            this.btnStop.Text = "手动停止";
            this.btnStop.UseVisualStyleBackColor = true;
            this.btnStop.Click += new System.EventHandler(this.BtnStop_Click);
            // 
            // btnStart
            // 
            this.btnStart.Location = new System.Drawing.Point(386, 637);
            this.btnStart.Margin = new System.Windows.Forms.Padding(0);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(183, 43);
            this.btnStart.TabIndex = 9;
            this.btnStart.Text = "开始检查";
            this.btnStart.UseVisualStyleBackColor = true;
            this.btnStart.Click += new System.EventHandler(this.BtnStart_Click);
            // 
            // btnBeamUp
            // 
            this.btnBeamUp.Location = new System.Drawing.Point(195, 637);
            this.btnBeamUp.Margin = new System.Windows.Forms.Padding(0);
            this.btnBeamUp.Name = "btnBeamUp";
            this.btnBeamUp.Size = new System.Drawing.Size(183, 43);
            this.btnBeamUp.TabIndex = 8;
            this.btnBeamUp.Text = "举升上升";
            this.btnBeamUp.UseVisualStyleBackColor = true;
            this.btnBeamUp.Click += new System.EventHandler(this.BtnBeamUp_Click);
            // 
            // btnBeamDown
            // 
            this.btnBeamDown.Location = new System.Drawing.Point(4, 637);
            this.btnBeamDown.Margin = new System.Windows.Forms.Padding(0);
            this.btnBeamDown.Name = "btnBeamDown";
            this.btnBeamDown.Size = new System.Drawing.Size(183, 43);
            this.btnBeamDown.TabIndex = 7;
            this.btnBeamDown.Text = "举升下降";
            this.btnBeamDown.UseVisualStyleBackColor = true;
            this.btnBeamDown.Click += new System.EventHandler(this.BtnBeamDown_Click);
            // 
            // chart1
            // 
            this.chart1.BackColor = System.Drawing.Color.Transparent;
            chartArea4.BackColor = System.Drawing.Color.Transparent;
            chartArea4.Name = "ChartArea1";
            this.chart1.ChartAreas.Add(chartArea4);
            legend4.BackColor = System.Drawing.Color.Transparent;
            legend4.DockedToChartArea = "ChartArea1";
            legend4.Name = "Legend1";
            this.chart1.Legends.Add(legend4);
            this.chart1.Location = new System.Drawing.Point(508, 76);
            this.chart1.Margin = new System.Windows.Forms.Padding(0);
            this.chart1.Name = "chart1";
            series4.ChartArea = "ChartArea1";
            series4.Legend = "Legend1";
            series4.Name = "Series1";
            this.chart1.Series.Add(series4);
            this.chart1.Size = new System.Drawing.Size(496, 502);
            this.chart1.TabIndex = 2;
            this.chart1.Text = "chart1";
            // 
            // dgvResult
            // 
            this.dgvResult.AllowUserToAddRows = false;
            this.dgvResult.AllowUserToDeleteRows = false;
            this.dgvResult.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvResult.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvResult.Location = new System.Drawing.Point(4, 76);
            this.dgvResult.Margin = new System.Windows.Forms.Padding(0);
            this.dgvResult.Name = "dgvResult";
            this.dgvResult.ReadOnly = true;
            this.dgvResult.RowHeadersVisible = false;
            this.dgvResult.RowTemplate.Height = 23;
            this.dgvResult.Size = new System.Drawing.Size(496, 502);
            this.dgvResult.TabIndex = 1;
            // 
            // lblMsg
            // 
            this.lblMsg.AutoSize = true;
            this.lblMsg.Font = new System.Drawing.Font("黑体", 30F, System.Drawing.FontStyle.Bold);
            this.lblMsg.ForeColor = System.Drawing.Color.Red;
            this.lblMsg.Location = new System.Drawing.Point(4, 4);
            this.lblMsg.Margin = new System.Windows.Forms.Padding(0);
            this.lblMsg.Name = "lblMsg";
            this.lblMsg.Size = new System.Drawing.Size(1000, 64);
            this.lblMsg.TabIndex = 0;
            this.lblMsg.Text = "Msg";
            this.lblMsg.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblPower
            // 
            this.lblPower.AutoSize = true;
            this.lblPower.BackColor = System.Drawing.Color.Black;
            this.lblPower.ForeColor = System.Drawing.Color.Gold;
            this.lblPower.Location = new System.Drawing.Point(135, 586);
            this.lblPower.Margin = new System.Windows.Forms.Padding(0);
            this.lblPower.Name = "lblPower";
            this.lblPower.Size = new System.Drawing.Size(113, 43);
            this.lblPower.TabIndex = 3;
            this.lblPower.Text = "--";
            this.lblPower.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblSpeed
            // 
            this.lblSpeed.AutoSize = true;
            this.lblSpeed.BackColor = System.Drawing.Color.Black;
            this.lblSpeed.ForeColor = System.Drawing.Color.Gold;
            this.lblSpeed.Location = new System.Drawing.Point(387, 586);
            this.lblSpeed.Margin = new System.Windows.Forms.Padding(0);
            this.lblSpeed.Name = "lblSpeed";
            this.lblSpeed.Size = new System.Drawing.Size(113, 43);
            this.lblSpeed.TabIndex = 4;
            this.lblSpeed.Text = "--";
            this.lblSpeed.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblTorqueF
            // 
            this.lblTorqueF.AutoSize = true;
            this.lblTorqueF.BackColor = System.Drawing.Color.Black;
            this.lblTorqueF.ForeColor = System.Drawing.Color.Gold;
            this.lblTorqueF.Location = new System.Drawing.Point(639, 586);
            this.lblTorqueF.Margin = new System.Windows.Forms.Padding(0);
            this.lblTorqueF.Name = "lblTorqueF";
            this.lblTorqueF.Size = new System.Drawing.Size(113, 43);
            this.lblTorqueF.TabIndex = 5;
            this.lblTorqueF.Text = "--";
            this.lblTorqueF.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // layoutControlItem1
            // 
            this.layoutControlItem1.Control = this.lblMsg;
            this.layoutControlItem1.Height = 10;
            this.layoutControlItem1.HeightType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            this.layoutControlItem1.MinSize = new System.Drawing.Size(64, 18);
            this.layoutControlItem1.Name = "layoutControlItem1";
            this.layoutControlItem1.Text = "Label:";
            this.layoutControlItem1.TextVisible = false;
            this.layoutControlItem1.Width = 100;
            this.layoutControlItem1.WidthType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            // 
            // layoutControlItem2
            // 
            this.layoutControlItem2.Control = this.dgvResult;
            this.layoutControlItem2.Height = 70;
            this.layoutControlItem2.HeightType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            this.layoutControlItem2.MinSize = new System.Drawing.Size(64, 18);
            this.layoutControlItem2.Name = "layoutControlItem2";
            this.layoutControlItem2.Text = "Label:";
            this.layoutControlItem2.TextVisible = false;
            this.layoutControlItem2.Width = 50;
            this.layoutControlItem2.WidthType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            // 
            // layoutControlItem3
            // 
            this.layoutControlItem3.Control = this.chart1;
            this.layoutControlItem3.Height = 70;
            this.layoutControlItem3.HeightType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            this.layoutControlItem3.MinSize = new System.Drawing.Size(64, 18);
            this.layoutControlItem3.Name = "layoutControlItem3";
            this.layoutControlItem3.Text = "Label:";
            this.layoutControlItem3.TextVisible = false;
            this.layoutControlItem3.Width = 50;
            this.layoutControlItem3.WidthType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            // 
            // layoutControlItem4
            // 
            this.layoutControlItem4.Control = this.lblPower;
            this.layoutControlItem4.Height = 7;
            this.layoutControlItem4.HeightType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            this.layoutControlItem4.MinSize = new System.Drawing.Size(64, 18);
            this.layoutControlItem4.Name = "layoutControlItem4";
            this.layoutControlItem4.Text = "功率(kW):";
            this.layoutControlItem4.TextAlignment = DevComponents.DotNetBar.Layout.eTextAlignment.Center;
            this.layoutControlItem4.TextLineAlignment = DevComponents.DotNetBar.Layout.eTextLineAlignment.Middle;
            this.layoutControlItem4.Width = 25;
            this.layoutControlItem4.WidthType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            // 
            // layoutControlItem5
            // 
            this.layoutControlItem5.Control = this.lblSpeed;
            this.layoutControlItem5.Height = 7;
            this.layoutControlItem5.HeightType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            this.layoutControlItem5.MinSize = new System.Drawing.Size(64, 18);
            this.layoutControlItem5.Name = "layoutControlItem5";
            this.layoutControlItem5.Text = "速度(km/h):";
            this.layoutControlItem5.TextAlignment = DevComponents.DotNetBar.Layout.eTextAlignment.Center;
            this.layoutControlItem5.TextLineAlignment = DevComponents.DotNetBar.Layout.eTextLineAlignment.Middle;
            this.layoutControlItem5.Width = 25;
            this.layoutControlItem5.WidthType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            // 
            // layoutControlItem6
            // 
            this.layoutControlItem6.Control = this.lblTorqueF;
            this.layoutControlItem6.Height = 7;
            this.layoutControlItem6.HeightType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            this.layoutControlItem6.MinSize = new System.Drawing.Size(64, 18);
            this.layoutControlItem6.Name = "layoutControlItem6";
            this.layoutControlItem6.Text = "扭力(N):";
            this.layoutControlItem6.TextAlignment = DevComponents.DotNetBar.Layout.eTextAlignment.Center;
            this.layoutControlItem6.TextLineAlignment = DevComponents.DotNetBar.Layout.eTextLineAlignment.Middle;
            this.layoutControlItem6.Width = 25;
            this.layoutControlItem6.WidthType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            // 
            // layoutControlItem7
            // 
            this.layoutControlItem7.Control = this.btnBeamDown;
            this.layoutControlItem7.Height = 7;
            this.layoutControlItem7.HeightType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            this.layoutControlItem7.MinSize = new System.Drawing.Size(32, 20);
            this.layoutControlItem7.Name = "layoutControlItem7";
            this.layoutControlItem7.Width = 19;
            this.layoutControlItem7.WidthType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            // 
            // layoutControlItem8
            // 
            this.layoutControlItem8.Control = this.btnBeamUp;
            this.layoutControlItem8.Height = 7;
            this.layoutControlItem8.HeightType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            this.layoutControlItem8.MinSize = new System.Drawing.Size(32, 20);
            this.layoutControlItem8.Name = "layoutControlItem8";
            this.layoutControlItem8.Width = 19;
            this.layoutControlItem8.WidthType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            // 
            // layoutControlItem9
            // 
            this.layoutControlItem9.Control = this.btnStart;
            this.layoutControlItem9.Height = 7;
            this.layoutControlItem9.HeightType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            this.layoutControlItem9.MinSize = new System.Drawing.Size(32, 20);
            this.layoutControlItem9.Name = "layoutControlItem9";
            this.layoutControlItem9.Width = 19;
            this.layoutControlItem9.WidthType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            // 
            // layoutControlItem10
            // 
            this.layoutControlItem10.Control = this.btnStop;
            this.layoutControlItem10.Height = 7;
            this.layoutControlItem10.HeightType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            this.layoutControlItem10.MinSize = new System.Drawing.Size(32, 20);
            this.layoutControlItem10.Name = "layoutControlItem10";
            this.layoutControlItem10.Width = 19;
            this.layoutControlItem10.WidthType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            // 
            // layoutGroup1
            // 
            this.layoutGroup1.Height = 7;
            this.layoutGroup1.HeightType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            this.layoutGroup1.Items.AddRange(new DevComponents.DotNetBar.Layout.LayoutItemBase[] {
            this.layoutSpacerItem1,
            this.layoutControlItem12});
            this.layoutGroup1.MinSize = new System.Drawing.Size(120, 32);
            this.layoutGroup1.Name = "layoutGroup1";
            this.layoutGroup1.TextPosition = DevComponents.DotNetBar.Layout.eLayoutPosition.Top;
            this.layoutGroup1.Width = 24;
            this.layoutGroup1.WidthType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            // 
            // layoutSpacerItem1
            // 
            this.layoutSpacerItem1.Height = 5;
            this.layoutSpacerItem1.HeightType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            this.layoutSpacerItem1.Name = "layoutSpacerItem1";
            this.layoutSpacerItem1.Width = 100;
            this.layoutSpacerItem1.WidthType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            // 
            // layoutControlItem12
            // 
            this.layoutControlItem12.Control = this.txtBoxIHP;
            this.layoutControlItem12.Height = 95;
            this.layoutControlItem12.HeightType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            this.layoutControlItem12.MinSize = new System.Drawing.Size(120, 0);
            this.layoutControlItem12.Name = "layoutControlItem12";
            this.layoutControlItem12.Text = "加载功率(kW):";
            this.layoutControlItem12.TextAlignment = DevComponents.DotNetBar.Layout.eTextAlignment.Center;
            this.layoutControlItem12.TextLineAlignment = DevComponents.DotNetBar.Layout.eTextLineAlignment.Middle;
            this.layoutControlItem12.Width = 100;
            this.layoutControlItem12.WidthType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            // 
            // layoutControlItem11
            // 
            this.layoutControlItem11.Control = this.lblNote;
            this.layoutControlItem11.Height = 6;
            this.layoutControlItem11.HeightType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            this.layoutControlItem11.MinSize = new System.Drawing.Size(64, 18);
            this.layoutControlItem11.Name = "layoutControlItem11";
            this.layoutControlItem11.Text = "Label:";
            this.layoutControlItem11.TextVisible = false;
            this.layoutControlItem11.Width = 87;
            this.layoutControlItem11.WidthType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            // 
            // layoutControlItem13
            // 
            this.layoutControlItem13.Control = this.chkBoxPreheat;
            this.layoutControlItem13.Height = 6;
            this.layoutControlItem13.HeightType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            this.layoutControlItem13.MinSize = new System.Drawing.Size(32, 20);
            this.layoutControlItem13.Name = "layoutControlItem13";
            this.layoutControlItem13.Width = 13;
            this.layoutControlItem13.WidthType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            // 
            // layoutControlItem14
            // 
            this.layoutControlItem14.Control = this.lblResult;
            this.layoutControlItem14.Height = 7;
            this.layoutControlItem14.HeightType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            this.layoutControlItem14.MinSize = new System.Drawing.Size(64, 18);
            this.layoutControlItem14.Name = "layoutControlItem14";
            this.layoutControlItem14.Text = "扭力(N):";
            this.layoutControlItem14.TextAlignment = DevComponents.DotNetBar.Layout.eTextAlignment.Center;
            this.layoutControlItem14.TextLineAlignment = DevComponents.DotNetBar.Layout.eTextLineAlignment.Middle;
            this.layoutControlItem14.Width = 25;
            this.layoutControlItem14.WidthType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            // 
            // lblResult
            // 
            this.lblResult.AutoSize = true;
            this.lblResult.BackColor = System.Drawing.Color.Black;
            this.lblResult.ForeColor = System.Drawing.Color.Gold;
            this.lblResult.Location = new System.Drawing.Point(891, 586);
            this.lblResult.Margin = new System.Windows.Forms.Padding(0);
            this.lblResult.Name = "lblResult";
            this.lblResult.Size = new System.Drawing.Size(113, 43);
            this.lblResult.TabIndex = 6;
            this.lblResult.Text = "--";
            this.lblResult.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // layoutControlItem15
            // 
            this.layoutControlItem15.Control = this.lblResult;
            this.layoutControlItem15.Height = 7;
            this.layoutControlItem15.HeightType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            this.layoutControlItem15.MinSize = new System.Drawing.Size(64, 18);
            this.layoutControlItem15.Name = "layoutControlItem15";
            this.layoutControlItem15.Text = "最终结果:";
            this.layoutControlItem15.TextAlignment = DevComponents.DotNetBar.Layout.eTextAlignment.Center;
            this.layoutControlItem15.TextLineAlignment = DevComponents.DotNetBar.Layout.eTextLineAlignment.Middle;
            this.layoutControlItem15.Width = 25;
            this.layoutControlItem15.WidthType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            // 
            // DynoGlideCheckForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1008, 729);
            this.Controls.Add(this.layoutMain);
            this.Name = "DynoGlideCheckForm";
            this.Text = "测功机滑行检查";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.DynoGlideCheckForm_FormClosing);
            this.Load += new System.EventHandler(this.DynoGlideCheckForm_Load);
            this.Resize += new System.EventHandler(this.DynoGlideCheckForm_Resize);
            this.layoutMain.ResumeLayout(false);
            this.layoutMain.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chart1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvResult)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevComponents.DotNetBar.Layout.LayoutControl layoutMain;
        private System.Windows.Forms.Label lblMsg;
        private DevComponents.DotNetBar.Layout.LayoutControlItem layoutControlItem1;
        private System.Windows.Forms.DataGridView dgvResult;
        private DevComponents.DotNetBar.Layout.LayoutControlItem layoutControlItem2;
        private System.Windows.Forms.DataVisualization.Charting.Chart chart1;
        private DevComponents.DotNetBar.Layout.LayoutControlItem layoutControlItem3;
        private System.Windows.Forms.Label lblPower;
        private DevComponents.DotNetBar.Layout.LayoutControlItem layoutControlItem4;
        private System.Windows.Forms.Label lblSpeed;
        private System.Windows.Forms.Label lblTorqueF;
        private DevComponents.DotNetBar.Layout.LayoutControlItem layoutControlItem5;
        private DevComponents.DotNetBar.Layout.LayoutControlItem layoutControlItem6;
        private System.Windows.Forms.Button btnBeamDown;
        private DevComponents.DotNetBar.Layout.LayoutControlItem layoutControlItem7;
        private System.Windows.Forms.Label lblNote;
        private System.Windows.Forms.Button btnStop;
        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.Button btnBeamUp;
        private DevComponents.DotNetBar.Layout.LayoutControlItem layoutControlItem8;
        private DevComponents.DotNetBar.Layout.LayoutControlItem layoutControlItem9;
        private DevComponents.DotNetBar.Layout.LayoutControlItem layoutControlItem10;
        private DevComponents.DotNetBar.Layout.LayoutControlItem layoutControlItem11;
        private System.Windows.Forms.CheckBox chkBoxPreheat;
        private System.Windows.Forms.TextBox txtBoxIHP;
        private DevComponents.DotNetBar.Layout.LayoutGroup layoutGroup1;
        private DevComponents.DotNetBar.Layout.LayoutControlItem layoutControlItem12;
        private DevComponents.DotNetBar.Layout.LayoutControlItem layoutControlItem13;
        private DevComponents.DotNetBar.Layout.LayoutSpacerItem layoutSpacerItem1;
        private System.Windows.Forms.Label lblResult;
        private DevComponents.DotNetBar.Layout.LayoutControlItem layoutControlItem15;
        private DevComponents.DotNetBar.Layout.LayoutControlItem layoutControlItem14;
    }
}