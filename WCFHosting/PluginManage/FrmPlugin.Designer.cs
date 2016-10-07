namespace WCFHosting
{
    partial class FrmPlugin
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmPlugin));
            this.panel1 = new System.Windows.Forms.Panel();
            this.gridlocal = new System.Windows.Forms.DataGridView();
            this.Column1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.重载插件ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.内存卸载ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.内存加载ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.panel2 = new System.Windows.Forms.Panel();
            this.btnreload = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.btnDel = new System.Windows.Forms.Button();
            this.btnAdd = new System.Windows.Forms.Button();
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.查看配置ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridlocal)).BeginInit();
            this.contextMenuStrip1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.gridlocal);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(534, 295);
            this.panel1.TabIndex = 0;
            // 
            // gridlocal
            // 
            this.gridlocal.AllowUserToAddRows = false;
            this.gridlocal.AllowUserToDeleteRows = false;
            this.gridlocal.AllowUserToResizeRows = false;
            this.gridlocal.BackgroundColor = System.Drawing.SystemColors.Control;
            this.gridlocal.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridlocal.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column1,
            this.Column2,
            this.Column3,
            this.Column4});
            this.gridlocal.ContextMenuStrip = this.contextMenuStrip1;
            this.gridlocal.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridlocal.Location = new System.Drawing.Point(0, 0);
            this.gridlocal.MultiSelect = false;
            this.gridlocal.Name = "gridlocal";
            this.gridlocal.ReadOnly = true;
            this.gridlocal.RowTemplate.Height = 23;
            this.gridlocal.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridlocal.Size = new System.Drawing.Size(534, 295);
            this.gridlocal.TabIndex = 0;
            // 
            // Column1
            // 
            this.Column1.DataPropertyName = "name";
            this.Column1.HeaderText = "标识";
            this.Column1.MinimumWidth = 120;
            this.Column1.Name = "Column1";
            this.Column1.ReadOnly = true;
            this.Column1.Width = 120;
            // 
            // Column2
            // 
            this.Column2.DataPropertyName = "title";
            this.Column2.HeaderText = "名称";
            this.Column2.MinimumWidth = 150;
            this.Column2.Name = "Column2";
            this.Column2.ReadOnly = true;
            this.Column2.Width = 150;
            // 
            // Column3
            // 
            this.Column3.DataPropertyName = "version";
            this.Column3.HeaderText = "版本";
            this.Column3.MinimumWidth = 100;
            this.Column3.Name = "Column3";
            this.Column3.ReadOnly = true;
            // 
            // Column4
            // 
            this.Column4.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.Column4.HeaderText = "介绍";
            this.Column4.Name = "Column4";
            this.Column4.ReadOnly = true;
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.重载插件ToolStripMenuItem,
            this.toolStripSeparator1,
            this.内存卸载ToolStripMenuItem,
            this.内存加载ToolStripMenuItem,
            this.toolStripSeparator2,
            this.查看配置ToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(153, 126);
            // 
            // 重载插件ToolStripMenuItem
            // 
            this.重载插件ToolStripMenuItem.Name = "重载插件ToolStripMenuItem";
            this.重载插件ToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.重载插件ToolStripMenuItem.Text = "重载插件";
            this.重载插件ToolStripMenuItem.Click += new System.EventHandler(this.重载插件ToolStripMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(149, 6);
            // 
            // 内存卸载ToolStripMenuItem
            // 
            this.内存卸载ToolStripMenuItem.Name = "内存卸载ToolStripMenuItem";
            this.内存卸载ToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.内存卸载ToolStripMenuItem.Text = "内存卸载";
            this.内存卸载ToolStripMenuItem.Click += new System.EventHandler(this.内存卸载ToolStripMenuItem_Click);
            // 
            // 内存加载ToolStripMenuItem
            // 
            this.内存加载ToolStripMenuItem.Name = "内存加载ToolStripMenuItem";
            this.内存加载ToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.内存加载ToolStripMenuItem.Text = "内存加载";
            this.内存加载ToolStripMenuItem.Click += new System.EventHandler(this.内存加载ToolStripMenuItem_Click);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.btnreload);
            this.panel2.Controls.Add(this.button3);
            this.panel2.Controls.Add(this.btnDel);
            this.panel2.Controls.Add(this.btnAdd);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel2.Location = new System.Drawing.Point(0, 295);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(534, 67);
            this.panel2.TabIndex = 1;
            // 
            // btnreload
            // 
            this.btnreload.Location = new System.Drawing.Point(193, 20);
            this.btnreload.Name = "btnreload";
            this.btnreload.Size = new System.Drawing.Size(75, 28);
            this.btnreload.TabIndex = 3;
            this.btnreload.Text = "重载插件";
            this.btnreload.UseVisualStyleBackColor = true;
            this.btnreload.Visible = false;
            this.btnreload.Click += new System.EventHandler(this.重载插件ToolStripMenuItem_Click);
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(418, 20);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(75, 28);
            this.button3.TabIndex = 2;
            this.button3.Text = "关闭";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // btnDel
            // 
            this.btnDel.Image = ((System.Drawing.Image)(resources.GetObject("btnDel.Image")));
            this.btnDel.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnDel.Location = new System.Drawing.Point(112, 20);
            this.btnDel.Name = "btnDel";
            this.btnDel.Size = new System.Drawing.Size(75, 28);
            this.btnDel.TabIndex = 1;
            this.btnDel.Text = "卸载插件";
            this.btnDel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnDel.UseVisualStyleBackColor = true;
            this.btnDel.Click += new System.EventHandler(this.btnDel_Click);
            // 
            // btnAdd
            // 
            this.btnAdd.Image = ((System.Drawing.Image)(resources.GetObject("btnAdd.Image")));
            this.btnAdd.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnAdd.Location = new System.Drawing.Point(31, 20);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(75, 28);
            this.btnAdd.TabIndex = 0;
            this.btnAdd.Text = "添加插件";
            this.btnAdd.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // openFileDialog
            // 
            this.openFileDialog.Filter = "插件包|*.zip";
            // 
            // 查看配置ToolStripMenuItem
            // 
            this.查看配置ToolStripMenuItem.Name = "查看配置ToolStripMenuItem";
            this.查看配置ToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.查看配置ToolStripMenuItem.Text = "查看配置";
            this.查看配置ToolStripMenuItem.Click += new System.EventHandler(this.查看配置ToolStripMenuItem_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(149, 6);
            // 
            // FrmPlugin
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(534, 362);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.panel2);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmPlugin";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "插件管理";
            this.Load += new System.EventHandler(this.FrmPlugin_Load);
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridlocal)).EndInit();
            this.contextMenuStrip1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.DataGridView gridlocal;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button btnDel;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.OpenFileDialog openFileDialog;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column2;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column3;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column4;
        private System.Windows.Forms.Button btnreload;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem 重载插件ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 内存卸载ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 内存加载ToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem 查看配置ToolStripMenuItem;
    }
}