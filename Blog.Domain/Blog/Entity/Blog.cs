using Blog.Common;
using Blog.Domain.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace Blog.Domain
{
    /// <summary>
    /// 博客聚合根
    /// </summary>
   public class Blog:AggregateRoot<int>
    {
        public Blog(string account,BlogType blogType, BlogBase<int> blogBase)
        {
            Account = account;
            BlogType = blogType;
            BlogBase = blogBase;
        }
        public Blog(string account, BlogType blogType, BlogBase<int> blogBase, DateTime createTime,DateTime? updateTime)
        :this(account,blogType, blogBase)
        {
            CreateTime = createTime;
            UpdateTime = updateTime;
        }
        /// <summary>
        /// 提交者
        /// </summary>
        public string Account { get; private set; }
        /// <summary>
        /// 博客类型
        /// </summary>        
        public BlogType BlogType { get; private set; }
        /// <summary>
        /// 数据持久化
        /// </summary>
        public int GetBlogType
        {
            get
            {
                return BlogType.GetEnumValue();
            }
        }
        /// <summary>
        /// 博客id 
        /// </summary>
        public int BlogBaseId { get; private set; }
        /// <summary>
        /// 博客
        /// </summary>
        public BlogBase<int> BlogBase { get; private set; }
       
        public static void SetBlogBaseId(int id, Blog blog)
        {
            blog.BlogBaseId = id;
        }


    }
}
