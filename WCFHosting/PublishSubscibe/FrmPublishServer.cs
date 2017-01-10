using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using EFWCoreLib.WcfFrame.ServerManage;

namespace WCFHosting.PublishSubscibe
{
    public partial class FrmPublishServer : Form
    {
        public FrmPublishServer()
        {
            InitializeComponent();
            dataGridView1.AutoGenerateColumns = false;
            dataGridView2.AutoGenerateColumns = false;
        }

        private void FrmPublishServer_Load(object sender, EventArgs e)
        {
            dataGridView1.DataSource = PublishServiceManage.serviceList;
            dataGridView1_Click(null, null);
        }

        private void dataGridView1_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentCell != null)
            {
                PublishServiceObject so = PublishServiceManage.serviceList[dataGridView1.CurrentCell.RowIndex];
                dataGridView2.DataSource = PublishServiceManage.subscriberList.FindAll(x => x.publishServiceName == so.publishServiceName);
            }
        }
        //发送通知
        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentCell != null)
            {
                PublishServiceObject so = PublishServiceManage.serviceList[dataGridView1.CurrentCell.RowIndex];
                PublishServiceManage.SendNotify(so.publishServiceName);
            }
        }
    }
}
