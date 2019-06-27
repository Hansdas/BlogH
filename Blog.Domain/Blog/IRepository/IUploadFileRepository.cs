using System;
using System.Collections.Generic;
using System.Text;

namespace Blog.Domain
{
   public interface IUploadFileRepository: IRepository<UploadFile, int>
    {
        /// <summary>
        /// 将查询结果转为实体
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
        UploadFile Map(dynamic d);
        /// <summary>
        /// 将查询结果转为实体结果集
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
        IList<UploadFile> Map(IEnumerable<dynamic> dynamics);
    }
}
