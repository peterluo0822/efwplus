namespace WCFHosting
{
    partial class FrmSetting
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmSetting));
            this.ckdebug = new System.Windows.Forms.CheckBox();
            this.btnOk = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.ckrouter = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.txthostname = new System.Windows.Forms.TextBox();
            this.ckwcf = new System.Windows.Forms.CheckBox();
            this.ckheartbeat = new System.Windows.Forms.CheckBox();
            this.ckJsoncompress = new System.Windows.Forms.CheckBox();
            this.ckEncryption = new System.Windows.Forms.CheckBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtheartbeattime = new System.Windows.Forms.TextBox();
            this.txtmessagetime = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.ckmessage = new System.Windows.Forms.CheckBox();
            this.txtovertime = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.ckovertime = new System.Windows.Forms.CheckBox();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.ckNginx = new System.Windows.Forms.CheckBox();
            this.ckmongo = new System.Windows.Forms.CheckBox();
            this.cktask = new System.Windows.Forms.CheckBox();
            this.cktoken = new System.Windows.Forms.CheckBox();
            this.cbSerializeType = new System.Windows.Forms.ComboBox();
            this.label13 = new System.Windows.Forms.Label();
            this.ckWebapi = new System.Windows.Forms.CheckBox();
            this.ckfile = new System.Windows.Forms.CheckBox();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.txtfilerouter = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.txtweb = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.txtfile = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.txtrouter = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.txtwcf = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.txtconnstr = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.tabPage5 = new System.Windows.Forms.TabPage();
            this.txtmongodb_conn = new System.Windows.Forms.TextBox();
            this.label17 = new System.Windows.Forms.Label();
            this.txtmongobinpath = new System.Windows.Forms.TextBox();
            this.label16 = new System.Windows.Forms.Label();
            this.txtMongodb = new System.Windows.Forms.TextBox();
            this.label15 = new System.Windows.Forms.Label();
            this.tabPage4 = new System.Windows.Forms.TabPage();
            this.txtlocalurl = new System.Windows.Forms.TextBox();
            this.label14 = new System.Windows.Forms.Label();
            this.txtfileurl = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.txtwcfurl = new System.Windows.Forms.TextBox();
            this.label12 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnSvcConfig = new System.Windows.Forms.Button();
            this.txtupdate = new System.Windows.Forms.TextBox();
            this.label18 = new System.Windows.Forms.Label();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.tabPage5.SuspendLayout();
            this.tabPage4.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // ckdebug
            // 
            this.ckdebug.AutoSize = true;
            this.ckdebug.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.ckdebug.Location = new System.Drawing.Point(87, 44);
            this.ckdebug.Name = "ckdebug";
            this.ckdebug.Size = new System.Drawing.Size(99, 21);
            this.ckdebug.TabIndex = 0;
            this.ckdebug.Text = "显示调试信息";
            this.ckdebug.UseVisualStyleBackColor = true;
            // 
            // btnOk
            // 
            this.btnOk.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnOk.Image = ((System.Drawing.Image)(resources.GetObject("btnOk.Image")));
            this.btnOk.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnOk.Location = new System.Drawing.Point(251, 21);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(75, 28);
            this.btnOk.TabIndex = 1;
            this.btnOk.Text = "确定";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnCancel.Location = new System.Drawing.Point(343, 21);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 28);
            this.btnCancel.TabIndex = 2;
            this.btnCancel.Text = "取消";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // ckrouter
            // 
            this.ckrouter.AutoSize = true;
            this.ckrouter.Enabled = false;
            this.ckrouter.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.ckrouter.Location = new System.Drawing.Point(238, 66);
            this.ckrouter.Name = "ckrouter";
            this.ckrouter.Size = new System.Drawing.Size(99, 21);
            this.ckrouter.TabIndex = 3;
            this.ckrouter.Text = "开启路由服务";
            this.ckrouter.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.Location = new System.Drawing.Point(6, 14);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(80, 17);
            this.label1.TabIndex = 4;
            this.label1.Text = "中间件名称：";
            // 
            // txthostname
            // 
            this.txthostname.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txthostname.ForeColor = System.Drawing.Color.Blue;
            this.txthostname.Location = new System.Drawing.Point(87, 11);
            this.txthostname.Name = "txthostname";
            this.txthostname.Size = new System.Drawing.Size(322, 23);
            this.txthostname.TabIndex = 5;
            // 
            // ckwcf
            // 
            this.ckwcf.AutoSize = true;
            this.ckwcf.Enabled = false;
            this.ckwcf.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.ckwcf.Location = new System.Drawing.Point(87, 66);
            this.ckwcf.Name = "ckwcf";
            this.ckwcf.Size = new System.Drawing.Size(99, 21);
            this.ckwcf.TabIndex = 6;
            this.ckwcf.Text = "开启基础服务";
            this.ckwcf.UseVisualStyleBackColor = true;
            // 
            // ckheartbeat
            // 
            this.ckheartbeat.AutoSize = true;
            this.ckheartbeat.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.ckheartbeat.Location = new System.Drawing.Point(87, 139);
            this.ckheartbeat.Name = "ckheartbeat";
            this.ckheartbeat.Size = new System.Drawing.Size(123, 21);
            this.ckheartbeat.TabIndex = 7;
            this.ckheartbeat.Text = "开启心跳检测功能";
            this.ckheartbeat.UseVisualStyleBackColor = true;
            // 
            // ckJsoncompress
            // 
            this.ckJsoncompress.AutoSize = true;
            this.ckJsoncompress.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.ckJsoncompress.Location = new System.Drawing.Point(87, 205);
            this.ckJsoncompress.Name = "ckJsoncompress";
            this.ckJsoncompress.Size = new System.Drawing.Size(99, 21);
            this.ckJsoncompress.TabIndex = 8;
            this.ckJsoncompress.Text = "开启数据压缩";
            this.ckJsoncompress.UseVisualStyleBackColor = true;
            // 
            // ckEncryption
            // 
            this.ckEncryption.AutoSize = true;
            this.ckEncryption.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.ckEncryption.Location = new System.Drawing.Point(87, 227);
            this.ckEncryption.Name = "ckEncryption";
            this.ckEncryption.Size = new System.Drawing.Size(99, 21);
            this.ckEncryption.TabIndex = 9;
            this.ckEncryption.Text = "开启数据加密";
            this.ckEncryption.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label2.Location = new System.Drawing.Point(235, 138);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(76, 17);
            this.label2.TabIndex = 10;
            this.label2.Text = "间隔时间(秒)";
            // 
            // txtheartbeattime
            // 
            this.txtheartbeattime.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtheartbeattime.Location = new System.Drawing.Point(315, 138);
            this.txtheartbeattime.Name = "txtheartbeattime";
            this.txtheartbeattime.Size = new System.Drawing.Size(56, 23);
            this.txtheartbeattime.TabIndex = 11;
            this.txtheartbeattime.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // txtmessagetime
            // 
            this.txtmessagetime.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtmessagetime.Location = new System.Drawing.Point(315, 162);
            this.txtmessagetime.Name = "txtmessagetime";
            this.txtmessagetime.Size = new System.Drawing.Size(56, 23);
            this.txtmessagetime.TabIndex = 14;
            this.txtmessagetime.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label3.Location = new System.Drawing.Point(235, 162);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(76, 17);
            this.label3.TabIndex = 13;
            this.label3.Text = "间隔时间(秒)";
            // 
            // ckmessage
            // 
            this.ckmessage.AutoSize = true;
            this.ckmessage.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.ckmessage.Location = new System.Drawing.Point(87, 161);
            this.ckmessage.Name = "ckmessage";
            this.ckmessage.Size = new System.Drawing.Size(99, 21);
            this.ckmessage.TabIndex = 12;
            this.ckmessage.Text = "开启消息发送";
            this.ckmessage.UseVisualStyleBackColor = true;
            // 
            // txtovertime
            // 
            this.txtovertime.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtovertime.Location = new System.Drawing.Point(315, 186);
            this.txtovertime.Name = "txtovertime";
            this.txtovertime.Size = new System.Drawing.Size(56, 23);
            this.txtovertime.TabIndex = 17;
            this.txtovertime.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label4.Location = new System.Drawing.Point(235, 187);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(76, 17);
            this.label4.TabIndex = 16;
            this.label4.Text = "超过时间(秒)";
            // 
            // ckovertime
            // 
            this.ckovertime.AutoSize = true;
            this.ckovertime.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.ckovertime.Location = new System.Drawing.Point(87, 183);
            this.ckovertime.Name = "ckovertime";
            this.ckovertime.Size = new System.Drawing.Size(147, 21);
            this.ckovertime.TabIndex = 15;
            this.ckovertime.Text = "开启日志记录耗时方法";
            this.ckovertime.UseVisualStyleBackColor = true;
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Controls.Add(this.tabPage3);
            this.tabControl1.Controls.Add(this.tabPage5);
            this.tabControl1.Controls.Add(this.tabPage4);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(430, 314);
            this.tabControl1.TabIndex = 18;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.ckNginx);
            this.tabPage1.Controls.Add(this.ckmongo);
            this.tabPage1.Controls.Add(this.cktask);
            this.tabPage1.Controls.Add(this.cktoken);
            this.tabPage1.Controls.Add(this.cbSerializeType);
            this.tabPage1.Controls.Add(this.label13);
            this.tabPage1.Controls.Add(this.ckWebapi);
            this.tabPage1.Controls.Add(this.ckfile);
            this.tabPage1.Controls.Add(this.ckdebug);
            this.tabPage1.Controls.Add(this.txtovertime);
            this.tabPage1.Controls.Add(this.ckrouter);
            this.tabPage1.Controls.Add(this.label4);
            this.tabPage1.Controls.Add(this.label1);
            this.tabPage1.Controls.Add(this.ckovertime);
            this.tabPage1.Controls.Add(this.txthostname);
            this.tabPage1.Controls.Add(this.txtmessagetime);
            this.tabPage1.Controls.Add(this.ckwcf);
            this.tabPage1.Controls.Add(this.label3);
            this.tabPage1.Controls.Add(this.ckheartbeat);
            this.tabPage1.Controls.Add(this.ckmessage);
            this.tabPage1.Controls.Add(this.ckJsoncompress);
            this.tabPage1.Controls.Add(this.txtheartbeattime);
            this.tabPage1.Controls.Add(this.ckEncryption);
            this.tabPage1.Controls.Add(this.label2);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(422, 288);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "基本参数";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // ckNginx
            // 
            this.ckNginx.AutoSize = true;
            this.ckNginx.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.ckNginx.Location = new System.Drawing.Point(87, 110);
            this.ckNginx.Name = "ckNginx";
            this.ckNginx.Size = new System.Drawing.Size(85, 21);
            this.ckNginx.TabIndex = 25;
            this.ckNginx.Text = "开启Nginx";
            this.ckNginx.UseVisualStyleBackColor = true;
            // 
            // ckmongo
            // 
            this.ckmongo.AutoSize = true;
            this.ckmongo.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.ckmongo.Location = new System.Drawing.Point(238, 110);
            this.ckmongo.Name = "ckmongo";
            this.ckmongo.Size = new System.Drawing.Size(111, 21);
            this.ckmongo.TabIndex = 24;
            this.ckmongo.Text = "开启MongoDB";
            this.ckmongo.UseVisualStyleBackColor = true;
            // 
            // cktask
            // 
            this.cktask.AutoSize = true;
            this.cktask.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.cktask.Location = new System.Drawing.Point(238, 44);
            this.cktask.Name = "cktask";
            this.cktask.Size = new System.Drawing.Size(99, 21);
            this.cktask.TabIndex = 23;
            this.cktask.Text = "开启定时任务";
            this.cktask.UseVisualStyleBackColor = true;
            // 
            // cktoken
            // 
            this.cktoken.AutoSize = true;
            this.cktoken.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.cktoken.Location = new System.Drawing.Point(238, 227);
            this.cktoken.Name = "cktoken";
            this.cktoken.Size = new System.Drawing.Size(99, 21);
            this.cktoken.TabIndex = 22;
            this.cktoken.Text = "开启身份验证";
            this.cktoken.UseVisualStyleBackColor = true;
            // 
            // cbSerializeType
            // 
            this.cbSerializeType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbSerializeType.FormattingEnabled = true;
            this.cbSerializeType.Items.AddRange(new object[] {
            "Newtonsoft",
            "protobuf",
            "fastJSON"});
            this.cbSerializeType.Location = new System.Drawing.Point(238, 254);
            this.cbSerializeType.Name = "cbSerializeType";
            this.cbSerializeType.Size = new System.Drawing.Size(133, 20);
            this.cbSerializeType.TabIndex = 21;
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label13.Location = new System.Drawing.Point(84, 254);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(92, 17);
            this.label13.TabIndex = 20;
            this.label13.Text = "数据序列化策略";
            // 
            // ckWebapi
            // 
            this.ckWebapi.AutoSize = true;
            this.ckWebapi.Enabled = false;
            this.ckWebapi.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.ckWebapi.Location = new System.Drawing.Point(238, 88);
            this.ckWebapi.Name = "ckWebapi";
            this.ckWebapi.Size = new System.Drawing.Size(121, 21);
            this.ckWebapi.TabIndex = 19;
            this.ckWebapi.Text = "开启WebAPI服务";
            this.ckWebapi.UseVisualStyleBackColor = true;
            // 
            // ckfile
            // 
            this.ckfile.AutoSize = true;
            this.ckfile.Enabled = false;
            this.ckfile.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.ckfile.Location = new System.Drawing.Point(87, 88);
            this.ckfile.Name = "ckfile";
            this.ckfile.Size = new System.Drawing.Size(123, 21);
            this.ckfile.TabIndex = 18;
            this.ckfile.Text = "开启文件传输服务";
            this.ckfile.UseVisualStyleBackColor = true;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.txtfilerouter);
            this.tabPage2.Controls.Add(this.label10);
            this.tabPage2.Controls.Add(this.txtweb);
            this.tabPage2.Controls.Add(this.label8);
            this.tabPage2.Controls.Add(this.txtfile);
            this.tabPage2.Controls.Add(this.label7);
            this.tabPage2.Controls.Add(this.txtrouter);
            this.tabPage2.Controls.Add(this.label6);
            this.tabPage2.Controls.Add(this.txtwcf);
            this.tabPage2.Controls.Add(this.label5);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(422, 288);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "发布通讯地址";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // txtfilerouter
            // 
            this.txtfilerouter.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtfilerouter.ForeColor = System.Drawing.Color.Blue;
            this.txtfilerouter.Location = new System.Drawing.Point(10, 170);
            this.txtfilerouter.Name = "txtfilerouter";
            this.txtfilerouter.Size = new System.Drawing.Size(404, 23);
            this.txtfilerouter.TabIndex = 9;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label10.Location = new System.Drawing.Point(8, 152);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(116, 17);
            this.label10.TabIndex = 8;
            this.label10.Text = "路由文件服务地址：";
            // 
            // txtweb
            // 
            this.txtweb.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtweb.ForeColor = System.Drawing.Color.Blue;
            this.txtweb.Location = new System.Drawing.Point(9, 214);
            this.txtweb.Name = "txtweb";
            this.txtweb.Size = new System.Drawing.Size(404, 23);
            this.txtweb.TabIndex = 7;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label8.Location = new System.Drawing.Point(7, 196);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(114, 17);
            this.label8.TabIndex = 6;
            this.label8.Text = "WebAPI服务地址：";
            // 
            // txtfile
            // 
            this.txtfile.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtfile.ForeColor = System.Drawing.Color.Blue;
            this.txtfile.Location = new System.Drawing.Point(10, 82);
            this.txtfile.Name = "txtfile";
            this.txtfile.Size = new System.Drawing.Size(404, 23);
            this.txtfile.TabIndex = 5;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label7.Location = new System.Drawing.Point(8, 64);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(116, 17);
            this.label7.TabIndex = 4;
            this.label7.Text = "文件传输服务地址：";
            // 
            // txtrouter
            // 
            this.txtrouter.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtrouter.ForeColor = System.Drawing.Color.Blue;
            this.txtrouter.Location = new System.Drawing.Point(10, 126);
            this.txtrouter.Name = "txtrouter";
            this.txtrouter.Size = new System.Drawing.Size(404, 23);
            this.txtrouter.TabIndex = 3;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label6.Location = new System.Drawing.Point(8, 108);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(116, 17);
            this.label6.TabIndex = 2;
            this.label6.Text = "路由基础服务地址：";
            // 
            // txtwcf
            // 
            this.txtwcf.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtwcf.ForeColor = System.Drawing.Color.Blue;
            this.txtwcf.Location = new System.Drawing.Point(10, 38);
            this.txtwcf.Name = "txtwcf";
            this.txtwcf.Size = new System.Drawing.Size(404, 23);
            this.txtwcf.TabIndex = 1;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label5.Location = new System.Drawing.Point(8, 20);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(116, 17);
            this.label5.TabIndex = 0;
            this.label5.Text = "基础数据服务地址：";
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.txtconnstr);
            this.tabPage3.Controls.Add(this.label9);
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Size = new System.Drawing.Size(422, 288);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "数据库连接";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // txtconnstr
            // 
            this.txtconnstr.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtconnstr.ForeColor = System.Drawing.Color.Blue;
            this.txtconnstr.Location = new System.Drawing.Point(9, 36);
            this.txtconnstr.Multiline = true;
            this.txtconnstr.Name = "txtconnstr";
            this.txtconnstr.Size = new System.Drawing.Size(404, 235);
            this.txtconnstr.TabIndex = 3;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label9.Location = new System.Drawing.Point(7, 18);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(80, 17);
            this.label9.TabIndex = 2;
            this.label9.Text = "连接字符串：";
            // 
            // tabPage5
            // 
            this.tabPage5.Controls.Add(this.txtmongodb_conn);
            this.tabPage5.Controls.Add(this.label17);
            this.tabPage5.Controls.Add(this.txtmongobinpath);
            this.tabPage5.Controls.Add(this.label16);
            this.tabPage5.Controls.Add(this.txtMongodb);
            this.tabPage5.Controls.Add(this.label15);
            this.tabPage5.Location = new System.Drawing.Point(4, 22);
            this.tabPage5.Name = "tabPage5";
            this.tabPage5.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage5.Size = new System.Drawing.Size(422, 288);
            this.tabPage5.TabIndex = 4;
            this.tabPage5.Text = "MongoDB配置";
            this.tabPage5.UseVisualStyleBackColor = true;
            // 
            // txtmongodb_conn
            // 
            this.txtmongodb_conn.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtmongodb_conn.ForeColor = System.Drawing.Color.Blue;
            this.txtmongodb_conn.Location = new System.Drawing.Point(8, 258);
            this.txtmongodb_conn.Name = "txtmongodb_conn";
            this.txtmongodb_conn.Size = new System.Drawing.Size(404, 23);
            this.txtmongodb_conn.TabIndex = 9;
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label17.Location = new System.Drawing.Point(5, 238);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(140, 17);
            this.label17.TabIndex = 8;
            this.label17.Text = "MongoDB连接字符串：";
            // 
            // txtmongobinpath
            // 
            this.txtmongobinpath.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtmongobinpath.ForeColor = System.Drawing.Color.Blue;
            this.txtmongobinpath.Location = new System.Drawing.Point(8, 39);
            this.txtmongobinpath.Name = "txtmongobinpath";
            this.txtmongobinpath.Size = new System.Drawing.Size(404, 23);
            this.txtmongobinpath.TabIndex = 7;
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label16.Location = new System.Drawing.Point(5, 19);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(128, 17);
            this.label16.TabIndex = 6;
            this.label16.Text = "MongoDB程序目录：";
            // 
            // txtMongodb
            // 
            this.txtMongodb.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtMongodb.ForeColor = System.Drawing.Color.Blue;
            this.txtMongodb.Location = new System.Drawing.Point(9, 85);
            this.txtMongodb.Multiline = true;
            this.txtMongodb.Name = "txtMongodb";
            this.txtMongodb.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtMongodb.Size = new System.Drawing.Size(404, 150);
            this.txtMongodb.TabIndex = 5;
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label15.Location = new System.Drawing.Point(6, 65);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(206, 17);
            this.label15.TabIndex = 4;
            this.label15.Text = "启动数据库配置文件(mongo.conf)：";
            // 
            // tabPage4
            // 
            this.tabPage4.Controls.Add(this.txtupdate);
            this.tabPage4.Controls.Add(this.label18);
            this.tabPage4.Controls.Add(this.txtlocalurl);
            this.tabPage4.Controls.Add(this.label14);
            this.tabPage4.Controls.Add(this.txtfileurl);
            this.tabPage4.Controls.Add(this.label11);
            this.tabPage4.Controls.Add(this.txtwcfurl);
            this.tabPage4.Controls.Add(this.label12);
            this.tabPage4.Location = new System.Drawing.Point(4, 22);
            this.tabPage4.Name = "tabPage4";
            this.tabPage4.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage4.Size = new System.Drawing.Size(422, 288);
            this.tabPage4.TabIndex = 3;
            this.tabPage4.Text = "与上级通讯地址";
            this.tabPage4.UseVisualStyleBackColor = true;
            // 
            // txtlocalurl
            // 
            this.txtlocalurl.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtlocalurl.Location = new System.Drawing.Point(9, 141);
            this.txtlocalurl.Name = "txtlocalurl";
            this.txtlocalurl.Size = new System.Drawing.Size(405, 23);
            this.txtlocalurl.TabIndex = 10;
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label14.Location = new System.Drawing.Point(6, 121);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(152, 17);
            this.label14.TabIndex = 9;
            this.label14.Text = "本地中间件业务请求地址：";
            // 
            // txtfileurl
            // 
            this.txtfileurl.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtfileurl.Location = new System.Drawing.Point(9, 92);
            this.txtfileurl.Name = "txtfileurl";
            this.txtfileurl.Size = new System.Drawing.Size(405, 23);
            this.txtfileurl.TabIndex = 8;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label11.Location = new System.Drawing.Point(6, 72);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(152, 17);
            this.label11.TabIndex = 7;
            this.label11.Text = "上级中间件文件传输地址：";
            // 
            // txtwcfurl
            // 
            this.txtwcfurl.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtwcfurl.Location = new System.Drawing.Point(9, 40);
            this.txtwcfurl.Name = "txtwcfurl";
            this.txtwcfurl.Size = new System.Drawing.Size(405, 23);
            this.txtwcfurl.TabIndex = 6;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label12.Location = new System.Drawing.Point(6, 20);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(152, 17);
            this.label12.TabIndex = 5;
            this.label12.Text = "上级中间件业务请求地址：";
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.White;
            this.panel1.Controls.Add(this.btnSvcConfig);
            this.panel1.Controls.Add(this.btnOk);
            this.panel1.Controls.Add(this.btnCancel);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 314);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(430, 61);
            this.panel1.TabIndex = 19;
            // 
            // btnSvcConfig
            // 
            this.btnSvcConfig.Location = new System.Drawing.Point(15, 21);
            this.btnSvcConfig.Name = "btnSvcConfig";
            this.btnSvcConfig.Size = new System.Drawing.Size(90, 28);
            this.btnSvcConfig.TabIndex = 3;
            this.btnSvcConfig.Text = "WCF服务配置";
            this.btnSvcConfig.UseVisualStyleBackColor = true;
            this.btnSvcConfig.Click += new System.EventHandler(this.btnSvcConfig_Click);
            // 
            // txtupdate
            // 
            this.txtupdate.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtupdate.Location = new System.Drawing.Point(9, 187);
            this.txtupdate.Name = "txtupdate";
            this.txtupdate.Size = new System.Drawing.Size(405, 23);
            this.txtupdate.TabIndex = 12;
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label18.Location = new System.Drawing.Point(6, 167);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(104, 17);
            this.label18.TabIndex = 11;
            this.label18.Text = "中间件升级地址：";
            // 
            // FrmSetting
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(430, 375);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmSetting";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "系统设置";
            this.Load += new System.EventHandler(this.FrmSetting_Load);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            this.tabPage3.ResumeLayout(false);
            this.tabPage3.PerformLayout();
            this.tabPage5.ResumeLayout(false);
            this.tabPage5.PerformLayout();
            this.tabPage4.ResumeLayout(false);
            this.tabPage4.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.CheckBox ckdebug;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.CheckBox ckrouter;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txthostname;
        private System.Windows.Forms.CheckBox ckwcf;
        private System.Windows.Forms.CheckBox ckheartbeat;
        private System.Windows.Forms.CheckBox ckJsoncompress;
        private System.Windows.Forms.CheckBox ckEncryption;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtheartbeattime;
        private System.Windows.Forms.TextBox txtmessagetime;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.CheckBox ckmessage;
        private System.Windows.Forms.TextBox txtovertime;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.CheckBox ckovertime;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.TextBox txtwcf;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtweb;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox txtfile;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox txtrouter;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txtconnstr;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.CheckBox ckfile;
        private System.Windows.Forms.CheckBox ckWebapi;
        private System.Windows.Forms.TextBox txtfilerouter;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TabPage tabPage4;
        private System.Windows.Forms.TextBox txtfileurl;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.TextBox txtwcfurl;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.ComboBox cbSerializeType;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.CheckBox cktoken;
        private System.Windows.Forms.CheckBox cktask;
        private System.Windows.Forms.TextBox txtlocalurl;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Button btnSvcConfig;
        private System.Windows.Forms.TabPage tabPage5;
        private System.Windows.Forms.TextBox txtMongodb;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.TextBox txtmongobinpath;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.CheckBox ckmongo;
        private System.Windows.Forms.TextBox txtmongodb_conn;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.CheckBox ckNginx;
        private System.Windows.Forms.TextBox txtupdate;
        private System.Windows.Forms.Label label18;
    }
}