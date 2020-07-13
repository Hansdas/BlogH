using System;
using System.Collections.Generic;
using System.Text;

namespace Blog.Application.ViewModel
{
   public class WhisperConditionModel:PageConditionModel
    {
        /// <summary>
        /// 账号查询
        /// </summary>
        public string Account { get; set; }
        /// <summary>
        /// 根据id查询
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// 是否根据登录人查询
        /// </summary>
        public bool LoginUser { get; set; } = false;
    }
}
