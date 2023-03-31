using System.Net.Mail;
using System.Net;

namespace AI_as_a_Service.Services
{
    public class EmailService
    {
        private readonly string _smtpHost;
        private readonly int _smtpPort;
        private readonly string _emailFrom;
        private readonly string _emailPassword;

        public EmailService(string smtpHost, int smtpPort, string emailFrom, string emailPassword)
        {
            _smtpHost = smtpHost;
            _smtpPort = smtpPort;
            _emailFrom = emailFrom;
            _emailPassword = emailPassword;
        }

        public async Task SendEmailAsync(string emailTo, string subject, string body)
        {
            using var client = new SmtpClient(_smtpHost, _smtpPort)
            {
                Credentials = new NetworkCredential(_emailFrom, _emailPassword),
                EnableSsl = true,
            };

            using var message = new MailMessage(_emailFrom, emailTo, subject, body);

            await client.SendMailAsync(message);
        }
    }
}