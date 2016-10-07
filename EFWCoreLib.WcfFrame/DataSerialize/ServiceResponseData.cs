using EFWCoreLib.WcfFrame.SDMessageHeader;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using ProtoBuf;
using ProtoBuf.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace EFWCoreLib.WcfFrame.DataSerialize
{
    /// <summary>
    /// 服务输出数据
    /// </summary>
    public class ServiceResponseData
    {
        List<string> _listjson;
        public ServiceResponseData()
        {
            _listjson = new List<string>();
        }

        public ServiceResponseData(bool IsCompress, bool IsEncrytion, SerializeType SerializeType)
        {
            _iscompressjson = IsCompress;
            _isencryptionjson = IsEncrytion;
            _serializetype = SerializeType;

            _listjson = new List<string>();
        }

        #region wcf服务配置属性
        bool _iscompressjson = false;
        bool _isencryptionjson = false;
        SerializeType _serializetype = SerializeType.Newtonsoft;

        public bool Iscompressjson
        {
            get
            {
                return _iscompressjson;
            }

            set
            {
                _iscompressjson = value;
            }
        }

        public bool Isencryptionjson
        {
            get
            {
                return _isencryptionjson;
            }

            set
            {
                _isencryptionjson = value;
            }
        }

        public SerializeType Serializetype
        {
            get
            {
                return _serializetype;
            }

            set
            {
                _serializetype = value;
            }
        }
        #endregion

        /// <summary>
        /// 添加输出数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        public void AddData<T>(T data)
        {
            if (data is DataTable)
            {
                List<string> dtjson = new List<string>();
                Dictionary<string, string> colnumHead = new Dictionary<string, string>();
                List<Dictionary<string, Object>> rowData = new List<Dictionary<string, object>>();
                //DataTable数据解析
                DataTable dt = data as DataTable;
                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    colnumHead.Add(dt.Columns[i].ColumnName, dt.Columns[i].DataType.FullName);
                }
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    Dictionary<string, Object> row = new Dictionary<string, object>();
                    foreach (var item in colnumHead)
                    {
                        row.Add(item.Key, dt.Rows[i][item.Key]);
                    }
                    rowData.Add(row);
                }
                dtjson.Add(JsonConvert.SerializeObject(colnumHead));
                dtjson.Add(JsonConvert.SerializeObject(rowData));
                _listjson.Add(JsonConvert.SerializeObject(dtjson));
            }
            else if(_serializetype == SerializeType.Newtonsoft)
            {
                if (data is DataTable)
                {
                    _listjson.Add(JsonConvert.SerializeObject(data, Formatting.Indented));
                }
                else
                {
                    _listjson.Add(JsonConvert.SerializeObject(data));
                }
            }
            else if (_serializetype == SerializeType.protobuf)
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    if (data is DataTable)
                    {
                        object obj = data;
                        DataSerializer.Serialize(ms, (DataTable)obj);
                    }
                    else
                    {
                        Serializer.Serialize(ms, data);
                    }
                    _listjson.Add(System.Text.Encoding.UTF8.GetString(ms.ToArray()));
                }
            }
            else if (_serializetype == SerializeType.fastJSON)
            {
                _listjson.Add(fastJSON.JSON.ToJSON(data));
            }
        }
        private Type getColumnType(string fullname)
        {
            switch (fullname)
            {
                case "System.DateTime":
                    return typeof(DateTime);
                case "System.Int64":
                    return typeof(long);
                case "System.Boolean":
                    return typeof(bool);
                case "System.Decimal":
                    return typeof(decimal);
                case "System.Double":
                    return typeof(double);
                case "System.Int32":
                    return typeof(int);
                case "System.Int16":
                    return typeof(short);
                case "System.Single":
                    return typeof(decimal);
                default:
                    return typeof(string);
            }
        }
        private object getRowValue(string fullname,object value)
        {
            switch (fullname)
            {
                case "System.DateTime":
                    return value == null ? new DateTime() : Convert.ToDateTime(value);
                case "System.Int64":
                    return value == null ? 0 : Convert.ToInt64(value);
                case "System.Boolean":
                    return value == null ? false : Convert.ToBoolean(value);
                case "System.Decimal":
                    return value == null ? 0 : Convert.ToDecimal(value);
                case "System.Double":
                    return value == null ? 0 : Convert.ToDouble(value);
                case "System.Int32":
                    return value == null ? 0 : Convert.ToInt32(value);
                case "System.Int16":
                    return value == null ? 0 : Convert.ToInt16(value);
                case "System.Single":
                    return value == null ? 0 : Convert.ToDecimal(value);
                default:
                    return value == null ? "" : value.ToString();
            }
        }
        /// <summary>
        /// 获取输出的指定数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="index">索引</param>
        /// <returns></returns>
        public T GetData<T>(int index)
        {
            if (typeof(T).Name=="DataTable")
            {
                List<string> dtjson = JsonConvert.DeserializeObject<List<string>>(_listjson[index]);
                Dictionary<string, string> colnumHead = JsonConvert.DeserializeObject<Dictionary<string, string>>(dtjson[0]);
                if (colnumHead.Count == 0)
                    return default(T);
                DataTable dt = new DataTable();
                List<Dictionary<string, Object>> rowData = JsonConvert.DeserializeObject<List<Dictionary<string, Object>>>(dtjson[1]);
                foreach (var item in colnumHead)
                {
                    DataColumn dc = new DataColumn(item.Key, getColumnType(item.Value));
                    dt.Columns.Add(dc);
                }

                foreach (var item in rowData)
                {
                    DataRow dr = dt.NewRow();
                    foreach (var val in item)
                    {
                        dr[val.Key] = getRowValue(colnumHead[val.Key], val.Value);
                    }
                    dt.Rows.Add(dr);
                }
                return (T)((Object)dt);
            }
            else if (_serializetype == SerializeType.Newtonsoft)
            {
                return JsonConvert.DeserializeObject<T>(_listjson[index]);
            }
            else if (_serializetype == SerializeType.protobuf)
            {
                byte[] bytes = System.Text.Encoding.UTF8.GetBytes(_listjson[index]);
                using (MemoryStream ms = new MemoryStream(bytes))
                {
                    if (typeof(T).Name == "DataTable")
                    {
                        Object obj = DataSerializer.DeserializeDataTable(ms);
                        return (T)obj;
                    }
                    else
                    {
                        return Serializer.Deserialize<T>(ms);
                    }
                }
            }
            else if (_serializetype == SerializeType.fastJSON)
            {
                return fastJSON.JSON.ToObject<T>(_listjson[index]);
            }
            else
                return default(T);
        }

        /// <summary>
        /// 获取输出的Json数据
        /// </summary>
        /// <returns></returns>
        public string GetJsonData()
        {
            return JsonConvert.SerializeObject(_listjson);
        }
        /// <summary>
        /// 设置输出的Json数据
        /// </summary>
        /// <param name="retData"></param>
        public void SetJsonData(string retData)
        {
            if (string.IsNullOrEmpty(retData) == false)
                _listjson = JsonConvert.DeserializeObject<List<string>>(retData);
        }
    }
}
