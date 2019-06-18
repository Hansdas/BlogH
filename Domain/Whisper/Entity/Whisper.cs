using Blog.Domain.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace Blog.Domain
{
   public class Whisper:Entity<int>
    {
        public Whisper(string account,DateTime createTime)
        {
            Account = account;
            CreateTime = createTime;
        }
        /// <summary>
        /// 作者
        /// </summary>
        public string Account { get; private set; }
        //public IList
    }
}
