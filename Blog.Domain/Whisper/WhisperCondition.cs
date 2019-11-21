using System;
using System.Collections.Generic;
using System.Text;

namespace Blog.Domain
{
   public class WhisperCondiiton:ICondition
    {
        /// <summary>
        /// 根账号查询
        /// </summary>
        public string Account { get; set; }
    }
}
