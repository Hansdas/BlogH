using System;
using System.Collections.Generic;
using System.Text;

namespace Blog.Domain.Core
{
  public  class DomainNotification: Event.Event
    {
        public DomainNotification(string key,string value)
        {
            Key = key;
            Value = value;
        }
        public string Key { get; private set; }
        public string Value { get; private set; }
    }
}
