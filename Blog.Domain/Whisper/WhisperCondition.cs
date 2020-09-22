using System;
using System.Collections.Generic;
using System.Text;

namespace Blog.Domain
{
   public class WhisperCondiiton
    {
        /// <summary>
        /// 根据id查询
        /// </summary>
        public int? Id { get; set; }
        /// <summary>
        /// 根账号查询
        /// </summary>
        public string Account { get; set; }
        /// <summary>
        /// 审核是否通过
        /// </summary>
        public bool? IsPassing { get; set; }
    }
}
