using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace efwplusWatcher
{
    public partial class FrmMain : Form
    {
        ProcessWatcher pwatcher;
        public FrmMain()
        {
            InitializeComponent();

            AutoRun.SetAutoRun("PWatcher", Application.ExecutablePath);
        }

        private void FrmMain_Load(object sender, EventArgs e)
        {
            this.notifyIcon1.Visible = true;
            this.notifyIcon1.Icon = this.Icon;
            this.notifyIcon1.Text = this.Text;

            List<WatcherFile> wflist = new List<WatcherFile>();
            //读取监控进程全路径
            string strProcessAddress = ConfigurationManager.AppSettings["ProcessAddress"].ToString();
            if (strProcessAddress != null)
            {
                foreach( var s in strProcessAddress.Split(','))
                {
                    WatcherFile wf = new WatcherFile();
                    wf.fileName = s.Trim();
                    wflist.Add(wf);
                }
            }

            dataGridView1.DataSource = wflist;


            pwatcher = new ProcessWatcher();
            pwatcher.OnStart();

           
        }

        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            this.Show();
        }

        private void 显示ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Show();
        }

        private void 退出ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            pwatcher.OnStop();
            this.Dispose();
        }

        private void FrmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            this.Hide();
        }
    }

    public class WatcherFile
    {
        public string fileName { get; set; }
    }
}
