using System;
using System.Collections.Generic;
using System.Text;

namespace Blog.Common.Configure
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
        /// <summary>
        /// 附件下载本地临时地址
        /// </summary>
        public string DownSavePathBase { get; set; }
    }
}
