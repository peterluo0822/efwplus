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
    public class FileManage
    {
        public static string filebufferpath= AppGlobal.AppRootPath + @"filebuffer\";

        private static void getprogress(long filesize, long readnum, ref int progressnum)
        {
            //decimal percent = Convert.ToDecimal(100 / Convert.ToDecimal(filesize / bufferlen));
            //progressnum = progressnum + percent > 100 ? 100 : progressnum + percent;
            decimal percent = Convert.ToDecimal(readnum) / Convert.ToDecimal(filesize) * 100;
            progressnum = Convert.ToInt32(Math.Ceiling(percent));
        }
        private static void getupdownprogress(Stream file, long flength, Action<int> action)
        {
            new Action<Stream, long, Action<int>>(delegate (Stream _file, long _flength, Action<int> _action)
            {
                try
                {
                    int oldnum = 0;
                    int num = 0;

                    while (num != 100)
                    {
                        getprogress(_flength - 1, _file.Position, ref num);
                        if (oldnum < num)
                        {
                            oldnum = num;
                            _action.BeginInvoke(num, null, null);
                        }
                        System.Threading.Thread.Sleep(100);
                    }
                    //_action(100);
                }
                catch (Exception e)
                {
                    throw new Exception(e.Message + "\n获取文件进度失败！");
                }

            }).BeginInvoke(file, flength, action, null, null);
        }

        public static UpFileResult UpLoadFile(UpFile filedata)
        {
            FileStream fs = null;
            try
            {
                if (WcfGlobal.IsDebug)
                    ShowHostMsg(Color.Black, DateTime.Now, "客户端[" + filedata.clientId + "]准备上传文件...");

                UpFileResult result = new UpFileResult();
                if (!Directory.Exists(filebufferpath))
                {
                    Directory.CreateDirectory(filebufferpath);
                }

                string _filename = DateTime.Now.Ticks.ToString() + filedata.FileExt;//生成唯一文件名，防止文件名相同会覆盖
                fs = new FileStream(filebufferpath + _filename, FileMode.Create, FileAccess.Write);

                if (WcfGlobal.IsDebug)
                {
                    //获取进度
                    getupdownprogress(filedata.FileStream, filedata.FileSize, (delegate (int _num)
                    {
                        ShowHostMsg(Color.Black, DateTime.Now, "客户端[" + filedata.clientId + "]上传文件进度：%" + _num);
                    }));
                }
                //int oldprogressnum = 0;
                //int progressnum = 0;
                int bufferlen = 4096;
                int count = 0;
                //long readnum = 0;
                byte[] buffer = new byte[bufferlen];
                while ((count = filedata.FileStream.Read(buffer, 0, bufferlen)) > 0)
                {
                    fs.Write(buffer, 0, count);
                    //readnum += count;
                    ////获取上传进度
                    //getprogress(filedata.FileSize, readnum, ref progressnum);
                    //if (oldprogressnum < progressnum)
                    //{
                    //    oldprogressnum = progressnum;
                    //    if (progressDic_Up.ContainsKey(filedata.UpKey))
                    //    {
                    //        progressDic_Up[filedata.UpKey] = progressnum;
                    //    }
                    //    else
                    //    {
                    //        progressDic_Up.Add(filedata.UpKey, progressnum);
                    //    }
                    //    if (WcfServerManage.IsDebug)
                    //    {
                    //        ShowHostMsg(Color.Black, DateTime.Now, "客户端[" + filedata.clientId + "]上传文件进度：%" + oldprogressnum);
                    //    }
                    //}
                }

                //清空缓冲区
                fs.Flush();
                //关闭流
                fs.Close();

                if (WcfGlobal.IsDebug)
                    ShowHostMsg(Color.Green, DateTime.Now, "客户端[" + filedata.clientId + "]上传文件完成");

                result.IsSuccess = true;
                result.Message = _filename;//返回保存文件

                return result;
            }
            catch (Exception err)
            {
                if (fs != null)
                {
                    fs.Flush();
                    fs.Close();
                }
                //记录错误日志
                EFWCoreLib.CoreFrame.EntLib.ZhyContainer.CreateException().HandleException(err, "HISPolicy");
                UpFileResult result = new UpFileResult();
                result.IsSuccess = false;
                return result;
            }
        }

        //下载文件
        public static DownFileResult DownLoadFile(DownFile filedata)
        {
            FileStream fs = null;
            try
            {
                if (WcfGlobal.IsDebug)
                    ShowHostMsg(Color.Black, DateTime.Now, "客户端[" + filedata.clientId + "]准备下载文件...");

                DownFileResult result = new DownFileResult();

                string path = filebufferpath + filedata.FileName;

                if (!File.Exists(path))
                {
                    result.IsSuccess = false;
                    result.FileSize = 0;
                    result.Message = "服务器不存在此文件";
                    result.FileStream = new MemoryStream();
                    return result;
                }
                Stream ms = new MemoryStream();
                fs = new FileStream(path, FileMode.Open, FileAccess.Read);
                fs.CopyTo(ms);
                ms.Position = 0;  //重要，不为0的话，客户端读取有问题
                result.IsSuccess = true;
                result.FileSize = ms.Length;
                result.FileStream = ms;

                if (WcfGlobal.IsDebug)
                {
                    //获取进度
                    getupdownprogress(result.FileStream, result.FileSize, (delegate (int _num)
                    {
                        ShowHostMsg(Color.Black, DateTime.Now, "客户端[" + filedata.clientId + "]下载文件进度：%" + _num);
                    }));
                }


                fs.Flush();
                fs.Close();

                if (WcfGlobal.IsDebug)
                    ShowHostMsg(Color.Green, DateTime.Now, "客户端[" + filedata.clientId + "]下载文件完成");

                return result;
            }
            catch (Exception err)
            {
                if (fs != null)
                {
                    fs.Flush();
                    fs.Close();
                }
                //记录错误日志
                EFWCoreLib.CoreFrame.EntLib.ZhyContainer.CreateException().HandleException(err, "HISPolicy");

                DownFileResult result = new DownFileResult();
                result.IsSuccess = false;
                return result;
            }
        }

        private static void ShowHostMsg(Color clr, DateTime time, string text)
        {
            MiddlewareLogHelper.WriterLog(LogType.MidLog, true, clr, text);
        }
    }
}
