using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using EFWCoreLib.CoreFrame.Init;
using EFWCoreLib.WcfFrame;
using EFWCoreLib.WcfFrame.DataSerialize;
using System.Windows.Forms;

namespace TestWcfPerformance
{
    //测试WCF在互联网下请求数据的性能
    class Program
    {
		static int connCount=100;
		static int time=100;
        static void Main(string[] args)
        {
            try
            {
                //TestWebClient();
                //Console.Read();
                //TestConcurrency();


                //Application.Run(new frmClient());

                Console.WriteLine("输入并发连接数(默认100)：");
                connCount = Convert.ToInt32(Console.ReadLine());
                Console.WriteLine("输入每次请求间隔时间(默认100微秒)：");
                time = Convert.ToInt32(Console.ReadLine());
                Console.WriteLine("#回车开始执行#");
                Console.Read();
                StartThread();

            }
            catch (Exception err)
            {
                Console.WriteLine(err.Message);
            }
        }

        static void StartThread()
        {
            int num = connCount;
            while (num > 0)
            {
                num -= 1;
                new Thread(new ThreadStart(TestConcurrency)).Start();
                Thread.Sleep(100);
            }
        }

        static void TestConcurrency()
        {
            try
            {
                ClientLink clientlink = new ClientLink("TestWcfPerformance", "Books.Service");
                clientlink.CreateConnection();
                int num = 100;
                while (num > 0)
                {
                    //num -= 1;
                    clientlink.Request("bookWcfController", "GetDiseaseData", null);
                    Thread.Sleep(time);
                }
                clientlink.Dispose();
            }
            catch (Exception err)
            {
                Console.WriteLine(err.Message);
            }
        }

        static void TestWebClient()
        {
            //创建对象
            //ReplyClientCallBack callback = new ReplyClientCallBack();
            ClientLink clientlink = new ClientLink("TestWcfPerformance", "Books.Service");

            begintime();
            //2.创建连接

            clientlink.CreateConnection();
            Console.WriteLine("2.创建连接时间(毫秒)：" + endtime());

            //Console.WriteLine("输入请求数据条数：");
            //string num = Console.ReadLine();
            string num = "100";
            begintime();

            //Action<ClientRequestData> requestAction = ((ClientRequestData request) =>
            //{
            //    //request.Iscompressjson = false;
            //    //request.Isencryptionjson = false;
            //    //request.Serializetype = WcfFrame.SDMessageHeader.SerializeType.Newtonsoft;
            //    request.AddData(num);
            //});
            ////3.同步请求数据
            //clientlink.Request("Books.Service@bookWcfController", "Test191", requestAction);
            //Console.WriteLine("3.请求数据时间(毫秒)：" + endtime());

            //begintime();
            ////3.同步请求数据
            //clientlink.Request("Books.Service@bookWcfController", "Test191", requestAction);
            //Console.WriteLine("3.请求数据时间(毫秒)：" + endtime());

            Console.Read();
            string s;
            begintime();
            s = clientlink.UpLoadFile(@"D:\DCWriter.rar", (delegate (int _num)
            {
                Console.WriteLine("4.文件上传进度：%" + _num);
            }));
            Console.WriteLine("4.文件上传时间(毫秒)：" + endtime() + "|" + s);

            begintime();
            s = clientlink.DownLoadFile("DCWriter.rar", (delegate (int _num)
            {
                Console.WriteLine("5.文件下载进度：%" + _num);
            }));
            Console.WriteLine("5.文件下载时间(毫秒)：" + endtime() + "|" + s);

            begintime();
            //5.关闭连接
            clientlink.UnConnection();
            Console.WriteLine("6.关闭连接时间(毫秒)：" + endtime());

            Console.ReadLine();
        }

        static void TestApp()
        {
            /*
            begintime();
            //1.初始化
            AppGlobal.AppStart();
            Console.WriteLine("1.初始化程序时间(毫秒)：" + endtime());

            begintime();
            //2.创建连接
            ReplyClientCallBack callback = new ReplyClientCallBack();
            WcfClientManage.CreateConnection(callback);
            Console.WriteLine("2.创建连接时间(毫秒)：" + endtime());

            Console.WriteLine("输入请求数据条数：");
            //string num = Console.ReadLine();
            string num = "100";
            begintime();
            //3.同步请求数据
            string retjson = WcfClientManage.Request("Books_Wcf@bookWcfController", "Test191", "[" + num + "]");
            Console.WriteLine("3.请求数据时间(毫秒)：" + endtime());

            begintime();
            //3.同步请求数据
            retjson = WcfClientManage.Request("Books_Wcf@bookWcfController", "Test191", "[" + num + "]");
            Console.WriteLine("3.请求数据时间(毫秒)：" + endtime());

            //for (int i = 0; i < 10; i++)
            //{
            //    begintime();
            //    retjson = WcfClientManage.Request("Books_Wcf@bookWcfController", "Test191", "[" + num + "]");
            //    object Result = JsonConvert.DeserializeObject(retjson);//测试反序列化
            //    System.Data.DataTable dt = JsonConvert.DeserializeObject<DataTable>(((Newtonsoft.Json.Linq.JObject)(Result))["data"].ToString());
            //    Console.WriteLine("3.请求数据时间(毫秒)：" + endtime());
            //    System.Threading.Thread.Sleep(1000);
            //}
            //3.异步请求数据
            //WcfClientManage.RequestAsync("Books_Wcf@bookWcfController", "GetBooks", "[" + num + "]", new Action<string>(
            //    (json) =>
            //    {
            //        Console.WriteLine("3.请求数据时间(毫秒)：" + endtime());
            //    }
            //    ));

            begintime();
            string s = WcfClientManage.UpLoadFile(@"D:\CloudHISDB.rar", (delegate(int _num)
            {
                Console.WriteLine("4.文件上传进度：%" + _num);
            }));
            Console.WriteLine("4.文件上传时间(毫秒)：" + endtime() + "|" + s);

            begintime();
            s = WcfClientManage.DownLoadFile("b83ec24f-3750-420e-9200-f411578a8fe7.exe", (delegate(int _num)
            {
                Console.WriteLine("4.文件下载进度：%" + _num);
            }));
            Console.WriteLine("4.文件下载时间(毫秒)：" + endtime() + "|" + s);
            System.Threading.Thread.Sleep(5000);

            Console.Read();

            begintime();
            s = WcfClientManage.UpLoadFile(@"c:\ora.sql");
            Console.WriteLine("4.文件上传时间(毫秒)：" + endtime() + "|" + s);

            Console.Read();
            Console.Read();

            begintime();
            s = WcfClientManage.DownLoadFile("ora.sql");
            Console.WriteLine("4.文件下载时间(毫秒)：" + endtime() + "|" + s);

            Console.Read();
            Console.Read();

            begintime();
            //4.回调消息
            callback.ReplyClientAction = new Action<string>((json) =>
            {

            });
            Console.WriteLine("5.回调消息时间(毫秒)：" + endtime());

            //begintime();
            ////5.关闭连接
            //WcfClientManage.UnConnection();
            //Console.WriteLine("6.关闭连接时间(毫秒)：" + endtime());
            */
            Console.Read();
        }


        static DateTime begindate;
        static void begintime()
        {
            begindate = DateTime.Now;
        }
        //返回毫秒
        static double endtime()
        {
            return DateTime.Now.Subtract(begindate).TotalMilliseconds;
        }
    }
}
