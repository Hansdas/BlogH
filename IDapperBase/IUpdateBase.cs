using System;
using System.Collections.Generic;
using System.Text;

namespace IDapperBase
{
   public interface IUpdateBase
    {
        /// <summary>
        /// 更新单个实体
        /// </summary>
        /// <param name="sql"></param>
        void UpdateSingle(string sql, object paras= null);
    }
}
