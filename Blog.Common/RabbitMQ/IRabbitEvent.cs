using System;
using System.Collections.Generic;
using System.Text;

namespace Blog.Common.RabbitMQ
{
    public interface IRabbitEvent
    {
        void Publish(MQEvent @event);
    }
}
