namespace Dyno_Geely {
    partial class MainForm {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent() {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.tblPnlMain = new System.Windows.Forms.TableLayoutPanel();
            this.tblLytPnlLogo = new System.Windows.Forms.TableLayoutPanel();
            this.picBoxLogo = new System.Windows.Forms.PictureBox();
            this.lblLogo = new System.Windows.Forms.Label();
            this.lblTitle = new System.Windows.Forms.Label();
            this.tblPnlBottom = new System.Windows.Forms.TableLayoutPanel();
            this.btnPreheating = new System.Windows.Forms.Button();
            this.btnLogin = new System.Windows.Forms.Button();
            this.lblInfo = new System.Windows.Forms.Label();
            this.tblPnlMain.SuspendLayout();
            this.tblLytPnlLogo.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picBoxLogo)).BeginInit();
            this.tblPnlBottom.SuspendLayout();
            this.SuspendLayout();
            // 
            // tblPnlMain
            // 
            this.tblPnlMain.ColumnCount = 1;
            this.tblPnlMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tblPnlMain.Controls.Add(this.tblLytPnlLogo, 0, 0);
            this.tblPnlMain.Controls.Add(this.lblTitle, 0, 1);
            this.tblPnlMain.Controls.Add(this.tblPnlBottom, 0, 3);
            this.tblPnlMain.Controls.Add(this.lblInfo, 0, 2);
            this.tblPnlMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tblPnlMain.Location = new System.Drawing.Point(0, 0);
            this.tblPnlMain.Name = "tblPnlMain";
            this.tblPnlMain.RowCount = 4;
            this.tblPnlMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 17F));
            this.tblPnlMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 10F));
            this.tblPnlMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 60F));
            this.tblPnlMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 13F));
            this.tblPnlMain.Size = new System.Drawing.Size(784, 561);
            this.tblPnlMain.TabIndex = 0;
            // 
            // tblLytPnlLogo
            // 
            this.tblLytPnlLogo.ColumnCount = 2;
            this.tblLytPnlLogo.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tblLytPnlLogo.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 75F));
            this.tblLytPnlLogo.Controls.Add(this.picBoxLogo, 0, 0);
            this.tblLytPnlLogo.Controls.Add(this.lblLogo, 1, 0);
            this.tblLytPnlLogo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tblLytPnlLogo.Location = new System.Drawing.Point(3, 3);
            this.tblLytPnlLogo.Name = "tblLytPnlLogo";
            this.tblLytPnlLogo.RowCount = 1;
            this.tblLytPnlLogo.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tblLytPnlLogo.Size = new System.Drawing.Size(778, 89);
            this.tblLytPnlLogo.TabIndex = 1;
            // 
            // picBoxLogo
            // 
            this.picBoxLogo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.picBoxLogo.Image = ((System.Drawing.Image)(resources.GetObject("picBoxLogo.Image")));
            this.picBoxLogo.Location = new System.Drawing.Point(3, 3);
            this.picBoxLogo.Name = "picBoxLogo";
            this.picBoxLogo.Size = new System.Drawing.Size(188, 83);
            this.picBoxLogo.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picBoxLogo.TabIndex = 0;
            this.picBoxLogo.TabStop = false;
            // 
            // lblLogo
            // 
            this.lblLogo.AutoSize = true;
            this.lblLogo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblLogo.Font = new System.Drawing.Font("宋体", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblLogo.Location = new System.Drawing.Point(197, 0);
            this.lblLogo.Name = "lblLogo";
            this.lblLogo.Size = new System.Drawing.Size(578, 89);
            this.lblLogo.TabIndex = 1;
            this.lblLogo.Text = "赛赫智能设备（上海）股份有限公司";
            this.lblLogo.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblTitle.Font = new System.Drawing.Font("黑体", 30F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblTitle.Location = new System.Drawing.Point(3, 95);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(778, 40);
            this.lblTitle.TabIndex = 2;
            this.lblTitle.Text = "尾气排放检测系统";
            this.lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // tblPnlBottom
            // 
            this.tblPnlBottom.ColumnCount = 2;
            this.tblPnlBottom.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tblPnlBottom.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tblPnlBottom.Controls.Add(this.btnPreheating, 0, 0);
            this.tblPnlBottom.Controls.Add(this.btnLogin, 1, 0);
            this.tblPnlBottom.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tblPnlBottom.Location = new System.Drawing.Point(3, 490);
            this.tblPnlBottom.Name = "tblPnlBottom";
            this.tblPnlBottom.RowCount = 1;
            this.tblPnlBottom.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tblPnlBottom.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 68F));
            this.tblPnlBottom.Size = new System.Drawing.Size(778, 68);
            this.tblPnlBottom.TabIndex = 3;
            // 
            // btnPreheating
            // 
            this.btnPreheating.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnPreheating.Font = new System.Drawing.Font("宋体", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnPreheating.Location = new System.Drawing.Point(3, 3);
            this.btnPreheating.Name = "btnPreheating";
            this.btnPreheating.Size = new System.Drawing.Size(383, 62);
            this.btnPreheating.TabIndex = 0;
            this.btnPreheating.Text = "设备预热";
            this.btnPreheating.UseVisualStyleBackColor = true;
            this.btnPreheating.Click += new System.EventHandler(this.BtnPreheating_Click);
            // 
            // btnLogin
            // 
            this.btnLogin.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnLogin.Font = new System.Drawing.Font("宋体", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnLogin.Location = new System.Drawing.Point(392, 3);
            this.btnLogin.Name = "btnLogin";
            this.btnLogin.Size = new System.Drawing.Size(383, 62);
            this.btnLogin.TabIndex = 1;
            this.btnLogin.Text = "进入系统";
            this.btnLogin.UseVisualStyleBackColor = true;
            this.btnLogin.Click += new System.EventHandler(this.BtnLogin_Click);
            // 
            // lblInfo
            // 
            this.lblInfo.AutoSize = true;
            this.lblInfo.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.lblInfo.Font = new System.Drawing.Font("宋体", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblInfo.Location = new System.Drawing.Point(3, 454);
            this.lblInfo.Name = "lblInfo";
            this.lblInfo.Size = new System.Drawing.Size(778, 33);
            this.lblInfo.TabIndex = 4;
            this.lblInfo.Text = "信息...";
            this.lblInfo.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(784, 561);
            this.Controls.Add(this.tblPnlMain);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "主窗口";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.Resize += new System.EventHandler(this.MainForm_Resize);
            this.tblPnlMain.ResumeLayout(false);
            this.tblPnlMain.PerformLayout();
            this.tblLytPnlLogo.ResumeLayout(false);
            this.tblLytPnlLogo.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picBoxLogo)).EndInit();
            this.tblPnlBottom.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tblPnlMain;
        private System.Windows.Forms.PictureBox picBoxLogo;
        private System.Windows.Forms.TableLayoutPanel tblLytPnlLogo;
        private System.Windows.Forms.Label lblLogo;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.TableLayoutPanel tblPnlBottom;
        private System.Windows.Forms.Button btnPreheating;
        private System.Windows.Forms.Button btnLogin;
        private System.Windows.Forms.Label lblInfo;
    }
}

