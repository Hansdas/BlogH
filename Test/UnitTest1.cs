using Blog.Application;
using Blog.Controllers.Add;
using Blog.Domain;
using Blog.Infrastruct;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Test
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
                PublishController publishController = new PublishController();
                publishController.AddWhisper();

            //Repository<User, int> repository = new Repository<User, int>();
           //User user= repository.SelectSingle(s => s.Account == "admin");
        }
    }
}
