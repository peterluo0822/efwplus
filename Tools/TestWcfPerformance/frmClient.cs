using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using EFWCoreLib.CoreFrame.Init;
using EFWCoreLib.WcfFrame;


namespace TestWcfPerformance
{
    public partial class frmClient : Form
    {
        ClientLink clientlink;
        public frmClient()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            clientlink = new ClientLink("TestWcfPerformance", "Books.Service");
            clientlink.CreateConnection();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            clientlink.Request("bookWcfController", "GetBooks", null);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            clientlink.Dispose();
        }
    }
}
