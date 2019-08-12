using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Text;

namespace Blog.Common.AppSetting
{
    public class RabbitSettingModel
    {
        public string HostName { get; set; }
        private string _prot;
        public int Port
        {
            get
            {
                return Convert.ToInt32(_prot);
            }
            set
            {
                _prot = value.ToString();
                if (string.IsNullOrEmpty(_prot))
                    _prot = AmqpTcpEndpoint.UseDefaultPort.ToString();
            }
        }
        private string _userName;
        public string UserName {
            get
            {
                return _userName;
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                    _userName = "admin";
                else
                    _userName = value;
            }
        }
        private string _password;
        public string Password {

            get
            {
                return _password;
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                    _password = "123456";
                else
                    _password = value;
            }
        }
        private string _virtualHost;
        public string VirtualHost {
            get
            {
                return _virtualHost; 
            }
            set
            {

                if (string.IsNullOrEmpty(value))
                    _virtualHost = "/";
                else
                    _virtualHost = value;
            }
        }
    }
}
