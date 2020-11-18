namespace Dyno_Geely {
    partial class LoadingForm {
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LoadingForm));
            this.lblLog = new System.Windows.Forms.Label();
            this.prgBar = new System.Windows.Forms.ProgressBar();
            this.tblPnlMain = new System.Windows.Forms.TableLayoutPanel();
            this.lblTimer = new System.Windows.Forms.Label();
            this.tblPnlMain.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblLog
            // 
            this.lblLog.BackColor = System.Drawing.Color.Transparent;
            this.lblLog.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblLog.Font = new System.Drawing.Font("黑体", 20F, System.Drawing.FontStyle.Bold);
            this.lblLog.Location = new System.Drawing.Point(3, 0);
            this.lblLog.Name = "lblLog";
            this.lblLog.Size = new System.Drawing.Size(494, 87);
            this.lblLog.TabIndex = 0;
            this.lblLog.Text = "show log";
            this.lblLog.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.lblLog.MouseDown += new System.Windows.Forms.MouseEventHandler(this.OnMouseDown);
            // 
            // prgBar
            // 
            this.prgBar.BackColor = System.Drawing.SystemColors.Control;
            this.prgBar.Dock = System.Windows.Forms.DockStyle.Fill;
            this.prgBar.Location = new System.Drawing.Point(3, 177);
            this.prgBar.Name = "prgBar";
            this.prgBar.Size = new System.Drawing.Size(494, 20);
            this.prgBar.TabIndex = 1;
            // 
            // tblPnlMain
            // 
            this.tblPnlMain.BackColor = System.Drawing.SystemColors.ControlDark;
            this.tblPnlMain.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("tblPnlMain.BackgroundImage")));
            this.tblPnlMain.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.tblPnlMain.ColumnCount = 1;
            this.tblPnlMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tblPnlMain.Controls.Add(this.lblLog, 0, 0);
            this.tblPnlMain.Controls.Add(this.prgBar, 0, 2);
            this.tblPnlMain.Controls.Add(this.lblTimer, 0, 1);
            this.tblPnlMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tblPnlMain.Location = new System.Drawing.Point(0, 0);
            this.tblPnlMain.Name = "tblPnlMain";
            this.tblPnlMain.RowCount = 3;
            this.tblPnlMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tblPnlMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tblPnlMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.tblPnlMain.Size = new System.Drawing.Size(500, 200);
            this.tblPnlMain.TabIndex = 2;
            // 
            // lblTimer
            // 
            this.lblTimer.BackColor = System.Drawing.Color.Transparent;
            this.lblTimer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblTimer.Font = new System.Drawing.Font("黑体", 20F, System.Drawing.FontStyle.Bold);
            this.lblTimer.Location = new System.Drawing.Point(3, 87);
            this.lblTimer.Name = "lblTimer";
            this.lblTimer.Size = new System.Drawing.Size(494, 87);
            this.lblTimer.TabIndex = 2;
            this.lblTimer.Text = "timer";
            this.lblTimer.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.lblTimer.MouseDown += new System.Windows.Forms.MouseEventHandler(this.OnMouseDown);
            // 
            // LoadingForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(500, 200);
            this.Controls.Add(this.tblPnlMain);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "LoadingForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "LoadingForm";
            this.Load += new System.EventHandler(this.LoadingForm_Load);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.OnMouseDown);
            this.tblPnlMain.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label lblLog;
        private System.Windows.Forms.ProgressBar prgBar;
        private System.Windows.Forms.TableLayoutPanel tblPnlMain;
        private System.Windows.Forms.Label lblTimer;
    }
}