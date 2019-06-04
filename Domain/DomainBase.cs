using System;
using System.Collections.Generic;
using System.Text;

namespace Domain
{
    public class DomainBase:Entity.Entity
    {      
        private DateTime? _CreateTime;
        /// <summary>
        /// 数据添加时间
        /// </summary>
        public DateTime CreateTime {
            get
            {
                if (_CreateTime.HasValue)
                    return _CreateTime.Value;
                return DateTime.Now;
            }
            set
            {
                _CreateTime =value;
            }
        }
        /// <summary>
        /// 更新时间
        /// </summary>
        public DateTime? UpdateTime { get; set; }
    }
}
