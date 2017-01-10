namespace UpgradePackage
{
    partial class FrmMain
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmMain));
            this.label1 = new System.Windows.Forms.Label();
            this.txtversions = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.btnServerPath = new System.Windows.Forms.Button();
            this.txtServerPath = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.btnClientPath = new System.Windows.Forms.Button();
            this.txtClientPath = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.btnOutput = new System.Windows.Forms.Button();
            this.txtOutput = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.btnCreate = new System.Windows.Forms.Button();
            this.btnTplSetting = new System.Windows.Forms.Button();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(79, 26);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "当前版本：";
            // 
            // txtversions
            // 
            this.txtversions.Location = new System.Drawing.Point(138, 23);
            this.txtversions.Name = "txtversions";
            this.txtversions.Size = new System.Drawing.Size(167, 21);
            this.txtversions.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(312, 27);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(101, 12);
            this.label2.TabIndex = 2;
            this.label2.Text = "(如：2.1.5.1228)";
            // 
            // btnServerPath
            // 
            this.btnServerPath.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnServerPath.Location = new System.Drawing.Point(453, 79);
            this.btnServerPath.Name = "btnServerPath";
            this.btnServerPath.Size = new System.Drawing.Size(75, 23);
            this.btnServerPath.TabIndex = 12;
            this.btnServerPath.Text = "浏览";
            this.btnServerPath.UseVisualStyleBackColor = true;
            this.btnServerPath.Click += new System.EventHandler(this.btnServerPath_Click);
            // 
            // txtServerPath
            // 
            this.txtServerPath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtServerPath.Location = new System.Drawing.Point(138, 81);
            this.txtServerPath.Name = "txtServerPath";
            this.txtServerPath.ReadOnly = true;
            this.txtServerPath.Size = new System.Drawing.Size(308, 21);
            this.txtServerPath.TabIndex = 11;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(19, 83);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(125, 12);
            this.label3.TabIndex = 9;
            this.label3.Text = "选择Server程序目录：";
            // 
            // btnClientPath
            // 
            this.btnClientPath.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClientPath.Location = new System.Drawing.Point(453, 48);
            this.btnClientPath.Name = "btnClientPath";
            this.btnClientPath.Size = new System.Drawing.Size(75, 23);
            this.btnClientPath.TabIndex = 10;
            this.btnClientPath.Text = "浏览";
            this.btnClientPath.UseVisualStyleBackColor = true;
            this.btnClientPath.Click += new System.EventHandler(this.btnClientPath_Click);
            // 
            // txtClientPath
            // 
            this.txtClientPath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtClientPath.Location = new System.Drawing.Point(138, 50);
            this.txtClientPath.Name = "txtClientPath";
            this.txtClientPath.ReadOnly = true;
            this.txtClientPath.Size = new System.Drawing.Size(308, 21);
            this.txtClientPath.TabIndex = 8;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(19, 53);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(125, 12);
            this.label4.TabIndex = 7;
            this.label4.Text = "选择Client程序目录：";
            // 
            // btnOutput
            // 
            this.btnOutput.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOutput.Location = new System.Drawing.Point(453, 123);
            this.btnOutput.Name = "btnOutput";
            this.btnOutput.Size = new System.Drawing.Size(75, 23);
            this.btnOutput.TabIndex = 15;
            this.btnOutput.Text = "浏览";
            this.btnOutput.UseVisualStyleBackColor = true;
            this.btnOutput.Click += new System.EventHandler(this.btnOutput_Click);
            // 
            // txtOutput
            // 
            this.txtOutput.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtOutput.Location = new System.Drawing.Point(138, 125);
            this.txtOutput.Name = "txtOutput";
            this.txtOutput.ReadOnly = true;
            this.txtOutput.Size = new System.Drawing.Size(308, 21);
            this.txtOutput.TabIndex = 14;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(43, 128);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(101, 12);
            this.label5.TabIndex = 13;
            this.label5.Text = "升级包输出目录：";
            // 
            // btnCreate
            // 
            this.btnCreate.ForeColor = System.Drawing.Color.Blue;
            this.btnCreate.Location = new System.Drawing.Point(138, 173);
            this.btnCreate.Name = "btnCreate";
            this.btnCreate.Size = new System.Drawing.Size(75, 23);
            this.btnCreate.TabIndex = 16;
            this.btnCreate.Text = "创建升级包";
            this.btnCreate.UseVisualStyleBackColor = true;
            this.btnCreate.Click += new System.EventHandler(this.btnCreate_Click);
            // 
            // btnTplSetting
            // 
            this.btnTplSetting.Location = new System.Drawing.Point(371, 173);
            this.btnTplSetting.Name = "btnTplSetting";
            this.btnTplSetting.Size = new System.Drawing.Size(75, 23);
            this.btnTplSetting.TabIndex = 17;
            this.btnTplSetting.Text = "配置模板";
            this.btnTplSetting.UseVisualStyleBackColor = true;
            this.btnTplSetting.Click += new System.EventHandler(this.button3_Click);
            // 
            // FrmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(540, 240);
            this.Controls.Add(this.btnTplSetting);
            this.Controls.Add(this.btnCreate);
            this.Controls.Add(this.btnOutput);
            this.Controls.Add(this.txtOutput);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.btnServerPath);
            this.Controls.Add(this.txtServerPath);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.btnClientPath);
            this.Controls.Add(this.txtClientPath);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtversions);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "升级包工具";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtversions;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnServerPath;
        private System.Windows.Forms.TextBox txtServerPath;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btnClientPath;
        private System.Windows.Forms.TextBox txtClientPath;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button btnOutput;
        private System.Windows.Forms.TextBox txtOutput;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button btnCreate;
        private System.Windows.Forms.Button btnTplSetting;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
    }
}