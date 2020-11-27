using System;
using System.IO;
using System.Text;
using BusinessLayer;
using DatabaseLayer.ConfigModels;
using DatabaseLayer.Entities;
using MimeKit;
using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace Apit.Service
{
    public class MailService
    {
        private readonly ILogger<MailService> _logger;
        private readonly ProjectConfig.MailboxConfig _config;


        public class Presets
        {
            public const string ConfirmEmail = "ConfirmEmail.htm";
            public const string ResetPassword = "ResetPassword.htm";
            public const string ArticleInfo = "ArticleInfo.htm";
        }


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

                UseSSL = config.MailboxDefaults.UseSSL,
                AddressName = config.MailboxDefaults.AddressName,
                ServiceHost = Environment.GetEnvironmentVariable("SERVICE_EMAIL_HOST")
                              ?? config.MailboxDefaults.ServiceHost,
                ServicePort = useEnvPort ? port : config.MailboxDefaults.ServicePort,

                MailSubjects = config.MailboxDefaults.MailSubjects ?? throw new NullReferenceException()
            };
        }


        public void SendArticleInfoEmail(string recipient, Article article, DataManager dataManager, string subject)
        {
            if (article.Options == null) throw new NullReferenceException(nameof(article.Options));
            
            var authors = new StringBuilder();
            foreach (var author in article.Authors)
            {
                var user = dataManager.Users.GetById(author.UserId);
                authors.Append($"<li>{author.NameString ?? user.FullName}</li>");
            }

            string html = GetMailHtmlPreset(Presets.ArticleInfo);

            SetContent(ref html, "Title", article.Title);
            SetContent(ref html, "TopicName", article.Options.Topic.Name);
            SetContent(ref html, "KeyWords", article.KeyWords);
            SetContent(ref html, "ShortDescription", article.ShortDescription);
            SetContent(ref html, "ArticleAuthors", authors.ToString());

            SetContent(ref html, "PageAbsoluteUrl", article.Options.PageAbsoluteUrl);
            SetContent(ref html, "DocumentAbsoluteUrl", article.Options.DocumentAbsoluteUrl);
            SetContent(ref html, "DateCreated", article.DateCreated.ToString("HH:mm - dd.MM.yyyy"));


            SendEmail(recipient, subject, html);
        }


        public void SendConfirmationEmail(string userEmail, string confirmationLink)
        {
            SendActionEmail(userEmail,
                _config.MailSubjects.ConfirmEmailSubject,
                Presets.ConfirmEmail, confirmationLink);
            _logger.LogInformation("Confirmation email was sent to: " + userEmail);
        }


        /// <summary>
        /// Send HTML email with linking button via SMTP-client
        /// </summary>
        /// <param name="recipient">User email address</param>
        /// <param name="subject">Mail title</param>
        /// <param name="preset">Mail content name</param>
        /// <param name="href">Specific URL to embed in an email</param>
        public void SendActionEmail(string recipient,
            string subject, string preset, string href)
        {
            try
            {
                string html = GetMailHtmlPreset(preset);
                href = href.Replace("\n", "");
                SetContent(ref html, "HYPERLINK", href);
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

                _config.AddressEmail ??= _config.RealEmail;
                message.From.Add(new MailboxAddress(_config.AddressName, _config.AddressEmail));
                message.To.Add(MailboxAddress.Parse(recipient));

                using var client = new SmtpClient();

                // use port 465 or 587
                client.Connect(_config.ServiceHost, _config.ServicePort, _config.UseSSL);

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


        private static void SetContent(ref string html, string variable, string content)
        {
            html = html.Replace("{{" + variable + "}}", content);
        }

        private static string GetMailHtmlPreset(string presetPath) =>
            File.ReadAllText(Path.Combine("Service/HtmlEmails/", presetPath));
    }
}