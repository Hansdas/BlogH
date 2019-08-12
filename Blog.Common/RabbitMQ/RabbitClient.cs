using Blog.Common.AppSetting;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Text;

namespace Blog.Common.RabbitMQ
{
  public class RabbitClient
    {
       public static ConnectionFactory CreateConnection()
        {
            RabbitSettingModel rabbitSettingModel = ConfigurationProvider.GetSettingModel<RabbitSettingModel>("RabbitMQ");
            ConnectionFactory connectionFactory = new ConnectionFactory()
            {
                HostName = rabbitSettingModel.HostName,
                Port = rabbitSettingModel.Port,
                UserName = rabbitSettingModel.UserName,
                Password = rabbitSettingModel.Password,
                VirtualHost = rabbitSettingModel.VirtualHost,
                NetworkRecoveryInterval = TimeSpan.FromSeconds(10)
            };
            return connectionFactory;
        }
    }
}
