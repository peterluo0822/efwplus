using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CDKEY
{
    public partial class FrmKey : Form
    {
        public FrmKey()
        {
            InitializeComponent();
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            if (txtcpu.Text.Trim() == "") return;

            txtCode.Text = Encryption.EncryPW(CreatSerialNumber(txtcpu.Text.Trim(), null), "kakake!@#123");
        }

        /* 生成序列号 */
        public string CreatSerialNumber(string cpu, string expiredate)
        {
            if (string.IsNullOrEmpty(expiredate))
            {
                if (DateTime.Now.AddMonths(2) > Convert.ToDateTime("2017-04-01"))
                {
                    expiredate = "20170401";
                }
                else
                    expiredate = DateTime.Now.AddMonths(2).ToString("yyyyMMdd");
            }

            string identify = DateTime.Now.Ticks.ToString();
            string SerialNumber = identify + "-" + cpu + "-" + expiredate;
            return SerialNumber;
        }
    }
}
