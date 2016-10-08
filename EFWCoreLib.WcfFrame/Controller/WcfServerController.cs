using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using EFWCoreLib.CoreFrame.Business;
using EFWCoreLib.WcfFrame.DataSerialize;
using System.Threading;
using System.Diagnostics;

namespace EFWCoreLib.WcfFrame.ServerController
{
    /// <summary>
    /// WCF控制器服务端基类
    /// </summary>
    public class WcfServerController : AbstractController
    {
        private SysLoginRight _loginRight = null;
        protected override SysLoginRight GetUserInfo()
        {
            if (_loginRight != null)
                return _loginRight;
            return base.GetUserInfo();
        }
        /// <summary>
        /// 客户端传送户权限
        /// </summary>
        /// <param name="loginRight"></param>
        public void BindLoginRight(SysLoginRight loginRight)
        {
            _loginRight = loginRight;
            if (loginRight != null)
                oleDb.WorkId = loginRight.WorkId;//重新绑定workid，因为wcf服务的workid每次调用都是客户端传递过来的
            else
                oleDb.WorkId = 0;
        }

         /// <summary>
        /// 创建BaseWCFController的实例
        /// </summary>
        public WcfServerController()
        {
            
        }

        /// <summary>
        /// 初始化全局web服务参数对象
        /// </summary>
        public virtual void Init() { }


        /// <summary>
        /// 客户端传递的参数
        /// </summary>
        public ClientRequestData requestData
        {
            get;
            set;
        }

        /// <summary>
        /// 服务输出数据
        /// </summary>
        public ServiceResponseData responseData {
            get;
            set;
        }

        #region CHDEP通讯，连接池方式
        private ClientLinkPool fromPoolGetClientLink(string wcfpluginname, out ClientLink clientlink, out int? index)
        {
            ClientLinkPool pool = ClientLinkPoolCache.GetClientPool("wcfserver");
            //获取的池子索引
            index = null;
            clientlink = null;
            //是否超时
            bool isouttime = false;
            //超时计时器
            Stopwatch sw = new Stopwatch();
            sw.Start();
            while (true)
            {
                bool isReap = true;
                //先判断池子中是否有此空闲连接
                if (pool.GetFreePoolNums(wcfpluginname) > 0)
                {
                    isReap = false;
                    clientlink = pool.GetClientLink(wcfpluginname);
                    if (clientlink != null)
                    {
                        index = clientlink.Index;
                    }
                }
                //如果没有空闲连接判断是否池子是否已满，未满，则创建新连接并装入连接池
                if (clientlink == null && !pool.IsPoolFull)
                {
                    //装入连接池
                    bool flag = pool.AddPool(wcfpluginname, out clientlink, out index);
                }

                //如果当前契约无空闲连接，并且队列已满，并且非当前契约有空闲，则踢掉一个非当前契约
                if (clientlink == null && pool.IsPoolFull && pool.GetFreePoolNums(wcfpluginname) == 0 && pool.GetUsedPoolNums(wcfpluginname) != 500)
                {
                    //创建新连接
                    pool.RemovePoolOneNotAt(wcfpluginname, out clientlink, out index);
                }

                if (clientlink != null)
                    break;

                //如果还未获取连接判断是否超时30秒，如果超时抛异常
                if (sw.Elapsed >= new TimeSpan(30 * 1000 * 10000))
                {
                    isouttime = true;
                    break;
                }
                else
                {
                    Thread.Sleep(100);
                }
            }
            sw.Stop();
            sw = null;

            if (isouttime)
            {
                throw new Exception("获取连接池中的连接超时");
            }

            return pool;
        }

        public ServiceResponseData InvokeWcfService(string wcfpluginname, string wcfcontroller, string wcfmethod)
        {
            return InvokeWcfService(wcfpluginname, wcfcontroller, wcfmethod, null);
        }

        public ServiceResponseData InvokeWcfService(string wcfpluginname, string wcfcontroller, string wcfmethod, Action<ClientRequestData> requestAction)
        {

            //获取的池子索引
            int? index = null;
            ClientLink clientlink = null;
            ClientLinkPool pool = fromPoolGetClientLink(wcfpluginname, out clientlink, out index);
            ServiceResponseData retData = new ServiceResponseData();

            try
            {
                //Thread.Sleep(2000);
                //ClientLink wcfClientLink = ClientLinkManage.CreateConnection(wcfpluginname);
                retData = clientlink.Request(wcfcontroller, wcfmethod, requestAction);
            }
            catch (Exception ex) { throw ex; }
            finally
            {
                if (index != null)
                    pool.ReturnPool(wcfpluginname, (int)index);
            }
            return retData;
        }

        public IAsyncResult InvokeWcfServiceAsync(string wcfpluginname, string wcfcontroller, string wcfmethod, Action<ClientRequestData> requestAction, Action<ServiceResponseData> responseAction)
        {
            //获取的池子索引
            int? index = null;
            ClientLink clientlink = null;
            ClientLinkPool pool = fromPoolGetClientLink(wcfpluginname, out clientlink, out index);

            IAsyncResult result = null;
            try
            {
                //ClientLink wcfClientLink = ClientLinkManage.CreateConnection(wcfpluginname);
                result = clientlink.RequestAsync(wcfcontroller, wcfmethod, requestAction, responseAction);
            }
            catch (Exception ex) { throw ex; }
            finally
            {
                if (index != null)
                    pool.ReturnPool(wcfpluginname, (int)index);
            }
            return result;
        }
        #endregion


    }


}
