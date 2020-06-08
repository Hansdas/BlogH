using Blog.Common;
using Blog.Common.Mail;
using Blog.Domain.Core;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BlogCommon
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMail()
        {
            try
            {
                MailBody mailBody = new MailBody();
                mailBody.Body = "你好，我是博客管理系统";
                mailBody.Revicer = "3248996258@qq.com";
                mailBody.RevicerAddress = "3248996258@qq.com";
                mailBody.Sender= "3248996258@qq.com";
                mailBody.SenderAddress= "3248996258@qq.com";
                mailBody.Subject = "您好";
                MailServerConfig mailServerConfig = new MailServerConfig();
                mailServerConfig.SMTP = "smtp.qq.com";
                mailServerConfig.Port = 465;
                mailServerConfig.Account = "3248996258@qq.com";
                mailServerConfig.Password = "phnibmehwablcijj";
                new Mail().SendMail(mailBody, mailServerConfig).GetAwaiter().GetResult();
            }
            catch (AggregateException ex)
            {

                throw;
            }
            catch (Exception ex)
            {
                throw;
            }

        }
    }
}
