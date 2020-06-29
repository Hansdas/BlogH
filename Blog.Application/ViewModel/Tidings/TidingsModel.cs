using System;
using System.Collections.Generic;
using System.Text;

namespace Blog.Application.ViewMode
{
   public class TidingsModel
    {
        public int Id { get; set; }
        /// <summary>
        /// 评论人账号
        /// </summary>
        public string PostUserAccount { get; set; }
        /// <summary>
        /// 评论人
        /// </summary>
        public string PostUsername { get; set; }
        /// <summary>
        /// 评论内容
        /// </summary>
        public string PostContent { get; set; }
        /// <summary>
        /// 接收人 账号
        /// </summary>
        public string ReviceUserAccount { get; set; }
        /// <summary>
        /// 接收人
        /// </summary>
        public string ReviceUsername { get; set; }
        /// <summary>
        /// 源评论引用
        /// </summary>
        public string Content { get; set; }
        /// <summary>
        /// 评论日期
        /// </summary>
        public string PostDate { get; set; }
        /// <summary>
        /// 跳转地址
        /// </summary>
        public string Url { get; set; }
        /// <summary>
        /// 是否已读
        /// </summary>
        public bool IsRead { get; set; }
    }
}
