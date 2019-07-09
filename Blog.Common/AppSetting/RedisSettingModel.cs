using System;
using System.Collections.Generic;
using System.Text;

namespace Blog.Common.AppSetting
{
    public class RedisSettingModel
    {
        public string Connection { get; set; }
        public string Port { get; set; }
        public string InstanceName { get; set; }
        public int DefaultDB { get; set; }
        public string Password{get;set;}
    }
}
