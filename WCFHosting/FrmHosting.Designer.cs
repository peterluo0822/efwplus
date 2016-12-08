namespace WCFHosting
{
    partial class FrmHosting
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmHosting));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.btnStart = new System.Windows.Forms.ToolStripButton();
            this.btnStop = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.btnrestart = new System.Windows.Forms.ToolStripButton();
            this.btnSetting = new System.Windows.Forms.ToolStripButton();
            this.btnrouter = new System.Windows.Forms.ToolStripButton();
            this.btnplugin = new System.Windows.Forms.ToolStripButton();
            this.btntask = new System.Windows.Forms.ToolStripButton();
            this.btnInfo = new System.Windows.Forms.ToolStripButton();
            this.toolStripDropDownButton1 = new System.Windows.Forms.ToolStripDropDownButton();
            this.帮助ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.注册ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.关于ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.btnPublishService = new System.Windows.Forms.ToolStripButton();
            this.btnSubscibe = new System.Windows.Forms.ToolStripButton();
            this.tabMain = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.richTextMsg = new System.Windows.Forms.ListBox();
            this.contextMenuStrip2 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.清除日志ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.复制日志ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.暂停日志ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.gridClientList = new System.Windows.Forms.DataGridView();
            this.Column1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column9 = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.Column2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column10 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column11 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column12 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.gridRouter = new System.Windows.Forms.DataGridView();
            this.Column5 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column7 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column6 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column8 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.notifyIcon1 = new System.Windows.Forms.NotifyIcon(this.components);
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.显示ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.启动ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.停止ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.退出ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.lbStatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel3 = new System.Windows.Forms.ToolStripStatusLabel();
            this.lsServerUrl = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel2 = new System.Windows.Forms.ToolStripStatusLabel();
            this.lbClientCount = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel5 = new System.Windows.Forms.ToolStripStatusLabel();
            this.lbRunTime = new System.Windows.Forms.ToolStripStatusLabel();
            this.timerRun = new System.Windows.Forms.Timer(this.components);
            this.timermsg = new System.Windows.Forms.Timer(this.components);
            this.toolStrip1.SuspendLayout();
            this.tabMain.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.contextMenuStrip2.SuspendLayout();
            this.tabPage2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridClientList)).BeginInit();
            this.tabPage3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridRouter)).BeginInit();
            this.contextMenuStrip1.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnStart,
            this.btnStop,
            this.toolStripSeparator3,
            this.btnrestart,
            this.btnSetting,
            this.btnrouter,
            this.btnplugin,
            this.btnPublishService,
            this.btnSubscibe,
            this.btntask,
            this.btnInfo,
            this.toolStripDropDownButton1});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(935, 25);
            this.toolStrip1.TabIndex = 0;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // btnStart
            // 
            this.btnStart.Image = ((System.Drawing.Image)(resources.GetObject("btnStart.Image")));
            this.btnStart.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(52, 22);
            this.btnStart.Text = "启动";
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // btnStop
            // 
            this.btnStop.Image = ((System.Drawing.Image)(resources.GetObject("btnStop.Image")));
            this.btnStop.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnStop.Name = "btnStop";
            this.btnStop.Size = new System.Drawing.Size(52, 22);
            this.btnStop.Text = "停止";
            this.btnStop.Click += new System.EventHandler(this.btnStop_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(6, 25);
            // 
            // btnrestart
            // 
            this.btnrestart.Image = ((System.Drawing.Image)(resources.GetObject("btnrestart.Image")));
            this.btnrestart.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnrestart.Name = "btnrestart";
            this.btnrestart.Size = new System.Drawing.Size(76, 22);
            this.btnrestart.Text = "一键重启";
            this.btnrestart.Click += new System.EventHandler(this.btnrestart_Click);
            // 
            // btnSetting
            // 
            this.btnSetting.Image = ((System.Drawing.Image)(resources.GetObject("btnSetting.Image")));
            this.btnSetting.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnSetting.Name = "btnSetting";
            this.btnSetting.Size = new System.Drawing.Size(52, 22);
            this.btnSetting.Text = "设置";
            this.btnSetting.Click += new System.EventHandler(this.btnSetting_Click);
            // 
            // btnrouter
            // 
            this.btnrouter.Image = ((System.Drawing.Image)(resources.GetObject("btnrouter.Image")));
            this.btnrouter.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnrouter.Name = "btnrouter";
            this.btnrouter.Size = new System.Drawing.Size(64, 22);
            this.btnrouter.Text = "路由表";
            this.btnrouter.Click += new System.EventHandler(this.btnrouter_Click);
            // 
            // btnplugin
            // 
            this.btnplugin.Image = ((System.Drawing.Image)(resources.GetObject("btnplugin.Image")));
            this.btnplugin.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnplugin.Name = "btnplugin";
            this.btnplugin.Size = new System.Drawing.Size(52, 22);
            this.btnplugin.Text = "插件";
            this.btnplugin.Click += new System.EventHandler(this.btnplugin_Click);
            // 
            // btntask
            // 
            this.btntask.Image = ((System.Drawing.Image)(resources.GetObject("btntask.Image")));
            this.btntask.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btntask.Name = "btntask";
            this.btntask.Size = new System.Drawing.Size(76, 22);
            this.btntask.Text = "定时任务";
            this.btntask.Click += new System.EventHandler(this.btntask_Click);
            // 
            // btnInfo
            // 
            this.btnInfo.Image = ((System.Drawing.Image)(resources.GetObject("btnInfo.Image")));
            this.btnInfo.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnInfo.Name = "btnInfo";
            this.btnInfo.Size = new System.Drawing.Size(76, 22);
            this.btnInfo.Text = "综合信息";
            this.btnInfo.Click += new System.EventHandler(this.btnInfo_Click);
            // 
            // toolStripDropDownButton1
            // 
            this.toolStripDropDownButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripDropDownButton1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.帮助ToolStripMenuItem,
            this.注册ToolStripMenuItem,
            this.toolStripSeparator4,
            this.关于ToolStripMenuItem});
            this.toolStripDropDownButton1.Image = ((System.Drawing.Image)(resources.GetObject("toolStripDropDownButton1.Image")));
            this.toolStripDropDownButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripDropDownButton1.Name = "toolStripDropDownButton1";
            this.toolStripDropDownButton1.Size = new System.Drawing.Size(29, 22);
            this.toolStripDropDownButton1.Text = "toolStripDropDownButton1";
            // 
            // 帮助ToolStripMenuItem
            // 
            this.帮助ToolStripMenuItem.Name = "帮助ToolStripMenuItem";
            this.帮助ToolStripMenuItem.Size = new System.Drawing.Size(100, 22);
            this.帮助ToolStripMenuItem.Text = "帮助";
            this.帮助ToolStripMenuItem.Click += new System.EventHandler(this.帮助ToolStripMenuItem_Click);
            // 
            // 注册ToolStripMenuItem
            // 
            this.注册ToolStripMenuItem.Name = "注册ToolStripMenuItem";
            this.注册ToolStripMenuItem.Size = new System.Drawing.Size(100, 22);
            this.注册ToolStripMenuItem.Text = "注册";
            this.注册ToolStripMenuItem.Click += new System.EventHandler(this.注册ToolStripMenuItem_Click);
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(97, 6);
            // 
            // 关于ToolStripMenuItem
            // 
            this.关于ToolStripMenuItem.Name = "关于ToolStripMenuItem";
            this.关于ToolStripMenuItem.Size = new System.Drawing.Size(100, 22);
            this.关于ToolStripMenuItem.Text = "关于";
            this.关于ToolStripMenuItem.Click += new System.EventHandler(this.关于ToolStripMenuItem_Click);
            // 
            // btnPublishService
            // 
            this.btnPublishService.Image = ((System.Drawing.Image)(resources.GetObject("btnPublishService.Image")));
            this.btnPublishService.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnPublishService.Name = "btnPublishService";
            this.btnPublishService.Size = new System.Drawing.Size(76, 22);
            this.btnPublishService.Text = "发布订阅";
            this.btnPublishService.Click += new System.EventHandler(this.btnPublishService_Click);
            // 
            // btnSubscibe
            // 
            this.btnSubscibe.Image = ((System.Drawing.Image)(resources.GetObject("btnSubscibe.Image")));
            this.btnSubscibe.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnSubscibe.Name = "btnSubscibe";
            this.btnSubscibe.Size = new System.Drawing.Size(76, 22);
            this.btnSubscibe.Text = "订阅服务";
            this.btnSubscibe.Click += new System.EventHandler(this.btnSubscibe_Click);
            // 
            // tabMain
            // 
            this.tabMain.Controls.Add(this.tabPage1);
            this.tabMain.Controls.Add(this.tabPage2);
            this.tabMain.Controls.Add(this.tabPage3);
            this.tabMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabMain.Location = new System.Drawing.Point(0, 25);
            this.tabMain.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.tabMain.Name = "tabMain";
            this.tabMain.SelectedIndex = 0;
            this.tabMain.Size = new System.Drawing.Size(935, 523);
            this.tabMain.TabIndex = 0;
            this.tabMain.SelectedIndexChanged += new System.EventHandler(this.tabMain_SelectedIndexChanged);
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.richTextMsg);
            this.tabPage1.Location = new System.Drawing.Point(4, 26);
            this.tabPage1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.tabPage1.Size = new System.Drawing.Size(927, 493);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "输出";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // richTextMsg
            // 
            this.richTextMsg.ContextMenuStrip = this.contextMenuStrip2;
            this.richTextMsg.Dock = System.Windows.Forms.DockStyle.Fill;
            this.richTextMsg.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.richTextMsg.FormattingEnabled = true;
            this.richTextMsg.HorizontalScrollbar = true;
            this.richTextMsg.IntegralHeight = false;
            this.richTextMsg.ItemHeight = 18;
            this.richTextMsg.Location = new System.Drawing.Point(3, 4);
            this.richTextMsg.Name = "richTextMsg";
            this.richTextMsg.Size = new System.Drawing.Size(921, 485);
            this.richTextMsg.TabIndex = 1;
            this.richTextMsg.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.richTextMsg_DrawItem);
            // 
            // contextMenuStrip2
            // 
            this.contextMenuStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.清除日志ToolStripMenuItem,
            this.复制日志ToolStripMenuItem,
            this.暂停日志ToolStripMenuItem});
            this.contextMenuStrip2.Name = "contextMenuStrip2";
            this.contextMenuStrip2.Size = new System.Drawing.Size(125, 70);
            // 
            // 清除日志ToolStripMenuItem
            // 
            this.清除日志ToolStripMenuItem.Name = "清除日志ToolStripMenuItem";
            this.清除日志ToolStripMenuItem.Size = new System.Drawing.Size(124, 22);
            this.清除日志ToolStripMenuItem.Text = "清除日志";
            this.清除日志ToolStripMenuItem.Click += new System.EventHandler(this.清除日志ToolStripMenuItem_Click);
            // 
            // 复制日志ToolStripMenuItem
            // 
            this.复制日志ToolStripMenuItem.Name = "复制日志ToolStripMenuItem";
            this.复制日志ToolStripMenuItem.Size = new System.Drawing.Size(124, 22);
            this.复制日志ToolStripMenuItem.Text = "复制日志";
            this.复制日志ToolStripMenuItem.Click += new System.EventHandler(this.复制日志ToolStripMenuItem_Click);
            // 
            // 暂停日志ToolStripMenuItem
            // 
            this.暂停日志ToolStripMenuItem.Name = "暂停日志ToolStripMenuItem";
            this.暂停日志ToolStripMenuItem.Size = new System.Drawing.Size(124, 22);
            this.暂停日志ToolStripMenuItem.Text = "暂停日志";
            this.暂停日志ToolStripMenuItem.Click += new System.EventHandler(this.暂停日志ToolStripMenuItem_Click);
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.gridClientList);
            this.tabPage2.Location = new System.Drawing.Point(4, 26);
            this.tabPage2.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.tabPage2.Size = new System.Drawing.Size(927, 493);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "客户端列表";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // gridClientList
            // 
            this.gridClientList.AllowUserToAddRows = false;
            this.gridClientList.AllowUserToDeleteRows = false;
            this.gridClientList.BackgroundColor = System.Drawing.SystemColors.Control;
            this.gridClientList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridClientList.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column1,
            this.Column9,
            this.Column2,
            this.Column4,
            this.Column3,
            this.Column10,
            this.Column11,
            this.Column12});
            this.gridClientList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridClientList.Location = new System.Drawing.Point(3, 4);
            this.gridClientList.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.gridClientList.Name = "gridClientList";
            this.gridClientList.ReadOnly = true;
            this.gridClientList.RowTemplate.Height = 23;
            this.gridClientList.Size = new System.Drawing.Size(921, 485);
            this.gridClientList.TabIndex = 0;
            // 
            // Column1
            // 
            this.Column1.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.Column1.DataPropertyName = "clientId";
            this.Column1.HeaderText = "客户端编码";
            this.Column1.MinimumWidth = 200;
            this.Column1.Name = "Column1";
            this.Column1.ReadOnly = true;
            // 
            // Column9
            // 
            this.Column9.DataPropertyName = "IsConnect";
            this.Column9.HeaderText = "连接/断开";
            this.Column9.Name = "Column9";
            this.Column9.ReadOnly = true;
            this.Column9.Width = 80;
            // 
            // Column2
            // 
            this.Column2.DataPropertyName = "clientName";
            this.Column2.HeaderText = "客户端名称";
            this.Column2.Name = "Column2";
            this.Column2.ReadOnly = true;
            this.Column2.Width = 150;
            // 
            // Column4
            // 
            this.Column4.DataPropertyName = "HeartbeatCount";
            this.Column4.HeaderText = "心跳数";
            this.Column4.Name = "Column4";
            this.Column4.ReadOnly = true;
            this.Column4.Width = 70;
            // 
            // Column3
            // 
            this.Column3.DataPropertyName = "startTime";
            dataGridViewCellStyle2.Format = "G";
            dataGridViewCellStyle2.NullValue = null;
            this.Column3.DefaultCellStyle = dataGridViewCellStyle2;
            this.Column3.HeaderText = "连接时间";
            this.Column3.Name = "Column3";
            this.Column3.ReadOnly = true;
            this.Column3.Width = 150;
            // 
            // Column10
            // 
            this.Column10.DataPropertyName = "RequestCount";
            this.Column10.HeaderText = "请求次数";
            this.Column10.Name = "Column10";
            this.Column10.ReadOnly = true;
            // 
            // Column11
            // 
            this.Column11.DataPropertyName = "receiveData";
            this.Column11.HeaderText = "接收数据";
            this.Column11.Name = "Column11";
            this.Column11.ReadOnly = true;
            // 
            // Column12
            // 
            this.Column12.DataPropertyName = "sendData";
            this.Column12.HeaderText = "发送数据";
            this.Column12.Name = "Column12";
            this.Column12.ReadOnly = true;
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.gridRouter);
            this.tabPage3.Location = new System.Drawing.Point(4, 26);
            this.tabPage3.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Size = new System.Drawing.Size(927, 493);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "路由转发";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // gridRouter
            // 
            this.gridRouter.AllowUserToAddRows = false;
            this.gridRouter.AllowUserToDeleteRows = false;
            this.gridRouter.BackgroundColor = System.Drawing.SystemColors.Control;
            this.gridRouter.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridRouter.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column5,
            this.Column7,
            this.Column6,
            this.Column8});
            this.gridRouter.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridRouter.Location = new System.Drawing.Point(0, 0);
            this.gridRouter.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.gridRouter.Name = "gridRouter";
            this.gridRouter.ReadOnly = true;
            this.gridRouter.RowTemplate.Height = 23;
            this.gridRouter.Size = new System.Drawing.Size(927, 493);
            this.gridRouter.TabIndex = 0;
            // 
            // Column5
            // 
            this.Column5.DataPropertyName = "HostName";
            this.Column5.HeaderText = "中间件名称";
            this.Column5.MinimumWidth = 200;
            this.Column5.Name = "Column5";
            this.Column5.ReadOnly = true;
            this.Column5.Width = 200;
            // 
            // Column7
            // 
            this.Column7.DataPropertyName = "ServiceType";
            this.Column7.HeaderText = "类型";
            this.Column7.MinimumWidth = 100;
            this.Column7.Name = "Column7";
            this.Column7.ReadOnly = true;
            // 
            // Column6
            // 
            this.Column6.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.Column6.DataPropertyName = "Address";
            this.Column6.HeaderText = "地址";
            this.Column6.MinimumWidth = 300;
            this.Column6.Name = "Column6";
            this.Column6.ReadOnly = true;
            // 
            // Column8
            // 
            this.Column8.DataPropertyName = "ClientNum";
            this.Column8.HeaderText = "分配客户端数";
            this.Column8.MinimumWidth = 120;
            this.Column8.Name = "Column8";
            this.Column8.ReadOnly = true;
            this.Column8.Width = 120;
            // 
            // notifyIcon1
            // 
            this.notifyIcon1.ContextMenuStrip = this.contextMenuStrip1;
            this.notifyIcon1.Text = "notifyIcon1";
            this.notifyIcon1.Visible = true;
            this.notifyIcon1.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.notifyIcon1_MouseDoubleClick);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.显示ToolStripMenuItem,
            this.toolStripSeparator1,
            this.启动ToolStripMenuItem,
            this.停止ToolStripMenuItem,
            this.toolStripSeparator2,
            this.退出ToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(101, 104);
            // 
            // 显示ToolStripMenuItem
            // 
            this.显示ToolStripMenuItem.Name = "显示ToolStripMenuItem";
            this.显示ToolStripMenuItem.Size = new System.Drawing.Size(100, 22);
            this.显示ToolStripMenuItem.Text = "显示";
            this.显示ToolStripMenuItem.Click += new System.EventHandler(this.显示ToolStripMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(97, 6);
            // 
            // 启动ToolStripMenuItem
            // 
            this.启动ToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("启动ToolStripMenuItem.Image")));
            this.启动ToolStripMenuItem.Name = "启动ToolStripMenuItem";
            this.启动ToolStripMenuItem.Size = new System.Drawing.Size(100, 22);
            this.启动ToolStripMenuItem.Text = "启动";
            this.启动ToolStripMenuItem.Click += new System.EventHandler(this.启动ToolStripMenuItem_Click);
            // 
            // 停止ToolStripMenuItem
            // 
            this.停止ToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("停止ToolStripMenuItem.Image")));
            this.停止ToolStripMenuItem.Name = "停止ToolStripMenuItem";
            this.停止ToolStripMenuItem.Size = new System.Drawing.Size(100, 22);
            this.停止ToolStripMenuItem.Text = "停止";
            this.停止ToolStripMenuItem.Click += new System.EventHandler(this.停止ToolStripMenuItem_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(97, 6);
            // 
            // 退出ToolStripMenuItem
            // 
            this.退出ToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("退出ToolStripMenuItem.Image")));
            this.退出ToolStripMenuItem.Name = "退出ToolStripMenuItem";
            this.退出ToolStripMenuItem.Size = new System.Drawing.Size(100, 22);
            this.退出ToolStripMenuItem.Text = "退出";
            this.退出ToolStripMenuItem.Click += new System.EventHandler(this.退出ToolStripMenuItem_Click);
            // 
            // statusStrip1
            // 
            this.statusStrip1.AutoSize = false;
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1,
            this.lbStatus,
            this.toolStripStatusLabel3,
            this.lsServerUrl,
            this.toolStripStatusLabel2,
            this.lbClientCount,
            this.toolStripStatusLabel5,
            this.lbRunTime});
            this.statusStrip1.Location = new System.Drawing.Point(0, 548);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.RenderMode = System.Windows.Forms.ToolStripRenderMode.Professional;
            this.statusStrip1.Size = new System.Drawing.Size(935, 30);
            this.statusStrip1.SizingGrip = false;
            this.statusStrip1.TabIndex = 2;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Image = ((System.Drawing.Image)(resources.GetObject("toolStripStatusLabel1.Image")));
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(48, 25);
            this.toolStripStatusLabel1.Text = "状态";
            // 
            // lbStatus
            // 
            this.lbStatus.AutoSize = false;
            this.lbStatus.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left | System.Windows.Forms.ToolStripStatusLabelBorderSides.Top) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Right) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom)));
            this.lbStatus.ForeColor = System.Drawing.Color.Blue;
            this.lbStatus.Name = "lbStatus";
            this.lbStatus.Size = new System.Drawing.Size(100, 25);
            // 
            // toolStripStatusLabel3
            // 
            this.toolStripStatusLabel3.Image = ((System.Drawing.Image)(resources.GetObject("toolStripStatusLabel3.Image")));
            this.toolStripStatusLabel3.Name = "toolStripStatusLabel3";
            this.toolStripStatusLabel3.Size = new System.Drawing.Size(72, 25);
            this.toolStripStatusLabel3.Text = "服务地址";
            // 
            // lsServerUrl
            // 
            this.lsServerUrl.AutoSize = false;
            this.lsServerUrl.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left | System.Windows.Forms.ToolStripStatusLabelBorderSides.Top) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Right) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom)));
            this.lsServerUrl.ForeColor = System.Drawing.Color.Blue;
            this.lsServerUrl.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lsServerUrl.Name = "lsServerUrl";
            this.lsServerUrl.Size = new System.Drawing.Size(324, 25);
            this.lsServerUrl.Spring = true;
            // 
            // toolStripStatusLabel2
            // 
            this.toolStripStatusLabel2.Image = ((System.Drawing.Image)(resources.GetObject("toolStripStatusLabel2.Image")));
            this.toolStripStatusLabel2.Name = "toolStripStatusLabel2";
            this.toolStripStatusLabel2.Size = new System.Drawing.Size(84, 25);
            this.toolStripStatusLabel2.Text = "客户端数量";
            // 
            // lbClientCount
            // 
            this.lbClientCount.AutoSize = false;
            this.lbClientCount.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left | System.Windows.Forms.ToolStripStatusLabelBorderSides.Top) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Right) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom)));
            this.lbClientCount.ForeColor = System.Drawing.Color.Blue;
            this.lbClientCount.Name = "lbClientCount";
            this.lbClientCount.Size = new System.Drawing.Size(110, 25);
            // 
            // toolStripStatusLabel5
            // 
            this.toolStripStatusLabel5.Image = ((System.Drawing.Image)(resources.GetObject("toolStripStatusLabel5.Image")));
            this.toolStripStatusLabel5.Name = "toolStripStatusLabel5";
            this.toolStripStatusLabel5.Size = new System.Drawing.Size(72, 25);
            this.toolStripStatusLabel5.Text = "运行时间";
            // 
            // lbRunTime
            // 
            this.lbRunTime.AutoSize = false;
            this.lbRunTime.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left | System.Windows.Forms.ToolStripStatusLabelBorderSides.Top) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Right) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom)));
            this.lbRunTime.ForeColor = System.Drawing.Color.Blue;
            this.lbRunTime.Name = "lbRunTime";
            this.lbRunTime.Size = new System.Drawing.Size(110, 25);
            // 
            // timerRun
            // 
            this.timerRun.Interval = 1000;
            this.timerRun.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // timermsg
            // 
            this.timermsg.Tick += new System.EventHandler(this.timermsg_Tick);
            // 
            // FrmHosting
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(935, 578);
            this.Controls.Add(this.tabMain);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.statusStrip1);
            this.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.MinimizeBox = false;
            this.Name = "FrmHosting";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "CMDEP 云医疗数据交换平台";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FrmHosting_FormClosing);
            this.Load += new System.EventHandler(this.FrmHosting_Load);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.tabMain.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.contextMenuStrip2.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridClientList)).EndInit();
            this.tabPage3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridRouter)).EndInit();
            this.contextMenuStrip1.ResumeLayout(false);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.TabControl tabMain;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.DataGridView gridClientList;
        private System.Windows.Forms.ToolStripButton btnStart;
        private System.Windows.Forms.ToolStripButton btnStop;
        private System.Windows.Forms.NotifyIcon notifyIcon1;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem 显示ToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem 启动ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 停止ToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem 退出ToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripButton btnSetting;
        private System.Windows.Forms.ToolStripDropDownButton toolStripDropDownButton1;
        private System.Windows.Forms.ToolStripMenuItem 帮助ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 注册ToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.ToolStripMenuItem 关于ToolStripMenuItem;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.DataGridView gridRouter;
        private System.Windows.Forms.ToolStripButton btnplugin;
        private System.Windows.Forms.ListBox richTextMsg;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip2;
        private System.Windows.Forms.ToolStripMenuItem 清除日志ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 复制日志ToolStripMenuItem;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.ToolStripStatusLabel lbStatus;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel3;
        private System.Windows.Forms.ToolStripStatusLabel lsServerUrl;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel5;
        private System.Windows.Forms.ToolStripStatusLabel lbRunTime;
        private System.Windows.Forms.Timer timerRun;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel2;
        private System.Windows.Forms.ToolStripStatusLabel lbClientCount;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column5;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column7;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column6;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column8;
        private System.Windows.Forms.ToolStripButton btnrouter;
        private System.Windows.Forms.ToolStripButton btnInfo;
        private System.Windows.Forms.Timer timermsg;
        private System.Windows.Forms.ToolStripMenuItem 暂停日志ToolStripMenuItem;
        private System.Windows.Forms.ToolStripButton btnrestart;
        private System.Windows.Forms.ToolStripButton btntask;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column12;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column11;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column10;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column3;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column4;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column2;
        private System.Windows.Forms.DataGridViewCheckBoxColumn Column9;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column1;
        private System.Windows.Forms.ToolStripButton btnPublishService;
        private System.Windows.Forms.ToolStripButton btnSubscibe;
    }
}

