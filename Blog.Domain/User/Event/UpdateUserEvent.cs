using Blog.Domain.Core.Event;
using System;
using System.Collections.Generic;
using System.Text;

namespace Blog.Domain
{
   public class UpdateUserEvent:EventData
    {
        public UpdateUserEvent(string key, string value)
        {
            Key = key;
            Value = value;
        }
        public string Key { get; private set; }
        public string Value { get; private set; }
    }
}
