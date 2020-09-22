using Blog.Domain.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace Blog.Domain
{
   public class leaveMessage:Entity<int>
    {
        public leaveMessage(string content,string contractEmail,bool isAction)
        {
            Content = content;
            ContractEmail = contractEmail;
            IsAction = IsAction;
        }
        public leaveMessage(int id,string content, string contractEmail, bool isAction,DateTime createTime)
        {
            Id = id;
            Content = content;
            ContractEmail = contractEmail;
            IsAction = IsAction;
            CreateTime = createTime;
        }
        /// <summary>
        /// 留言内容
        /// </summary>
        public string Content { get; private set; }
        /// <summary>
        /// 联系方式
        /// </summary>
        public string ContractEmail { get; private set; }
        /// <summary>
        /// 是否处理
        /// </summary>
        public bool IsAction { get; private set; }
    }
}
