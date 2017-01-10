using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace EFWCoreLib.WcfFrame.Utility.Nginx
{
    /// <summary>
    /// Nginx Web服务器
    /// </summary>
    public class NginxManager
    {
        /// <summary>
        /// 开启Nginx
        /// </summary>
        public static void StartWeb()
        {
            string nginxExe = CoreFrame.Init.AppGlobal.AppRootPath + @"nginx\nginx.exe";
            System.IO.FileInfo file = new System.IO.FileInfo(nginxExe);

            System.Diagnostics.Process pro = new System.Diagnostics.Process();
            pro.StartInfo.FileName = "cmd.exe";
            //pro.StartInfo.Arguments = "--config " + mongoConf;
            pro.StartInfo.UseShellExecute = false;
            pro.StartInfo.RedirectStandardInput = true;
            pro.StartInfo.RedirectStandardOutput = true;
            pro.StartInfo.RedirectStandardError = true;
            pro.StartInfo.CreateNoWindow = true;
            pro.Start();
            //pro.WaitForExit();
            pro.StandardInput.WriteLine("cd " + file.Directory.Root);
            pro.StandardInput.WriteLine("cd " + file.DirectoryName);
            pro.StandardInput.WriteLine("start " + file.Name);
            pro.StandardInput.WriteLine("exit");
            pro.StandardInput.AutoFlush = true;
            //string output = pro.StandardOutput.ReadToEnd();
        }
        /// <summary>
        /// 停止Nginx
        /// </summary>
        public static void StopWeb()
        {
            Process[] proc = Process.GetProcessesByName("nginx");//创建一个进程数组，把与此进程相关的资源关联。
            for (int i = 0; i < proc.Length; i++)
            {
                proc[i].Kill();  //逐个结束进程.
            }
            //杀两次，因为nginx自带守护进程
            proc = Process.GetProcessesByName("nginx");//创建一个进程数组，把与此进程相关的资源关联。
            for (int i = 0; i < proc.Length; i++)
            {
                proc[i].Kill();  //逐个结束进程.
            }
        }
    }
}
