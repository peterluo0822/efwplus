using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using EFWCoreLib.CoreFrame.Common;
using EFWCoreLib.WcfFrame;
using EFWCoreLib.WcfFrame.ServerController;
using EFWCoreLib.WcfFrame.ServerManage;
using EFWCoreLib.WebApiFrame;
using WCFHosting.RouterManage;
using WCFHosting.TimingTask;

namespace WCFHosting
{
    public partial class FrmHosting : Form
    {
        long timeCount = 0;//运行次数
        string identify;
        string expireDate;
        Queue<msgobject> msgList=null;
        //List<ClientInfo> clientdic = null;
        //List<RegistrationInfo> routerdic = null;

        HostState RunState
        {
            set
            {
                if (value == HostState.NoOpen)
                {
                    btnStart.Enabled = true;
                    btnStop.Enabled = false;
                    启动ToolStripMenuItem.Enabled = true;
                    停止ToolStripMenuItem.Enabled = false;

                    lbStatus.Text = "服务未启动";
                    timerRun.Enabled = false;
                    //timermsg.Enabled = false;
                    暂停日志ToolStripMenuItem.Text = "开启日志";
                }
                else
                {
                    btnStart.Enabled = false;
                    btnStop.Enabled = true;
                    启动ToolStripMenuItem.Enabled = false;
                    停止ToolStripMenuItem.Enabled = true;

                    lbStatus.Text = "服务已运行";
                    timeCount = 0;
                    timerRun.Enabled = true;
                    //timermsg.Enabled = true;
                    暂停日志ToolStripMenuItem.Text = "暂停日志";
                }
            }
        }

        public FrmHosting()
        {
            InitializeComponent();
            msgList = new Queue<msgobject>();
        }

        private void StartAllHost()
        {
            if (Convert.ToInt32(HostSettingConfig.GetValue("wcfservice")) == 1)
            {
                ClientManage.clientinfoList = new ClientInfoListHandler(BindGridClient);
                WcfGlobal.Identify = identify;
                WcfGlobal.Main(StartType.BaseService);
            }
            if (Convert.ToInt32(HostSettingConfig.GetValue("filetransfer")) == 1)
            {
                WcfGlobal.Main(StartType.FileService);
            }
            if (Convert.ToInt32(HostSettingConfig.GetValue("router")) == 1)
            {
                EFWCoreLib.WcfFrame.ServerManage.RouterManage.hostwcfRouter = new HostWCFRouterListHandler(BindGridRouter);
                WcfGlobal.Main(StartType.RouterBaseService);
                WcfGlobal.Main(StartType.RouterFileService);
            }
            if (Convert.ToInt32(HostSettingConfig.GetValue("webapi")) == 1)
            {
                WebApiGlobal.Main();
            }
            if (Convert.ToInt32(HostSettingConfig.GetValue("timingtask")) == 1)
                WcfGlobal.Main(StartType.MiddlewareTask);

            WcfGlobal.Main(StartType.SuperClient);
            RunState = HostState.Opened;
        }

        private void StopAllHost()
        {
            MiddlewareLogHelper.WriterLog(LogType.MidLog, true, Color.Red, "正在准备关闭中间件服务，请等待...");

            WcfGlobal.Exit(StartType.MiddlewareTask);
            WcfGlobal.Exit(StartType.SuperClient);
            WcfGlobal.Exit(StartType.BaseService);
            WcfGlobal.Exit(StartType.FileService);
            WcfGlobal.Exit(StartType.RouterBaseService);
            WcfGlobal.Exit(StartType.RouterFileService);
            WebApiGlobal.Exit();
    
            RunState = HostState.NoOpen;
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            timermsg.Start();
            MiddlewareLogHelper.hostwcfMsg = new MiddlewareMsgHandler(AddMsg);
            MiddlewareLogHelper.StartWriteFileLog();//开放中间件日志
            int res = TimeCDKEY.InitRegedit(out expireDate,out identify);
            if (res == 0)
            {
                MiddlewareLogHelper.WriterLog(LogType.MidLog, true, Color.Green, "软件已注册，到期时间【" + expireDate + "】");
                StartAllHost();
            }
            else if (res == 1)
            {
                MiddlewareLogHelper.WriterLog(LogType.MidLog, true, Color.Red, "软件尚未注册，请注册软件");
            }
            else if (res == 2)
            {
                MiddlewareLogHelper.WriterLog(LogType.MidLog, true, Color.Red, "注册机器与本机不一致,请联系管理员");
            }
            else if (res == 3)
            {
                MiddlewareLogHelper.WriterLog(LogType.MidLog, true, Color.Red, "软件试用已到期");
            }
            else
            {
                MiddlewareLogHelper.WriterLog(LogType.MidLog, true, Color.Red, "软件运行出错，请重新启动");
            }
        }
        private void btnStop_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("您确定要停止服务吗？", "询问窗", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
            {
                StopAllHost();
            }
        }

        private void FrmHosting_Load(object sender, EventArgs e)
        {
            this.Text = "CMDEP 云医疗数据交换平台【" + HostSettingConfig.GetValue("hostname") + "】";
            this.notifyIcon1.Visible = true;
            this.notifyIcon1.Icon = this.Icon;
            this.notifyIcon1.Text = this.Text;

            RunState = HostState.NoOpen;
            lsServerUrl.Text = ReadConfig.GetWcfServerUrl();
            btnStart_Click(null, null);//打开服务主机后自动启动服务
        }

        public delegate void textInvoke(Color clr, string msg);
        public delegate void gridInvoke(DataGridView grid, object data);
        private void settext(Color clr, string msg)
        {
            if (richTextMsg.InvokeRequired)
            {
                textInvoke ti = new textInvoke(settext);
                this.BeginInvoke(ti, new object[] { clr, msg });
            }
            else
            {
                ListViewItem lstItem = new ListViewItem(msg);
                lstItem.ForeColor = clr;
                if (richTextMsg.Items.Count > 1000)
                    richTextMsg.Items.Clear();
                richTextMsg.Items.Add(lstItem);
                richTextMsg.SelectedIndex = richTextMsg.Items.Count - 1;
            }
        }
        private void setgrid(DataGridView grid, object data)
        {
            if (grid.InvokeRequired)
            {
                gridInvoke gi = new gridInvoke(setgrid);
                this.BeginInvoke(gi, new object[] { grid, data });
            }
            else
            {
                grid.AutoGenerateColumns = false;
                grid.DataSource = data;
                grid.Refresh();
            }
        }

        private void BindGridClient(List<ClientInfo> dic)
        {
            setgrid(gridClientList, dic);
        }
        private void AddMsg(Color clr, DateTime time, string msg)
        {
            msg = msg.Length > 10000 ? msg.Substring(0, 10000) : msg;
            msgobject msgo = new msgobject(clr,time,msg);
            msgList.Enqueue(msgo);
            //settext(clr,"[" + time.ToString("yyyy-MM-dd HH:mm:ss") + "] : " + msg);
        }
        private void BindGridRouter(List<RegistrationInfo> dic)
        {
            setgrid(gridRouter, dic);
        }

        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            this.Show();
        }

        private void 显示ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Show();
        }

        private void 启动ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            btnStart_Click(null, null);
        }

        private void 停止ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            btnStop_Click(null, null);
        }

        private void 退出ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("您确定要退出中间件服务器吗？", "询问窗", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
            {
                try
                {
                    StopAllHost();
                }
                catch { }
                this.Dispose(true);
            }
        }

        private void FrmHosting_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            this.Hide();
        }

        private void btnSetting_Click(object sender, EventArgs e)
        {
            FrmSetting set = new FrmSetting();
            set.ShowDialog();
            if (set.isOk == true)
            {
                this.Text = "CMDEP 云医疗数据交换平台【" + HostSettingConfig.GetValue("hostname") + "】";
                this.notifyIcon1.Text = this.Text;
            }
        }

        private void 关于ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new AboutBox().ShowDialog();
        }

        private void btnplugin_Click(object sender, EventArgs e)
        {
            FrmPlugin plugin = new FrmPlugin();
            plugin.ShowDialog();
        }

        private void 清除日志ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            msgList.Clear();
            richTextMsg.Items.Clear();
        }

        private void 复制日志ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (richTextMsg.SelectedItem == null)
                return;
            StringBuilder strMessage = new StringBuilder();
            for (int i = 0; i < richTextMsg.Items.Count; i++)
            {
                if (richTextMsg.GetSelected(i))
                    strMessage.Append(richTextMsg.SelectedItem.ToString());
            }

            Clipboard.SetDataObject(strMessage.ToString());
        }

        private void 暂停日志ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (timermsg.Enabled)
            {
                timermsg.Enabled = false;
                暂停日志ToolStripMenuItem.Text = "开启日志";
            }
            else
            {
                timermsg.Enabled = true;
                暂停日志ToolStripMenuItem.Text = "暂停日志";
            }
        }

        private void richTextMsg_DrawItem(object sender, DrawItemEventArgs e)
        {
            if (e.Index < 0)
                return;
            ListViewItem lstItem = (ListViewItem)richTextMsg.Items[e.Index];
            e.DrawBackground();
            Brush brsh = Brushes.White;
            if ((e.State & DrawItemState.Selected) != DrawItemState.Selected)
                brsh = new SolidBrush(lstItem.ForeColor);
            String sText = lstItem.Text.Replace('\n', ' ');
            SizeF sz = e.Graphics.MeasureString(sText, e.Font, new SizeF(e.Bounds.Width, e.Bounds.Height));
            e.Graphics.DrawString(sText, e.Font, brsh, e.Bounds.Left, e.Bounds.Top + (e.Bounds.Height - sz.Height) / 2 + 0.5f);
        }

        private void tabMain_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tabMain.SelectedIndex == 2)
                lsServerUrl.Text = ReadConfig.GetRouterUrl();
            else
                lsServerUrl.Text = ReadConfig.GetWcfServerUrl();
        }
        //运行时间显示
        private void timer1_Tick(object sender, EventArgs e)
        {
            timeCount++;
            //显示运行时间
            long iHour = timeCount / 3600;
            long iMin = (timeCount % 3600) / 60;
            long iSec = timeCount % 60;
            if (iHour > 23)
                lbRunTime.Text = String.Format("{0}天 {1:02d}:{2:0#}:{3:0#}", iHour / 24, iHour % 24, iMin, iSec);
            else
                lbRunTime.Text = String.Format("{0:0#}:{1:0#}:{2:0#}", iHour, iMin, iSec);

            lbClientCount.Text = gridClientList.RowCount.ToString();
        }
        //消息显示
        private void timermsg_Tick(object sender, EventArgs e)
        {
            if (msgList.Count == 0) return;
            msgobject msgo = msgList.Dequeue();
            if (msgo == null) return;
            settext(msgo.clr, "[" + msgo.time.ToString("yyyy-MM-dd HH:mm:ss") + "] : " + msgo.msg);
        }

        private void 帮助ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Process.Start("http://www.efwplus.cn");
        }

        private void 注册ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmCDKEY cdkey = new frmCDKEY();
            cdkey.ShowDialog();
        }
        //路由表
        private void btnrouter_Click(object sender, EventArgs e)
        {
            FrmRouterXML router = new FrmRouterXML();
            router.ShowDialog();
        }

        private void btnInfo_Click(object sender, EventArgs e)
        {
            FrmInfo info = new FrmInfo();
            info.ShowDialog();
        }

        private void btnrestart_Click(object sender, EventArgs e)
        {
            

            if (MessageBox.Show("您确定要重启中间件服务器吗？", "询问窗", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
            {
                try
                {
                    MiddlewareLogHelper.WriterLog(LogType.MidLog, true, Color.Red, "正在准备重启中间件服务，请等待...");
                    StopAllHost();
                    Application.Restart();
                }
                catch { }
                this.Dispose(true);
            }
        }
        //定时任务
        private void btntask_Click(object sender, EventArgs e)
        {
            FrmTimingTask frmtask = new FrmTimingTask();
            frmtask.ShowDialog();
        }
    }

    public enum HostState
    {
        NoOpen,Opened
    }

    public class msgobject
    {
        public msgobject(Color _clr, DateTime _time, string _msg)
        {
            clr = _clr;
            time = _time;
            msg = _msg;
        }
        public Color clr { get; set; }
        public DateTime time { get; set; }
        public string msg { get; set; }
    }
}
