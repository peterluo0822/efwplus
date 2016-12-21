using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using EFWCoreLib.CoreFrame.Business;

namespace EFWCoreLib.UnitTestFrame
{
    public class BaseDaoUnitTest : AbstractDao
    {
        private Type getType(string type)
        {
            switch (type)
            {
                case "string":
                    return typeof(String);
                case "int":
                    return typeof(Int32);
                case "decimal":
                    return typeof(Decimal);
                case "DateTime":
                    return typeof(DateTime);
            }

            return typeof(String);
        }

        private object convertVal(Type t, object _data)
        {
            string data = _data.ToString();
            object val = null;
            if (t == typeof(Int32))
                val = Convert.ToInt32(data);
            else if (t == typeof(DateTime))
                val = Convert.ToDateTime(data);
            else if (t == typeof(Decimal))
                val = Convert.ToDecimal(data);
            else if (t == typeof(Boolean))
                val = Convert.ToBoolean(data);
            else if (t == typeof(String))
                val = Convert.ToString(data).Trim();
            else if (t == typeof(Guid))
                val = new Guid(data.ToString());
            else if (t == typeof(byte[]))
                if (data != null && data.ToString().Length > 0)
                {
                    val = Convert.FromBase64String(data.ToString());
                }
                else
                {
                    val = null;
                }
            else
                val = data;
            return val;
        }

        protected void createDataTable(DataTable dt, string colnameAndtype)
        {
            if (string.IsNullOrEmpty(colnameAndtype)) return;
            string[] cols = colnameAndtype.Split(new char[] { ',' });
            for (int i = 0; i < cols.Length; i++)
            {
                string colname = cols[i].Split(new char[] { '|' })[0];
                string coltype = cols[i].Split(new char[] { '|' })[1];
                DataColumn dc=new DataColumn(colname,getType(coltype));
                dt.Columns.Add(dc);
            }
        }

        protected void adddataDataTable(DataTable dt, string val)
        {
            if (string.IsNullOrEmpty(val)) return;
            DataRow dr = dt.NewRow();
            string[] vals = val.Split(',');
            for (int i = 0; i < (dt.Columns.Count <= vals.Length ? dt.Columns.Count : vals.Length); i++)
            {
                dr[i] = vals[i];
            }
            dt.Rows.Add(dr);
        }

        protected void adddataList<T>(List<T> list, string val)
        {
            if (string.IsNullOrEmpty(val)) return;
            T t = NewObject<T>();
            string[] vals = val.Split(',');
            PropertyInfo[] pl = t.GetType().GetProperties();
            int ct = pl.Length <= vals.Length ? pl.Length : vals.Length;
            for (int i = 0; i < ct; i++)
            {
                pl[i].SetValue(t, convertVal(pl[i].PropertyType, vals[i]),null);
            }

            list.Add(t);
        }

        protected void adddataObject<T>(T obj, string val)
        {
            if (string.IsNullOrEmpty(val)) return;
            string[] vals = val.Split(',');
            PropertyInfo[] pl = obj.GetType().GetProperties();
            int ct = pl.Length <= vals.Length ? pl.Length : vals.Length;
            for (int i = 0; i < ct; i++)
            {
                pl[i].SetValue(obj, convertVal(pl[i].PropertyType, vals[i]),null);
            }
        }
    }
}
