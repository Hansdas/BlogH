using BaseEntity;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.System
{
   public class UploadFile:DomainBase
    {
        /// <summary>
        /// 账号
        /// </summary>
        public string UserAccount { get; set; }
        /// <summary>
        /// 存储路径（不含文件名）
        /// </summary>
        public string Path { get; set; }
        /// <summary>
        /// 文件名
        /// </summary>
        public string FileName { get; set; }
        /// <summary>
        /// 文件大小，以字节为单位
        /// </summary>
        public long FileSize { get; set; }
        /// <summary>
        /// 存储路径
        /// </summary>
        public string FilePath { get; set; }
    }
}
