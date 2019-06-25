using Blog.Domain.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace Blog.Domain
{
    /// <summary>
    /// 博客聚合根
    /// </summary>
   public class BlogContent:AggregateRoot<int>
    {
        public BlogContent(string account,BlogType blogType,ContentBase<int> contentBase)
        {
            Account = account;
            BlogType = blogType;
            ContentBase = contentBase;
        }
        public BlogContent(string account, BlogType blogType, ContentBase<int> contentBase,DateTime createTime,DateTime? updateTime)
        :this(account,blogType,contentBase)
        {
            CreateTime = createTime;
            UpdateTime = updateTime;
        }
        /// <summary>
        /// 提交着
        /// </summary>
        public string Account { get; private set; }
        /// <summary>
        /// 博客类型
        /// </summary>
        public BlogType BlogType { get; private set; }
        /// <summary>
        /// 提交内容
        /// </summary>
        public ContentBase<int>  ContentBase { get; private set; }
     
    }
}
