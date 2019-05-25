using CommonHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Domain.Domain
{
    /// <summary>
    /// 微语实体
    /// </summary>
   public class Whisper:DomainBase
    {
        /// <summary>
        /// 作者
        /// </summary>
        public int Author { get; set; }
        /// <summary>
        /// 内容
        /// </summary>
        public string Content { get; set; }

        public string PhotoIds { get; set; }
        public IList<int> PhotoIdList
        {
            get { if (string.IsNullOrEmpty(PhotoIds))
                    return new List<int>();
                return PhotoIds.Split(',').Cast<int>().ToList();
               }
            set
            {
                PhotoIds = string.Join( ",", value);
            }
        }
    }
}
