using EFWCoreLib.CoreFrame.Init;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFWCoreLib.CoreFrame.Common
{
    /// <summary>
    /// 中间件宿主消息处理
    /// </summary>
    /// <param name="clr"></param>
    /// <param name="time"></param>
    /// <param name="text"></param>
    public delegate void MiddlewareMsgHandler(System.Drawing.Color clr, DateTime time, string text);
    /// <summary>
    /// 中间件日志记录
    /// </summary>
    public class MiddlewareLogHelper
    {
        public static MiddlewareMsgHandler hostwcfMsg;
        private static Dictionary<LogType, string> LogNameDic = new Dictionary<LogType, string>();
        private static Dictionary<LogType, StringBuilder> LogSbDic = new Dictionary<LogType, StringBuilder>();
        /// <summary>
        /// 往中间件写入日志
        /// </summary>
        /// <param name="logType">日志类型</param>
        /// <param name="isShowMsg">是否显示</param>
        /// <param name="clr">颜色</param>
        /// <param name="text">内容</param>
        public static void WriterLog(LogType logType, bool isShowMsg, Color clr, string text)
        {
            LogSbDic[logType].AppendLine("[" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "] : " + text);

            if (isShowMsg && hostwcfMsg != null)
            {
                hostwcfMsg(clr, DateTime.Now, text);
            }
        }

        private static void WriterFile()
        {
            foreach (var item in LogNameDic)
            {
                string info = null;
                lock (syncObj)
                {
                    info = LogSbDic[item.Key].ToString();
                    if (info != null)//清空
                        LogSbDic[item.Key].Clear();
                }
                if (string.IsNullOrEmpty(info) == false)
                {
                    string filepath = AppGlobal.AppRootPath + LogNameDic[item.Key] + "\\" + DateTime.Now.ToString("yyyyMM") + ".txt";
                    if (System.IO.Directory.Exists(AppGlobal.AppRootPath + LogNameDic[item.Key]) == false)
                    {
                        System.IO.Directory.CreateDirectory(AppGlobal.AppRootPath + LogNameDic[item.Key]);
                    }
                    System.IO.File.AppendAllText(filepath, info);
                }
            }
        }


        static System.Timers.Timer timer;
        public static void StartWriteFileLog()
        {
            if (timer == null)
            {
                LogNameDic.Add(LogType.MidLog, getLogName(LogType.MidLog));
                LogSbDic.Add(LogType.MidLog, new StringBuilder());

                LogNameDic.Add(LogType.OverTimeLog, getLogName(LogType.OverTimeLog));
                LogSbDic.Add(LogType.OverTimeLog, new StringBuilder());

                LogNameDic.Add(LogType.WebApiLog, getLogName(LogType.WebApiLog));
                LogSbDic.Add(LogType.WebApiLog, new StringBuilder());

                LogNameDic.Add(LogType.TimingTaskLog, getLogName(LogType.TimingTaskLog));
                LogSbDic.Add(LogType.TimingTaskLog, new StringBuilder());

                LogNameDic.Add(LogType.MILog, getLogName(LogType.MILog));
                LogSbDic.Add(LogType.MILog, new StringBuilder());

                timer = new System.Timers.Timer();
                timer.Interval = 1000;
                timer.Elapsed += new System.Timers.ElapsedEventHandler(timer_Elapsed);
                timer.Start();
            }
        }
        static Object syncObj = new Object();////定义一个静态对象用于线程部份代码块的锁定，用于lock操作
        static void timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                WriterFile();
            }
            catch (Exception err)
            {
                WriterLog(LogType.MidLog, true, Color.Red, "写入日志文件失败，停止继续写入操作！");
                timer.Stop();
            }
        }
        static string getLogName(LogType logtype)
        {
            string logName;
            switch (logtype)
            {
                case LogType.MILog:
                    logName = "MILog";
                    break;
                case LogType.MidLog:
                    logName = "MidLog";
                    break;
                case LogType.OverTimeLog:
                    logName = "OverTimeLog";
                    break;
                case LogType.WebApiLog:
                    logName = "WebApiLog";
                    break;
                case LogType.TimingTaskLog:
                    logName = "TimingTaskLog";
                    break;
                default:
                    logName = "MidLog";
                    break;
            }
            return logName;
        }
    }

    /// <summary>
    /// 日志类型
    /// </summary>
    public enum LogType
    {
        /// <summary>
        /// 医保日志
        /// </summary>
        MILog,
        /// <summary>
        /// 中间件业务日志
        /// </summary>
        MidLog,
        /// <summary>
        /// 超时请求日志
        /// </summary>
        OverTimeLog,
        /// <summary>
        /// WebApi日志
        /// </summary>
        WebApiLog,
        /// <summary>
        /// 定时任务日志
        /// </summary>
        TimingTaskLog
    }
}
