using System;
using System.Collections.Generic;
using System.Text;

namespace BaseEntity
{
   public class DomainBase
    {
        /// <summary>
        /// 自增id
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// 数据添加时间
        /// </summary>
        public DateTime CreateTime { get; set; }
        /// <summary>
        /// 数据删除时间
        /// </summary>
        public DateTime? DeleteTime { get; set; }
    }
}
