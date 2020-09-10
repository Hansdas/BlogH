using Blog.Application.ViewModel;
using Blog.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using System.Threading.Tasks;

namespace BlogApi.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IHttpContextAccessor _context;
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
            bool noLogin =Response.Headers.TryGetValue("auth", out StringValues value);
            if (noLogin)
                return ApiResult.AuthError();
            return ApiResult.Success(); ;
        }
    }
}