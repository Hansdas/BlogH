using Blog.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace Blog.Application.ViewModel
{
    /// <summary>
    /// 博客模型DTO
    /// </summary>
   public class WhisperModel
    {
        public string Id { get; set; }
        /// <summary>
        /// 作者账号
        /// </summary>
        public string Account { get; set; }
        /// <summary>
        /// 作者名字
        /// </summary>
        public string AccountName { get; set; }
        private string _contet;
        /// <summary>
        ///  内容
        /// </summary>
        public string Content {
            get
            {
                if (_contet.Contains(ConstantKey.OLD_FILE_HTTP))
                    _contet = _contet.Replace(ConstantKey.OLD_FILE_HTTP, ConstantKey.FILE_HTTPS);
                if(_contet.Contains("http://www.ttblog.site"))
                    _contet = _contet.Replace("http://www.ttblog.site", ConstantKey.FILE_HTTPS);
                return _contet;
            }
            set
            {
                _contet = value;
            }

        }
        /// <summary>
        /// 发表日期
        /// </summary>
        public string CreateDate { get; set; }
        /// <summary>
        /// 评论集合
        /// </summary>
        public IList<CommentModel> Commentdatas { get; set; }
        public int CommentCount
        {
            get { return Commentdatas==null?0:Commentdatas.Count; }
        }
    }
}
