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

namespace WCFHosting
{
    public partial class FrmPlugin : Form
    {
        public FrmPlugin()
        {
            InitializeComponent();
            gridlocal.AutoGenerateColumns = false;
        }

        private void InitPlugin()
        {
            List<PluginClass> plist = new List<PluginClass>();
            List<PluginClass> plist2 = PluginSysManage.GetAllPlugin();
            plist.AddRange(plist2.FindAll(x => (x.plugintype == "WcfModulePlugin")));

            for (int i = 0; i < plist.Count; i++)
            {
                FileInfo finfo = new FileInfo(PluginSysManage.localpath + "\\" + plist[i].path);
                PluginXmlManage.pluginfile = finfo.FullName;
                pluginxmlClass plugin = PluginXmlManage.getpluginclass();
                plist[i].version = plugin.version;
                plist[i].author = plugin.author;
            }
            gridlocal.DataSource = plist;
        }

        
        private void button3_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        private void FrmPlugin_Load(object sender, EventArgs e)
        {
            InitPlugin();
        }

        private void btnDel_Click(object sender, EventArgs e)
        {
            if (gridlocal.CurrentCell == null) return;

            if (MessageBox.Show("是否卸载此插件？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) == DialogResult.No)
                return;

            List<PluginClass> plist = gridlocal.DataSource as List<PluginClass>;
            PluginClass pc = plist[gridlocal.CurrentCell.RowIndex];
            FileInfo finfo = null;
            if (pc.plugintype == "WcfModulePlugin")
            {
                //1.卸载插件
                EFWCoreLib.CoreFrame.Init.AppPluginManage.RemovePlugin(pc.name);
                //2.删除插件的相关文件
                finfo = new FileInfo(PluginSysManage.localpath + "\\" + pc.path);
                if (finfo.Exists)
                {
                    PluginSysManage.DeletePlugin(pc.plugintype, pc.name);
                    if (finfo.Directory.Exists)
                        finfo.Directory.Delete(true);
                    //MessageBox.Show("此插件卸载成功，必须重启WCF服务主机！","提示",MessageBoxButtons.OK,MessageBoxIcon.Information);
                }
            }

            InitPlugin();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (openFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                string pluginfile;
                if (PluginSetup(openFileDialog.FileName,out pluginfile) == true)
                {
                    EFWCoreLib.CoreFrame.Init.AppPluginManage.AddPlugin(pluginfile);
                    //MessageBox.Show("完成插件包安装，必须重启WCF服务主机！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    InitPlugin();
                }
            }
        }

        #region 内部方法
        private string PathCombine(string absolutePath, string relativePath)
        {
            return Path.GetFullPath(Path.Combine(absolutePath, relativePath));
        }

        private bool PluginSetup(string localzippath,out string pluginfile)
        {
            pluginfile = "";
            FileInfo fileinfo = new FileInfo(localzippath);
            if (fileinfo.Exists == false) throw new Exception("插件包不存在！");

            string temp_pluginpath = fileinfo.Directory.FullName + "\\" + fileinfo.Name.Replace(fileinfo.Extension, "");
            FastZipHelper.decompress(temp_pluginpath, localzippath);
            PluginXmlManage.pluginfile = temp_pluginpath + "\\plugin.xml";
            pluginxmlClass plugin = PluginXmlManage.getpluginclass();

            string pluginpath = "";
            //string pluginsyspath = "";
            //string dbconfig = "";
            string plugintype = "";
            string ptype = plugin.plugintype.ToLower();

            bool ishave = false;
            switch (ptype)
            {
                case "wcf":
                    //pluginsyspath = PluginSysManage.localpath + "\\Config\\pluginsys.xml";
                    //dbconfig = PluginSysManage.localpath + "\\Config\\EntLib.config";
                    plugintype = "WcfModulePlugin";
                    pluginpath = PluginSysManage.localpath + "\\ModulePlugin\\" + plugin.name;
                    //PluginSysManage.pluginsysFile = CommonHelper.WinformPlatformPath + "\\Config\\pluginsys.xml";
                    if (PluginSysManage.ContainsPlugin("WcfModulePlugin", plugin.name))
                    {
                        ishave = true;
                    }
                    break;

            }

            //先判断此插件本地是否存在
            if (ishave == true)
            {
                MessageBox.Show("你选择的插件本地已安装！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }

            

            //移动到插件目录
            if (temp_pluginpath != pluginpath)
                new DirectoryInfo(temp_pluginpath).MoveTo(pluginpath);

            pluginfile = pluginpath + "\\plugin.xml";
            //pluginsys.xml
            //PluginSysManage.pluginsysFile = pluginsyspath;
            PluginSysManage.AddPlugin(plugintype, plugin.name, "ModulePlugin/" + plugin.name + "/plugin.xml", plugin.title, "0");

            return true;
        }

        /// <summary>          
        /// Copy文件夹          
        /// </summary>          
        /// <param name="sPath">源文件夹路径</param>          
        /// <param name="dPath">目的文件夹路径</param>          
        /// <returns>完成状态：success-完成；其他-报错</returns>          
        private void CopyFolder(string sPath, string dPath)
        {
            // 创建目的文件夹                  
            if (!Directory.Exists(dPath))
            {
                Directory.CreateDirectory(dPath);
            }
            // 拷贝文件                  
            DirectoryInfo sDir = new DirectoryInfo(sPath);
            FileInfo[] fileArray = sDir.GetFiles();
            foreach (FileInfo file in fileArray)
            {
                file.CopyTo(dPath + "\\" + file.Name, true);
            }
            // 循环子文件夹                  
            DirectoryInfo dDir = new DirectoryInfo(dPath);
            DirectoryInfo[] subDirArray = sDir.GetDirectories();
            foreach (DirectoryInfo subDir in subDirArray)
            {
                CopyFolder(subDir.FullName, dPath + "//" + subDir.Name);
            }
        }

        #endregion

        private void 重载插件ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (gridlocal.CurrentCell == null) return;
            List<PluginClass> plist = gridlocal.DataSource as List<PluginClass>;
            PluginClass pc = plist[gridlocal.CurrentCell.RowIndex];
            EFWCoreLib.CoreFrame.Init.AppPluginManage.RemovePlugin(pc.name);
            EFWCoreLib.CoreFrame.Init.AppPluginManage.AddPlugin(PluginSysManage.localpath + "\\ModulePlugin\\" + pc.name + "\\plugin.xml");
            MessageBox.Show("重载插件完成！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void 内存卸载ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (gridlocal.CurrentCell == null) return;
            List<PluginClass> plist = gridlocal.DataSource as List<PluginClass>;
            PluginClass pc = plist[gridlocal.CurrentCell.RowIndex];
            EFWCoreLib.CoreFrame.Init.AppPluginManage.RemovePlugin(pc.name);
            //EFWCoreLib.CoreFrame.Init.AppPluginManage.AddPlugin(PluginSysManage.localpath + "\\" + pc.name + "\\plugin.xml");
            MessageBox.Show("内存卸载插件完成！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void 内存加载ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (gridlocal.CurrentCell == null) return;
            List<PluginClass> plist = gridlocal.DataSource as List<PluginClass>;
            PluginClass pc = plist[gridlocal.CurrentCell.RowIndex];
            // EFWCoreLib.CoreFrame.Init.AppPluginManage.RemovePlugin(pc.name);
            EFWCoreLib.CoreFrame.Init.AppPluginManage.AddPlugin(PluginSysManage.localpath + "\\ModulePlugin\\" + pc.name + "\\plugin.xml");
            MessageBox.Show("内存加载插件完成！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void 查看配置ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (gridlocal.CurrentCell == null) return;
            List<PluginClass> plist = gridlocal.DataSource as List<PluginClass>;
            PluginClass pc = plist[gridlocal.CurrentCell.RowIndex];

            WCFHosting.PluginManage.FrmPluginXML xml = new PluginManage.FrmPluginXML(PluginSysManage.localpath + "\\ModulePlugin\\" + pc.name + "\\plugin.xml");
            xml.ShowDialog();
        }
    }
}
