using EFWCoreLib.CoreFrame.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WCFHosting.TimingTask
{
    public partial class FrmTimingTask : Form
    {
        public FrmTimingTask()
        {
            InitializeComponent();
            dataGridView1.AutoGenerateColumns = false;
            dataGridView2.AutoGenerateColumns = false;
        }

        
        private void FrmTimingTask_Load(object sender, EventArgs e)
        {
            dataGridView1.DataSource = MiddlewareTask.TaskConfigList;
        }

        private void dataGridView1_CurrentCellChanged(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentCell != null)
            {
                TaskConfig taskconfig = (dataGridView1.DataSource as List<TaskConfig>)[dataGridView1.CurrentCell.RowIndex];
                dataGridView2.DataSource = taskconfig.taskService;
            }
        }
        //执行任务
        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentCell != null)
            {
                TaskConfig taskconfig = (dataGridView1.DataSource as List<TaskConfig>)[dataGridView1.CurrentCell.RowIndex];
                MiddlewareTask.ExecuteTask(taskconfig);
            }
        }


        //开启任务
        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentCell != null)
            {
                TaskConfig taskconfig = (dataGridView1.DataSource as List<TaskConfig>)[dataGridView1.CurrentCell.RowIndex];
                taskconfig.qswitch = true;
                MiddlewareTask.SettingTask(taskconfig);
                dataGridView1.Refresh();
            }
        }
        //停止任务
        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentCell != null)
            {
                TaskConfig taskconfig = (dataGridView1.DataSource as List<TaskConfig>)[dataGridView1.CurrentCell.RowIndex];
                taskconfig.qswitch = false;
                MiddlewareTask.SettingTask(taskconfig);
                dataGridView1.Refresh();
            }
        }
    }
}
