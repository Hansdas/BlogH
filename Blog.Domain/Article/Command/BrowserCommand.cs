using Blog.Domain.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace Blog.Domain
{
    /// <summary>
    ///  更新浏览次数
    /// </summary>
   public class BrowserCommand:Command
    {
        public BrowserCommand(int id)
        {
            Id = id;
        }
        public int Id { get; private set; }
    }
}
