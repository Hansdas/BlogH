using Blog.Common.EnumExtensions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Blog.Domain.Core
{
   public enum ArticleType
    {
        [EnumAdditional("5")]
        散文礼记=1,

        [EnumAdditional("3")]

        韶华追忆 =2,
        [EnumAdditional("1")]

        编程世界 =3,
        [EnumAdditional("2")]

        娱乐竞技 = 4,

        [EnumAdditional("4")]
        趣味百态 =5
    }
}
