using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WCFHosting
{
    public partial class frmCDKEY : Form
    {
        public frmCDKEY()
        {
            InitializeComponent();
            txtcpu.Text = TimeCDKEY.GetCpuId();
        }

        private void txtCode_TextChanged(object sender, EventArgs e)
        {
            if (txtCode.Text.Trim() == "kakake!@#123")
            {
                txtCode.Text = Encryption.EncryPW(TimeCDKEY.CreatSerialNumber(null, txtcpu.Text.Trim(), null), "kakake!@#123");
            }
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            TimeCDKEY.WriteSetting(txtCode.Text);
            string expireDate;
            string identify;
            int res = TimeCDKEY.InitRegedit(out expireDate,out identify);
            if (res == 0)
            {
                MessageBox.Show("激活成功，到期时间【" + expireDate + "】，请重新启动软件！");
                this.Close();
            }
            else if (res == 1)
            {
                MessageBox.Show("软件尚未注册，请注册软件！");
            }
            else if (res == 2)
            {
                MessageBox.Show("注册机器与本机不一致,请联系管理员！");
            }
            else if (res == 3)
            {
                MessageBox.Show("软件试用已到期！");
            }
            else
            {
                MessageBox.Show("软件运行出错，请重新启动！");
            }
        }

        private void txtcpu_DoubleClick(object sender, EventArgs e)
        {
            txtcpu.ReadOnly = false;
        }
    }
}
