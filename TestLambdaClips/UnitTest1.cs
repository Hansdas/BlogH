using DapperFactory;
using Domain;
using IDapperFactory;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TestLambdaClips
{
    [TestClass]
    public class UnitTest1
    {

        protected IDistributedCache _distributedCache;
        public UnitTest1(IDistributedCache distributedCache)
        {
            _distributedCache = distributedCache;

        }
        [TestMethod]
        public void TestMethod1()
        {
          var v=  _distributedCache.GetString("1");
        }
    }
}
