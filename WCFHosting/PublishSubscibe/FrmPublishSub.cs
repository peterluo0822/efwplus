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
using EFWCoreLib.WcfFrame.Utility;

namespace WCFHosting.PublishSubscibe
{
    public partial class FrmPublishSub : Form
    {
        public FrmPublishSub()
        {
            InitializeComponent();
            dataGridView1.AutoGenerateColumns = false;
        }

        private void FrmPublishSub_Load(object sender, EventArgs e)
        {
            dataGridView1.DataSource = PublishSubManager.GetCenterPublishService();
        }
        //执行订阅
        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentCell != null)
            {
                PublishSubService ps = (dataGridView1.DataSource as List<PublishSubService>)[dataGridView1.CurrentCell.RowIndex];
                PublishSubManager.ProcessPublishService(ps.publishServiceName);
            }
        }
        //订阅
        private void toolStripButton2_Click(object sender, EventArgs e)
        {

        }
        //取消订阅
        private void toolStripButton3_Click(object sender, EventArgs e)
        {

        }
    }
}
