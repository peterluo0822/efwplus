using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace EFWCoreLib.WebFrame.HttpHandler
{
    /// <summary>
    /// Web输出数据对象
    /// </summary>
    public class WebResponseData
    {
        public WebResponseData()
        {
        }

        private ResponseDataType _dataType = ResponseDataType.JsonResult;
        /// <summary>
        /// 数据类型
        /// </summary>
        public ResponseDataType DataType
        {
            get
            {
                return _dataType;
            }

            set
            {
                _dataType = value;
            }
        }

        private WebUIFrameType _uiframeType = WebUIFrameType.EasyUI;
        /// <summary>
        /// UI框架
        /// </summary>
        public WebUIFrameType UiframeType
        {
            get
            {
                return _uiframeType;
            }
            set
            {
                _uiframeType = value;
                if (_uiframeType == WebUIFrameType.EasyUI)
                {
                    _uiFrame = new EasyUIFrame();
                }
            }
        }

        private WebUIFrame _uiFrame;
        public WebUIFrame UiFrame
        {
            get
            {
                return _uiFrame;
            }

            set
            {
                _uiFrame = value;
            }
        }

        /// <summary>
        /// 数据结果
        /// </summary>
        public string DataResult { get; set; }


    }
    /// <summary>
    /// Web界面框架
    /// </summary>
    public enum WebUIFrameType
    {
        EasyUI,Resport
    }
    /// <summary>
    /// 输出数据类型
    /// </summary>
    public enum ResponseDataType
    {
        JsonResult=0,
        ViewResult=1
    }

    public abstract class WebUIFrame
    {

    }

    public class EasyUIFrame: WebUIFrame
    {
        #region EasyUI

        public string ToJson(object model)
        {
            string value = JsonConvert.SerializeObject(model);
            return value;
        }

        public string ToJson(System.Data.DataTable dt)
        {
            string value = JsonConvert.SerializeObject(dt, Formatting.Indented);
            return value;
        }

        public string ToJson(System.Collections.Hashtable hash)
        {
            string value = JsonConvert.SerializeObject(hash);
            return value;
        }

        public string ToGridJson(string rowsjson, int totalCount)
        {
            return ToGridJson(rowsjson, null, totalCount);
        }

        public string ToGridJson(string rowsjson, string footjson, int totalCount)
        {
            if (footjson == null)
                return "{\"total\":" + totalCount + ",\"rows\":" + rowsjson + "}";
            else
                return "{\"total\":" + totalCount + ",\"rows\":" + rowsjson + ",\"footer\":" + footjson + "}";
        }

        public string ToGridJson(System.Data.DataTable dt)
        {
            return ToGridJson(dt, -1, null);
        }

        public string ToGridJson(System.Data.DataTable dt, int totalCount)
        {
            return ToGridJson(dt, totalCount, null);
        }

        public string ToGridJson(System.Data.DataTable dt, int totalCount, System.Collections.Hashtable[] footers)
        {
            totalCount = totalCount == -1 ? dt.Rows.Count : totalCount;
            string rowsjson = ToJson(dt);
            string footjson = footers == null ? null : ToJson(footers);
            return ToGridJson(rowsjson, footjson, totalCount);
        }

        public string ToGridJson(System.Collections.IList list)
        {
            return ToGridJson(list, -1, null);
        }

        public string ToGridJson(System.Collections.IList list, int totalCount)
        {
            return ToGridJson(list, totalCount, null);
        }

        public string ToGridJson(System.Collections.IList list, int totalCount, System.Collections.Hashtable[] footers)
        {
            totalCount = totalCount == -1 ? list.Count : totalCount;
            string rowsjson = ToJson(list);
            string footjson = footers == null ? null : ToJson(footers);
            return ToGridJson(rowsjson, footjson, totalCount);

        }

        public string ToTreeJson(List<treeNode> list)
        {
            string value = JsonConvert.SerializeObject(list);
            value = value.Replace("check", "checked");
            return value;
        }

        public string ToTreeGridJson(List<treeNodeGrid> list)
        {
            return ToTreeGridJson(list, null);
        }

        public string ToTreeGridJson(List<treeNodeGrid> list, System.Collections.Hashtable[] footers)
        {
            List<Hashtable> hashlist = new List<Hashtable>();
            for (int i = 0; i < list.Count; i++)
            {
                Hashtable hash = new Hashtable();
                hash.Add("id", list[i].id);
                if (list[i]._parentId > 0)
                    hash.Add("_parentId", list[i]._parentId);
                if (!string.IsNullOrEmpty(list[i].state))
                    hash.Add("state", list[i].state);
                if (!string.IsNullOrEmpty(list[i].iconCls))
                    hash.Add("iconCls", list[i].iconCls);
                if (list[i].check)
                    hash.Add("check", list[i].check);
                if (list[i].model != null)
                {
                    PropertyInfo[] propertys = list[i].model.GetType().GetProperties();
                    for (int j = 0; j < propertys.Length; j++)
                    {
                        if (!hash.ContainsKey(propertys[j].Name))
                            hash.Add(propertys[j].Name, propertys[j].GetValue(list[i].model, null));
                    }
                }

                hashlist.Add(hash);
            }

            int totalCount = hashlist.Count;
            string rowsjson = ToJson(hashlist);
            string footjson = footers == null ? null : ToJson(footers);
            return ToTreeGridJson(rowsjson, footjson, totalCount);
        }

        public string ToTreeGridJson(System.Data.DataTable dt, string IdName, string _parentIdName)
        {
            return ToTreeGridJson(dt, IdName, _parentIdName, null);
        }

        public string ToTreeGridJson(System.Data.DataTable dt, string IdName, string _parentIdName, System.Collections.Hashtable[] footers)
        {
            List<Hashtable> hashlist = new List<Hashtable>();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                Hashtable hash = new Hashtable();
                hash.Add("id", dt.Rows[i][IdName]);
                if (Convert.ToInt32(dt.Rows[i][_parentIdName]) > 0)
                    hash.Add("_parentId", dt.Rows[i][_parentIdName]);
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    if (dt.Columns[j].ColumnName.ToLower() == IdName.ToLower()) continue;
                    if (dt.Columns[j].ColumnName.ToLower() == _parentIdName.ToLower()) continue;

                    hash.Add(dt.Columns[j].ColumnName, dt.Rows[i][j]);
                }
                hashlist.Add(hash);
            }

            int totalCount = hashlist.Count;
            string rowsjson = ToJson(hashlist);
            string footjson = footers == null ? null : ToJson(footers);
            return ToTreeGridJson(rowsjson, footjson, totalCount);
        }

        public string ToTreeGridJson(string rowsjson, string footjson, int totalCount)
        {
            if (footjson == null)
                return "{\"total\":" + totalCount + ",\"rows\":" + rowsjson + "}";
            else
                return "{\"total\":" + totalCount + ",\"rows\":" + rowsjson + ",\"footer\":" + footjson + "}";
        }

        #endregion
    }

    /// <summary>
    /// 树形数据，EasyUI
    /// </summary>
    public class treeNode
    {
        private int _id;

        public int id
        {
            get { return _id; }
            set { _id = value; }
        }
        private string _text;

        public string text
        {
            get { return _text; }
            set { _text = value; }
        }
        private bool _check = false;
        public bool check
        {
            get { return _check; }
            set { _check = value; }
        }

        private string _state = "open";
        public string state
        {
            get { return _state; }
            set { _state = value; }
        }

        private string _iconCls = "";
        public string iconCls
        {
            get { return _iconCls; }
            set { _iconCls = value; }
        }

        private List<treeNode> _children;

        public List<treeNode> children
        {
            get { return _children; }
            set { _children = value; }
        }

        private Dictionary<string, Object> _attributes;
        public Dictionary<string, Object> attributes
        {
            get { return _attributes; }
            set { _attributes = value; }
        }

        public treeNode(int id, string text)
        {
            _id = id;
            _text = text;
        }
    }
    /// <summary>
    /// 树形网格,EasyUI
    /// </summary>
    public class treeNodeGrid
    {
        private string _state = "";
        public string state
        {
            get { return _state; }
            set { _state = value; }
        }

        public string iconCls
        {
            get;
            set;
        }

        private bool _check = false;
        public bool check
        {
            get { return _check; }
            set { _check = value; }
        }

        public int id { get; set; }

        public int _parentId { get; set; }

        public object model { get; set; }
    }
}
