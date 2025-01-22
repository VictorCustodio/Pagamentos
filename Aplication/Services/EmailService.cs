using System.Net;
using System.Net.Mail;
using EmailService.Configuration;
using EmailService.Models;
using Microsoft.Extensions.Options;

namespace EmailService.Application.Services
{
   
    public class EmailService : IEmailService
    {
        private readonly EmailConfiguration _config;

        public EmailService(IOptions<EmailConfiguration> config)
        {
            _config = config.Value;
        }

        public async Task SendEmailAsync(EmailMessage emailMessage)
        {
           /* var client2 = new SmtpClient("sandbox.smtp.mailtrap.io", 2525)
            {
                Credentials = new NetworkCredential("user", "senha"),
                EnableSsl = true
            };
            client2.Send("from@example.com", "to@example.com", "Hello world", "testbody");
            System.Console.WriteLine("Sent");
           */
            using var client = new SmtpClient(_config.SmtpServer, _config.Port)
            {
                Credentials = new NetworkCredential(_config.SenderEmail, _config.Password),
                EnableSsl = false
            };

            var mailMessage = new MailMessage
            {
                From = new MailAddress(_config.SenderEmail, _config.SenderName),
                Subject = emailMessage.Email,
                Body = emailMessage.Assunto,
            };
            //Lista de Envio
            //mailMessage.To.Add(emailMessage.To);

            await client.SendMailAsync(mailMessage);
        }
    }
}
