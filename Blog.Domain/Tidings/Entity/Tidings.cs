﻿using Blog.Domain.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace Blog.Domain
{
    /// <summary>
    /// 未读和已读消息
    /// </summary>
   public  class Tidings:Entity<int>
    {
        public Tidings(string commentId,string reviceUser,bool isRead,string url,string additionalData)
        {
            CommentId = commentId;
            ReviceUser = reviceUser;
            IsRead = isRead;
            Url = url;
            AdditionalData = additionalData;
        }
        /// <summary>
        /// 评论id
        /// </summary>
        public string CommentId { get; private set; }
        /// <summary>
        /// 接收人
        /// </summary>
        public string ReviceUser { get; private set; }
        /// <summary>
        /// 是否已读
        /// </summary>
        public bool IsRead { get; private set; }
        /// <summary>
        /// 发送日期
        /// </summary>
        public DateTime SendDate { get; private set; }
        /// <summary>
        /// 跳转url
        /// </summary>
        public string Url { get; set; }
        /// <summary>
        /// 附加信息
        /// </summary>
        public string AdditionalData { get; private set; }
    }
}
