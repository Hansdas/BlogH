
using Blog.Quartz;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace BlogQuartzTest
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            try
            {
                //new NewsQuartz().StartJob().GetAwaiter().GetResult();
            }
            catch (AggregateException ex)
            {

                throw;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

    }
}
