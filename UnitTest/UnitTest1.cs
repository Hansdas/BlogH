using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace UnitTest
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            double ExpireTime = 1D;
            TimeSpan timeSpan= TimeSpan.FromDays(ExpireTime);
        }
    }
}
