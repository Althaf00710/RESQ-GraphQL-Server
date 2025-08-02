using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace Application.Utils
{
    public class EmailSender
    {
        private readonly IConfiguration _configuration;

        public EmailSender(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<bool> SendEmailAsync(string toEmail, string subject, string message)
        {
            var smtpSettings = _configuration.GetSection("SmtpSettings");
            string host = smtpSettings["Host"];
            int port = int.Parse(smtpSettings["Port"]);
            bool enableSsl = bool.Parse(smtpSettings["EnableSsl"]);
            string username = smtpSettings["Username"];
            string password = smtpSettings["Password"];
            string fromEmail = smtpSettings["FromEmail"];

            var smtpClient = new SmtpClient(host)
            {
                Port = port,
                Credentials = new NetworkCredential(username, password),
                EnableSsl = enableSsl
            };

            var mailMessage = new MailMessage(fromEmail, toEmail, subject, message)
            {
                IsBodyHtml = true
            };

            try
            {
                await smtpClient.SendMailAsync(mailMessage);
                return true;
            }
            catch (System.Exception ex)
            {
                Console.WriteLine($"Email send error: {ex.Message}");
                return false;
            }
        }
    }
}
