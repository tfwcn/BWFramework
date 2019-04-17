using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace BWCore.Common
{
    public static class DBEx
    {
        /// <summary>
        /// 對象轉DataTable
        /// </summary>
        public static DataTable ModelToDataTable(this object obj, bool? dbCanRead, bool? dbCanWrite)
        {
            Type objType = obj.GetType();
            //获取表名
            string tableName = objType.Name;
            AttributeEx.DBTableAttribute tableAttribute = Attribute.GetCustomAttribute(objType, typeof(AttributeEx.DBTableAttribute)) as AttributeEx.DBTableAttribute;
            if (tableAttribute != null && !tableAttribute.Name.IsNullOrEmpty())
            {
                tableName = tableAttribute.Name;
            }
            DataTable dt = new DataTable(tableName);
            List<DataColumn> pkList = new List<DataColumn>();
            foreach (var propertieInfo in objType.GetPropertiesPGS(dbCanRead, dbCanWrite))
            {
                //获取字段名
                string colName = propertieInfo.Name;
                AttributeEx.DBColAttribute attribute = Attribute.GetCustomAttribute(propertieInfo, typeof(AttributeEx.DBColAttribute)) as AttributeEx.DBColAttribute;
                if (attribute != null && !attribute.Name.IsNullOrEmpty())
                {
                    colName = attribute.Name;
                }
                DataColumn col = dt.Columns.Add(colName, (propertieInfo.PropertyType.IsGenericType && propertieInfo.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>)) ? propertieInfo.PropertyType.GetGenericArguments()[0] : propertieInfo.PropertyType);
                if (attribute != null && attribute.PKey == true)
                {
                    pkList.Add(col);
                }
            }
            if (pkList.Count > 0)
            {
                dt.PrimaryKey = pkList.ToArray();
            }
            return dt;
        }
        /// <summary>
        /// 對象轉DataTable(含值)
        /// </summary>
        public static DataTable ModelToDataTableHasValue(this object obj, bool? dbCanRead, bool? dbCanWrite)
        {
            Type objType = obj.GetType();
            DataTable dt = ModelToDataTable(obj, dbCanRead, dbCanWrite);
            DataRow dr = dt.NewRow();
            obj.ModelToDataRow(dr, objType.GetPropertiesPGS(dbCanRead, dbCanWrite));
            dt.Rows.Add(dr);
            return dt;
        }
        /// <summary>
        /// 獲取可讀寫公共屬性
        /// </summary>
        public static PropertyInfo[] GetPropertiesPGS(this Type objType, bool? dbCanRead, bool? dbCanWrite)
        {
            List<PropertyInfo> properties = new List<PropertyInfo>();
            foreach (var property in objType.GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.GetProperty | BindingFlags.SetProperty))
            {
                AttributeEx.DBColAttribute attribute = Attribute.GetCustomAttribute(property, typeof(AttributeEx.DBColAttribute)) as AttributeEx.DBColAttribute;
                if (attribute == null || ((dbCanRead == null || attribute.CanRead == dbCanRead) && (dbCanWrite == null || attribute.CanWrite == dbCanWrite)))
                {
                    properties.Add(property);
                }
            }
            return properties.ToArray();
        }
        /// <summary>
        /// 獲取屬性值
        /// </summary>
        public static object GetPropertyValue(this object obj, string name)
        {
            Type objType = obj.GetType();
            return objType.GetProperty(name).GetValue(obj);
        }
        /// <summary>
        /// 獲取屬性标识
        /// </summary>
        public static T GetAttribute<T>(this object obj, string name) where T : class
        {
            Type objType = obj.GetType();
            var property = objType.GetProperty(name);
            if (property == null)
                return null;
            return Attribute.GetCustomAttribute(property, typeof(T)) as T;
        }
        /// <summary>
        /// 獲取屬性名称
        /// </summary>
        public static string GetSummaryName(this object obj, string name)
        {
            Type objType = obj.GetType();
            var property = objType.GetProperty(name);
            if (property == null)
                return null;
            AttributeEx.SummaryAttribute attribute = obj.GetAttribute<AttributeEx.SummaryAttribute>(name);
            if (attribute == null)
                return null;
            return attribute.Name;
        }
        /// <summary>
        /// 獲取屬性描述
        /// </summary>
        public static string GetSummaryDescription(this object obj, string name)
        {
            Type objType = obj.GetType();
            var property = objType.GetProperty(name);
            if (property == null)
                return null;
            AttributeEx.SummaryAttribute attribute = obj.GetAttribute<AttributeEx.SummaryAttribute>(name);
            if (attribute == null)
                return null;
            return attribute.Name;
        }
        /// <summary>
        /// 對象轉DataRow
        /// </summary>
        public static DataRow ModelToDataRow(this object obj, DataRow dr, PropertyInfo[] properties)
        {
            Type objType = obj.GetType();
            foreach (var propertieInfo in properties)
            {
                //获取字段名
                string colName = propertieInfo.Name;
                AttributeEx.DBColAttribute attribute = Attribute.GetCustomAttribute(propertieInfo, typeof(AttributeEx.DBColAttribute)) as AttributeEx.DBColAttribute;
                if (attribute != null && !attribute.Name.IsNullOrEmpty())
                {
                    colName = attribute.Name;
                }
                dr[colName] = propertieInfo.GetValue(obj) == null ? DBNull.Value : propertieInfo.GetValue(obj);
            }
            return dr;
        }
        /// <summary>
        /// 對象轉DataTable
        /// </summary>
        public static DataTable ListToDataTable<T>(this List<T> list, bool? dbCanRead, bool? dbCanWrite) where T : new()
        {
            if (list == null || list.Count <= 0)
            {
                return null;
            }
            T t = list[0];
            DataTable dt = t.ModelToDataTable(dbCanRead, dbCanWrite);
            Type objType = t.GetType();
            PropertyInfo[] properties = objType.GetPropertiesPGS(dbCanRead, dbCanWrite);
            foreach (var item in list)
            {
                DataRow dr = dt.NewRow();
                item.ModelToDataRow(dr, properties);
                dt.Rows.Add(dr);
            }
            return dt;
        }
        /// <summary>
        /// DataRow轉對象
        /// </summary>
        public static T DataRowToModel<T>(this DataRow dr, bool? dbCanRead, bool? dbCanWrite) where T : new()
        {
            PropertyInfo[] properties = typeof(T).GetPropertiesPGS(dbCanRead, dbCanWrite);
            return dr.DataRowToModel<T>(properties);
        }
        /// <summary>
        /// DataRow轉對象
        /// </summary>
        public static T DataRowToModel<T>(this DataRow dr, PropertyInfo[] properties) where T : new()
        {
            T t = new T();
            foreach (var propertieInfo in properties)
            {
                //获取字段名
                string colName = propertieInfo.Name;
                AttributeEx.DBColAttribute attribute = Attribute.GetCustomAttribute(propertieInfo, typeof(AttributeEx.DBColAttribute)) as AttributeEx.DBColAttribute;
                if (attribute != null && !attribute.Name.IsNullOrEmpty())
                {
                    colName = attribute.Name;
                }
                if (dr[colName] == DBNull.Value)
                    propertieInfo.SetValue(t, null);
                else
                    propertieInfo.SetValue(t, dr[colName]);
            }
            return t;
        }
        /// <summary>
        /// DataTable轉對象
        /// </summary>
        public static List<T> DataTableToList<T>(this DataTable dt, bool? dbCanRead, bool? dbCanWrite) where T : new()
        {
            if (dt == null || dt.Rows.Count <= 0)
            {
                return null;
            }
            List<T> list = new List<T>();
            PropertyInfo[] properties = typeof(T).GetPropertiesPGS(dbCanRead, dbCanWrite);
            foreach (DataRow row in dt.Rows)
            {
                list.Add(row.DataRowToModel<T>(properties));
            }
            return list;
        }
        /// <summary>
        /// 设置行状态为新增
        /// </summary>
        public static void SetRowsAdd(this DataTable dt)
        {
            if (dt == null || dt.Rows.Count <= 0)
            {
                return;
            }
            dt.AcceptChanges();
            foreach (DataRow row in dt.Rows)
            {
                row.SetAdded();
            }
        }
        /// <summary>
        /// 设置行状态为更新
        /// </summary>
        public static void SetRowsUpdate(this DataTable dt)
        {
            if (dt == null || dt.Rows.Count <= 0)
            {
                return;
            }
            dt.AcceptChanges();
            foreach (DataRow row in dt.Rows)
            {
                row.SetModified();
            }
        }
        /// <summary>
        /// 设置行状态为刪除
        /// </summary>
        public static void SetRowsDelete(this DataTable dt)
        {
            if (dt == null || dt.Rows.Count <= 0)
            {
                return;
            }
            dt.AcceptChanges();
            foreach (DataRow row in dt.Rows)
            {
                row.Delete();
            }
        }
    }
}
