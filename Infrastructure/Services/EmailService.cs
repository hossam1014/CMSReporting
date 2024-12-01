using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
// using System.Net.Mail;
using System.Threading.Tasks;
using Infrastructure.Models;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;

namespace Infrastructure.Services
{
    public class EmailService
    {
        private readonly EmailSettings _emailSettings;

        public EmailService(IOptions<EmailSettings> emailSettings)
        {
            _emailSettings = emailSettings.Value;
        }

        public async Task SendEmailAsync(string toEmail, string subject, string message)
        {
            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse(_emailSettings.FromEmail));
            email.To.Add(MailboxAddress.Parse(toEmail));
            email.Subject = subject;
            email.Body = new TextPart(MimeKit.Text.TextFormat.Html) { Text = message };

            using var smtp = new SmtpClient();
            await smtp.ConnectAsync(_emailSettings.SmtpServer, int.Parse(_emailSettings.Port), true);
            await smtp.AuthenticateAsync(_emailSettings.FromEmail, _emailSettings.Password);
            await smtp.SendAsync(email);
            await smtp.DisconnectAsync(true);
        }

        // public async Task SendEmailAsync(string toEmail, string subject, string message)
        // {
        //     var fromEmail = _emailSettings.FromEmail;
        //     var smtpServer = _emailSettings.SmtpServer;
        //     var port = int.Parse(_emailSettings.Port);
        //     var username = _emailSettings.FromEmail;
        //     var password = _emailSettings.Password;
        //     var enableSsl = bool.Parse(_emailSettings.EnableSsl);

        //     var mailMessage = new MailMessage
        //     {
        //         From = new MailAddress(fromEmail),
        //         Subject = subject,
        //         Body = message,
        //         IsBodyHtml = true,
        //     };

        //     mailMessage.To.Add(toEmail);

        //     using var smtpClient = new SmtpClient(smtpServer, port)
        //     {
        //         Credentials = new NetworkCredential(username, password),
        //         EnableSsl = enableSsl,
        //     };

        //     await smtpClient.SendMailAsync(mailMessage);
        // }
    }
}