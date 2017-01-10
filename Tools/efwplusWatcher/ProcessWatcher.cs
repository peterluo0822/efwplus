using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace efwplusWatcher
{
    public class ProcessWatcher
    {
        //字段
        private string[] _processAddress;
        private object _lockerForLog = new object();
        private string _logPath = string.Empty;
        private List<Thread> threadList;

        /// <summary>
        /// 构造函数
        /// </summary>
        public ProcessWatcher()
        {
            try
            {
                //读取监控进程全路径
                string strProcessAddress = ConfigurationManager.AppSettings["ProcessAddress"].ToString();
                if (strProcessAddress.Trim() != "")
                {
                    this._processAddress = strProcessAddress.Split(',');
                }
                else
                {
                    throw new Exception("读取配置档ProcessAddress失败，ProcessAddress为空！");
                }

                //创建日志目录
                this._logPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "CocoWatcherLog");
                if (!Directory.Exists(_logPath))
                {
                    Directory.CreateDirectory(_logPath);
                }
            }
            catch (Exception ex)
            {
                this.SaveLog("Watcher()初始化出错！错误描述为：" + ex.Message.ToString());
            }
        }


        /// <summary>
        /// 启动服务
        /// </summary>
        /// <param name="args"></param>
        public void OnStart()
        {
            try
            {
                threadList = new List<Thread>();
                this.StartWatch();
            }
            catch (Exception ex)
            {
                this.SaveLog("OnStart() 出错，错误描述：" + ex.Message.ToString());
            }
        }


        /// <summary>
        /// 停止服务
        /// </summary>
        public void OnStop()
        {
            try
            {
                if (threadList != null)
                {
                    foreach(var t in threadList)
                    {
                        t.Abort();
                    }
                }
            }
            catch (Exception ex)
            {
                this.SaveLog("OnStop 出错，错误描述：" + ex.Message.ToString());
            }
        }


        /// <summary>
        /// 开始监控
        /// </summary>
        private void StartWatch()
        {
            if (this._processAddress != null)
            {
                if (this._processAddress.Length > 0)
                {
                    foreach (string str in _processAddress)
                    {
                        if (str.Trim() != "")
                        {
                            if (File.Exists(str.Trim()))
                            {
                                this.ScanProcessList(str.Trim());
                            }
                        }
                    }
                }
            }
        }


        /// <summary>
        /// 扫描进程列表，判断进程对应的全路径是否与指定路径一致
        /// 如果一致，说明进程已启动
        /// 如果不一致，说明进程尚未启动
        /// </summary>
        /// <param name="strAddress"></param>
        private void ScanProcessList(string address)
        {
            Process[] arrayProcess = Process.GetProcesses();
            foreach (Process p in arrayProcess)
            {
                //System、Idle进程会拒绝访问其全路径
                if (p.ProcessName != "System" && p.ProcessName != "Idle")
                {
                    try
                    {
                        if (this.FormatPath(address) == this.FormatPath(p.MainModule.FileName.ToString()))
                        {
                            //进程已启动
                            this.WatchProcess(p, address);
                            return;
                        }
                    }
                    catch
                    {
                        //拒绝访问进程的全路径
                        this.SaveLog("进程(" + p.Id.ToString() + ")(" + p.ProcessName.ToString() + ")拒绝访问全路径！");
                    }
                }
            }

            //进程尚未启动
            Process process = new Process();
            process.StartInfo.WorkingDirectory = new FileInfo(address).DirectoryName;
            process.StartInfo.FileName = address;
            process.Start();
            this.WatchProcess(process, address);
        }


        /// <summary>
        /// 监听进程
        /// </summary>
        /// <param name="p"></param>
        /// <param name="address"></param>
        private void WatchProcess(Process process, string address)
        {
            ProcessRestart objProcessRestart = new ProcessRestart(process, address);
            Thread thread = new Thread(new ThreadStart(objProcessRestart.RestartProcess));
            thread.Start();

            threadList.Add(thread);
        }


        /// <summary>
        /// 格式化路径
        /// 去除前后空格
        /// 去除最后的"\"
        /// 字母全部转化为小写
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        private string FormatPath(string path)
        {
            return path.ToLower().Trim().TrimEnd('\\');
        }


        /// <summary>
        /// 记录日志
        /// </summary>
        /// <param name="content"></param>
        public void SaveLog(string content)
        {
            try
            {
                lock (_lockerForLog)
                {
                    FileStream fs;
                    fs = new FileStream(Path.Combine(this._logPath, DateTime.Now.ToString("yyyyMMdd") + ".log"), FileMode.OpenOrCreate);
                    StreamWriter streamWriter = new StreamWriter(fs);
                    streamWriter.BaseStream.Seek(0, SeekOrigin.End);
                    streamWriter.WriteLine("[" + DateTime.Now.ToString() + "]：" + content);
                    streamWriter.Flush();
                    streamWriter.Close();
                    fs.Close();
                }
            }
            catch
            {
            }
        }

    }


    public class ProcessRestart
    {
        //字段
        private Process _process;
        private string _address;


        /// <summary>
        /// 构造函数
        /// </summary>
        public ProcessRestart()
        { }


        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="process"></param>
        /// <param name="address"></param>
        public ProcessRestart(Process process, string address)
        {
            this._process = process;
            this._address = address;
        }


        /// <summary>
        /// 重启进程
        /// </summary>
        public void RestartProcess()
        {
            try
            {
                while (true)
                {
                    this._process.WaitForExit();
                    this._process.Close();    //释放已退出进程的句柄
                    this._process.StartInfo.FileName = this._address;
                    this._process.Start();

                    Thread.Sleep(1000);
                }
            }
            catch (Exception ex)
            {
                ProcessWatcher objProcessWatcher = new ProcessWatcher();
                objProcessWatcher.SaveLog("RestartProcess() 出错，监控程序已取消对进程("
                    + this._process.Id.ToString() + ")(" + this._process.ProcessName.ToString()
                    + ")的监控，错误描述为：" + ex.Message.ToString());
            }
        }


    }
}
