using Blog.Common;
using Blog.Common.EnumExtensions;
using Blog.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace Blog.Application.ViewModel
{
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
        /// 评论类型
        /// </summary>
        public int CommentType { get; set; }
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
        /// 附加数据
        /// </summary>
        public string AdditionalData { get; set; }
        /// <summary>
        /// 评论接收人
        /// </summary>
        public string Revicer { get; set; }
        /// <summary>
        /// 评论接收人
        /// </summary>
        public string RevicerName { get; set; }
        /// <summary>
        /// 原内容
        /// </summary>
        public string UsingContent { get; set; }
        public static CommentModel ConvertToCommentModel(Comment comment)
        {
            CommentModel commentModel = new CommentModel();
            commentModel.Guid = comment.Guid;
            commentModel.Content = comment.Content;
            commentModel.PostUser = comment.PostUser;
            commentModel.PostUsername = comment.PostUsername;
            commentModel.Revicer = comment.RevicerUser;
            commentModel.AdditionalData = comment.AdditionalData;
            commentModel.RevicerName = comment.RevicerUsername;
            commentModel.CommentType = comment.CommentType.GetEnumValue();
            commentModel.PostDate = comment.PostDate.ToString("yyyy-MM-dd hh:mm");
            commentModel.UsingContent = comment.UsingContent;
            return commentModel;
        }
        public static IList<CommentModel> ConvertToCommentModels(IList<Comment> comments)
        {
            IList<CommentModel> commentModels = new List<CommentModel>();
            foreach (var item in comments)
                commentModels.Add(ConvertToCommentModel(item));
            return commentModels;
        }
    }
}
