using Blog.Application.IService;
using Blog.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace Blog.Application.Service
{
    public class TidingsService : ITidingsService
    {
        private ITidingsRepository _tidingsRepository;
        public TidingsService(ITidingsRepository tidingsRepository)
        {
            _tidingsRepository = tidingsRepository;
        }
        public int SelectCountByAccount(string account)
        {
          return  _tidingsRepository.SelectCountByAccount(account);
        }
    }
}
