using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Blog.Domain.Core.Event
{
   public abstract class Event: INotification
    {
        public DateTime EventTime { get; private set; }
        protected Event()
        {
            EventTime = DateTime.Now;
        }
    }
}
