using System;
using System.Collections.Generic;
using System.Text;

namespace Blog.Domain.Core
{
    /// <summary>
    /// 领域通知类
    /// </summary>
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
