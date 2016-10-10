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

namespace PulginPack
{
    public partial class FrmMain : Form
    {
        public FrmMain()
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

        private void FrmMain_Load(object sender, EventArgs e)
        {
            InitPlugin();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {

        }

        private void btnPack_Click(object sender, EventArgs e)
        {
            if (gridlocal.CurrentCell == null) return;
            List<PluginClass> plist = gridlocal.DataSource as List<PluginClass>;
            PluginClass pc = plist[gridlocal.CurrentCell.RowIndex];

            
            saveFileDialog.FileName = pc.name;
            if (saveFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                string bagpath = saveFileDialog.FileName;
                if (File.Exists(bagpath))
                {
                    File.Delete(bagpath);
                }

                string path = PluginSysManage.localpath + "\\" + pc.path;
                string prjname =null;
                string prjpath =null;
                FileInfo finfo = new FileInfo(path);
                if (finfo.Exists)
                {
                    prjpath = finfo.Directory.FullName;
                    prjname = finfo.Directory.Name;
                }
                PluginXmlManage.pluginfile = path;
                pluginxmlClass plugin = PluginXmlManage.getpluginclass();

                string temp = prjpath + "_temp";
                Directory.CreateDirectory(temp);
                foreach (issueClass ic in plugin.issue)
                {
                    if (ic.type == "dir")
                    {
                        if (ic.source != "")
                        {
                            CopyFolder(prjpath + "\\" + ic.source, temp + "\\" + ic.path);
                        }
                        else
                        {
                            if (Directory.Exists(prjpath + "\\" + ic.path))
                                CopyFolder(prjpath + "\\" + ic.path, temp + "\\" + ic.path);
                            else
                                Directory.CreateDirectory(temp + "\\" + ic.path);
                        }
                    }
                    else if (ic.type == "file")
                    {
                        if (ic.source != "")
                        {
                            new FileInfo(prjpath + "\\" + ic.source).CopyTo(temp + "\\" + ic.path, true);
                        }
                        else
                        {
                            new FileInfo(prjpath + "\\" + ic.path).CopyTo(temp + "\\" + ic.path, true);
                        }
                    }
                }


                FastZipHelper.compress(temp, bagpath);
                Directory.Delete(temp, true);

            }
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

        private void btnQuit_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
