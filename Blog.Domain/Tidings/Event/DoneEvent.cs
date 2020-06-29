using Blog.Domain.Core.Event;
using System;
using System.Collections.Generic;
using System.Text;

namespace Blog.Domain
{
   public class DoneEvent:EventData
    {
        public string Account { get; private set; }
        public DoneEvent(string account)
        {
            Account = account;
        }
    }
}
