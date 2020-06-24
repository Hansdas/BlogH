using Microsoft.AspNetCore.Http;
using NLog;
using System;
using System.Collections.Generic;
using System.Text;

namespace Blog.Common
{
    public class LogUtils
    {
        private ILogger _logger;
        public static bool EnableNlog;//是否启用log
        public LogUtils()
        {
            _logger = LogManager.GetLogger("");
        }
        /// <summary>
        /// 记录错误信息
        /// </summary>
        /// <param name="exception"></param>
        /// <param name="logger"></param>
        /// <param name="message"></param>
        /// <param name="account"></param>
        public void LogError(Exception exception, string logger = null, string message = null, string account = null,string requestIp=null,string requestAddress=null)
        {
            Log(LogLevel.Error, exception, logger, message, account);
        }
        /// <summary>
        /// 记录一般信息
        /// </summary>
        /// <param name="exception"></param>
        /// <param name="logger"></param>
        /// <param name="message"></param>
        /// <param name="account"></param>
        public void LogInfo(string logger = null, string message = null, string account = null, string requestIp = null, string requestAddress = null)
        {
            Log(LogLevel.Info, null, logger, message, account, requestIp, requestAddress);
        }
        /// <summary>
        /// 记录警告
        /// </summary>
        /// <param name="exception"></param>
        /// <param name="logger"></param>
        /// <param name="message"></param>
        /// <param name="account"></param>
        public void LogWarn(Exception exception, string logger = null, string message = null, string account = null, string requestIp = null, string requestAddress = null)
        {
            Log(LogLevel.Warn, exception, logger, message, account, requestIp, requestAddress);
        }
        /// <summary>
        /// 记录灾难级信息
        /// </summary>
        /// <param name="exception"></param>
        /// <param name="logger"></param>
        /// <param name="message"></param>
        /// <param name="account"></param>
        public void LogFatal(Exception exception = null, string logger = null, string message = null, string account = null, string requestIp = null, string requestAddress = null)
        {
            Log(LogLevel.Fatal, exception, logger, message, account,requestIp,requestAddress);
        }
        private void Log(LogLevel logLevel, Exception exception, string logger, string message, string account, string requestIp = null, string requestAddress = null)
        {
            if (!EnableNlog)
                return;
            if (message == null && exception != null)
                message = exception.Message;
            LogEventInfo logEventInfo = LogEventInfo.Create(logLevel, logger, exception, null, message);
            logEventInfo.Properties["Account"] = account;
            logEventInfo.Properties["Request"] = requestIp ;
            logEventInfo.Properties["RequestAddress"] = requestAddress;
            _logger.Log(logEventInfo);
        }

    }
}
