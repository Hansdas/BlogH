using System;
using System.Collections.Generic;
using System.Text;

namespace Blog.Domain
{
    public class TidingsCondition
    {
        /// <summary>
        /// 根据接收人账号查询
        /// </summary>
        public string Account { get; set; }
        /// <summary>
        /// 是否已读
        /// </summary>
        public bool? IsRead { get; set; }
    }
}
