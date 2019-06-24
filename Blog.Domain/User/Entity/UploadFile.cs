using Blog.Domain.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace Blog.Domain
{
   public class UploadFile: Entity<int>, IAggregateRoot
    {
        public UploadFile(string account,string savePath,string fileName,long fileSize)
        {
            Account = account;
            SavePath = savePath;
            FileName = fileName;
            FileSize = fileSize;
        }
        /// <summary>
        /// 所属用户
        /// </summary>
        public string Account { get; private set; }
        /// <summary>
        /// 保存路径
        /// </summary>
        public string SavePath { get; private set; }
        /// <summary>
        /// 附件名字
        /// </summary>
        public string FileName { get; private set; }
        /// <summary>
        /// 附件大小
        /// </summary>
        public long FileSize { get; private set; }
    }
}
