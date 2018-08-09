using Domain.System;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using ServiceSvc.IService;

namespace BlogApi.Controllers
{
    [Route("api/[controller]")]
    public class ValuesController : Controller
    {
        private readonly IUserServiceSvc _UserServiceSvc;
        public ValuesController(IUserServiceSvc BaseServiceSvc)
        {
            _UserServiceSvc = BaseServiceSvc;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Account">账号</param>
        /// <param name="Password">密码</param>
        /// <param name="IsRemeberAccount">是否记住账号</param>
        /// <returns></returns>
        //[HttpGet]
        //public LoginResult Get(string Account,string Password,bool IsRemeberAccount)
        //{
        //    LoginResult loginResult = new LoginResult();           
        //    User user = new User();
        //    user.Account = "1";
        //    _UserServiceSvc.GetSingleUser(user);
        //    return loginResult;
        //}

        // GET api/values/5
        [HttpGet("{id}")]
        [EnableCors("Core")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
        private void CacheUserData(User user)
        {
            //Domain.System.User.CacheUserInfo(user);
        }
    }
}
