using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EFWCoreLib.CoreFrame.Common;
using EFWCoreLib.CoreFrame.Init;
using EFWCoreLib.WcfFrame.DataSerialize;

namespace EFWCoreLib.WcfFrame.ServerManage
{
    /// <summary>
    /// 文件传输管理
    /// </summary>
    public class FileManage
    {
        public static string filebufferpath= AppGlobal.AppRootPath + @"FileStore\filebuffer\";//缓存文件路径
        public static string clientupgradepath= AppGlobal.AppRootPath + @"FileStore\ClientUpgrade\";//客户端升级包路径
        public static string serverupgradepath = AppGlobal.AppRootPath + @"FileStore\ServerUpgrade\";//服务端升级包路径

        private static void getprogress(long filesize, long readnum, ref int progressnum)
        {
            if (filesize <= 0)
            {
                return;
            }
            //decimal percent = Convert.ToDecimal(100 / Convert.ToDecimal(filesize / bufferlen));
            //progressnum = progressnum + percent > 100 ? 100 : progressnum + percent;
            decimal percent = Convert.ToDecimal(readnum) / Convert.ToDecimal(filesize) * 100;
            progressnum = Convert.ToInt32(Math.Ceiling(percent));
        }
        private static void getupdownprogress(Stream file, long flength, Action<int> action)
        {
            new Action<Stream,long, Action<int>>(delegate (Stream _file, long _flength, Action<int> _action)
            {
                try
                {
                    int oldnum = 0;
                    int num = 0;

                    while (num < 100)
                    {
                        if (_file == null || !_file.CanRead)
                            break;

                        getprogress(_flength - 1, _file.Position, ref num);
                        if (oldnum < num)
                        {
                            oldnum = num;
                            if (num < 100)
                                _action.BeginInvoke(num, null, null);
                        }

                        System.Threading.Thread.Sleep(100);
                    }

                    _action(100);
                }
                catch (Exception e)
                {
                    throw new Exception(e.Message + "\n获取文件进度失败！");
                }

            }).BeginInvoke(file, flength, action, null, null);
        }
        /// <summary>
        /// 上传文件
        /// </summary>
        /// <param name="filedata"></param>
        /// <returns></returns>
        public static UpFileResult UpLoadFile(UpFile filedata)
        {
            try
            {
                if (WcfGlobal.IsDebug)
                {
                    ShowHostMsg(Color.Black, DateTime.Now, "客户端[" + filedata.clientId + "]准备上传文件...");
                    //获取进度
                    getupdownprogress(filedata.FileStream,filedata.FileSize, (delegate (int _num)
                    {
                        ShowHostMsg(Color.Black, DateTime.Now, "客户端[" + filedata.clientId + "]上传文件进度：%" + _num);
                    }));
                }

                UpFileResult result = new UpFileResult();

                if (filedata.FileType == 0)//0：filebuffer目录  1：Upgrade升级包 2：Mongodb
                {
                    result = UpLoadfilebuffer(filedata);
                }
                else if (filedata.FileType == 1)
                {
                    result = UpLoadUpgrade(filedata);
                }
                else if (filedata.FileType == 2)
                {
                    result = UpLoadMongodb(filedata);
                }
                else
                {
                    result = UpLoadfilebuffer(filedata);
                }

                if (WcfGlobal.IsDebug)
                    ShowHostMsg(Color.Green, DateTime.Now, "客户端[" + filedata.clientId + "]上传文件完成");

                return result;
            }
            catch (Exception err)
            {
                //记录错误日志
                EFWCoreLib.CoreFrame.EntLib.ZhyContainer.CreateException().HandleException(err, "HISPolicy");
                UpFileResult result = new UpFileResult();
                result.IsSuccess = false;
                return result;
            }
        }
        /// <summary>
        /// 上传文件，到文件缓存目录
        /// </summary>
        /// <param name="filedata"></param>
        /// <returns></returns>
        private static UpFileResult UpLoadfilebuffer(UpFile filedata)
        {
            if (!Directory.Exists(filebufferpath))
            {
                Directory.CreateDirectory(filebufferpath);
            }
            string _filename = DateTime.Now.Ticks.ToString() + filedata.FileExt;//生成唯一文件名，防止文件名相同会覆盖
            FileStream fs = new FileStream(filebufferpath + _filename, FileMode.Create, FileAccess.Write);
            using (fs)
            {
                int bufferlen = 4096;
                int count = 0;
                byte[] buffer = new byte[bufferlen];
                while ((count = filedata.FileStream.Read(buffer, 0, bufferlen)) > 0)
                {
                    fs.Write(buffer, 0, count);
                }

                //清空缓冲区
                //fs.Flush();
                //关闭流
                //fs.Close();
            }


            UpFileResult result = new UpFileResult();
            result.IsSuccess = true;
            result.Message = _filename;//返回保存文件

            return result;
        }
        /// <summary>
        /// 上传文件,程序升级包
        /// </summary>
        /// <param name="filedata"></param>
        /// <returns></returns>
        private static UpFileResult UpLoadUpgrade(UpFile filedata)
        {



            UpFileResult result = new UpFileResult();
            result.IsSuccess = true;
            result.Message = "升级包上传成功！";

            return result;
        }
        /// <summary>
        /// 上传文件，保存到Mongodb
        /// </summary>
        /// <param name="filedata"></param>
        /// <returns></returns>
        private static UpFileResult UpLoadMongodb(UpFile filedata)
        {



            UpFileResult result = new UpFileResult();
            result.IsSuccess = true;
            result.Message = "数据上传到Mongodb成功！";

            return result;
        }
        /// <summary>
        /// 下载文件
        /// </summary>
        /// <param name="filedata"></param>
        /// <returns></returns>
        public static DownFileResult DownLoadFile(DownFile filedata)
        {
            try
            {
                if (WcfGlobal.IsDebug)
                    ShowHostMsg(Color.Black, DateTime.Now, "客户端[" + filedata.clientId + "]准备下载文件...");

                MemoryStream ms = new MemoryStream();

                DownFileResult result = new DownFileResult();

                if (filedata.FileType == 0)//0：filebuffer目录  1：Upgrade升级包 2：Mongodb
                {
                    result = DownLoadfilebuffer(filedata, ref ms);
                }
                else if (filedata.FileType == 1)
                {
                    result = DownLoadUpgrade(filedata, ref ms);
                }
                else if (filedata.FileType == 2)
                {
                    result = DownLoadMongodb(filedata, ref ms);
                }
                else
                {
                    result = DownLoadfilebuffer(filedata, ref ms);
                }

                if (WcfGlobal.IsDebug)
                {
                    //获取进度
                    getupdownprogress(ms, result.FileSize, (delegate (int _num)
                    {
                        ShowHostMsg(Color.Black, DateTime.Now, "客户端[" + filedata.clientId + "]下载文件进度：%" + _num);
                    }));
                }


                if (WcfGlobal.IsDebug)
                    ShowHostMsg(Color.Green, DateTime.Now, "客户端[" + filedata.clientId + "]下载文件完成");

                //ms.Close();
                return result;
            }
            catch (Exception err)
            {
                //记录错误日志
                EFWCoreLib.CoreFrame.EntLib.ZhyContainer.CreateException().HandleException(err, "HISPolicy");

                DownFileResult result = new DownFileResult();
                result.IsSuccess = false;
                return result;
            }
        }
        /// <summary>
        /// 下载文件，从文件缓存目录
        /// </summary>
        /// <param name="filedata"></param>
        /// <param name="ms"></param>
        /// <returns></returns>
        private static DownFileResult DownLoadfilebuffer(DownFile filedata, ref MemoryStream ms)
        {
            DownFileResult result = new DownFileResult();

            if (ms == null)
                ms = new MemoryStream();

            string path = filebufferpath + filedata.FileName;
            if (!File.Exists(path))
            {
                result.IsSuccess = false;
                result.FileSize = 0;
                result.Message = "服务器不存在此文件";
                result.FileStream = new MemoryStream();
                return result;
            }

            FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read);
            fs.CopyTo(ms);

            ms.Position = 0;  //重要，不为0的话，客户端读取有问题
            result.IsSuccess = true;
            result.FileSize = ms.Length;
            result.FileStream = ms;
            fs.Flush();
            fs.Close();

            return result;
        }
        /// <summary>
        /// 下载文件，从升级包中
        /// </summary>
        /// <param name="filedata"></param>
        /// <param name="ms"></param>
        /// <returns></returns>
        private static DownFileResult DownLoadUpgrade(DownFile filedata, ref MemoryStream ms)
        {
            DownFileResult result = new DownFileResult();
            if (ms == null)
                ms = new MemoryStream();

            string path = clientupgradepath + filedata.FileName;
            if (!File.Exists(path))
            {
                result.IsSuccess = false;
                result.FileSize = 0;
                result.Message = "服务器不存在此文件";
                result.FileStream = ms;
                return result;
            }

            FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read);
            fs.CopyTo(ms);

            ms.Position = 0;  //重要，不为0的话，客户端读取有问题
            result.IsSuccess = true;
            result.FileSize = ms.Length;
            result.FileStream = ms;
            fs.Flush();
            fs.Close();

            return result;
        }
        /// <summary>
        /// 下载文件,从Mongodb数据库中
        /// </summary>
        /// <param name="filedata"></param>
        /// <param name="ms"></param>
        /// <returns></returns>
        private static DownFileResult DownLoadMongodb(DownFile filedata, ref MemoryStream ms)
        {
            DownFileResult result = new DownFileResult();
            if (ms == null)
                ms = new MemoryStream();

            return result;
        }

        private static void ShowHostMsg(Color clr, DateTime time, string text)
        {
            MiddlewareLogHelper.WriterLog(LogType.MidLog, true, clr, text);
        }
    }
}
