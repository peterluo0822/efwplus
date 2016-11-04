using EfwControls.HISControl.Orders.Controls.IView;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using EfwControls.HISControl.Orders.Controls.Entity;
using EFWCoreLib.CoreFrame.Common;
using System.Windows.Forms;

namespace EfwControls.HISControl.Orders.Controls.Controller
{
    public class OrdersControlController
    {
        private IOrdersControlView iview;
        public IOrderDbHelper OrderDataSource;//数据
        private DataSet CardDataSource;
        private DataTable dtItemDrug;
        private DataTable dtShowCard = new DataTable();
        public OrdersControlController(IOrdersControlView _view)
        {
            iview = _view;
        }

        //绑定选项卡数据源
        public void BindCardDataSource(IOrderDbHelper dbHelper)
        {
            OrderDataSource = dbHelper;
            CardDataSource = new DataSet();
            int type = (int)iview.OrderStyle;
            Entity.OrderProcess.OrderDataSource = dbHelper;
            DataTable dt1 = OrderDataSource.GetDrugItem(type, 1, 10, "");// EFWCoreLib.CoreFrame.Common.ConvertExtend.ToDataTable(OrderDataSource.GetDrugItem(type, 1, 10, ""));
            dt1.TableName = "itemdrug";
            CardDataSource.Tables.Add(dt1);
            dtItemDrug = dt1;


            DataTable dt2 = new DataTable("execdept");
            CardDataSource.Tables.Add(dt2);

            List<Entity.CardDataSourceUnit> list_unit = new List<Entity.CardDataSourceUnit>();
            Entity.CardDataSourceUnit munit = new Entity.CardDataSourceUnit();
            list_unit.Add(munit);


            DataTable dt3 = EFWCoreLib.CoreFrame.Common.ConvertExtend.ToDataTable(list_unit);
            dt3.TableName = "unitdic";
            dt3.Clear();
            CardDataSource.Tables.Add(dt3);

            DataTable dt4 = EFWCoreLib.CoreFrame.Common.ConvertExtend.ToDataTable(OrderDataSource.GetUsage());
            dt4.TableName = "usagedic";
            CardDataSource.Tables.Add(dt4);

            DataTable dt5 = EFWCoreLib.CoreFrame.Common.ConvertExtend.ToDataTable(OrderDataSource.GetFrequency());
            dt5.TableName = "frequencydic";
            CardDataSource.Tables.Add(dt5);


            DataTable dt7 = EFWCoreLib.CoreFrame.Common.ConvertExtend.ToDataTable(OrderDataSource.GetEntrust());
            dt7.TableName = "entrustdic";
            CardDataSource.Tables.Add(dt7);

            iview.InitializeCardData(CardDataSource);
        }
        /// <summary>
        /// 刷新选项卡数据源
        /// </summary>
        public void RefreshOrderShowCard(string yfIds, bool isGetAgain)
        {
            int type = (int)iview.OrderStyle;
            if (isGetAgain)//从数据库获取刷新
            {
                DataTable dt1 = OrderDataSource.GetDrugItem(type, 1, 10, "");// EFWCoreLib.CoreFrame.Common.ConvertExtend.ToDataTable(OrderDataSource.GetDrugItem(type, 1, 10, ""));
                dtItemDrug = dt1;                
            }
            if (yfIds != "-1")
            {
                dtShowCard = dtItemDrug.Clone();
                dtShowCard.Clear();
                DataRow[] dr = dtItemDrug.Select(" itemclass=1 and ExecDeptId in ( " + yfIds + ")");
                DataRow[] drItem = dtItemDrug.Select(" itemclass <>1");
                for (int i = 0; i < dr.Length; i++)
                {
                    dtShowCard.Rows.Add(dr[i].ItemArray);
                }
                for (int i = 0; i < drItem.Length; i++)
                {
                    dtShowCard.Rows.Add(drItem[i].ItemArray);
                }
                iview.ShowCardItemDrugSet(dtShowCard);
            }
            else
            {
                iview.ShowCardItemDrugSet(dtItemDrug);
                dtShowCard = dtItemDrug.Clone();
            }
        }

        //绑定医嘱数据
        public void BindOrderData()
        {
            int type = (int)iview.OrderStyle;
            DataTable dt = Entity.OrderProcess.GetOrderRecords(type, iview.CurrPatListId, iview.PresDeptId);
            iview.LoadGridOrderData(dt);
            iview.SetGridColor();
        }
        /// <summary>
        ///新增一行时修改药品项目的选项卡数据源
        /// </summary>
        /// <param name="statid"></param>
        public void ShowCardItemDrugChange(int statid, int itemtype,int itemid, bool isGroupNew)
        {
            if (isGroupNew)
            {
                iview.ShowCardItemDrugSet(dtShowCard);
            }
            else
            {
                DataTable dtCopy = dtShowCard.Clone();
                dtCopy.Clear();
                DataRow[] rows;
                if (statid == 100 || statid == 101)
                {
                    rows = dtShowCard.Select(" statid in(100,101) and itemid<>"+itemid+"", "");
                }
                else if (statid == 102)
                {
                    rows = dtShowCard.Select(" statid =102 and itemid<>"+itemid+"", "");
                }
                else
                {
                    rows = dtShowCard.Select(" itemclass=" + itemtype + " and itemid<>"+itemid+" ");
                }
                foreach (DataRow dr in rows)
                {
                    dtCopy.Rows.Add(dr.ItemArray);
                }
                iview.ShowCardItemDrugSet(dtCopy);
            }
        }

        #region 网格事件
        /// <summary>
        /// 根据病人状态判断是否可以修改医嘱
        /// </summary>
        /// <returns></returns>
        public bool CanEditOrder()
        {
            if (iview.PatStatus == 2)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public void SetReadOnly(int RowIndex)
        {
            try
            {
                DataTable tbPresc = iview.GetGridOrder;
                if (RowIndex >= tbPresc.Rows.Count)
                    return;
                if (!CanEditOrder())
                {
                    iview.SetReadOnly(ReadOnlyType.全部只读);
                }
                if (Convert.ToInt32(tbPresc.Rows[RowIndex]["OrderStatus"]) > (int)OrderStatus.医生发送)
                {
                    iview.SetReadOnly(ReadOnlyType.不能修改);
                    return;
                }
                if (Convert.ToInt32(tbPresc.Rows[RowIndex]["OrderStatus"]) != -1 && Convert.ToInt32(tbPresc.Rows[RowIndex]["OrderStatus"]) < (int)OrderStatus.医生发送)
                {
                    if (Convert.ToInt32(tbPresc.Rows[RowIndex]["ModifyFlag"]) == 0)
                    {
                        iview.SetReadOnly(ReadOnlyType.不能修改);
                    }
                }
                if (Convert.ToInt32(tbPresc.Rows[RowIndex]["ItemID"]) == 0 && Convert.ToInt32(tbPresc.Rows[RowIndex]["GroupFlag"]) == 1)
                {
                    iview.SetReadOnly(ReadOnlyType.新开);
                    return;
                }
                if (Convert.ToInt32(tbPresc.Rows[RowIndex]["ModifyFlag"]) == 1 && Convert.ToInt32(tbPresc.Rows[RowIndex]["GroupFlag"]) == 0)
                {
                    iview.SetReadOnly(ReadOnlyType.同组增加);
                    return;
                }
                if (Convert.ToInt32(tbPresc.Rows[RowIndex]["ModifyFlag"]) == 1 && Convert.ToInt32(tbPresc.Rows[RowIndex]["ItemID"]) != 0 && Convert.ToInt32(tbPresc.Rows[RowIndex]["GroupFlag"]) == 1)
                {
                    int statid = Convert.ToInt32(tbPresc.Rows[RowIndex]["statid"]);
                    if (statid != 100 && statid != 101 && statid != 102)
                    {
                        iview.SetReadOnly(ReadOnlyType.项目);
                        return;
                    }
                    if (statid == 102)
                    {
                        iview.SetReadOnly(ReadOnlyType.中草药);
                        return;
                    }
                    else
                    {
                        iview.SetReadOnly(ReadOnlyType.药品非中草药);
                        return;
                    }
                }
            }
            catch
            {
                iview.SetReadOnly(ReadOnlyType.新开);
                return;
            }
        }
        /// <summary>
        /// 插入空行
        /// </summary>
        /// <param name="destRowIndex">插入处</param>
        /// <param name="type">0新开 1回车插入</param>
        public void AddEmptyRow(int destRowIndex, int type)
        {
            DataTable tbPresc = iview.GetGridOrder;
            DataRow dr = tbPresc.NewRow();
            if (type == 0)
            {
                dr["ShowOrderBdate"] = DateTime.Now;
                dr["OrderBdate"] = dr["ShowOrderBdate"];
                dr["GroupFlag"] = 1;
                dr["GroupID"] = 0;
                dr["ShowDoseNum"] = 1;
            }
            else
            {
                dr["ShowOrderBdate"] = DBNull.Value;
                dr["ShowDoseNum"] = DBNull.Value;
                dr["GroupFlag"] = 0;
            }
            dr["ItemID"] = 0;
            dr["ItemName"] = "";
            dr["ShowChannel"] = "";
            dr["ShowFrency"] = "";
            dr["ChannelID"] = 0;
            dr["FrenquencyID"] = 0;         
            dr["Dosage"] = 0;
            dr["Amount"] = 0;
            dr["ModifyFlag"] = 1;
            dr["OrderStatus"] = (int)OrderStatus.医生新开;
            dr["OrderCategory"] = (int)iview.OrderStyle;
            dr["PatListID"] = iview.CurrPatListId;
            dr["PresDeptID"] = iview.PresDeptId;
            dr["OrderDoc"] = iview.PresDoctorId;
            dr["OrderType"] = 0;
            tbPresc.Rows.InsertAt(dr, destRowIndex);
            if (type == 1)
            {
                int itemid= Convert.ToInt32(tbPresc.Rows[destRowIndex - 1]["ItemID"]);
                int statid = Convert.ToInt32(tbPresc.Rows[destRowIndex - 1]["StatId"]);
                int itemtype = Convert.ToInt32(tbPresc.Rows[destRowIndex - 1]["ItemType"]);
                dr["GroupID"] = Convert.ToInt32(tbPresc.Rows[destRowIndex - 1]["GroupID"]);
                dr["OrderBdate"] = Convert.ToDateTime(tbPresc.Rows[destRowIndex - 1]["OrderBdate"]);
                ShowCardItemDrugChange(statid, itemtype,itemid, false);
            }
            else
            {
                ShowCardItemDrugChange(0, 0,0, true);
            }
        }
        public void SelectCardBindData(int rowid, DataRow selectedRow, string columnName)
        {
            DataTable dt = null;
            dt = iview.GetGridOrder;
            bool pscl = false;
            switch (columnName)
            {
                case "ItemName":
                    dt.Rows[rowid]["ItemID"] = selectedRow["ItemId"];
                    dt.Rows[rowid]["ItemName"] = selectedRow["ItemName"];
                    dt.Rows[rowid]["ItemPrice"] = selectedRow["SellPrice"];
                    dt.Rows[rowid]["ItemType"] = selectedRow["ItemClass"];
                    dt.Rows[rowid]["StatID"] = selectedRow["StatID"];
                    dt.Rows[rowid]["ExecDeptID"] = selectedRow["ExecDeptId"];
                    dt.Rows[rowid]["DosageUnit"] = selectedRow["DoseUnitName"];
                    dt.Rows[rowid]["Factor"] = selectedRow["DoseConvertNum"];
                    dt.Rows[rowid]["Unit"] = selectedRow["MiniUnitName"];
                    dt.Rows[rowid]["Unit"] = selectedRow["MiniUnitName"];
                    dt.Rows[rowid]["UnitNO"] = 1;
                    dt.Rows[rowid]["AstFlag"] = -1;
                    iview.SetReadOnly(ReadOnlyType.新开);
                    if (Convert.ToInt32(selectedRow["ItemClass"]) == 1)
                    {
                        if (Convert.ToInt32(selectedRow["StatID"]) == 100 || Convert.ToInt32(selectedRow["StatID"]) == 101 || Convert.ToInt32(selectedRow["StatID"]) == 102)
                        {
                            if (iview.OrderStyle == OrderCategory.临时医嘱)
                            {
                                GetUnit(Convert.ToInt32(selectedRow["ItemId"]));
                            }
                        }
                    }
                    if (Convert.ToInt32(selectedRow["SkinTestFlag"]) == 1)//需要皮试
                    {
                        //if (MessageBox.Show("该药品是需要皮试的药品，你要开‘皮试’医嘱吗？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                        //{

                        //    pscl = true;
                        //    dt.Rows[rowid]["AstFlag"] = 0;
                        //    DateTime dTime = DateTime.Now;
                        //    decimal strDate = Convert.ToDecimal(dTime.ToString("yyyyMMddHHmmss.ffffff"));
                        //    dt.Rows[rowid]["AstOrderID"] = strDate;
                        //    InsertPSYZ(selectedRow, strDate, rowid);
                        //    //if (MessageBox.Show("是否开皮试用药？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                        //    //{
                        //    //    InsertPSYY(Convert.ToInt32(dr["order_type"].ToString()), XcConvert.IsNull(dr["ITEMNAME"], ""), Convert.ToDecimal(dr["dosenum"].ToString()),
                        //    //        XcConvert.IsNull(dr["doseunit"], ""), XcConvert.IsNull(dr["EXECDEPTCODE"], "0"), XcConvert.IsNull(dr["standard"], ""), XcConvert.IsNull(dr["itemid"], "0"), dTime, orderkind, rowid);
                        //    //}
                        //}
                        //else
                        //{
                        //    dt.Rows[rowid]["ItemName"] = dt.Rows[rowid]["ItemName"].ToString().Trim() + "(免试)";
                        //    dt.Rows[rowid]["AstFlag"] = 3;
                        //}
                    }
                    break;
                case "Entrust":
                    dt.Rows[rowid]["Entrust"] = selectedRow["NAME"].ToString();      
                    break;
                case "ShowChannel":
                    dt.Rows[rowid]["ChannelID"] = selectedRow["UsageId"];
                    dt.Rows[rowid]["ChannelName"] = selectedRow["UsageName"];
                    if (Convert.ToInt32(dt.Rows[rowid]["GroupFlag"]) == 1)
                    {
                        dt.Rows[rowid]["ShowChannel"] = selectedRow["UsageName"];
                        this.ChangeValue(dt, rowid, "ShowChannel");//如果是第一组的每一项，则改变值的同时，这一组的同时改变
                    }
                    break;
                case "ShowFrency":
                    dt.Rows[rowid]["Frequency"] = selectedRow["Name"];
                    dt.Rows[rowid]["FrenquencyID"] = selectedRow["FrequencyId"];
                    if (iview.OrderStyle == OrderCategory.长期医嘱)//算出首次
                    {

                    }
                    if (Convert.ToInt32(dt.Rows[rowid]["GroupFlag"]) == 1) //如果是第一组的每一项，则改变值的同时，这一组的同时改变
                    {
                        dt.Rows[rowid]["ShowFrency"] = selectedRow["Name"];
                        this.ChangeValue(dt, rowid, "ShowFrency");
                    }
                    break;
                default: break;
            }
            if (iview.OrderStyle == OrderCategory.临时医嘱 && columnName == "Unit") //只有临嘱里边可以修改单位
            {
                dt.Rows[rowid]["Unit"] = selectedRow["UnitName"];
                dt.Rows[rowid]["UnitNO"] = selectedRow["UnitNO"];
            }
            if (!pscl)
            {
                //List<HIS.Model.zy_doc_orderrecord_son> records = new List<HIS.Model.zy_doc_orderrecord_son>();
                //records.Add(record);
                //DataTable table = HIS.SYSTEM.PubicBaseClasses.ApiFunction.ObjToDataTable(records);
                //dt.Rows[rowid].ItemArray = table.Rows[0].ItemArray;
                //controlmethod.SetNull(dt.Rows[rowid], 0);
                //if (colid != 7)
                //{
                //    dt.Rows[rowid]["first"] = DBNull.Value;
                //}
            }
        }
        #region 增加皮试医嘱
        /// <summary>
        /// 增加皮试医嘱 
        /// </summary>
        /// <param name="itemtype"></param>
        /// <param name="itemname"></param>
        /// <param name="strDate"></param>
        /// <param name="ordertype"></param>
        /// <param name="rowid"></param>
        private void InsertPSYZ(DataRow row, decimal strDate,  int rowid)
        {
            if (iview.OrderStyle == OrderCategory.长期医嘱)
            {
                List<OrderRecord> data = new List<Entity.OrderRecord>();
                OrderRecord record = new OrderRecord();
                record.ItemID =Convert.ToInt32( row["itemid"]);
                record.ItemName = row["itemname"].ToString();
                record.ShowChannel = "皮试";
                record.ChannelName = "皮试";
                record.ChannelID = 34;
                record.FrenquencyID = 0;
                record.Amount = 1;
                record.Unit = row["MiniUnitName"].ToString();
                record.UnitNO = 1;
                record.StatID = Convert.ToInt32(row["StatID"]);
                record.DoseNum = 1;
                record.ShowDoseNum =1;
                record.OrderStatus = -1;
                record.OrderBdate = DateTime.Now;
                record.OrderCategory = 1;
                record.OrderType = 0;
                record.ItemType =Convert.ToInt32( row["ItemClass"]);
                data.Add(record);
                int actDrugid = OrderDataSource.GetActDrugID();
                if (actDrugid != 0)
                {

                }
                iview.SaveAstoflinkage(data);
            }
            else
            {
                //DataTable tb = view.BindTempOrderData;
                //if (tb == null || tb.Rows.Count == 0)
                //{
                //    this.AddRow(1, 0);
                //}
                //tb = view.BindTempOrderData;
                //DataRow dr = tb.NewRow();
                //controlmethod.GivePsRowData(dr, itemtype, "皮试", "", 0, strDate, itemname, itemid, deptid, 0, "", "");
                //dr["order_doc"] = employid;
                //if (ordertype == 1)
                //{
                //    while (rowid >= 0)
                //    {
                //        if (tb.Rows[rowid]["BeginTime"].ToString() != timeformat.ToString())
                //        {
                //            tb.Rows.InsertAt(dr, rowid); //在临嘱时，插入该医嘱上一行
                //            break;
                //        }
                //        rowid--;
                //    }
                //}
                //else
                //{
                //    int n = tb.Rows.Count;
                //    while (n > 0 && tb.Rows[n - 1]["order_content"].ToString() == "")
                //    {
                //        view.Plus(0);
                //        tb.Rows.Remove(tb.Rows[n - 1]);
                //        view.Plus(1);
                //        n--;
                //    }
                //    tb.Rows.Add(dr);
                //}
            }            
        }
        #endregion

        #region  增加皮试用药
        ///// <summary>
        /////  增加皮试用药
        ///// </summary>
        ///// <param name="myRow"></param>
        ///// <param name="dTime"></param>
        ///// <param name="page"></param>
        ///// <param name="nrow"></param>
        //private void InsertPSYY(int itemtype, string itemname, decimal dosenum, string doseunit, string deptid, string standard, string itemid, DateTime dTime, int ordertype, int rowid)
        //{
        //    DataTable tb = view.BindTempOrderData;
        //    if (tb == null || tb.Rows.Count == 0)
        //    {
        //        this.AddRow(1, 0);
        //    }
        //    tb = view.BindTempOrderData;
        //    DataRow dr = tb.NewRow();
        //    controlmethod.GivePsRowData(dr, itemtype, "皮试用", "Qd", dosenum, 0, itemname, Convert.ToInt32(itemid), Convert.ToInt32(deptid), -1, standard, doseunit.ToString().Trim());
        //    dr["order_doc"] = employid;
        //    if (ordertype == 1) //本来是在临时医嘱界面上开的皮试
        //    {
        //        while (rowid >= 0)
        //        {
        //            if (tb.Rows[rowid]["BeginTime"].ToString() != timeformat.ToString())
        //            {
        //                tb.Rows.InsertAt(dr, rowid); //在临嘱时，插入该医嘱上一行
        //                break;
        //            }
        //            rowid--;
        //        }
        //    }
        //    else
        //    {
        //        int n = tb.Rows.Count;
        //        while (n > 0 && tb.Rows[n - 1]["order_content"].ToString() == "")
        //        {
        //            view.Plus(0);
        //            tb.Rows.Remove(tb.Rows[n - 1]);
        //            view.Plus(1);
        //            n--;
        //        }
        //        tb.Rows.Add(dr);
        //    }
        //}
        #endregion     
        /// <summary>
        /// 根据药品ID动态获取单位数据源
        /// </summary>
        /// <param name="itemid"></param>
        private void GetUnit(int itemid)
        {
            DataTable dt = new DataTable();
            DataColumn col = new DataColumn();
            col.ColumnName = "UnitName";
            dt.Columns.Add(col);
            col = new DataColumn();
            col.ColumnName = "UnitNO";
            dt.Columns.Add(col);
            DataRow[] dr = dtItemDrug.Select(" ItemID=" + itemid + "");
            if (dr.Length > 0)
            {
                DataRow row = dt.NewRow();
                row["UnitName"] = dr[0]["MiniUnitName"];
                row["UnitNO"] = 1;
                dt.Rows.Add(row);
                row = dt.NewRow();
                row["UnitName"] = dr[0]["UnPickUnit"];
                row["UnitNO"] = dr[0]["MiniConvertNum"];
                dt.Rows.Add(row);
            }
            iview.ShowCardUnitSet(dt);
        }
        public void CaculateTempOrderAmout(int rowIndex)
        {
            DataTable dt = iview.GetGridOrder;
            dt.Rows[rowIndex]["Amount"] = Math.Ceiling(Convert.ToDecimal(dt.Rows[rowIndex]["Dosage"]) / Convert.ToDecimal(dt.Rows[rowIndex]["Factor"]));
        }
        #region 一组医嘱用法，频率自动改变
        /// <summary>
        /// 当改变一组中第一个的频率，用法时这组的医嘱自动改变
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="rowindex"></param>
        /// <param name="colname"></param>
        public void ChangeValue(DataTable dt, int rowindex, string colname)
        {
            for (int i = rowindex + 1; i < dt.Rows.Count; i++)
            {
                if (dt.Rows[i]["GroupFlag"].ToString().Trim() == "0")
                {
                    dt.Rows[i]["OrderBdate"] = dt.Rows[rowindex]["OrderBdate"];
                    dt.Rows[i]["ChannelID"] = dt.Rows[rowindex]["ChannelID"];
                    dt.Rows[i]["ChannelName"] = dt.Rows[rowindex]["ChannelName"];
                    dt.Rows[i]["Frequency"] = dt.Rows[rowindex]["Frequency"];
                    dt.Rows[i]["FrenquencyID"] = dt.Rows[rowindex]["FrenquencyID"];
                    dt.Rows[i]["FirstNum"] = dt.Rows[rowindex]["FirstNum"];
                    dt.Rows[i]["DoseNum"] = dt.Rows[rowindex]["DoseNum"];
                    if (dt.Rows[i]["OrderStatus"].ToString().Trim() != "-1")
                    {
                        dt.Rows[i]["ModifyFlag"] = 1;
                    }
                }
                if (dt.Rows[i]["GroupFlag"].ToString().Trim() == "1")
                {
                    break;
                }
            }
        }
        #endregion
        #endregion

        /// <summary>
        /// 医嘱保存
        /// </summary>
        /// <param name="notSavedRecords"></param>
        public void SaveRecores(List<OrderRecord> notSavedRecords)
        {
            if (CheckOrders(notSavedRecords))
            {
                List<OrderRecord> groupRecords = notSavedRecords.Where(p => p.GroupFlag == 1 && p.OrderStatus == -1 && p.GroupID == 0).OrderBy(p => p.OrderBdate).ToList();
                foreach (OrderRecord groupRecord in groupRecords)
                {
                    groupRecord.GroupID = OrderDataSource.GetGroupMax(groupRecord.PatListID, (int)iview.OrderStyle);
                    groupRecord.FirstNum = groupRecord.ShowFirstNum;
                    groupRecord.TeminalNum = groupRecord.ShowTeminalNum;
                    groupRecord.DoseNum = groupRecord.ShowDoseNum;
                    List<OrderRecord> sameGroupRecords = notSavedRecords.Where(p => p.OrderBdate == groupRecord.OrderBdate && p.GroupID == 0 && p.GroupFlag == 0).ToList();
                    foreach (OrderRecord notsaveRecord in sameGroupRecords)
                    {
                        notsaveRecord.GroupID = groupRecord.GroupID;
                        notsaveRecord.FrenquencyID = groupRecord.FrenquencyID;
                        notsaveRecord.Frequency = groupRecord.Frequency;
                        notsaveRecord.ChannelID = groupRecord.ChannelID;
                        notsaveRecord.ChannelName = groupRecord.ChannelName;
                        notsaveRecord.FirstNum = groupRecord.FirstNum;
                        notsaveRecord.TeminalNum = groupRecord.TeminalNum;
                        notsaveRecord.ShowDoseNum = groupRecord.DoseNum;//付数
                    }
                }
                foreach (OrderRecord record in notSavedRecords)
                {
                    if (record.OrderStatus == (int)OrderStatus.医生新开)
                    {
                        record.OrderStatus = (int)OrderStatus.医生保存;
                    }
                }
                if (OrderDataSource.Save(notSavedRecords))
                {
                    string strMes = null;
                    for (int i = 0; i < notSavedRecords.Count; i++)
                    {
                        strMes += notSavedRecords[i].ItemName + "\n";
                    }
                    iview.ShowMessage("下列医嘱已经保存！\n" + strMes);
                    BindOrderData();
                }
            }
        }
        public bool DeleteRecored(List<OrderRecord> delRecords)
        {
            if (OrderDataSource.Save(delRecords))
            {
                string strMes = null;
                for (int i = 0; i < delRecords.Count; i++)
                {
                    strMes += delRecords[i].ItemName + "\n";
                }
                iview.ShowMessage("下列医嘱已经删除！\n" + strMes);
                return true;
            }
            return false;
        }

        public bool SendOrderRecord()
        {

            DataTable dt = iview.GetGridOrder;
            List<OrderRecord> sendRecords = EFWCoreLib.CoreFrame.Common.ConvertExtend.ToList<OrderRecord>(dt);
            sendRecords = sendRecords.Where(p => p.OrderStatus == 0).ToList();
            if (sendRecords.Count == 0)
            {
                return true;
            }
            foreach (OrderRecord sendRecord in sendRecords)
            {
                sendRecord.OrderStatus = (int)OrderStatus.医生发送;                
            }
            if (OrderDataSource.Save(sendRecords))
            {
                string strMes = null;
                for (int i = 0; i < sendRecords.Count; i++)
                {
                    strMes += sendRecords[i].ItemName + "\n";
                }
                iview.ShowMessage("下列医嘱已经确认！\n" + strMes);
                return true;
            }
            return false; ;
        }
     
        #region 医嘱检查判断
        /// <summary>
        /// 医嘱保存判断
        /// </summary>
        /// <param name="notSavedRecords"></param>
        /// <returns></returns>
        private bool CheckOrders(List<OrderRecord> notSavedRecords)
        {
            DataTable dt = iview.GetGridOrder;
            List<OrderRecord> records = notSavedRecords.Where(p => p.ItemType == 1).ToList();
            List<OrderRecord> errRecords = new List<OrderRecord>();
            //判断库存
            if (records.Count>0 && OrderDataSource.IsDrugStore(records, errRecords) == false)
            {
                //检查药品是否有库存               
                string errDrugName = null;
                for (int i = 0; i < errRecords.Count; i++)
                {
                    errDrugName += errRecords[i].ItemName + "\t" + errRecords[i].Spec + "\t" + errRecords[i].Dosage + errRecords[i].DosageUnit + "\n";
                }
                iview.ShowMessage("下列这些药品库存不足或已停用，不能开出！\n" + errDrugName);
                return false;
            }
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (Convert.ToInt32(dt.Rows[i]["ModifyFlag"]) == 1)
                {
                    string itemName = dt.Rows[i]["ItemName"] == DBNull.Value ? "" : dt.Rows[i]["ItemName"].ToString();
                    if (itemName == "")
                    {
                        continue;
                    }
                    int statid = Convert.ToInt32(dt.Rows[i]["StatID"]);                    
                    if (statid == 100 || statid == 101)
                    {
                        if (Convert.ToDecimal(dt.Rows[i]["Dosage"]) <= 0)
                        {
                            iview.ShowMessage("" + itemName + "的剂量输入不正确，请重新输入");
                            iview.SetGridCurrentCell(i, "Dosage");
                            return false;
                        }
                        if (Convert.ToInt32(dt.Rows[i]["GroupFlag"])==1 && !CheckChannel(dt.Rows[i]["ShowChannel"].ToString(), Convert.ToInt32(dt.Rows[i]["ChannelID"])))
                        {
                            iview.ShowMessage("" + itemName + "的用法输入不正确，请重新输入");
                            iview.SetGridCurrentCell(i, "ShowChannel");
                            return false;
                        }
                        if (Convert.ToInt32(dt.Rows[i]["GroupFlag"]) == 1 && !CheckFrenquency(dt.Rows[i]["ShowFrency"].ToString(), Convert.ToInt32(dt.Rows[i]["FrenquencyID"])))
                        {
                            iview.ShowMessage("" + itemName + "的频次输入不正确，请重新输入");
                            iview.SetGridCurrentCell(i, "ShowFrency");
                            return false;
                        }
                    }
                    if (iview.OrderStyle == OrderCategory.临时医嘱)
                    {
                        if (Convert.ToDecimal(dt.Rows[i]["Amount"]) <= 0)
                        {
                            iview.ShowMessage("" + itemName + "的数量输入不正确，请重新输入");
                            iview.SetGridCurrentCell(i, "Amount");
                            return false;
                        }
                        if (statid == 102)
                        {
                            if (Convert.ToDecimal(dt.Rows[i]["Dosage"]) <= 0)
                            {
                                iview.ShowMessage("" + itemName + "的剂量输入不正确，请重新输入");
                                iview.SetGridCurrentCell(i, "Dosage");
                                return false;
                            }
                            if (Convert.ToInt32(dt.Rows[i]["GroupFlag"]) == 1 && Convert.ToDecimal(dt.Rows[i]["ShowDoseNum"]) <= 0)
                            {
                                iview.ShowMessage("" + itemName + "的付数输入不正确，请重新输入");
                                iview.SetGridCurrentCell(i, "ShowDoseNum");
                                return false;
                            }
                        }
                    }
                    int orderType = Convert.ToInt32(dt.Rows[i]["OrderType"]);
                    if (statid>0 && orderType <= 3)
                    {
                        if (Convert.ToDecimal(dt.Rows[i]["Dosage"]) <= 0)
                        {
                            iview.ShowMessage("" + itemName + "的剂量输入不正确，请重新输入");
                            iview.SetGridCurrentCell(i, "Dosage");
                            return false;
                        }
                    }
                }
            }
            return true;
        }
        /// <summary>
        /// 用法判断
        /// </summary>
        /// <param name="channelName"></param>
        /// <param name="channelID"></param>
        /// <returns></returns>
        private bool CheckChannel(string channelName, int channelID)
        {
            if (channelName == "")
            {
                return false;
            }
            DataTable dtUsage = CardDataSource.Tables["usagedic"];
            DataRow[] rows = dtUsage.Select("UsageName='"+channelName+ "' and UsageId="+channelID+"");
            if (rows == null || rows.Length == 0)
            {
                return false;
            }
            return true;
        }
        /// <summary>
        /// 频次判断
        /// </summary>
        /// <param name="frenquencyName"></param>
        /// <param name="frenquencyId"></param>
        /// <returns></returns>
        private bool CheckFrenquency(string frenquencyName, int frenquencyId)
        {
            if (frenquencyName == "")
            {
                return false;
            }   
            DataTable dtFrequency = CardDataSource.Tables["frequencydic"];
            DataRow[] rows = dtFrequency.Select("Name='" + frenquencyName + "' and FrequencyId=" + frenquencyId + "");
            if (rows == null || rows.Length == 0)
            {
                return false;
            }
            return true;
        }
        #endregion

        /// <summary>
        /// 设置修改状态
        /// </summary>
        /// <param name="rowIndex"></param>
        public void SetOrderModifyStatus(int rowIndex)
        {
            DataTable tbPresc = iview.GetGridOrder;
            if (tbPresc.Rows.Count == 0)
                return;
            tbPresc.Rows[rowIndex]["ModifyFlag"] = (short)1;
            //int groupID = Convert.ToInt32(tbPresc.Rows[rowIndex]["GroupId"]);
            //for (int i = 0; i < tbPresc.Rows.Count; i++)
            //{
            //    if (Convert.ToInt32(tbPresc.Rows[i]["GroupId"]) == groupID)
            //    {
            //        tbPresc.Rows[i]["ModifyFlag"] = (short)1;
            //    }
            //}
            iview.SetGridColor();
        }
    }
}
