using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Blog.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BlogApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DefaultController : ControllerBase
    {
        [HttpGet]
        public JsonResult Default()
        {
            try
            {
                throw new Exception("11");
            }
            catch (Exception ex)
            {
                new LogUtils().LogError(ex, "BlogApi.Controllers.DefaultController","ceshi","system");
            }
            return new JsonResult("api application start OK");
        }
    }
}