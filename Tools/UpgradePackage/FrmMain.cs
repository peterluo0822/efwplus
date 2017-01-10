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
using System.Xml;

namespace UpgradePackage
{
    public partial class FrmMain : Form
    {
        string tplconfig = Application.StartupPath + @"/TplConfig.xml";
        XmlDocument xmlDoc;
        public FrmMain()
        {
            InitializeComponent();
            LoadXML();
        }

        public void LoadXML()
        {
            xmlDoc = new XmlDocument();
            xmlDoc.Load(tplconfig);

            XmlNode xn = xmlDoc.SelectSingleNode("root/baseItem");
            txtversions.Text = xn.Attributes["version"].Value;
            txtClientPath.Text = xn.Attributes["clientpath"].Value;
            txtServerPath.Text = xn.Attributes["serverpath"].Value;
            txtOutput.Text = xn.Attributes["output"].Value;

        }

        public void SaveXML()
        {
            XmlNode xn = xmlDoc.SelectSingleNode("root/baseItem");
            (xn as XmlElement).SetAttribute("version", txtversions.Text);
            (xn as XmlElement).SetAttribute("clientpath", txtClientPath.Text);
            (xn as XmlElement).SetAttribute("serverpath", txtServerPath.Text);
            (xn as XmlElement).SetAttribute("output", txtOutput.Text);
            xmlDoc.Save(tplconfig);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (txtClientPath.Text.Trim() == "" || txtServerPath.Text.Trim() == "")
            {
                MessageBox.Show("请先设置好程序目录！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            FrmUpgrade frm = new FrmUpgrade(txtClientPath.Text,txtServerPath.Text);
            frm.ShowDialog();
        }

        private void btnClientPath_Click(object sender, EventArgs e)
        {
            if (this.folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                txtClientPath.Text = this.folderBrowserDialog1.SelectedPath;
            }
        }

        private void btnServerPath_Click(object sender, EventArgs e)
        {
            if (this.folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                txtServerPath.Text = this.folderBrowserDialog1.SelectedPath;
            }
        }

        private void btnOutput_Click(object sender, EventArgs e)
        {
            if (this.folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                txtOutput.Text = this.folderBrowserDialog1.SelectedPath;
            }
        }

       
        private void btnCreate_Click(object sender, EventArgs e)
        {
            if (txtversions.Text.Trim() == "")
            {
                MessageBox.Show("请先输入当前版本号", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (txtClientPath.Text.Trim() == "" || txtServerPath.Text.Trim() == "")
            {
                MessageBox.Show("请先设置好程序目录！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (txtOutput.Text.Trim() == "")
            {
                MessageBox.Show("请先设置好升级包输出目录！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            List<string> clientDic = new List<string>();
            List<string> serverDic = new List<string>();

            //LoadXML();

            foreach (XmlNode item in xmlDoc.SelectNodes("root/tplItem[@name='客户端']/Item"))
            {
                clientDic.Add( item.Attributes["path"].Value);
            }
            foreach (XmlNode item in xmlDoc.SelectNodes("root/tplItem[@name='服务端']/Item"))
            {
                serverDic.Add( item.Attributes["path"].Value);
            }

            string clientOutput = txtOutput.Text + @"\UpgradeClient";
            string serverOutput = txtOutput.Text + @"\UpgradeServer";
            if (Directory.Exists(clientOutput))
            {
                Directory.Delete(clientOutput, true);
            }
            if (Directory.Exists(serverOutput))
            {
                Directory.Delete(serverOutput, true);
            }

            foreach (var path in clientDic)
            {
                CopyFile(txtClientPath.Text + path, clientOutput + path);
            }

            foreach (var path in serverDic)
            {
                CopyFile(txtServerPath.Text + path, serverOutput + path);
            }

            SaveXML();

            MessageBox.Show("生成升级包成功！");
        }

        private void CopyFile(string sPath,string dPath)
        {
            //string dPath = sPath.Replace(txtClientPath.Text, txtServerPath.Text);
            if (IsDir(sPath))
            {
                // 创建目的文件夹                  
                if (!Directory.Exists(dPath))
                {
                    Directory.CreateDirectory(dPath);
                }
            }
            else
            {
                File.Copy(sPath, dPath);
            }
        }
        public static bool IsDir(string filepath)
        {
            FileInfo fi = new FileInfo(filepath);
            if ((fi.Attributes & FileAttributes.Directory) != 0)
                return true;
            else
            {
                return false;
            }
        }
    }
}
