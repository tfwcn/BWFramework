using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace BWFramework.BLL.Base
{
    public abstract class BLLBase<T> where T : new()
    {
        protected TLog bllTLog;
        protected string typeName = typeof(T).Name;
        protected BWFramework.DAL.Base.DALBase<T> dalObject;
        public BLLBase()
        {
            if (!(this is TLog))//防止死循环
            {
                bllTLog = new TLog();
            }
            foreach (var assemblies in AppDomain.CurrentDomain.GetAssemblies())
            {
                if (assemblies.FullName.IndexOf("BWFramework.DAL,") == 0)
                {
                    dalObject = assemblies.CreateInstance("BWFramework.DAL."+typeName) as BWFramework.DAL.Base.DALBase<T>;
                    break;
                }
            }
        }
        /// <summary>
        /// 获取数据
        /// </summary>
        public virtual T GetModel(string CID)
        {
            return dalObject.GetModel(CID);
        }
        /// <summary>
        /// 新增数据
        /// </summary>
        public virtual void Add(T model)
        {
            dalObject.Add(model);
        }
        public virtual void Add(List<T> list)
        {
            dalObject.Add(list);
        }

        /// <summary>
        /// 更新数据
        /// </summary>
        public virtual void Update(T model)
        {
            dalObject.Update(model);
            UpdateLog(model);
        }
        public virtual void Update(List<T> list)
        {
            dalObject.Update(list);
            list.ForEach(m => UpdateLog(m));
        }

        /// <summary>
        /// 删除数据
        /// </summary>
        public virtual void Delete(string CID)
        {
            dalObject.Delete(CID);
        }
        public virtual void Delete(T model)
        {
            dalObject.Delete(model);
        }
        public virtual void Delete(List<T> list)
        {
            dalObject.Delete(list);
        }
        /// <summary>
        /// 创建事务
        /// </summary>
        public DBTransactionHelper.DBTransactionHelper CreateDBTransactionHelper()
        {
            return dalObject.CreateDBTransactionHelper();
        }
        /// <summary>
        /// 更新完成后，记录更新日志，并更新旧值
        /// </summary>
        protected virtual void UpdateLog(T model)
        {
            if (!(this is TLog))//防止死循环
            {
                if (model is Model.Base.ModelBase)
                {
                    Model.Base.ModelBase modelBase = model as Model.Base.ModelBase;
                    Model.TLog modelTLog = bllTLog.GetUpdateLog(modelBase);
                    if (modelTLog != null)
                    {
                        bllTLog.Add(modelTLog);
                        modelBase.SaveOldValues();//更新旧值
                    }
                }
            }
        }
    }
}
