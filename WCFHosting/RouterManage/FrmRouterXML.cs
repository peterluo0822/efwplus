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

namespace WCFHosting.RouterManage
{
    public partial class FrmRouterXML : Form
    {
        public FrmRouterXML()
        {
            InitializeComponent();
        }

        private void btnsave_Click(object sender, EventArgs e)
        {

            if (HostRouterXml.SaveXml(txtxml.Text))
            {
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
            txtxml.Text = HostRouterXml.GetXml();
        }
    }
}
