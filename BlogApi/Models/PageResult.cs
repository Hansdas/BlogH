using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlogApi
{
    public class PageResult:ReturnResult
    {
        public int Total { get; set; }
    }
}
