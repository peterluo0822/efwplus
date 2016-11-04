using EfwControls.HISControl.Orders.Controls.Entity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EfwControls.HISControl.Orders.Controls
{
    public interface IOrderDbHelper
    {
        /// <summary>
        /// 获取医嘱选项卡数据
        /// </summary>
        /// <param name="orderCategory">0长嘱1临嘱</param>
        /// <param name="pageNo"></param>
        /// <param name="pageSize"></param>
        /// <param name="filter"></param>
        /// <returns></returns>
        DataTable  GetDrugItem(int orderCategory, int pageNo, int pageSize, string filter);
        /// <summary>
        /// 根据药品ID，获取药品数据
        /// </summary>
        /// <param name="ItemId">药品ID</param>
        /// <returns></returns>        
        CardDataSourceDrugItem GetDrugItem(int ItemId);
        /// <summary>
        /// 获取用法数据
        /// </summary>
        /// <returns></returns>
        List<CardDataSourceUsage> GetUsage();
        /// <summary>
        /// 获取频次数据
        /// </summary>
        /// <returns></returns>
        List<CardDataSourceFrequency> GetFrequency();
        /// <summary>
        /// 获取嘱托数据
        /// </summary>
        /// <returns></returns>
        List<CardDataSourceEntrust> GetEntrust();
        /// <summary>
        /// 获取药品单位
        /// </summary>
        /// <param name="itemId"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        List<CardDataSourceUnit> GetUnit(int itemId, int type);
        //加载处方模板分类
        DataTable LoadTemplateList(int deptId, int doctorId, int mealCls);
        //加载处方模板明细
        DataTable LoadTemplateDetail(int tplId);
        //获取处方模板
        List<Entity.OrderRecord> GetPresTemplate(int type, int tplId);       
        //获取处方模板行
        List<Entity.OrderRecord> GetPresTemplateRow(int type, int[] tpldetailIds);
        //另存为处方模板
        void AsSavePresTemplate(int level, string mName, int presType, int deptId, int doctorId, List<Entity.OrderRecord> data);
        //检查药品库存是否足够
        bool IsDrugStore(OrderRecord pres);
        //检查药品库存是否足够
        bool IsDrugStore(List<OrderRecord> list, List<OrderRecord> errlist);
        /// <summary>
        /// 医嘱保存
        /// </summary>
        /// <param name="list"></param>
        bool Save(List<OrderRecord> list);//保存      
        /// <summary>
        /// 获得医嘱信息
        /// </summary>      
        DataTable GetOrders(int orderCategory, int status,int patlistid, int deptid); // 获得医嘱信息    
        /// <summary>
        /// 获取组号
        /// </summary>
        /// <param name="patlistid"></param>
        /// <param name="orderCategory"></param>
        /// <returns></returns>
        int GetGroupMax(int patlistid, int orderCategory);
        /// <summary>
        /// 获取医嘱状态
        /// </summary>
        /// <param name="orderid"></param>
        /// <returns></returns>
        int GetStatus(int orderid);
        /// <summary>
        /// 获取皮试用药药品ID
        /// </summary>
        /// <returns></returns>
        int GetActDrugID();
    }
}
