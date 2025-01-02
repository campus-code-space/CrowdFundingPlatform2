using Microsoft.AspNetCore.Identity.UI.Services;
using NuGet.Protocol.Plugins;
using System.Net;
using System.Net.Mail;

namespace EndeKisse2.Services
{
    //public interface IEmailService
    //{
    //    Task SendEmail(string receptor, string subject, string body);
    //}

    public class EmailService : IEmailSender
    {
        private readonly IConfiguration _configuration;

        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task SendEmailAsync(string receptor, string subject, string body)
        {
            var email = _configuration.GetValue<string>("EMAIL_CONFIGURATION:EMAIL");
            var password = _configuration.GetValue<string>("EMAIL_CONFIGURATION:PASSWORD");
            var host = _configuration.GetValue<string>("EMAIL_CONFIGURATION:HOST");
            var port = _configuration.GetValue<int>("EMAIL_CONFIGURATION:PORT");

            var smtpClient = new SmtpClient(host, port);
            smtpClient.EnableSsl = true;
            smtpClient.UseDefaultCredentials = false;

            smtpClient.Credentials = new NetworkCredential(email, password);
            var message = new MailMessage(email!, receptor, subject, body);
            await smtpClient.SendMailAsync(message);
        }
    }
}
