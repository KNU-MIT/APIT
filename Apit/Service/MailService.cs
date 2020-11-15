using System;
using System.IO;
using MimeKit;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Logging;

namespace Apit.Service
{
    public class MailService
    {
        private readonly ILogger<MailService> _logger;
        private readonly ProjectConfig.MailboxConfig _config;

        public MailService(ILogger<MailService> logger, ProjectConfig config)
        {
            _logger = logger;

            bool useEnvPort = int.TryParse(Environment.GetEnvironmentVariable("SERVICE_EMAIL_PORT"), out int port);

            _config = new ProjectConfig.MailboxConfig
            {
                RealEmail = Environment.GetEnvironmentVariable("SERVICE_EMAIL_ADDRESS")
                            ?? config.MailboxDefaults.RealEmail,
                RealEmailPassword = Environment.GetEnvironmentVariable("SERVICE_EMAIL_PASSWORD")
                                    ?? config.MailboxDefaults.RealEmailPassword,
                AddressEmail = Environment.GetEnvironmentVariable("SERVICE_EMAIL_ADDRESS")
                               ?? config.MailboxDefaults.AddressEmail,
                ServiceHost = Environment.GetEnvironmentVariable("SERVICE_EMAIL_HOST")
                              ?? config.MailboxDefaults.ServiceHost,
                ServicePort = useEnvPort ? port : config.MailboxDefaults.ServicePort
            };
        }


        public class Presets
        {
            public const string ConfirmEmail = "ConfirmEmail.htm";
            public const string ResetPassword = "ResetPassword.htm";
        }

        /// <summary>
        /// Send HTML email with linking button via SMTP-client
        /// </summary>
        /// <param name="recipient">User email address</param>
        /// <param name="subject">Mail title</param>
        /// <param name="resourceName">Mail content name</param>
        /// <param name="href">Specific URL to embed in an email</param>
        public void SendActionEmail(string recipient,
            string subject, string resourceName, string href)
        {
            try
            {
                string html = File.ReadAllText(Path.Combine("Service/HtmlEmails/", resourceName));
                href = href.Replace("\n", "");
                html = html.Replace("{{HYPERLINK}}", href);
                _logger.LogDebug(html);
                SendEmail(recipient, subject, html);
            }
            catch (Exception e)
            {
                _logger.LogCritical("Mail service error: " + e.Message);
            }
        }

        /// <summary>
        /// Send email via default configured SMTP-client
        /// </summary>
        /// <param name="recipient">User email address</param>
        /// <param name="subject">Mail title</param>
        /// <param name="body">Mail content (supports HTML)</param>
        public void SendEmail(string recipient, string subject, string body)
        {
            try
            {
                var message = new MimeMessage
                {
                    Subject = subject,
                    Body = new BodyBuilder
                    {
                        HtmlBody = body,
                    }.ToMessageBody()
                };

                message.From.Add(new MailboxAddress(_config.AddressName, _config.AddressEmail));
                message.To.Add(MailboxAddress.Parse(recipient));

                using var client = new SmtpClient();

                // use port 465 or 587
                client.Connect(_config.ServiceHost, _config.ServicePort, true);

                client.Authenticate(_config.RealEmail, _config.RealEmailPassword);
                client.Send(message);

                client.Disconnect(true);
                _logger.LogInformation("Email sent to recipient: " + recipient);
            }
            catch (Exception e)
            {
                _logger.LogError(e.GetBaseException().Message);
            }
        }
    }
}