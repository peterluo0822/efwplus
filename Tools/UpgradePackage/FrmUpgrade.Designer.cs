namespace UpgradePackage
{
    partial class FrmUpgrade
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmUpgrade));
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnOutputPath = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.txtServerPath = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.btnProgramPath = new System.Windows.Forms.Button();
            this.txtClientPath = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.cbTpl = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.panel3 = new System.Windows.Forms.Panel();
            this.treeProgram = new System.Windows.Forms.TreeView();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.panel1.SuspendLayout();
            this.panel3.SuspendLayout();
            this.SuspendLayout();
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "folder-open.png");
            this.imageList1.Images.SetKeyName(1, "document.png");
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.btnOutputPath);
            this.panel1.Controls.Add(this.button2);
            this.panel1.Controls.Add(this.txtServerPath);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.button1);
            this.panel1.Controls.Add(this.btnProgramPath);
            this.panel1.Controls.Add(this.txtClientPath);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.cbTpl);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(760, 100);
            this.panel1.TabIndex = 0;
            // 
            // btnOutputPath
            // 
            this.btnOutputPath.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOutputPath.Location = new System.Drawing.Point(656, 38);
            this.btnOutputPath.Name = "btnOutputPath";
            this.btnOutputPath.Size = new System.Drawing.Size(75, 23);
            this.btnOutputPath.TabIndex = 6;
            this.btnOutputPath.Text = "浏览";
            this.btnOutputPath.UseVisualStyleBackColor = true;
            this.btnOutputPath.Visible = false;
            this.btnOutputPath.Click += new System.EventHandler(this.btnOutputPath_Click);
            // 
            // button2
            // 
            this.button2.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.button2.ForeColor = System.Drawing.Color.Blue;
            this.button2.Location = new System.Drawing.Point(72, 71);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(121, 23);
            this.button2.TabIndex = 6;
            this.button2.Text = "打包";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Visible = false;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // txtServerPath
            // 
            this.txtServerPath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtServerPath.Location = new System.Drawing.Point(295, 40);
            this.txtServerPath.Name = "txtServerPath";
            this.txtServerPath.ReadOnly = true;
            this.txtServerPath.Size = new System.Drawing.Size(355, 21);
            this.txtServerPath.TabIndex = 5;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(200, 43);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(89, 12);
            this.label3.TabIndex = 4;
            this.label3.Text = "Server程序目录";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(72, 40);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(121, 23);
            this.button1.TabIndex = 5;
            this.button1.Text = "保存为模板";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // btnProgramPath
            // 
            this.btnProgramPath.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnProgramPath.Location = new System.Drawing.Point(656, 8);
            this.btnProgramPath.Name = "btnProgramPath";
            this.btnProgramPath.Size = new System.Drawing.Size(75, 23);
            this.btnProgramPath.TabIndex = 4;
            this.btnProgramPath.Text = "浏览";
            this.btnProgramPath.UseVisualStyleBackColor = true;
            this.btnProgramPath.Visible = false;
            this.btnProgramPath.Click += new System.EventHandler(this.btnProgramPath_Click);
            // 
            // txtClientPath
            // 
            this.txtClientPath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtClientPath.Location = new System.Drawing.Point(295, 9);
            this.txtClientPath.Name = "txtClientPath";
            this.txtClientPath.ReadOnly = true;
            this.txtClientPath.Size = new System.Drawing.Size(355, 21);
            this.txtClientPath.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(200, 13);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(89, 12);
            this.label2.TabIndex = 2;
            this.label2.Text = "Client程序目录";
            // 
            // cbTpl
            // 
            this.cbTpl.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbTpl.FormattingEnabled = true;
            this.cbTpl.Location = new System.Drawing.Point(72, 9);
            this.cbTpl.Name = "cbTpl";
            this.cbTpl.Size = new System.Drawing.Size(121, 20);
            this.cbTpl.TabIndex = 1;
            this.cbTpl.SelectedIndexChanged += new System.EventHandler(this.cbTpl_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "选择模板";
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.treeProgram);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel3.Location = new System.Drawing.Point(0, 100);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(760, 347);
            this.panel3.TabIndex = 2;
            // 
            // treeProgram
            // 
            this.treeProgram.CheckBoxes = true;
            this.treeProgram.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeProgram.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.treeProgram.ImageIndex = 0;
            this.treeProgram.ImageList = this.imageList1;
            this.treeProgram.Location = new System.Drawing.Point(0, 0);
            this.treeProgram.Name = "treeProgram";
            this.treeProgram.SelectedImageIndex = 0;
            this.treeProgram.Size = new System.Drawing.Size(760, 347);
            this.treeProgram.TabIndex = 0;
            this.treeProgram.AfterCheck += new System.Windows.Forms.TreeViewEventHandler(this.treeProgram_AfterCheck);
            // 
            // FrmUpgrade
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(760, 447);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel1);
            this.Name = "FrmUpgrade";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "配置模板";
            this.Load += new System.EventHandler(this.FrmUpgrade_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ImageList imageList1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btnProgramPath;
        private System.Windows.Forms.TextBox txtClientPath;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox cbTpl;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Button btnOutputPath;
        private System.Windows.Forms.TextBox txtServerPath;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TreeView treeProgram;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
    }
}

