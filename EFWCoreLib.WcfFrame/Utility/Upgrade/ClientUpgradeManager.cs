using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EFWCoreLib.CoreFrame.Common;
using EFWCoreLib.CoreFrame.Init;
using EFWCoreLib.WcfFrame.DataSerialize;
using EFWCoreLib.WcfFrame.ServerManage;

namespace EFWCoreLib.WcfFrame.Utility.Upgrade
{

    /// <summary>
    /// 客户端程序升级管理
    /// 1.中间件通过定时任务从中心下载客户端升级包
    /// 2.中心中间件客户端升级包更新后通过订阅的方式通知下级中间件下载升级包
    /// 
    /// 下载升级包之前先下载升级配置文件，判断升级包的版本是否有更新
    /// 如果中心的版本要高，就从中心下载最新版本
    /// 
    /// 客户端配置更新地址：http://localhost:8021/upgrade/update.xml
    /// </summary>
    public class ClientUpgradeManager
    {
        public static string clientupgradepath = AppGlobal.AppRootPath + @"FileStore\ClientUpgrade\";//客户端升级包路径
        static string updatexml = clientupgradepath + "update.xml";
        static string updatezip = clientupgradepath + "update.zip";
        public static void LoadTask(List<TimingTask> _taskList)
        {
            TaskContent task = new TaskContent();
            TimingTask timing = new TimingTask();
            timing.TimingTaskExcuter = task;
            timing.TimingTaskType = TimingTaskType.PerDay;
            timing.ExcuteTime = new ShortTime(0, 0, 10);
            _taskList.Add(timing);
        }

        public static void DownLoadUpgrade()
        {
            //SuperClient.superclient
            //1.查询本地的update.xml配置文件
            //2.如果不存在则直接下载
            //3.如果存在则下载中心update.xml配置文件
            //4.比对两个版本的大小
            //5.如果本地版本小则直接下载，大则放弃下载
            if (!Directory.Exists(clientupgradepath))
            {
                Directory.CreateDirectory(clientupgradepath);
            }
           
            FileInfo finfo = new FileInfo(updatexml);
            if (finfo.Exists == false)//本地不存在，直接下载
            {
                File.Delete(updatexml);
                File.Delete(updatezip);
                downloadRemoteFile();
            }
            else//本地存在升级包文件
            {
                Version localver = readLocalUpdateXml();
                Version remotever = getRemoteUpdateXml();
                int tm= localver.CompareTo(remotever);
                if (tm < 0)//本地版本小
                {
                    File.Delete(updatexml);
                    File.Delete(updatezip);
                    downloadRemoteFile();
                }
            }
        }

        private static void downloadRemoteFile()
        {
            DownFile df = new DownFile();
            df.clientId = SuperClient.superclient.clientObj.ClientID;
            df.DownKey = Guid.NewGuid().ToString();
            df.FileName = "update.xml";
            df.FileType = 1;
            SuperClient.superclient.DownLoadFile(df, updatexml, null);

            df = new DownFile();
            df.clientId = SuperClient.superclient.clientObj.ClientID;
            df.DownKey = Guid.NewGuid().ToString();
            df.FileName = "update.zip";
            df.FileType = 1;
            SuperClient.superclient.DownLoadFile(df, updatezip, null);
        }

        private static Version readLocalUpdateXml()
        {
            System.Xml.XmlDocument xmlDoc = new System.Xml.XmlDocument();
            xmlDoc.Load(updatexml);
            System.Xml.XmlNode xn = xmlDoc.DocumentElement.SelectSingleNode("AppVersion");
            Version ver = new Version(xn.InnerText);
            return ver;
        }

        private static Version getRemoteUpdateXml()
        {
            DownFile df = new DownFile();
            df.clientId = SuperClient.superclient.clientObj.ClientID;
            df.DownKey = Guid.NewGuid().ToString();
            df.FileName = "update.xml";
            df.FileType = 1;
            MemoryStream ms = new MemoryStream();
            SuperClient.superclient.DownLoadFile(df, ms, null);
            System.Xml.XmlDocument xmlDoc = new System.Xml.XmlDocument();
            xmlDoc.Load(ms);
            System.Xml.XmlNode xn = xmlDoc.DocumentElement.SelectSingleNode("AppVersion");
            Version ver = new Version(xn.InnerText);
            return ver;
        }
    }

    /// <summary>
    /// 执行的升级包下载
    /// </summary>
    public class TaskContent : ITimingTaskExcuter
    {
        public void ExcuteOnTime(DateTime dt)
        {
            MiddlewareLogHelper.WriterLog(LogType.TimingTaskLog, true, System.Drawing.Color.Blue, string.Format("任务【下载客户端升级包】准备开始执行..."));
            ClientUpgradeManager.DownLoadUpgrade();
        }
    }
}
