using Lendship.Backend.DTO.Authentication;
using Lendship.Backend.Interfaces.Services;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;
using MimeKit;
using System;

namespace Lendship.Backend.Services
{
    public class EmailService : IEmailService
    {
        private readonly MailSettings _mailSettings;

        public EmailService(IOptions<MailSettings> mailSettings)
        {
            _mailSettings = mailSettings.Value;
        }

        public void SendEmailAsync(string username, string email, string url)
        {
            var mailMessage = new MimeMessage();
            mailMessage.From.Add(new MailboxAddress(_mailSettings.DisplayName, _mailSettings.Mail));
            mailMessage.To.Add(new MailboxAddress(username, email));
            mailMessage.Subject = _mailSettings.ResetEmailSubject;
            mailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Text) { Text = url };

            using var client = new SmtpClient();
            try
            {
                client.CheckCertificateRevocation = false;
                client.Connect("smtp.gmail.com", 465, true);
                client.AuthenticationMechanisms.Remove("XOAUTH2");
                client.Authenticate(_mailSettings.Mail, _mailSettings.Password);
                client.Send(mailMessage);
            }
            catch (Exception e)
            {
                Console.WriteLine("Error at sending email: " + e.Message);
                throw;
            }
            finally
            {
                client.Disconnect(true);
                client.Dispose();
            }
        }
    }
}
