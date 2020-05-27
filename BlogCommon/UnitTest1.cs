using Blog.Common;
using Blog.Domain.Core;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace BlogCommon
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
          Dictionary<int,string> dic= EnumConvert<ArticleType>.AsDictionary();
        }
    }
}
