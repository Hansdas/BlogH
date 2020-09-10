using Blog.Common;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace BlogApi.Controllers
{
    [Route("api/qq")]
    [ApiController]
    public class QQLoginController : Controller
    {
        [Route("login/{code}")]
        [HttpGet]
        public async Task<ApiResult> Login(string code)
        {
            string accessToken =await QQClient.GetAccessToken(code);
            string openId = await QQClient.GetOpenId(accessToken);
            dynamic qqInfo = await QQClient.GetQQUser(accessToken, openId);
            return ApiResult.Success(new { qqInfo});
        }
    }
}
