﻿using Blog.Domain.Core.Event;
using System;
using System.Collections.Generic;
using System.Text;

namespace Blog.Domain.Core
{
    /// <summary>
    /// 领域通知类
    /// </summary>
  public  class DomainNotification:EventData
    {
        public DomainNotification(string value)
        {
            Value = value;
        }
        public string Value { get; private set; }
    }
}
