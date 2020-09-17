using Blog.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace Blog.Application.ViewModel
{
   public class VideoModel
    {
        /// <summary>
        /// id
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// 作者账号
        /// </summary>
        public string AuthorAccount { get; set; }
        /// <summary>
        /// 作者名字
        /// </summary>
        public string Author { get; set; }
        /// <summary>
        /// 描述
        /// </summary>
        public string Description { get; set; }
        private string _url;
        /// <summary>
        /// 地址
        /// </summary>
        public string Url 
        {
            get
            {
                if (_url.Contains(ConstantKey.OLD_FILE_HTTP))
                    _url=_url.Replace(ConstantKey.OLD_FILE_HTTP, ConstantKey.FILE_HTTPS);
                if (_url.Contains("http://www.ttblog.site"))
                    _url = _url.Replace(ConstantKey.OLD_FILE_HTTP, ConstantKey.FILE_HTTPS);
                return _url;
            }
            set
            {
                _url = value;
            }
        }
        /// <summary>
        /// 大小
        /// </summary>
        public int Size { get; set; }
        /// <summary>
        /// 标签
        /// </summary>
        public string Lable { get; set; }
        /// <summary>
        /// 标签
        /// </summary>
        public IList<string> Lables { get; set; }
        /// <summary>
        /// 观看次数
        /// </summary>
        public int WatchCount { get;set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public string CreateTime { get; set; }
    }
}
