using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace EFWCoreLib.WcfFrame.Utility.Mongodb
{
    /// <summary>
    /// Mongodb处理
    /// </summary>
    public class MongodbManager
    {
        //private static System.Diagnostics.Process pro;
        /// <summary>
        /// 开启Mongodb
        /// </summary>
        public static void StartDB()
        {
            StopDB();
            string mongodExe = HostSettingConfig.GetValue("mongodb_binpath") + @"\mongod.exe";
            string mongoConf = CoreFrame.Init.AppGlobal.AppRootPath + @"Config\mongo.conf";

            System.Diagnostics.Process pro = new System.Diagnostics.Process();
            pro.StartInfo.FileName = mongodExe;
            pro.StartInfo.Arguments = "--config " + mongoConf;
            pro.StartInfo.UseShellExecute = false;
            pro.StartInfo.RedirectStandardInput = true;
            pro.StartInfo.RedirectStandardOutput = true;
            pro.StartInfo.RedirectStandardError = true;
            pro.StartInfo.CreateNoWindow = true;
            pro.Start();
            //pro.WaitForExit();
        }
        /// <summary>
        /// 停止Mongodb
        /// </summary>
        public static void StopDB()
        {
            Process[] proc = Process.GetProcessesByName("mongod");//创建一个进程数组，把与此进程相关的资源关联。
            for (int i = 0; i < proc.Length; i++)
            {
                proc[i].Kill();  //逐个结束进程.
            }
        }
    }
}
