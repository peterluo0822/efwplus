using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WCFHosting.PluginManage
{
    public partial class FrmPluginXML : Form
    {
        private string pluginfile;
        public FrmPluginXML(string _pluginfile)
        {
            InitializeComponent();
            pluginfile = _pluginfile;
        }

        public FrmPluginXML(string _RouterBill, string title)
        {
            InitializeComponent();
            pluginfile = _RouterBill;
            this.Text = title;
        }

        private void btnsave_Click(object sender, EventArgs e)
        {
            FileInfo file = new FileInfo(pluginfile);
            if (file.Exists)
            {
                file.Delete();
                using (System.IO.StreamWriter sw = new System.IO.StreamWriter(pluginfile, true))
                {
                    sw.Write(txtxml.Text);//直接追加文件末尾，不换行 
                }
                MessageBox.Show("保存成功！");
                this.Close();
            }
        }

        private void btnclose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void FrmPluginXML_Load(object sender, EventArgs e)
        {
            FileInfo file = new FileInfo(pluginfile);
            if (file.Exists)
            {
                txtxml.Text = file.OpenText().ReadToEnd();
            }
        }
    }
}
