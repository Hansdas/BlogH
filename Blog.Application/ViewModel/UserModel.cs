using Blog.Common;
using Blog.Domain.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace Blog.Application.ViewModel
{
    /// <summary>
    /// 用户DTO模型
    /// </summary>
   public class UserModel
    {
        /// <summary>
        /// 用户名
        /// </summary>
        public string Username { get; set; }
        /// <summary>
        /// 账号
        /// </summary>
        public string Account { get; set; }
        /// <summary>
        /// 密码
        /// </summary>
        public string Password { get; set; }
        /// <summary>
        /// 账号类型
        /// </summary>
        public LoginType LoginType { get; set; }
        /// <summary>
        /// 性别
        /// </summary>
        public string Sex { get; set; }
        /// <summary>
        /// 出生日期
        /// </summary>
        public string BirthDate { get; set; }
        /// <summary>
        /// 邮箱
        /// </summary>
        public string Email { get; set; }
        /// <summary>
        /// 个性签名
        /// </summary>
        public string Sign { get; set; }
        /// <summary>
        /// 联系方式
        /// </summary>
        public string Phone { get; set; }
        /// <summary>
        /// 是否失效
        /// </summary>
        public bool IsValid { get;  set; }
        private string _headPhoto;
        /// <summary>
        /// 头像
        /// </summary>
        public string HeadPhoto
        {
            get
            {
                if (string.IsNullOrEmpty(_headPhoto))
                    _headPhoto= "/style/images/touxiang.jpg";
                if (_headPhoto.Contains(ConstantKey.NGINX_FILE_ROUTE_OLD))
                    _headPhoto= _headPhoto.Replace(ConstantKey.NGINX_FILE_ROUTE_OLD, ConstantKey.NGINX_FILE_ROUTE);
                if (_headPhoto.Contains(ConstantKey.OLD_FILE_HTTP))
                    _headPhoto= _headPhoto.Replace(ConstantKey.OLD_FILE_HTTP, ConstantKey.FILE_HTTPS);
                return _headPhoto;
            }
            set
            {
                _headPhoto=value;
            }
        }
    }
}
