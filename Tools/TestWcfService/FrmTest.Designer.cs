namespace TestWcfService
{
    partial class FrmTest
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
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmTest));
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.treePlugin = new System.Windows.Forms.TreeView();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.btnreload = new System.Windows.Forms.ToolStripButton();
            this.panel2 = new System.Windows.Forms.Panel();
            this.txtResult = new System.Windows.Forms.RichTextBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label5 = new System.Windows.Forms.Label();
            this.btnRequest = new System.Windows.Forms.Button();
            this.txtparams = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.cbmothed = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.cbcontroller = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.cbplugin = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.treePlugin);
            this.splitContainer1.Panel1.Controls.Add(this.toolStrip1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.panel2);
            this.splitContainer1.Panel2.Controls.Add(this.panel1);
            this.splitContainer1.Size = new System.Drawing.Size(637, 466);
            this.splitContainer1.SplitterDistance = 212;
            this.splitContainer1.TabIndex = 0;
            // 
            // treePlugin
            // 
            this.treePlugin.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treePlugin.Font = new System.Drawing.Font("宋体", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.treePlugin.ImageIndex = 0;
            this.treePlugin.ImageList = this.imageList1;
            this.treePlugin.Location = new System.Drawing.Point(0, 25);
            this.treePlugin.Name = "treePlugin";
            this.treePlugin.SelectedImageIndex = 0;
            this.treePlugin.Size = new System.Drawing.Size(212, 441);
            this.treePlugin.TabIndex = 0;
            this.treePlugin.DoubleClick += new System.EventHandler(this.treePlugin_DoubleClick);
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "service.png");
            this.imageList1.Images.SetKeyName(1, "controller.png");
            this.imageList1.Images.SetKeyName(2, "method.png");
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnreload});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(212, 25);
            this.toolStrip1.TabIndex = 1;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // btnreload
            // 
            this.btnreload.Image = ((System.Drawing.Image)(resources.GetObject("btnreload.Image")));
            this.btnreload.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnreload.Name = "btnreload";
            this.btnreload.Size = new System.Drawing.Size(52, 22);
            this.btnreload.Text = "刷新";
            this.btnreload.Click += new System.EventHandler(this.btnreload_Click);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.txtResult);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 167);
            this.panel2.Name = "panel2";
            this.panel2.Padding = new System.Windows.Forms.Padding(2);
            this.panel2.Size = new System.Drawing.Size(421, 299);
            this.panel2.TabIndex = 1;
            // 
            // txtResult
            // 
            this.txtResult.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtResult.Location = new System.Drawing.Point(2, 2);
            this.txtResult.Name = "txtResult";
            this.txtResult.Size = new System.Drawing.Size(417, 295);
            this.txtResult.TabIndex = 0;
            this.txtResult.Text = "";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.label5);
            this.panel1.Controls.Add(this.btnRequest);
            this.panel1.Controls.Add(this.txtparams);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.cbmothed);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.cbcontroller);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.cbplugin);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(421, 167);
            this.panel1.TabIndex = 0;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(19, 147);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(65, 12);
            this.label5.TabIndex = 9;
            this.label5.Text = "返回结果：";
            // 
            // btnRequest
            // 
            this.btnRequest.Image = ((System.Drawing.Image)(resources.GetObject("btnRequest.Image")));
            this.btnRequest.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnRequest.Location = new System.Drawing.Point(82, 117);
            this.btnRequest.Name = "btnRequest";
            this.btnRequest.Size = new System.Drawing.Size(75, 25);
            this.btnRequest.TabIndex = 8;
            this.btnRequest.Text = "  请求";
            this.btnRequest.UseVisualStyleBackColor = true;
            this.btnRequest.Click += new System.EventHandler(this.btnRequest_Click);
            // 
            // txtparams
            // 
            this.txtparams.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtparams.Location = new System.Drawing.Point(82, 89);
            this.txtparams.Name = "txtparams";
            this.txtparams.Size = new System.Drawing.Size(327, 21);
            this.txtparams.TabIndex = 7;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(2, 92);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(77, 12);
            this.label4.TabIndex = 6;
            this.label4.Text = "输入参数Json";
            // 
            // cbmothed
            // 
            this.cbmothed.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cbmothed.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbmothed.FormattingEnabled = true;
            this.cbmothed.Location = new System.Drawing.Point(82, 62);
            this.cbmothed.Name = "cbmothed";
            this.cbmothed.Size = new System.Drawing.Size(327, 20);
            this.cbmothed.TabIndex = 5;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(26, 65);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(53, 12);
            this.label3.TabIndex = 4;
            this.label3.Text = "选择方法";
            // 
            // cbcontroller
            // 
            this.cbcontroller.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cbcontroller.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbcontroller.FormattingEnabled = true;
            this.cbcontroller.Location = new System.Drawing.Point(82, 36);
            this.cbcontroller.Name = "cbcontroller";
            this.cbcontroller.Size = new System.Drawing.Size(327, 20);
            this.cbcontroller.TabIndex = 3;
            this.cbcontroller.SelectedIndexChanged += new System.EventHandler(this.cbcontroller_SelectedIndexChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(14, 39);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(65, 12);
            this.label2.TabIndex = 2;
            this.label2.Text = "选择控制器";
            // 
            // cbplugin
            // 
            this.cbplugin.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cbplugin.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbplugin.FormattingEnabled = true;
            this.cbplugin.Location = new System.Drawing.Point(82, 10);
            this.cbplugin.Name = "cbplugin";
            this.cbplugin.Size = new System.Drawing.Size(327, 20);
            this.cbplugin.TabIndex = 1;
            this.cbplugin.SelectedIndexChanged += new System.EventHandler(this.cbplugin_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(26, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "选择插件";
            // 
            // FrmTest
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(637, 466);
            this.Controls.Add(this.splitContainer1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FrmTest";
            this.Text = "测试CHDEP平台服务插件";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FrmTest_FormClosed);
            this.Load += new System.EventHandler(this.FrmTest_Load);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.TreeView treePlugin;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton btnreload;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.ComboBox cbplugin;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnRequest;
        private System.Windows.Forms.TextBox txtparams;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox cbmothed;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox cbcontroller;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.RichTextBox txtResult;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ImageList imageList1;
    }
}

