﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Blog.Application.IService;
using Blog.Application.ViewModel;
using Blog.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BlogApi
{
    [Route("api/[controller]")]
    [ApiController]
    public class TidingsController : ControllerBase
    {
        private ITidingsService _tidingsService;
        private IHttpContextAccessor _httpContext;
        public TidingsController(ITidingsService tidingsService, IHttpContextAccessor httpContext)
        {
            _tidingsService = tidingsService;
            _httpContext = httpContext;
        }
        [HttpGet]
        public int NewCount()
        {
            string json = new JWT(_httpContext).ResolveToken();
            UserModel userModel = JsonHelper.DeserializeObject<UserModel>(json);
            int count = _tidingsService.SelectCountByAccount(userModel.Account);
            return count;
        }
    }
}