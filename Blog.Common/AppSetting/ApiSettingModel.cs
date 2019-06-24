﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Blog.Common.AppSetting
{
   public class ApiSettingModel
    {
        /// <summary>
        /// webapi地址
        /// </summary>
        public string HttpAddresss { get; set; }
        /// <summary>
        /// 附件上传地址
        /// </summary>
        public string UploadSavePathBase { get; set; }
}
}
