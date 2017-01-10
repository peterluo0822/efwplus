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
    public partial class FrmUpgrade : Form
    {
        string tplconfig = Application.StartupPath + @"/TplConfig.xml";
        System.Xml.XmlDocument xmlDoc;
        List<string> tplDic = new List<string>();
        string basepath = "";
        /// <summary>
        /// 
        /// </summary>
        /// <param name="type">0客户端 1服务端</param>
        /// <param name="clientpath"></param>
        /// <param name="serverpath"></param>
        public FrmUpgrade(string clientpath,string serverpath)
        {
            InitializeComponent();
            txtClientPath.Text = clientpath;
            txtServerPath.Text = serverpath;
            //cbTpl.SelectedIndex = type;
        }

        private void BindTree(string path)
        {
            //if (File.Exists(txtClientPath.Text))
            //{
            //    MessageBox.Show("程序目录不存在！");
            //    return;
            //}
            treeProgram.Nodes.Clear();
            iTreeNode itn = iTreeNodeManager.getTreefromPath(path, tplDic);
            itn.Expand();
            treeProgram.Nodes.Add(itn);
        }

        private void btnProgramPath_Click(object sender, EventArgs e)
        {
            if (this.folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                txtClientPath.Text = this.folderBrowserDialog1.SelectedPath;

                //BindTree();
            }
        }

        private void btnOutputPath_Click(object sender, EventArgs e)
        {
            if (this.folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                txtServerPath.Text = this.folderBrowserDialog1.SelectedPath;
            }
        }

        private void treeProgram_AfterCheck(object sender, TreeViewEventArgs e)
        {
            setTreeChecked(e.Node);
            //treeProgram.SelectedNode
        }

        private void FrmUpgrade_Load(object sender, EventArgs e)
        {
            xmlDoc = new XmlDocument();
            xmlDoc.Load(tplconfig);
            foreach (XmlNode item in xmlDoc.SelectNodes("root/tplItem"))
            {
                cbTpl.Items.Add(item.Attributes["name"].Value);
            }
        }

        private void cbTpl_SelectedIndexChanged(object sender, EventArgs e)
        {
            //XmlNode xmlnode = xmlDoc.SelectSingleNode("root/tplItem[@name='" + cbTpl.Text + "']");
            //txtProgramPath.Text = xmlnode.Attributes["ProgramPath"].Value;
            //txtOutputPath.Text = xmlnode.Attributes["OutputPath"].Value;

            if (cbTpl.SelectedIndex == 0)
                basepath = txtClientPath.Text;
            else
                basepath = txtServerPath.Text;

            tplDic = new List<string>();
            foreach (XmlNode item in xmlDoc.SelectNodes("root/tplItem[@name='" + cbTpl.Text + "']/Item"))
            {
                tplDic.Add(basepath + item.Attributes["path"].Value);
            }

            BindTree(basepath);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (treeProgram.Nodes.Count == 0) return;
            tplDic = new List<string>();
            getTreeChecked(treeProgram.Nodes[0]);

            XmlNode xmlnode = xmlDoc.SelectSingleNode("root/tplItem[@name='" + cbTpl.Text + "']");

            xmlnode.RemoveAll();

            (xmlnode as XmlElement).SetAttribute("name", cbTpl.Text);
            //(xmlnode as XmlElement).SetAttribute("ProgramPath", txtClientPath.Text);
            //(xmlnode as XmlElement).SetAttribute("OutputPath", txtServerPath.Text);


            foreach (var v in tplDic)
            {
                XmlElement xe1 = xmlDoc.CreateElement("Item");
                xe1.SetAttribute("path", v.Replace(basepath,""));
                //xe1.SetAttribute("checked", v.Value ? "1" : "0");
                xmlnode.AppendChild(xe1);
            }
            xmlDoc.Save(tplconfig);
            MessageBox.Show("保存为模板成功！");
        }


        private void getTreeChecked(TreeNode nodex)
        {
            if (nodex.Checked)
                tplDic.Add(nodex.Tag.ToString());
            foreach (TreeNode tn in nodex.Nodes)
            {
                getTreeChecked(tn);
            }
        }

        private void setTreeChecked(TreeNode nodex)
        {
            foreach (TreeNode tn in nodex.Nodes)
            {
                tn.Checked = nodex.Checked;
                setTreeChecked(tn);
            }
        }

        /// <summary>
        /// 判断目标是文件夹还是目录(目录包括磁盘)
        /// </summary>
        /// <param name="filepath">文件名</param>
        /// <returns></returns>
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
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sPath">源文件</param>
        private void CopyFile(string sPath)
        {
            string dPath = sPath.Replace(txtClientPath.Text, txtServerPath.Text);
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

        private void button2_Click(object sender, EventArgs e)
        {
            //if (treeProgram.Nodes.Count == 0) return;
            //tplDic = new Dictionary<string, bool>();
            //getTreeChecked(treeProgram.Nodes[0]);

            //if (Directory.Exists(txtServerPath.Text))
            //{
            //    Directory.Delete(txtServerPath.Text, true);
            //}

            //foreach (var v in tplDic)
            //{
            //    CopyFile(v.Key);
            //}

            //MessageBox.Show("生成升级包成功！");
        }
    }
}
