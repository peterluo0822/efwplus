using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace EfwControls.HISControl.Orders.Controls.Entity
{
    public class OrderProcess
    {
        public static IOrderDbHelper OrderDataSource;//数据
        /// <summary>
        /// 获取医嘱
        /// </summary>
        /// <param name="orderStyle"></param>
        /// <param name="patlitist"></param>
        /// <param name="deptid"></param>
        /// <returns></returns>
        public static DataTable GetOrderRecords(int orderStyle,int patlitist,int deptid)
        {
            DataTable dtOrder= OrderDataSource.GetOrders(orderStyle, (int)OrderStatus.所有, patlitist, deptid);
            List<OrderRecord> records = EFWCoreLib.CoreFrame.Common.ConvertExtend.ToList<OrderRecord>(dtOrder).OrderBy(p=>p.OrderBdate).ThenBy(p=>p.GroupID).ThenBy(p=>p.OrderID).ToList();          
            DataTable dt = EFWCoreLib.CoreFrame.Common.ConvertExtend.ToDataTable(records);
            if (dt.Rows.Count > 0)
            {
                dt.Rows[0]["ShowOrderBdate"] = dt.Rows[0]["OrderBdate"];
                dt.Rows[0]["ShowDoseNum"] = dt.Rows[0]["DoseNum"];
                dt.Rows[0]["ShowChannel"] = dt.Rows[0]["ChannelName"];
                dt.Rows[0]["ShowFrency"] = dt.Rows[0]["Frequency"];
                dt.Rows[0]["ShowFirstNum"] = dt.Rows[0]["FirstNum"];
                dt.Rows[0]["ShowTeminalNum"] = dt.Rows[0]["TeminalNum"];
                dt.Rows[0]["GroupFlag"] = 1;
                dt.Rows[0]["ModifyFlag"] = 0;
                if (Convert.ToInt32(dt.Rows[0]["OrderStatus"]) <=2)
                {
                    dt.Rows[0]["EOrderDate"] = DBNull.Value;
                    dt.Rows[0]["ExecDate"] = DBNull.Value;
                    dt.Rows[0]["ShowTeminalNum"] = DBNull.Value;
                }
            }
            for (int i = 0; i <dt.Rows.Count - 1; i++)
            {
                if (Convert.ToInt32( dt.Rows[i + 1]["GroupID"]) !=Convert.ToInt32( dt.Rows[i]["GroupID"]))
                {
                    dt.Rows[i + 1]["GroupFlag"] = 1;
                    dt.Rows[i + 1]["ShowOrderBdate"] = dt.Rows[i + 1]["OrderBdate"];
                    dt.Rows[i + 1]["ShowDoseNum"] = dt.Rows[i + 1]["DoseNum"];
                    dt.Rows[i + 1]["ShowChannel"] = dt.Rows[i + 1]["ChannelName"];
                    dt.Rows[i + 1]["ShowFrency"] = dt.Rows[i + 1]["Frequency"];
                    dt.Rows[i + 1]["ShowFirstNum"] = dt.Rows[i + 1]["FirstNum"];
                    dt.Rows[i + 1]["ShowTeminalNum"] = dt.Rows[i + 1]["TeminalNum"];
                    if (Convert.ToInt32(dt.Rows[i+1]["OrderStatus"]) <= 2)
                    {
                        dt.Rows[i+1]["EOrderDate"] = DBNull.Value;
                        dt.Rows[i + 1]["ExecDate"] = DBNull.Value;
                        dt.Rows[i + 1]["ShowTeminalNum"] = DBNull.Value;
                    }
                }
                else
                {
                    dt.Rows[i + 1]["GroupFlag"] = 0;
                    dt.Rows[i + 1]["ShowOrderBdate"] = DBNull.Value;
                    dt.Rows[i + 1]["ShowDoseNum"] = DBNull.Value;
                    dt.Rows[i + 1]["ShowChannel"] = DBNull.Value;
                    dt.Rows[i + 1]["ShowFrency"] = DBNull.Value;
                    dt.Rows[i + 1]["ShowFirstNum"] = DBNull.Value;
                    dt.Rows[i + 1]["ShowTeminalNum"] = DBNull.Value;

                    dt.Rows[i + 1]["OrderDocName"] = DBNull.Value;
                    dt.Rows[i + 1]["ExecNurseName"] = DBNull.Value;
                    dt.Rows[i + 1]["EOrderDocName"] = DBNull.Value;
                    dt.Rows[i + 1]["EOrderDate"] = DBNull.Value;
                    dt.Rows[i + 1]["ExecDate"] = DBNull.Value;
                }
                dt.Rows[i + 1]["ModifyFlag"] = 0;
            }
            return dt;
        }

        /// <summary>
        /// 医嘱保存
        /// </summary>
        /// <param name="records"></param>
        public static void SaveRecords(List<OrderRecord> records)
        {
            OrderDataSource.Save(records);
        }
    }
}
