using EFWCoreLib.CoreFrame.Common;
using EFWCoreLib.WcfFrame;
using EFWCoreLib.WcfFrame.DataSerialize;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace EFWCoreLib.CoreFrame.Common
{
    /// <summary>
    /// 中间件上执行定时任务
    /// </summary>
    public class MiddlewareTask
    {
        public static string taskfile = System.Windows.Forms.Application.StartupPath + "\\Config\\TaskTiming.xml";
        public static List<TaskConfig> TaskConfigList=new List<TaskConfig>();

        static List<TimingTask> taskList;
        static TimingTaskManager taskmanager;
        /// <summary>
        /// 导入任务
        /// </summary>
        private static void LoadTask(List<TimingTask> _taskList)
        {
            TaskConfigList = LoadXML();
            foreach (var item in TaskConfigList)
            {
                TaskContent task = new TaskContent(item);
                TimingTask timing = new TimingTask();
                timing.TimingTaskExcuter = task;
                timing.TimingTaskType = item.execfrequency;
                timing.ExcuteTime = item.shorttime;
                _taskList.Add(timing);
            }
        }
        /// <summary>
        /// 开启任务
        /// </summary>
        public static void StartTask()
        {
            taskList = new List<TimingTask>();
            taskmanager = new TimingTaskManager();

            LoadTask(taskList);//加载配置的定时任务
            WcfFrame.Utility.Upgrade.ClientUpgradeManager.LoadTask(taskList);//加载升级定时任务

            if (taskList.Count > 0)
            {
                taskmanager.TaskList = taskList;
                taskmanager.Initialize();

                //MiddlewareLogHelper.WriterLog(LogType.TimingTaskLog, true, System.Drawing.Color.Blue, "定时任务已启动！");
            }
            else
            {
                taskmanager = null;
            }
        }
        /// <summary>
        /// 停止任务
        /// </summary>
        public static void StopTask()
        {
            if (taskmanager != null)
            {
                taskmanager.Dispose();
                
            }
        }

        private static List<TaskConfig> LoadXML()
        {
            List<TaskConfig> taskConfigList = new List<TaskConfig>();

            try
            {
                XmlDocument xmlDoc = new System.Xml.XmlDocument();
                xmlDoc.Load(MiddlewareTask.taskfile);

                XmlNodeList tasklist = xmlDoc.DocumentElement.SelectNodes("task");
                foreach (XmlNode xe in tasklist)
                {
                    TaskConfig taskconfig = new TaskConfig();
                    taskconfig.taskname = xe.Attributes["name"].Value;
                    taskconfig.qswitch = xe.Attributes["switch"].Value == "1" ? true : false;
                    taskconfig.execfrequency = (TimingTaskType)Convert.ToInt32(xe.Attributes["execfrequency"].Value);
                    string[] vals = xe.Attributes["shorttime"].Value.Split(':');
                    taskconfig.shorttime = new ShortTime(Convert.ToInt32(vals[0]), Convert.ToInt32(vals[1]), Convert.ToInt32(vals[2]));
                    taskconfig.serialorparallel = Convert.ToInt32(xe.Attributes["serialorparallel"].Value);
                    taskconfig.taskService = new List<TackServiceConfig>();

                    XmlNodeList servicelist = xe.SelectNodes("service");
                    foreach (XmlNode se in servicelist)
                    {
                        TackServiceConfig serviceconfig = new TackServiceConfig();
                        serviceconfig.pluginname = se.Attributes["pluginname"].Value;
                        serviceconfig.controller = se.Attributes["controller"].Value;
                        serviceconfig.method = se.Attributes["method"].Value;
                        serviceconfig.argument = se.Attributes["argument"].Value;

                        taskconfig.taskService.Add(serviceconfig);

                    }
                    taskConfigList.Add(taskconfig);
                }
            }
            catch (Exception e)
            {
                MiddlewareLogHelper.WriterLog(LogType.TimingTaskLog, true, System.Drawing.Color.Red, "加载定时任务配置文件错误！");
                MiddlewareLogHelper.WriterLog(LogType.TimingTaskLog, true, System.Drawing.Color.Red, e.Message);
            }
            return taskConfigList;
        }

        /// <summary>
        /// 设置开始或关闭任务
        /// </summary>
        public static void SettingTask(TaskConfig _taskconfig)
        {
            XmlDocument xmlDoc = new System.Xml.XmlDocument();
            xmlDoc.Load(MiddlewareTask.taskfile);
            XmlNode xn = xmlDoc.DocumentElement.SelectSingleNode("task[@name='"+_taskconfig.taskname+"']");
            if (xn != null)
            {
                xn.Attributes["switch"].Value = _taskconfig.qswitch ? "1" : "0";
            }
            xmlDoc.Save(MiddlewareTask.taskfile);
        }
     
        public static void ExecuteTask(TaskConfig _taskConfig)
        {
            TaskContent task = new TaskContent(_taskConfig);
            new Task(() =>
            {
                task.Excute(true);
            }).Start();
        }
    }

    /// <summary>
    /// 执行的任务内容
    /// </summary>
    public class TaskContent : ITimingTaskExcuter
    {
        TaskConfig taskConfig;
        public TaskContent(TaskConfig _taskConfig)
        {
            taskConfig = _taskConfig;
        }


        private ServiceResponseData InvokeWcfService(string wcfpluginname, string wcfcontroller, string wcfmethod)
        {
            return InvokeWcfService(wcfpluginname, wcfcontroller, wcfmethod, null);
        }

        private ServiceResponseData InvokeWcfService(string wcfpluginname, string wcfcontroller, string wcfmethod, Action<ClientRequestData> requestAction)
        {
            ClientLink wcfClientLink = ClientLinkManage.CreateConnection("localendpoint", wcfpluginname);
            //绑定LoginRight
            Action<ClientRequestData> _requestAction = ((ClientRequestData request) =>
            {
                request.LoginRight = new EFWCoreLib.CoreFrame.Business.SysLoginRight();
                if (requestAction != null)
                    requestAction(request);
            });
            ServiceResponseData retData = wcfClientLink.Request(wcfcontroller, wcfmethod, _requestAction);
            return retData;
        }

        public void ExcuteOnTime(DateTime dt)
        {
            Excute(false);
        }
        /// <summary>
        /// 执行
        /// </summary>
        /// <param name="isimmediately">是否马上</param>
        public void Excute(bool isimmediately)
        {
            if (taskConfig != null && (isimmediately == true || taskConfig.qswitch == true))
            {
                try
                {

                    MiddlewareLogHelper.WriterLog(LogType.TimingTaskLog, true, System.Drawing.Color.Blue, string.Format("任务【{0}】准备开始执行...", taskConfig.taskname));
                    if (taskConfig.serialorparallel == 0)//串行
                    {
                        foreach (var item in taskConfig.taskService)
                        {
                            MiddlewareLogHelper.WriterLog(LogType.TimingTaskLog, true, System.Drawing.Color.Blue, string.Format("正在执行服务{0}/{1}/{2}/{3}", item.pluginname, item.controller, item.method, item.argument));
                            ServiceResponseData retjson = InvokeWcfService(
                                item.pluginname
                                , item.controller
                                , item.method
                                , (ClientRequestData request) =>
                                {
                                    request.SetJsonData(item.argument);
                                });
                            string txtResult = retjson.GetJsonData();
                            MiddlewareLogHelper.WriterLog(LogType.TimingTaskLog, true, System.Drawing.Color.Blue, string.Format("服务执行完成，返回结果：{0}", txtResult));
                        }
                        taskConfig.exectimes += 1;
                        MiddlewareLogHelper.WriterLog(LogType.TimingTaskLog, true, System.Drawing.Color.Blue, string.Format("任务【{0}】执行完成！", taskConfig.taskname));
                    }
                    else if (taskConfig.serialorparallel == 1)//并行
                    {
                        List<Task> tasks = new List<Task>();
                        foreach (var item in taskConfig.taskService)
                        {
                            tasks.Add(Task.Factory.StartNew(() =>
                            {
                                MiddlewareLogHelper.WriterLog(LogType.TimingTaskLog, true, System.Drawing.Color.Blue, string.Format("正在执行服务{0}/{1}/{2}/{3}", item.pluginname, item.controller, item.method, item.argument));
                                ServiceResponseData retjson = InvokeWcfService(
                                    item.pluginname
                                    , item.controller
                                    , item.method
                                    , (ClientRequestData request) =>
                                    {
                                        request.SetJsonData(item.argument);
                                    });
                                string txtResult = retjson.GetJsonData();
                                MiddlewareLogHelper.WriterLog(LogType.TimingTaskLog, true, System.Drawing.Color.Blue, string.Format("服务执行完成，返回结果：{0}", txtResult));
                            }));
                        }
                        //并行执行
                        Task.Factory.ContinueWhenAll(tasks.ToArray(), completedTasks =>
                        {
                            taskConfig.exectimes += 1;
                            MiddlewareLogHelper.WriterLog(LogType.TimingTaskLog, true, System.Drawing.Color.Blue, string.Format("任务【{0}】执行完成！", taskConfig.taskname));
                        });
                    }
                }
                catch (Exception err)
                {
                    MiddlewareLogHelper.WriterLog(LogType.TimingTaskLog, true, System.Drawing.Color.Red, string.Format("任务【{0}】执行出错！", taskConfig.taskname));
                    MiddlewareLogHelper.WriterLog(LogType.TimingTaskLog, true, System.Drawing.Color.Red, err.Message);
                }
            }
        } 
    }


    /// <summary>
    /// 任务配置
    /// </summary>
    public class TaskConfig
    {
        /// <summary>
        /// 任务名称
        /// </summary>
        public string taskname { get; set; }
        /// <summary>
        /// 开关
        /// </summary>
        public bool qswitch{get;set;}
        /// <summary>
        /// 执行频次
        /// </summary>
        public TimingTaskType execfrequency { get; set; }
        /// <summary>
        /// 执行具体时刻
        /// </summary>
        public ShortTime shorttime { get; set; }

        /// <summary>
        /// 0：串行执行，1：并行执行
        /// </summary>
        public int serialorparallel { get; set; }

        public List<TackServiceConfig> taskService { get; set; }
        /// <summary>
        /// 执行次数
        /// </summary>
        public int exectimes { get; set; }

        public string execfrequencyName
        {
            get
            {
                switch (execfrequency)
                {
                    case TimingTaskType.PerHour:
                        return "每小时一次";
                    case TimingTaskType.PerDay:
                        return "每天一次";
                    case TimingTaskType.PerWeek:
                        return "每周一次";
                    case TimingTaskType.PerMonth:
                        return "每月一次";
                }

                return "";
            }
        }

        public string serialorparallelName
        {
            get
            {
                if (serialorparallel == 0)
                {
                    return "串行";
                }
                else if (serialorparallel == 1)
                {
                    return "并行";
                }
                return "";
            }
        }

        public string shorttimeName
        {
            get
            {
                return shorttime.Hour.ToString().PadLeft(2, '0') + ":" + shorttime.Minute.ToString().PadLeft(2, '0') + ":" + shorttime.Second.ToString().PadLeft(2, '0');
            }
        }
    }

    public class TackServiceConfig
    {
        public string pluginname { get; set; }
        public string controller { get; set; }
        public string method { get; set; }
        public string argument { get; set; }
    }
}
