using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Blog.Application.ViewModel;
using Blog.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BlogApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private IHttpContextAccessor _httpContext;
        public ValuesController(IHttpContextAccessor httpContext)
        {
            _httpContext = httpContext;
        }
        // GET api/values
        [HttpGet]
        public ActionResult<IEnumerable<string>> Get(string str)
        {
            return new string[] { "value1", str };
        }

        // GET api/values/5
      
        [HttpPost]
        public ActionResult<string> GetJWT(int a, int b)
        {
            ReturnResult returnResult = new ReturnResult();
            try
            {
                string json = new JWT(_httpContext).ResolveToken();
                UserModel userModel = JsonHelper.DeserializeObject<UserModel>(json);
                returnResult.Code = "200";
                returnResult.Data = userModel;
            }
            catch (ValidationException)
            {
                returnResult.Code = "500";
                returnResult.Message = "checkfail";
            }
            return new JsonResult(returnResult);
        }
        // POST api/values
        [HttpPost]
        public ActionResult DoPost()
        {
            IList<Claim> claims = new List<Claim>()
                {
                    new Claim("account", "11"),
                //     new Claim(JwtRegisteredClaimNames.Nbf,$"{new DateTimeOffset(DateTime.Now).ToUnixTimeSeconds()}") ,
                ////这个就是过期时间，目前是过期1000秒，可自定义，注意JWT有自己的缓冲过期时间
                //   new Claim (JwtRegisteredClaimNames.Exp,$"{new DateTimeOffset(DateTime.Now.AddSeconds(30)).ToUnixTimeSeconds()}"),

                };
            //string jwtToken = JWT.CreateToken(claims);
            return new JsonResult(new ReturnResult() { Code = "200", Data = "" });
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
