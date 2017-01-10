using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using EFWCoreLib.CoreFrame.Init;

namespace WCFHosting
{
    public partial class FrmSetting : Form
    {
        public bool isOk = false;
        public FrmSetting()
        {
            InitializeComponent();
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            HostSettingConfig.SetValue("hostname", txthostname.Text);
            HostSettingConfig.SetValue("debug", ckdebug.Checked ? "1" : "0");
            HostSettingConfig.SetValue("timingtask", cktask.Checked ? "1" : "0");
            HostSettingConfig.SetValue("wcfservice", ckwcf.Checked ? "1" : "0");
            HostSettingConfig.SetValue("router", ckrouter.Checked ? "1" : "0");
            HostSettingConfig.SetValue("filetransfer", ckfile.Checked ? "1" : "0");
            HostSettingConfig.SetValue("webapi", ckWebapi.Checked ? "1" : "0");
            HostSettingConfig.SetValue("mongodb", ckmongo.Checked ? "1" : "0");
            HostSettingConfig.SetValue("mongodb_binpath", txtmongobinpath.Text);
            HostSettingConfig.SetValue("mongodb_conn", txtmongodb_conn.Text);
            HostSettingConfig.SetValue("heartbeat", ckheartbeat.Checked ? "1" : "0");
            HostSettingConfig.SetValue("heartbeattime", txtheartbeattime.Text);
            HostSettingConfig.SetValue("message", ckmessage.Checked ? "1" : "0");
            HostSettingConfig.SetValue("messagetime", txtmessagetime.Text);
            HostSettingConfig.SetValue("compress", ckJsoncompress.Checked ? "1" : "0");
            HostSettingConfig.SetValue("encryption", ckEncryption.Checked ? "1" : "0");
            HostSettingConfig.SetValue("token", cktoken.Checked ? "1" : "0");
            HostSettingConfig.SetValue("overtime", ckovertime.Checked ? "1" : "0");
            HostSettingConfig.SetValue("overtimetime", txtovertime.Text);
            HostSettingConfig.SetValue("serializetype", cbSerializeType.SelectedIndex.ToString());
            HostSettingConfig.SetValue("nginx", ckNginx.Checked ? "1" : "0");
            HostSettingConfig.SaveConfig();


            HostAddressConfig.SetWcfAddress(txtwcf.Text);
            HostAddressConfig.SetFileAddress(txtfile.Text);
            HostAddressConfig.SetRouterAddress(txtrouter.Text);
            HostAddressConfig.SetfileRouterAddress(txtfilerouter.Text);
            HostAddressConfig.SetClientWcfAddress(txtwcfurl.Text);
            HostAddressConfig.SetClientFileAddress(txtfileurl.Text);
            HostAddressConfig.SetClientLocalAddress(txtlocalurl.Text);
            HostAddressConfig.SetWebapiAddress(txtweb.Text);
            HostAddressConfig.SetUpdaterUrl(txtupdate.Text);
            HostAddressConfig.SaveConfig();


            HostDataBaseConfig.SetConnString(txtconnstr.Text);
            HostDataBaseConfig.SaveConfig();

            HostMongoDBConfig.SetConfig(txtMongodb.Text);//保存mongodb配置文件

            isOk = true;
            MessageBox.Show("保存参数后，需重启程序才会生效！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void FrmSetting_Load(object sender, EventArgs e)
        {
            txthostname.Text = HostSettingConfig.GetValue("hostname");
            ckdebug.Checked = HostSettingConfig.GetValue("debug") == "1" ? true : false;
            cktask.Checked = HostSettingConfig.GetValue("timingtask") == "1" ? true : false;
            ckwcf.Checked = HostSettingConfig.GetValue("wcfservice") == "1" ? true : false;
            ckrouter.Checked = HostSettingConfig.GetValue("router") == "1" ? true : false;
            ckfile.Checked = HostSettingConfig.GetValue("filetransfer") == "1" ? true : false;
            ckWebapi.Checked = HostSettingConfig.GetValue("webapi") == "1" ? true : false;
            ckmongo.Checked= HostSettingConfig.GetValue("mongodb") == "1" ? true : false;
            ckheartbeat.Checked = HostSettingConfig.GetValue("heartbeat") == "1" ? true : false;
            txtheartbeattime.Text = HostSettingConfig.GetValue("heartbeattime");
            ckmessage.Checked = HostSettingConfig.GetValue("message") == "1" ? true : false;
            txtmessagetime.Text = HostSettingConfig.GetValue("messagetime");
            ckJsoncompress.Checked = HostSettingConfig.GetValue("compress") == "1" ? true : false;
            ckEncryption.Checked = HostSettingConfig.GetValue("encryption") == "1" ? true : false;
            cktoken.Checked = HostSettingConfig.GetValue("token") == "1" ? true : false;
            ckovertime.Checked = HostSettingConfig.GetValue("overtime") == "1" ? true : false;
            txtovertime.Text = HostSettingConfig.GetValue("overtimetime");
            cbSerializeType.SelectedIndex = Convert.ToInt32(HostSettingConfig.GetValue("serializetype"));
            ckNginx.Checked = HostSettingConfig.GetValue("nginx") == "1" ? true : false;

            txtwcf.Text = HostAddressConfig.GetWcfAddress();
            txtfile.Text = HostAddressConfig.GetFileAddress();
            txtrouter.Text = HostAddressConfig.GetRouterAddress();
            txtfilerouter.Text = HostAddressConfig.GetfileRouterAddress();
            txtwcfurl.Text = HostAddressConfig.GetClientWcfAddress();
            txtfileurl.Text = HostAddressConfig.GetClientFileAddress();
            txtlocalurl.Text = HostAddressConfig.GetClientLocalAddress();
            txtweb.Text = HostAddressConfig.GetWebapiAddress();
            txtupdate.Text = HostAddressConfig.GetUpdaterUrl();

            txtconnstr.Text = HostDataBaseConfig.GetConnString();
            txtmongobinpath.Text = HostSettingConfig.GetValue("mongodb_binpath");
            txtmongodb_conn.Text= HostSettingConfig.GetValue("mongodb_conn");
            txtMongodb.Text = HostMongoDBConfig.GetConfig();
        }

        private void btnSvcConfig_Click(object sender, EventArgs e)
        {
            string appconfig = AppGlobal.AppRootPath + "efwplusServer.exe.config";
            string svcconfigExe = AppGlobal.AppRootPath + "SvcConfigEditor.exe";

            System.Diagnostics.Process.Start(svcconfigExe,appconfig);
        }
    }
}
