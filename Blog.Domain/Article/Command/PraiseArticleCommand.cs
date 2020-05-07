using Blog.Domain.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace Blog.Domain
{
    /// <summary>
    /// 点赞命令
    /// </summary>
   public class PraiseArticleCommand: Command
    {
        public PraiseArticleCommand(int id,string account,bool cancle)
        {
            Id = id;
            Account = account;
            Cancle = cancle;
        }
        public int Id
        {
            get;
        }
        public string Account
        {
            get;
        }
        public bool Cancle
        {
            get;
        }
    }
}
