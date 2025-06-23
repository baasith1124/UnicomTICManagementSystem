using MimeKit;
using MailKit.Net.Smtp;
using MailKit.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnicomTICManagementSystem.Helpers
{
    public static class EmailService
    {
        private static readonly string smtpServer = "smtp.gmail.com";
        private static readonly int smtpPort = 587;
        private static readonly string fromEmail = "abdulbaasith1124@gmail.com"; // your Gmail
        private static readonly string fromPassword = "halo jfbr zpmv ghgu"; // Gmail app password

        public static async Task<bool> SendEmailAsync(string toEmail, string subject, string body)
        {
            try
            {
                if (!MailboxAddress.TryParse(toEmail, out var toAddress))
                {
                    ErrorLogger.Log("Invalid recipient emailEmailService.SendEmailAsync"+toEmail);
                    return false;
                }
                var message = new MimeMessage();
                message.From.Add(new MailboxAddress("Unicom TIC", fromEmail));
                message.To.Add(toAddress);
                message.Subject = subject;

                message.Body =  new BodyBuilder
                {
                    HtmlBody = body
                }.ToMessageBody();

                message.ReplyTo.Add(new MailboxAddress("Support", fromEmail));

                using (var client = new SmtpClient())
                {
                    await client.ConnectAsync(smtpServer, smtpPort, MailKit.Security.SecureSocketOptions.StartTls);
                    await client.AuthenticateAsync(fromEmail, fromPassword);
                    await client.SendAsync(message);
                    await client.DisconnectAsync(true);
                }

                return true;
            }
            catch (SmtpCommandException ex)
            {
                ErrorLogger.Log(ex, "SMTP Command Error in EmailService.SendEmailAsync");
                return false;
            }
            catch (SmtpProtocolException ex)
            {
                ErrorLogger.Log(ex, "SMTP Protocol Error in EmailService.SendEmailAsync");
                return false;
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "EmailService.SendEmailAsync");
                Console.WriteLine($"SMTP Command Error: {ex.Message} ");
                return false;
            }
            
            
        }
    }
}
