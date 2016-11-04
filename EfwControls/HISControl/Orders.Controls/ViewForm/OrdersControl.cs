using DevComponents.DotNetBar;
using EfwControls.HISControl.Orders.Controls.Controller;
using EfwControls.HISControl.Orders.Controls.Entity;
using EfwControls.HISControl.Orders.Controls.IView;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EfwControls.HISControl.Orders.Controls.ViewForm
{
    public partial class OrdersControl : UserControl,IOrdersControlView
    {
        public OrdersControlController controller;
        public OrdersControl()
        {
            InitializeComponent();
            controller = new OrdersControlController(this);
        }
        [Description("医嘱保存时费用联动")]
        public event PrescriptionCostoflinkage Costoflinkage;
        [Description("开皮试医嘱时增加皮试医嘱")]
        public event PrescriptionAstoflinkage Astoflinkage;

        #region 自定义属性
        private bool _isShowToolBar = true;
        [Description("是否显示工具栏")]
        public bool IsShowToolBar
        {
            get { return _isShowToolBar; }
            set
            {
                _isShowToolBar = value;
                this.btnBar.Visible = _isShowToolBar;
            }
        }
       
        private OrderCategory _orderCategory = OrderCategory.长期医嘱;
        [Description("医嘱类别")]
        public OrderCategory OrderStyle
        {
            get { return _orderCategory; }
            set
            {
                _orderCategory = value;
                if (_orderCategory == OrderCategory.长期医嘱)
                {
                    ShowDoseNum.Visible = false;
                    ShowFirstNum.Visible = true;
                    ShowTeminalNum .Visible = true;
                    EOrderDocName.Visible = true;
                    EOrderDate.Visible = true;
                    Amount.Visible = false;
                    Unit.Visible = false;
                }
                else
                {
                    ShowDoseNum.Visible = true;
                    ShowFirstNum.Visible = false;
                    ShowTeminalNum.Visible = false;
                    EOrderDocName.Visible = false;
                    EOrderDate.Visible = false;
                    Amount.Visible = true;
                    Unit.Visible = true;
                }       
            }
        }

        private string[] _oldhideColName;
        private string[] _hideColName;
        [Description("需要隐藏的列名")]
        public string[] HideColName
        {
            get { return _hideColName; }
            set
            {
                _hideColName = value;
                if (_oldhideColName != null && _oldhideColName.Length > 0)
                {
                    for (int i = 0; i < _oldhideColName.Length; i++)
                    {
                        if (_oldhideColName[i].Trim() != "")
                        {
                            if (dgvOrders.Columns.Contains(_oldhideColName[i]) == true)
                            {
                                dgvOrders.Columns[_oldhideColName[i]].Visible = true;
                            }
                        }
                    }
                }
                if (_hideColName != null && _hideColName.Length > 0)
                {
                    for (int i = 0; i < _hideColName.Length; i++)
                    {
                        if (_hideColName[i].Trim() != "")
                        {
                            if (dgvOrders.Columns.Contains(_hideColName[i]) == true)
                            {
                                dgvOrders.Columns[_hideColName[i]].Visible = false;
                            }
                        }
                    }
                }

                _oldhideColName = _hideColName;
            }
        }
        #endregion

        #region 事件
        public void SaveCostoflinkage(List<Entity.OrderRecord> data)
        {
            if (Costoflinkage != null)
            {
                Costoflinkage(CurrPatListId, data);
            }
        }
        public void InitDbHelper(IOrderDbHelper iOrderDbHelper)
        {
            controller.BindCardDataSource(iOrderDbHelper);           
        }
        private string enterDate;
        /// <summary>
        /// 数据补始化
        /// </summary>
        /// <param name="patListID">病人ID</param>
        /// <param name="patStatus">病人状态</param>
        /// <param name="presDeptCode">开嘱科室</param>
        /// <param name="presDeptName">开嘱科室名称</param>
        /// <param name="presDoctorId">开嘱医生</param>
        /// <param name="presDoctorName">开嘱医生姓名</param>
        public void LoadPatData(int patListID, int patStatus, int presDeptCode, string presDeptName, int presDoctorId, string presDoctorName, int patDeptId, string defaultDrugStore,string _enterDate)
        {
            CurrPatListId = patListID;
            PresDeptId = presDeptCode;
            PresDeptName = presDeptName;
            PresDoctorId = presDoctorId;
            PresDoctorName = presDoctorName;
            PatStatus = PatStatus;
            PatDeptID = patDeptId;
            enterDate = _enterDate;
            if (_drugStoreDeptID != defaultDrugStore)
            {
                _drugStoreDeptID = defaultDrugStore;
                controller.RefreshOrderShowCard(_drugStoreDeptID, false);
            }
            (controller as OrdersControlController).BindOrderData();
            this.dgvOrders.Refresh();
        }
        /// <summary>
        /// 选择药房刷新数据源
        /// </summary>
        /// <param name="defaultDrugStore"></param>
        public void RefreshDrugData(string defaultDrugStore)
        {
            if (_drugStoreDeptID != defaultDrugStore)
            {
                _drugStoreDeptID = defaultDrugStore;
                controller.RefreshOrderShowCard(_drugStoreDeptID, false);
            }
        }
        #endregion

        #region 接口实现
        private string _drugStoreDeptID;     

        private int _patDeptID;
        [Browsable(false)]
        public int PatDeptID
        {
            get
            {
                return _patDeptID;
            }

            set
            {
                _patDeptID = value;
            }
        }
        private int _currpatlistid;
        [Browsable(false)]
        public int CurrPatListId
        {
            get
            {
                return _currpatlistid;
            }

            set
            {
                _currpatlistid = value;
            }
        }
        private int _presdeptid;
        [Browsable(false)]
        public int PresDeptId
        {
            get
            {
                return _presdeptid;
            }

            set
            {
                _presdeptid = value;
            }
        }
        private string _presdeptname;
        [Browsable(false)]
        public string PresDeptName
        {
            get
            {
                return _presdeptname;
            }

            set
            {
                _presdeptname = value;
            }
        }
        private int _presdoctorid;
        [Browsable(false)]
        public int PresDoctorId
        {
            get
            {
                return _presdoctorid;
            }

            set
            {
                _presdoctorid = value;
            }
        }
        private string _presdoctorname;
        [Browsable(false)]
        public string PresDoctorName
        {
            get
            {
                return _presdoctorname;
            }

            set
            {
                _presdoctorname = value;
            }
        }
        private int _patstatus;
        [Browsable(false)]
        public int PatStatus
        {
            get
            {
                return _patstatus;
            }

            set
            {
                _patstatus = value;
            }
        }
        public DataTable GetGridOrder
        {
            get
            {
                return (DataTable)dgvOrders.DataSource;
            }
        }
        [Browsable(false)]
        public int GridRowIndex
        {
            get
            {
                if (dgvOrders.CurrentCell != null)
                    return dgvOrders.CurrentCell.RowIndex;
                else
                    return -1;
            }
        }

        public void InitializeCardData(DataSet cardDataSource)
        {
            this.dgvOrders.BindSelectionCardDataSource(0, cardDataSource.Tables["itemdrug"]);//药品项目 
            this.dgvOrders.BindSelectionCardDataSource(1, cardDataSource.Tables["usagedic"]);//用法
            this.dgvOrders.BindSelectionCardDataSource(2, cardDataSource.Tables["frequencydic"]);//频次  
            this.dgvOrders.BindSelectionCardDataSource(3, cardDataSource.Tables["unitdic"]);//单位         
            this.dgvOrders.BindSelectionCardDataSource(4, cardDataSource.Tables["entrustdic"]);//嘱托
        }

        public void ShowCardItemDrugSet(DataTable dtItemDrug)
        {            
            this.dgvOrders.BindSelectionCardDataSource(0, dtItemDrug);//药品项目     
        }
        public void ShowCardUnitSet(DataTable dtUnit)
        {
            this.dgvOrders.BindSelectionCardDataSource(3, dtUnit);//药品单位    
        }
        public void LoadGridOrderData(DataTable orderData)
        {
            dgvOrders.DataSource = orderData;
        }
        //设置颜色
        public void SetGridColor()
        {
            if (dgvOrders != null && dgvOrders.Rows.Count > 0)
            {
                for (int i = 0; i < dgvOrders.Rows.Count; i++)
                {
                    int flag = Convert.IsDBNull(dgvOrders[Status.Name, i].Value) ? 0 : Convert.ToInt32(dgvOrders[Status.Name, i].Value);
                    int _modifyFlag = Convert.IsDBNull(dgvOrders[ModifyFlag.Name, i].Value) ? 0 : Convert.ToInt32(dgvOrders[ModifyFlag.Name, i].Value);
                    int groupFlag = Convert.IsDBNull(dgvOrders[GroupFlag.Name, i].Value) ? 0 : Convert.ToInt32(dgvOrders[GroupFlag.Name, i].Value);
                    if (flag == (int)OrderStatus.医生新开)
                    {
                        dgvOrders.SetRowColor(i, Color.Black, true);
                    }
                    else if (flag == (int)OrderStatus.医生保存 && _modifyFlag==0)
                    {
                        dgvOrders.SetRowColor(i, Color.Black, Color.Honeydew);
                    }                    
                    else if (flag == (int)OrderStatus.医生发送 && _modifyFlag==0)
                    {
                        dgvOrders.SetRowColor(i, Color.SeaGreen, Color.White);
                    }
                    else if (flag != (int)OrderStatus.医生新开 && _modifyFlag == 1)
                    {
                        dgvOrders.SetRowColor(i, Color.Black, Color.Orange);
                    }
                    else if (flag == (int)OrderStatus.已经转抄)
                    {
                        dgvOrders.SetRowColor(i, Color.Blue, Color.White);
                    }
                    else if (flag == (int)OrderStatus.医生停嘱)
                    {
                        dgvOrders.SetRowColor(i, Color.Gray, Color.White);
                    }
                    else if (flag == (int)OrderStatus.转抄停嘱)
                    {
                        dgvOrders.SetRowColor(i, Color.Maroon, Color.WhiteSmoke);
                    }
                    else if (flag == (int)OrderStatus.执行完毕)
                    {
                        dgvOrders.SetRowColor(i, Color.Black, Color.WhiteSmoke);
                    }
                    else if (flag != (int)OrderStatus.医生新开 && _modifyFlag == 1)
                    {
                        dgvOrders.SetRowColor(i, Color.Black, Color.White);
                    }

                }
            }
        }
        //设置只读
        public void SetReadOnly(ReadOnlyType readonlyType)
        {
            if (readonlyType == ReadOnlyType.全部只读)
            {
                dgvOrders.ReadOnly = true;
            }
            else
            {
                dgvOrders.ReadOnly = false;
                ShowTeminalNum.ReadOnly = true;
                OrderDocName.ReadOnly = true;
                ExecNurseName.ReadOnly = true;
                ExecDate.ReadOnly = true;
                EOrderDocName.ReadOnly = true;
                EOrderDate.ReadOnly = true;
                DosageUnit.ReadOnly = true;

                ShowOrderBdate.ReadOnly = true;
                ShowChannel.ReadOnly = true;
                ShowFrency.ReadOnly = true;
                ShowFirstNum.ReadOnly = true;
                ShowTeminalNum.ReadOnly = true;

                Entrust.ReadOnly = true; 
                ShowDoseNum.ReadOnly = true;
                Amount.ReadOnly = true;               
                ItemName.ReadOnly = true;
                DropSpec.ReadOnly = true;
                Dosage.ReadOnly = true;
                if (readonlyType == ReadOnlyType.新开)
                {
                    ItemName.ReadOnly = false;
                    Dosage.ReadOnly = false;
                    //ShowChannel.ReadOnly = false;
                    //ShowFrency.ReadOnly = false;
                    //DropSpec.ReadOnly = false;              
                    //Entrust.ReadOnly = false;             
                    //Amount.ReadOnly = false;
                    //Unit.ReadOnly = false;
                    //ShowFirstNum.ReadOnly = false;                    
                }
                if (readonlyType == ReadOnlyType.不能修改)
                {
                 
                }
                if (readonlyType == ReadOnlyType.中草药)
                {
                    Unit.ReadOnly = false;
                    ShowDoseNum.ReadOnly = false;
                    Dosage.ReadOnly = false;
                    Amount.ReadOnly = false;
                    ShowChannel.ReadOnly = false;
                    ShowFrency.ReadOnly = false;
                    Entrust.ReadOnly = false;
                    ItemName.ReadOnly = false;
                }
                if (readonlyType == ReadOnlyType.项目)
                {
                    if (OrderStyle == OrderCategory.临时医嘱)
                    {
                        ItemName.ReadOnly = false;
                        Amount.ReadOnly = false;
                        Unit.ReadOnly = false;
                        ShowChannel.ReadOnly = false;
                        Entrust.ReadOnly = false;
                        Dosage.ReadOnly = false;
                    }
                    if (OrderStyle == OrderCategory.长期医嘱)
                    {
                        ItemName.ReadOnly = false;
                        ShowChannel.ReadOnly = false;
                        ShowFrency.ReadOnly = false;
                        ShowFirstNum.ReadOnly = false;
                        Entrust.ReadOnly = false;
                        Dosage.ReadOnly = false;
                    }                    
                }
                if (readonlyType == ReadOnlyType.药品非中草药)
                {
                    ItemName.ReadOnly = false;
                    ShowChannel.ReadOnly = false;
                    ShowFrency.ReadOnly = false;
                    ShowFirstNum.ReadOnly = false;
                    Entrust.ReadOnly = false;
                    DropSpec.ReadOnly = false;
                    Dosage.ReadOnly = false;
                    if (OrderStyle == OrderCategory.临时医嘱)
                    {
                        Unit.ReadOnly = false;
                        Amount.ReadOnly = false;
                    }                  
                }
                if (readonlyType == ReadOnlyType.同组增加)
                {
                    ItemName.ReadOnly = false;
                    Dosage.ReadOnly = false;
                    Unit.ReadOnly = false;
                    Amount.ReadOnly = false;
                }
            }
        }
        public Rectangle GridCellBounds(int rowIndex)
        {
            Rectangle rectangle = new Rectangle(this.dgvOrders.GetCellDisplayRectangle(this.ItemName.Index, rowIndex, false).X,
                   this.dgvOrders.GetCellDisplayRectangle(this.ItemName.Index, rowIndex, false).Y,
                   this.dgvOrders.GetCellDisplayRectangle(this.ItemName.Index, rowIndex, false).Width + this.dgvOrders.GetCellDisplayRectangle(this.ItemName.Index, rowIndex, false).Width,
                   this.dgvOrders.GetCellDisplayRectangle(this.ItemName.Index, rowIndex, false).Height);

            return rectangle;
        }
        public void SetGridCurrentCell(int rowIndex, int colIndex)
        {
            this.dgvOrders.Focus();
            if (rowIndex > -1 && this.dgvOrders.Rows.Count > 0)
            {
                this.dgvOrders.CurrentCell = this.dgvOrders[colIndex, rowIndex];               
            }
        }
        public void SetGridCurrentCell(int rowIndex, string colName)
        {
            this.dgvOrders.Focus();
            if (rowIndex > -1 && this.dgvOrders.Rows.Count > 0)
            {
                this.dgvOrders.CurrentCell = this.dgvOrders[colName, rowIndex];
            }
        }

        #region 医嘱管理界面中获得一组医嘱的起始点和终点
        /// <summary>
        /// 医嘱管理界面中获得一组医嘱的起始点和终点  
        /// </summary>
        /// <param name="nrow"></param>
        /// <param name="myTb"></param>
        /// <param name="beginNum"></param>
        /// <param name="endNum"></param>
        public void FindBeginEnd(int nrow, DataTable myTb, ref int beginNum, ref int endNum)
        {
            if (myTb.Rows.Count > 0)
            {
                int groupid = Convert.ToInt32(myTb.Rows[nrow]["GroupID"]==DBNull.Value?-1: myTb.Rows[nrow]["GroupID"]);
                int i = 0;
                beginNum = nrow;
                endNum = nrow;
                for (i = nrow; i <= myTb.Rows.Count - 1; i++)
                {
                    if (i + 1 == myTb.Rows.Count)
                    {
                        endNum = i;
                        break;
                    }
                    if (myTb.Rows[i + 1]["GroupID"].ToString() == groupid.ToString() && Convert.ToInt32(myTb.Rows[i + 1]["GroupFlag"]) ==0)
                    {
                        endNum = i + 1;
                    }
                    else break;
                }
                for (i = nrow; i >= 0; i--)
                {
                    if (i == 0)
                    {
                        break;
                    }
                    if (myTb.Rows[i]["GroupID"].ToString() == groupid.ToString() && Convert.ToInt32(myTb.Rows[i]["GroupFlag"]) == 1)
                    {
                        beginNum = i;
                        break;
                    }
                    else break;
                }
            }
        }
        #endregion

        #endregion

        private void dgvOrders_Paint(object sender, PaintEventArgs e)
        {
            Pen pen = new Pen(Color.Black,2);//组线画笔
            int x1, y1, x2, y2, y3, y4;//y1为组头横线位置，y2为组线底位置，y3为组线顶位置，y4为组尾横线位置
            x1 = y1 = x2 = y2 = 0;
            for (int i = 0; i < dgvOrders.Rows.Count; i++)
            {
                int beginNum = 0;
                int endNum = 0;

                this.FindBeginEnd(i, (DataTable)dgvOrders.DataSource, ref beginNum, ref endNum);

                if (beginNum != endNum)
                {
                    for (int j = beginNum; j < endNum + 1; j++)
                    {
                        x1 = dgvOrders.GetCellDisplayRectangle(0, j, false).Right-5;
                        x2 = dgvOrders.GetCellDisplayRectangle(0, j, false).Right;
                        y1 = dgvOrders.GetCellDisplayRectangle(0, j, false).Top + dgvOrders.GetCellDisplayRectangle(0, j, false).Height * 1 / 5;
                        y2 = dgvOrders.GetCellDisplayRectangle(0, j, false).Bottom;
                        y3 = dgvOrders.GetCellDisplayRectangle(0, j, false).Top;
                        y4 = dgvOrders.GetCellDisplayRectangle(0, j, false).Bottom - dgvOrders.GetCellDisplayRectangle(0, j, false).Height * 1 / 5;
                        if (j == beginNum)
                        {
                            e.Graphics.DrawLine(pen, x1, y1, x2, y1);
                            e.Graphics.DrawLine(pen, x1, y1, x1, y2);
                        }
                        else if (j == endNum)
                        {
                            e.Graphics.DrawLine(pen, x1, y3, x1, y4);
                            e.Graphics.DrawLine(pen, x1, y4, x2, y4);
                        }
                        else
                        {
                            e.Graphics.DrawLine(pen, x1, y3, x1, y2);
                        }
                    }
                }
                i = endNum;
            }
        }
        #region 网格事件

        private void dgvOrders_CurrentCellChanged(object sender, EventArgs e)
        {
            //控制网格读写状态
            if (dgvOrders.CurrentCell != null)
            {
                int currentRowIndex = dgvOrders.CurrentCell.RowIndex;
                controller.SetReadOnly(currentRowIndex);                
                if (!this.dgvOrders.CurrentCell.ReadOnly
                   && this.dgvOrders.CurrentCell.ColumnIndex != this.ItemName.Index)
                {
                    this.dgvOrders.BeginEdit(true);
                }
            }
        }

        private void dgvOrders_SelectCardRowSelected(object SelectedValue, ref bool stop, ref int customNextColumnIndex)
        {
            dgvOrders.CurrentCellChanged -= dgvOrders_CurrentCellChanged;
            dgvOrders.EndEdit();
            controller.SelectCardBindData(dgvOrders.CurrentCell.RowIndex, (DataRow)SelectedValue, dgvOrders.Columns[dgvOrders.CurrentCell.ColumnIndex].Name);
            if (dgvOrders.Columns[dgvOrders.CurrentCell.ColumnIndex].Name == "Unit")
            {
                controller.AddEmptyRow(dgvOrders.CurrentCell.RowIndex + 1, 1);
                dgvOrders.CurrentCell = this.dgvOrders[1, dgvOrders.CurrentCell.RowIndex + 1];
                stop = true;
            }
            dgvOrders.CurrentCellChanged += new EventHandler(dgvOrders_CurrentCellChanged);
       
        }

        private void dgvOrders_DataGridViewCellPressEnterKey(object sender, int colIndex, int rowIndex, ref bool jumpStop)
        {
            DataTable dtt = (DataTable)dgvOrders.DataSource;

            //if (Convert.ToInt32( dtt.Rows[rowIndex]["orderstatus"]) == -1)
            //{
            //    if (rowIndex == dgvOrders.Rows.Count - 1)
            //    {
            //        if (dtt.Columns[colIndex].ColumnName == "Entrust")
            //        {
            //            controller.AddEmptyRow(rowIndex, 1);
            //            //设置录入焦点
            //            jumpStop = true;
            //            dgvOrders.CurrentCell = dgvOrders[ItemName.Name, rowIndex + 1];

            //        }

            //        //else if (colIndex == dgPrescription.Columns[ItemID.Name].Index)
            //        //{
            //        //    //如果是检索列
            //        //    if (Convert.ToInt32(dgPrescription[ItemID.Name, rowIndex].Value) == 0)
            //        //    {
            //        //        jumpStop = true;
            //        //    }
            //        //}
            //    }
            //}


            if (dtt.Rows[rowIndex]["orderstatus"].ToString() != "-1")
            {
                return;
            }
            //if (dtt.Columns[colIndex].ColumnName == "Entrust" && rowIndex == (this.dgvOrders.RowCount - 1))
            //{
            //    controller.AddEmptyRow(rowIndex+1, 1);
            //    jumpStop = true;
            //    dgvOrders.CurrentCell = this.dgvOrders[1, rowIndex+1];
            //    return;
            //}
            if (OrderStyle == OrderCategory.长期医嘱)
            {                
                if (colIndex >=ShowFirstNum.Index && Convert.ToInt32(dtt.Rows[rowIndex]["groupFlag"]) == 1)
                {
                    if (Convert.ToInt32(dtt.Rows[rowIndex]["ItemID"]) > 0 && Convert.ToInt32(dtt.Rows[rowIndex]["ItemType"]) == 1)
                    {
                        controller.AddEmptyRow(rowIndex + 1, 1);
                        jumpStop = true;
                        dgvOrders.CurrentCell = this.dgvOrders[1, rowIndex + 1];
                        return;
                    }
                    else//只有药品才分组
                    {
                        jumpStop = true;
                        NewOrder();
                        return;
                    }
                }
                if (colIndex >= Dosage.Index && Convert.ToInt32(dtt.Rows[rowIndex]["groupFlag"]) == 0 )
                {
                    if (Convert.ToInt32(dtt.Rows[rowIndex]["ItemID"]) > 0 && Convert.ToInt32(dtt.Rows[rowIndex]["ItemType"]) == 1)
                    {
                        controller.AddEmptyRow(rowIndex + 1, 1);
                        jumpStop = true;
                        dgvOrders.CurrentCell = this.dgvOrders[1, rowIndex + 1];
                        return;
                    }
                    else
                    {
                        jumpStop = true;
                        NewOrder();
                        return;
                    }
                }
            }
            else
            {
                if (colIndex >= Unit.Index  && Convert.ToInt32(dtt.Rows[rowIndex]["ItemType"]) == 1)
                {
                    controller.AddEmptyRow(rowIndex + 1, 1);
                    jumpStop = true;
                    dgvOrders.CurrentCell = this.dgvOrders[1, rowIndex + 1];
                    return;
                }
                else
                {
                    jumpStop = true;
                    NewOrder();
                    return;
                }
            }
        }
        #endregion

        private void buttonItem1_Click(object sender, EventArgs e)
        {
            if (dgvOrders.Rows.Count == 0)
            {
                controller.AddEmptyRow(dgvOrders.Rows.Count, 0);
                dgvOrders.CurrentCell = dgvOrders[ItemName.Name, dgvOrders.Rows.Count - 1];
            }
            else
            {
                if (dgvOrders["ItemName", dgvOrders.Rows.Count - 1].Value.ToString() != "")
                {
                    controller.AddEmptyRow(dgvOrders.Rows.Count-1, 0);
                    dgvOrders.CurrentCell = dgvOrders[ItemName.Name, dgvOrders.Rows.Count - 1];
                }
                else
                {
                    dgvOrders.CurrentCell = dgvOrders[ItemName.Name, dgvOrders.Rows.Count - 1];
                }
            }
        }
        /// <summary>
        /// 新开
        /// </summary>
        public void NewOrder()
        {
            //controller.AddEmptyRow(dgvOrders.Rows.Count, 0);
            //dgvOrders.CurrentCell = dgvOrders[ItemName.Name, dgvOrders.Rows.Count - 1];
            if (dgvOrders.Rows.Count == 0)
            {
                controller.AddEmptyRow(dgvOrders.Rows.Count, 0);
                dgvOrders.CurrentCell = dgvOrders[ItemName.Name, dgvOrders.Rows.Count - 1];
            }
            else
            {
                if (dgvOrders["ItemName", dgvOrders.Rows.Count - 1].Value.ToString() != "")
                {
                    controller.AddEmptyRow(dgvOrders.Rows.Count, 0);
                    dgvOrders.CurrentCell = dgvOrders[ItemName.Name, dgvOrders.Rows.Count - 1];
                }
                else
                {
                    dgvOrders.CurrentCell = dgvOrders[ItemName.Name, dgvOrders.Rows.Count - 1];
                }
            }
        }
        /// <summary>
        /// 刷新
        /// </summary>
        public void RefreshOrder()
        {
            List<OrderRecord> notSaveRecores = GetNotSaveOrders();
            if (notSaveRecores.Count > 0)
            {
                string strName = "";
                foreach (OrderRecord order in notSaveRecores)
                {
                    strName += order.ItemName + "\n";
                }
                if (MessageBoxEx.Show("还有下列医嘱未保存:\n" +strName+ "是否继续刷新？", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    controller.BindOrderData();
                }
            }
            else
            {
                controller.BindOrderData();
            }
        }
        private List<OrderRecord> GetNotSaveOrders()
        {
            DataTable dt = (DataTable)dgvOrders.DataSource;
            List<OrderRecord> listRecords = EFWCoreLib.CoreFrame.Common.ConvertExtend.ToList<OrderRecord>(dt);
            List<OrderRecord> notSaveRecores = listRecords.Where(p => p.ModifyFlag == 1 && !string.IsNullOrEmpty(p.ItemName) && p.ItemID>0).ToList();
            return notSaveRecores;
        }
        /// <summary>
        /// 医嘱保存
        /// </summary>
        public void SaveOrder()
        {
            dgvOrders.EndEdit();
            List<OrderRecord> notSaveRecores = GetNotSaveOrders();
            if (notSaveRecores.Count > 0)
            {
                controller.SaveRecores(notSaveRecores);                
            }
            else
            {
                controller.BindOrderData();
            }
        }
        public void ShowMessage(string message)
        {
            MessageBoxEx.Show(message);
        }
        private void dgvOrders_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (OrderStyle == OrderCategory.临时医嘱)
            {
                if (dgvOrders.CurrentCell != null)
                {
                    if (e.ColumnIndex == dgvOrders.Columns[Dosage.Name].Index)
                    {
                        controller.CaculateTempOrderAmout(e.RowIndex);
                        dgvOrders.Refresh();
                    }
                }
            }
        }

        private void 修改医嘱ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dgvOrders.CurrentCell != null)
            {
                int rowIndex = dgvOrders.CurrentCell.RowIndex;
                int status = Convert.ToInt32(dgvOrders["Status", rowIndex].Value);
                DataTable dt =(DataTable) dgvOrders.DataSource;
                if (Convert.ToInt32(dt.Rows[rowIndex]["ItemType"]) == 4)
                {
                    //调用医技申请界面修改

                }
                else
                {
                    if (status == 0 || status == 1)
                    {
                        controller.SetOrderModifyStatus(rowIndex);
                    }
                }  
            }
        }

        private void 插入一行ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dgvOrders.CurrentCell != null)
            {
                int rowIndex = dgvOrders.CurrentCell.RowIndex;
                int status = Convert.ToInt32(dgvOrders["Status", rowIndex].Value);                
                if (status == 0 || status == 1)
                { 
                    DataTable dt =(DataTable) dgvOrders.DataSource;
                    if (Convert.ToInt32(dt.Rows[rowIndex]["ItemType"]) == 1)
                    {
                        controller.AddEmptyRow(rowIndex + 1, 1);
                        dgvOrders.CurrentCell = dgvOrders[1, rowIndex + 1];
                    }
                    else
                    {
                        if (dgvOrders["ItemName", rowIndex].Value.ToString() != "")
                        {
                            controller.AddEmptyRow(rowIndex+1, 0);
                            dgvOrders.CurrentCell = dgvOrders[ItemName.Name,rowIndex+1];
                        }
                        else
                        {
                            dgvOrders.CurrentCell = dgvOrders[ItemName.Name, rowIndex];
                        }

                        //controller.AddEmptyRow(rowIndex + 1, 0);
                        //dgvOrders.CurrentCell = dgvOrders[1, rowIndex + 1];
                    }
                }
            }
        }

        private void 刷新选项卡ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            controller.RefreshOrderShowCard(_drugStoreDeptID, true);
        }

        private void dgvOrders_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvOrders != null && dgvOrders.CurrentCell != null)
            {
                int rowindex = dgvOrders.CurrentCell.RowIndex;
                int colindex = dgvOrders.CurrentCell.ColumnIndex;
                if (colindex != 0)
                {
                    return;
                }
                DataTable dt = (DataTable)dgvOrders.DataSource;
                if (Convert.ToInt32(dt.Rows[rowindex]["GroupFlag"]) == 0)
                {
                    return;
                }
                if (dt.Rows[rowindex]["ItemName"].ToString() == "")
                {
                    return;
                }
                if (Convert.ToInt32(dt.Rows[rowindex]["orderStatus"].ToString().Trim()) > 1)
                {
                    MessageBox.Show("该医嘱已执行，不能修改录入时间");
                    return;
                }
                FrmOrderTime ftime = new FrmOrderTime(Convert.ToDateTime(dt.Rows[rowindex]["OrderBdate"].ToString()), Convert.ToDateTime(enterDate));
                ftime.ShowDialog();
                if (ftime.Ok)
                {
                    DateTime btime = ftime.alterDate;
                    dt.Rows[rowindex]["ShowOrderBdate"] = btime;
                    dt.Rows[rowindex]["ModifyFlag"] = 1;
                    dt.Rows[rowindex]["OrderBdate"] = btime;
                    if (Convert.ToInt32(dt.Rows[rowindex]["orderStatus"].ToString().Trim()) != -1)
                    {
                        controller.ChangeValue(dt, rowindex, "ShowOrderBdate");
                        SetGridColor();
                    }
                }

            }
        }

        private void 删除一行ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //如果特殊医嘱，还需要恢复停嘱
            if (dgvOrders != null && dgvOrders.CurrentCell != null)
            {
                int rowindex = dgvOrders.CurrentCell.RowIndex;
                int colindex = dgvOrders.CurrentCell.ColumnIndex;
                DataTable dt = (DataTable)dgvOrders.DataSource;
                int groupFlag = Convert.ToInt32(dt.Rows[rowindex]["GroupFlag"]);
                int status = Convert.ToInt32(dt.Rows[rowindex]["orderStatus"]);
                if (status > 1)
                {
                    MessageBox.Show("该医嘱已执行，不能删除");
                    return;
                }
                if (groupFlag == 0)
                {
                    if (status != -1)
                    {
                        dt.Rows[rowindex]["DeleteFlag"] = 1;
                        OrderRecord delRecord = EFWCoreLib.CoreFrame.Common.ConvertExtend.ToObject<OrderRecord>(dt, rowindex);
                        List<OrderRecord> list = new List<Entity.OrderRecord>();
                        list.Add(delRecord);
                        if (controller.DeleteRecored(list))
                        {
                            dt.Rows.RemoveAt(rowindex);
                        }
                    }
                    else
                    {
                        dt.Rows.RemoveAt(rowindex);
                    }
                    
                }
                else
                {
                    int begnum = 0;
                    int endnum = 0;
                    FindBeginEnd(rowindex, dt, ref begnum, ref endnum);
                    if (begnum == endnum)
                    {
                        if (status != -1)
                        {
                            dt.Rows[rowindex]["DeleteFlag"] = 1;
                            OrderRecord delRecord = EFWCoreLib.CoreFrame.Common.ConvertExtend.ToObject<OrderRecord>(dt, rowindex);
                            List<OrderRecord> list = new List<Entity.OrderRecord>();
                            list.Add(delRecord);
                            if (controller.DeleteRecored(list))
                            {
                                dt.Rows.RemoveAt(rowindex);
                            }
                        }
                        else
                        {
                            dt.Rows.RemoveAt(rowindex);
                        }

                    }
                    else
                    {
                        dt.Rows[rowindex + 1]["ShowOrderBdate"] = dt.Rows[rowindex]["ShowOrderBdate"];
                        dt.Rows[rowindex + 1]["ShowDoseNum"] = dt.Rows[rowindex]["ShowDoseNum"];
                        dt.Rows[rowindex + 1]["ShowChannel"] = dt.Rows[rowindex]["ShowChannel"];
                        dt.Rows[rowindex + 1]["ShowFrency"] = dt.Rows[rowindex]["ShowFrency"];
                        dt.Rows[rowindex + 1]["ShowFirstNum"] = dt.Rows[rowindex]["ShowFirstNum"];
                        dt.Rows[rowindex + 1]["GroupFlag"] = 1;
                        if (status != -1)
                        {
                            dt.Rows[rowindex]["DeleteFlag"] = 1;
                            OrderRecord delRecord = EFWCoreLib.CoreFrame.Common.ConvertExtend.ToObject<OrderRecord>(dt, rowindex);
                            List<OrderRecord> list = new List<Entity.OrderRecord>();
                            list.Add(delRecord);
                            if (controller.DeleteRecored(list))
                            {
                                dt.Rows.RemoveAt(rowindex);
                            }
                        }
                        else
                        {
                            dt.Rows.RemoveAt(rowindex);
                        }
                    }
                }
                dgvOrders.CurrentCell = dgvOrders[1, rowindex - 1];
            }
        }

        private void 删除一组ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dgvOrders != null && dgvOrders.CurrentCell != null)
            {
                int rowindex = dgvOrders.CurrentCell.RowIndex;
                int colindex = dgvOrders.CurrentCell.ColumnIndex;
                DataTable dt = (DataTable)dgvOrders.DataSource;
                int groupFlag = Convert.ToInt32(dt.Rows[rowindex]["GroupFlag"]);
                int status = Convert.ToInt32(dt.Rows[rowindex]["orderStatus"]);
                if (status > 1)
                {
                    MessageBox.Show("该医嘱已执行，不能删除");
                    return;
                }
                int begnum = 0;
                int endnum = 0;
                FindBeginEnd(rowindex, dt, ref begnum, ref endnum);
                List<OrderRecord> list = new List<Entity.OrderRecord>();
                for (int index = endnum; index >= begnum; index--)
                {
                    if (status != -1)
                    {
                        dt.Rows[rowindex]["DeleteFlag"] = 1;
                        OrderRecord delRecord = EFWCoreLib.CoreFrame.Common.ConvertExtend.ToObject<OrderRecord>(dt, index);                       
                        list.Add(delRecord);                     
                    }
                    else
                    {
                        dt.Rows.RemoveAt(index);
                    }
                }
                if (status != -1)
                {
                    if (controller.DeleteRecored(list))
                    {
                        for (int index = endnum; index >= begnum; index--)
                        {
                            dt.Rows.RemoveAt(index);
                        }
                    }
                }
            }
        }

        public bool CloseCheck()
        {
            List<OrderRecord> notSaveRecores = GetNotSaveOrders();
            if (notSaveRecores.Count == 0)
            {
                return true;
            }
            else
            {
                string strName = "";
                foreach (OrderRecord order in notSaveRecores)
                {
                    strName += order.ItemName + "\n";
                }
                if (MessageBoxEx.Show("还有下列医嘱未保存:\n" + strName + "是否继续关闭？", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    return true;
                }
            }
            return false;
        }
        public void SendOrder()
        {
            dgvOrders.EndEdit();
            List<OrderRecord> notSaveRecores = GetNotSaveOrders();
            if (notSaveRecores.Count == 0)
            {
                controller.SendOrderRecord();
            }
            else
            {
                string strName = "";
                foreach (OrderRecord order in notSaveRecores)
                {
                    strName += order.ItemName + "\n";
                }
                MessageBoxEx.Show("还有下列医嘱未保存:\n" + strName + ",请先保存医嘱");              
            }
        }
        public void SaveAstoflinkage(List<Entity.OrderRecord> data)
        {
            if (Astoflinkage != null)
            {
                Astoflinkage(CurrPatListId, data);
            }
        }
    }
    
    //医嘱保存时费用联动
    public delegate void PrescriptionCostoflinkage(int patListId, List<Entity.OrderRecord> data);
    /// <summary>
    /// 开医嘱时增加皮试医嘱
    /// </summary>
    /// <param name="patListId"></param>
    /// <param name="data"></param>
    public delegate void PrescriptionAstoflinkage(int patListId, List<Entity.OrderRecord> data);
}
