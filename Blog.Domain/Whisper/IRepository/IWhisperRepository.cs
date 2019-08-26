using Blog.Domain.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace Blog.Domain
{
   public interface IWhisperRepository : IRepository<Whisper, int>
    {
        /// <summary>
        /// 发表微语
        /// </summary>
        void Insert(Whisper whisper);
        /// <summary>
        /// 分页查询微语
        /// </summary>
        /// <returns></returns>
        IEnumerable<Whisper> SelectByPage(int pageIndex, int pageSize,BlogType blogType);
        /// <summary>
        ///查询总数
        /// </summary>
        /// <returns></returns>
        int SelectCount();
    }
}
