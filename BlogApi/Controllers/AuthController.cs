using Blog.Application.ViewModel;
using Blog.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using System;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace BlogApi.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IHttpContextAccessor _context;
        private const string WERXIN_TOKEN = "weixin_blog";
        public AuthController(IHttpContextAccessor httpContext)
        {
            _context = httpContext;
        }
        [HttpPost]
        public ApiResult GetLoginUser()
        {
            try
            {
                bool noLogin = Response.Headers.TryGetValue("auth", out StringValues value);
                if (noLogin)
                    throw new AuthException();
                string json = new JWT(_context).ResolveToken();
                UserModel userModel = JsonHelper.DeserializeObject<UserModel>(json);
                return ApiResult.Success(userModel);
            }
            catch (AuthException)
            {
                return ApiResult.AuthError();
            }
        }
        /// <summary>
        /// 是否登录，通过中间件处理
        /// </summary>
        [HttpPost]
        [Route("islogin")]
        public ApiResult IsLogin()
        {
            bool noLogin = Response.Headers.TryGetValue("auth", out StringValues value);
            if (noLogin)
                return ApiResult.AuthError();
            return ApiResult.Success(); ;
        }
        [HttpGet]
        [Route("weixi")]
        public HttpResponseMessage WeiXinAuth(string signature, string timestamp, string nonce, string echostr)
        {
            try
            {
                if (CheckSignature(WERXIN_TOKEN, signature, timestamp, nonce))
                {
                    return new HttpResponseMessage()
                    {
                        Content = new StringContent(echostr, Encoding.GetEncoding("UTF-8"), "application/x-www-from-urlencoded")
                    };
                }
                else
                {
                    return null;
                }
            }
            catch (Exception)
            {

                return null;
            }

        }

        private bool CheckSignature(string token, string signature, string timestamp, string nonce)
        {
            string[] ArrTmp = { token, timestamp, nonce };
            Array.Sort(ArrTmp);
            string tmpStr = string.Join("", ArrTmp);
            tmpStr = GetSHA1(tmpStr);
            tmpStr = tmpStr.ToLower();
            if (tmpStr == signature)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private string GetSHA1(string pwd)
        {
            SHA1 algorithm = SHA1.Create();
            byte[] data = algorithm.ComputeHash(Encoding.UTF8.GetBytes(pwd));
            string sh1 = "";
            for (int i = 0; i < data.Length; i++)
            {
                sh1 += data[i].ToString("x2").ToUpperInvariant();
            }
            return sh1;
        }

    }
}