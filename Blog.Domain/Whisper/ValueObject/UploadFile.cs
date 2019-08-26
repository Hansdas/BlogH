using Blog.Domain.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace Blog.Domain
{
   public class UploadFile: ValueObject
    {
        public UploadFile(string account,string guid,string savePath,string fileName,long fileSize)
        {
            Account = account;
            GUID = guid;
            SavePath = savePath;
            FileName = fileName;
            FileSize = fileSize;
        }
        /// <summary>
        /// guid
        /// </summary>
        public string GUID { get; private set; }
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
        /// <summary>
        /// 全路径
        /// </summary>
        public string SaveFullPath { get
            {
                return SavePath + "/" + FileName;
            } 
        }
    }
}
