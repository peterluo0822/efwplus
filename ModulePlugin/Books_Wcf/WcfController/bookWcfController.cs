using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EFWCoreLib.CoreFrame.Business.AttributeInfo;
using EFWCoreLib.WcfFrame.ServerController;
using Books_Wcf.Entity;
using System.Data;
using Books_Wcf.Dao;
using EFWCoreLib.WcfFrame.DataSerialize;
using EFWCoreLib.WcfFrame.ServerManage;

namespace Books_Wcf.WcfController
{
    [WCFController]
    public class bookWcfController : WcfServerController
    {
        [WCFMethod]
        public ServiceResponseData SaveBook()
        {
            Books book = requestData.GetData<Books>(0);
            book.BindDb(oleDb, _container,_cache,_pluginName);//反序列化的对象，必须绑定数据库操作对象
            book.save();
            responseData.AddData(true);
            return responseData;
        }

        [WCFMethod]
        public ServiceResponseData GetBooks()
        {
            //DataTable dt = NewDao<IBookDao>().GetBooks("", 0);
            EFWCoreLib.WcfFrame.Utility.Upgrade.ClientUpgradeManager.DownLoadUpgrade();
            DistributedCacheManage.SetCache("test", "kakake", "kakake123");
            //responseData.AddData(dt);
            return responseData;
        }
        [WCFMethod]
        public ServiceResponseData Test()
        {
            DataTable dt = oleDb.GetDataTable(@"SELECT 
	            bg.GroupId,
	            Name,
	            DelFlag,
	            Admin,
	            Everyone,
	            bg.Memo,
	            Property AS Pro,
	            bg.WorkId,
	            ISNULL(bgu.Id,0) AS bFlag
            FROM BaseGroup bg
            LEFT JOIN BaseGroupUser bgu ON bg.GroupId = bgu.GroupId AND bgu.WorkId = 1 AND bgu.UserId = 202
            WHERE bg.WorkId = 1");
            responseData.AddData(dt);
            return responseData;
        }
    }
}

