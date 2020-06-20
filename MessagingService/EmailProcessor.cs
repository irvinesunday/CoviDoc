using Microsoft.Extensions.Configuration;
using System;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace MessagingService
{
    public static class EmailProcessor
    {
        public static async Task SendEmailAsync(Email email,
                                    string emailUserName,
                                    string emailPassword)
        {
            try
            {
                MailMessage mail = new MailMessage();
                SmtpClient smtpClient = new SmtpClient("smtp.gmail.com");

                mail.From = new MailAddress(email.From);
                mail.To.Add(email.To);
                mail.Subject = email.Subject;
                mail.Body = email.Body;
                mail.Sender = new MailAddress(email.From);
                mail.IsBodyHtml = true;

                smtpClient.Port = 587;
                smtpClient.Credentials = new NetworkCredential(emailUserName, emailPassword);
                smtpClient.EnableSsl = true;

                await smtpClient.SendMailAsync(mail);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
    }
}
