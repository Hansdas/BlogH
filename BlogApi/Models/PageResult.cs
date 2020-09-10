using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlogApi
{
    public class PageResult: ApiResult
    {
        public int Total { get; set; }
    }
}
