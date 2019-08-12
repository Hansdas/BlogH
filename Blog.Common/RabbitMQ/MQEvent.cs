using System;
using System.Collections.Generic;
using System.Text;

namespace Blog.Common.RabbitMQ
{
   public class MQEvent
    {
        public MQEvent()
        {
            Guid = Guid.NewGuid();
            CreateTime = DateTime.Now;
        }
        public Guid Guid { get; }
        public DateTime CreateTime { get; }
    }
}
