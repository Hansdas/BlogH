using Blog.Domain.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace Blog.Domain
{
   public class Video:Entity<int>
    {
        public Video(string description,string author,string url,string lable,long length,int watchCount)
        {
            Description = description;
            Author = author;
            Url = url;
            Lable = lable;
            Length = length;
            WatchCount = watchCount;
        }
        public Video(int id,string description, string author, string url, string lable, long length, int watchCount,DateTime createTime)
        {
            Id = id;
            Description = description;
            Author = author;
            Url = url;
            Lable = lable;
            Length = length;
            WatchCount = watchCount;
            CreateTime = createTime;
        }
        /// <summary>
        /// 描述
        /// </summary>
        public string Description { get; private set; }
        /// <summary>
        /// 作者
        /// </summary>
        public string Author { get; private set; }
        /// <summary>
        /// 地址
        /// </summary>
        public string Url { get; private set; }
        /// <summary>
        /// 标签
        /// </summary>
        public string Lable { get; private set; }
        /// <summary>
        /// 大小
        /// </summary>
        public long Length { get; private set; }
        /// <summary>
        /// 大小（M为单位）
        /// </summary>
        public int Size
        {
            get
            {
                return (int)(Length / 1048576);
            }
        }
        /// <summary>
        /// 观看次数
        /// </summary>
        public int WatchCount { get; private set; }
       
    }
}
