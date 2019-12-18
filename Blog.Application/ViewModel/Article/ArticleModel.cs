using System;
using System.Collections.Generic;
using System.Text;

namespace Blog.Application.ViewModel
{
    /// <summary>
    /// 文章模型DTO
    /// </summary>
    public class ArticleModel
    {
        public int Id { get; set; }
        /// <summary>
        /// 标题
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// 内容
        /// </summary>
        public string TextSection { get; set; }
        /// <summary>
        /// 专栏
        /// </summary>
        public string ArticleType { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public string CreateTime { get; set; }
        /// <summary>
        /// 内容
        /// </summary>
        public string Content { get; set; }
        /// <summary>
        /// 作者
        /// </summary>
        public string Author { get; set; }
        /// <summary>
        /// 是否草稿
        /// </summary>
        public string IsDraft { get; set; }
        /// <summary>
        /// 图片路径
        /// </summary>
        public IList<string> FilePaths { get; set; }
        /// <summary>
        /// 评论数
        /// </summary>
        public int CommentCount { get; set; }
        /// <summary>
        /// 评论
        /// </summary>
        public IList<CommentModel> Comments { get; set; }
    }
    public class CommentModel
    {
        /// <summary>
        /// guid
        /// </summary>
        public string Guid { get; set; }
        /// <summary>
        /// 评论内容
        /// </summary>
        public string Content { get; set; }
        /// <summary>
        /// 评论人账号
        /// </summary>
        public string PostUser { get; set; }
        /// <summary>
        /// 评论人昵称
        /// </summary>
        public string PostUsername { get; set; }
        /// <summary>
        /// 评论时间
        /// </summary>
        public string PostDate { get; set; }
        /// <summary>
        /// 回复评论的id
        /// </summary>
        public string ReplyGuid { get; set; }
        /// <summary>
        /// 回复的对象
        /// </summary>
        public string ReplyUser { get; set; }
    }
    public class PageInfoMode
    {
        /// <summary>
        /// 上一篇
        /// </summary>
        public int BeforeId { get; set; }
        /// <summary>
        /// 下一篇
        /// </summary>
        public int NextId { get; set; }
        /// <summary>
        /// 上一篇标题
        /// </summary>
        public string BeforeTitle { get; set; }
        /// <summary>
        /// 下一篇标题
        /// </summary>
        public string NextTitle { get; set; }
    }
}
