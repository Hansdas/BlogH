using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Text;

namespace Blog.Common.RabbitMQ
{
    public class RabbitEvent : IRabbitEvent
    {
        public void Publish(MQEvent @event)
        {
            ConnectionFactory connectionFactory = RabbitClient.CreateConnection();
        }
    }
}
