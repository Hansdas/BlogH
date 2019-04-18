using System.Collections.Generic;

namespace Domain
{
    public class Information : DomainBase
    {
        /// <summary>
        /// 用户
        /// </summary>
        public int UserId { get; set; }
        /// <summary>
        /// 标题
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// 内容
        /// </summary>
        public string Content { get; set; }
        /// <summary>
        /// 信息
        /// </summary>
        public string News { get; set; }
        /// <summary>
        /// 信息类型
        /// </summary>
        public NewsType NewsType { get; set; }
        /// <summary>
        /// 评论
        /// </summary>
        public List<Comment> CommentList { get; set; }
        /// <summary>
        /// 评论数量
        /// </summary>
        public int CommentCount
        {
            get { return CommentList.Count; }
        }
        /// <summary>
        /// 赞数量
        /// </summary>
        public int Praises { get; set; }
    }
}
