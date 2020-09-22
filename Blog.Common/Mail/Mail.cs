using MailKit.Security;
using MimeKit;
using MimeKit.Text;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Common.Mail
{
    public class Mail
    {
        public async Task SendMail(MailBody mailBody, MailServerConfig mailServer, Action messageSentAction = null)
        {
            if (mailBody == null)
                throw new ArgumentException("参数为null");
            var mimeMessage = new MimeMessage();
            mimeMessage.Subject = mailBody.Subject;
            mimeMessage.From.Add(new MailboxAddress(mailBody.Sender, mailBody.SenderAddress));
            mimeMessage.Body = new TextPart(mailBody.BodyType) { Text = mailBody.Body };
            mimeMessage.To.Add(new MailboxAddress(mailBody.Revicer, mailBody.RevicerAddress));
            using (var smtp = new MailKit.Net.Smtp.SmtpClient())
            {
                if (messageSentAction != null)
                {
                    smtp.MessageSent += (sender, e) =>
                    {
                        messageSentAction();
                    };
                }
                smtp.ServerCertificateValidationCallback = (s, c, h, e) => true; 
                
                await smtp.ConnectAsync(mailServer.SMTP, mailServer.Port, SecureSocketOptions.SslOnConnect);

                await smtp.AuthenticateAsync(mailServer.Account, mailServer.Password);

                await smtp.SendAsync(mimeMessage);

                await smtp.DisconnectAsync(true);
            }
        }
    }
}
