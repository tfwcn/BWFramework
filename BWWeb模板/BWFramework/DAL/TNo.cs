using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BWFramework.DAL
{
    public class TNo : BWFramework.DAL.Base.DALBase<Model.TNo>
    {
        /// <summary>
        /// 通过CNo获取数据
        /// </summary>
        public Model.TNo GetModelByCNo(string CNo)
        {
            string whereStr = "where CNo=@CNo";
            List<DbParameter> paramenters = new List<DbParameter>();
            paramenters.Add(dbHelper.NewDbParameter("@CNo", DbType.String, CNo, 20));
            return base.GetModel(whereStr, paramenters);
        }
    }
}
