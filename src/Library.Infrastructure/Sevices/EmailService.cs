using Library.Application.Interfaces;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Library.Infrastructure.Sevices
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _config;
        public EmailService(IConfiguration config)
        {
            _config = config;
        }

        public async Task SendEmailAsync(string to, string subject, string body)
        {
            try
            {
                var email = _config["EmailSettings:Email"];
                var password = _config["EmailSettings:Password"];
                var host = _config["EmailSettings:Host"];
                var port = int.Parse(_config["EmailSettings:Port"]);

                using var smtp = new SmtpClient(host, port)
                {
                    UseDefaultCredentials = false,
                    Credentials = new NetworkCredential(email, password),
                    EnableSsl = true,
                    DeliveryMethod = SmtpDeliveryMethod.Network
                };

                using var mail = new MailMessage(email, to, subject, body);

                await smtp.SendMailAsync(mail);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Send mail failed: " + ex.Message);
            }
        }
    }
}
