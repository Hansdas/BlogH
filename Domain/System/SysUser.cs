using BaseEntity;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.System
{
   public class SysUser:DomainBase
    {
        /// <summary>
        /// 账号
        /// </summary>
        public string Account { get; set; }
        /// <summary>
        /// 密码
        /// </summary>
        public string Password { get; set; }
        /// <summary>
        /// 手机号
        /// </summary>
        public string PhoneNumber { get; set; }
    }
}
