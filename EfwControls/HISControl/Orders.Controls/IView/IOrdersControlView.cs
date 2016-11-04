using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace EfwControls.HISControl.Orders.Controls.IView
{
    public interface IOrdersControlView
    {
        Entity.OrderCategory OrderStyle { get; set; }
        /// <summary>
        /// 当前病人
        /// </summary>
        int CurrPatListId { get; set; }
        int PatDeptID { get; set; }
        int PresDeptId { get; set; }
        string PresDeptName { get; set; }
        int PresDoctorId { get; set; }
        string PresDoctorName { get; set; }
         int PatStatus { get; set; }
        /// <summary>
        /// 初始化选项卡数据源
        /// </summary>
        /// <param name="cardDataSource"></param>
        void InitializeCardData(DataSet cardDataSource);
        /// <summary>
        /// 药品项目选项卡数据源
        /// </summary>
        /// <param name="dtItemDrug"></param>
        void ShowCardItemDrugSet(DataTable dtItemDrug);
        /// <summary>
        /// 加载医嘱数据
        /// </summary>
        /// <param name="presData"></param>
        void LoadGridOrderData(DataTable orderData);

        /// <summary>
        /// 获取医嘱数据源
        /// </summary>
        DataTable GetGridOrder{ get; }

        /// <summary>
        /// 画组线 
        /// </summary>
        /// <param name="rowIndex"></param>
        /// <returns></returns>
        System.Drawing.Rectangle GridCellBounds(int rowIndex);

        /// <summary>
        /// Grid当前行号
        /// </summary>
        int GridRowIndex { get; }

        /// <summary>
        /// 设置网格当前单元格
        /// </summary>
        /// <param name="rowIndex"></param>
        /// <param name="column"></param>
        void SetGridCurrentCell(int rowIndex, int colIndex);

        void SetGridColor();
        void SetReadOnly(ReadOnlyType readonlyType);     
        /// <summary>
        /// 保存医嘱生成联动费用
        /// </summary>
        /// <param name="data"></param>
        void SaveCostoflinkage(List<Entity.OrderRecord> data);
        void ShowCardUnitSet(DataTable dtUnit);
        void SetGridCurrentCell(int rowIndex, string colName);
        void ShowMessage(string message);
        void SaveAstoflinkage(List<Entity.OrderRecord> data);
    }

    public enum ReadOnlyType
    { 
        项目 ,
        中草药 ,
        药品非中草药,
        新开,
        不能修改,
        全部只读,
        同组增加
    }
}
