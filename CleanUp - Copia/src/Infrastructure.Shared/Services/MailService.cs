using ErbertPranzi.Application.Configurations;
using ErbertPranzi.Application.Interfaces.Services;
using ErbertPranzi.Application.Requests.Mail;
using ErbertPranzi.Application.Responses.Mail;
using MailKit.Net.Imap;
using MailKit.Net.Pop3;
using MailKit.Net.Smtp;
using MailKit.Search;
using MailKit.Security;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MimeKit;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ErbertPranzi.Infrastructure.Shared.Services
{
    public class MailService : IMailService
    {
        private readonly MailConfiguration _config;

        public MailService(IOptions<MailConfiguration> config)
        {
            _config = config.Value;
        }

        public MailService(MailConfiguration config)
        {
            _config = config;
        }

        public int ImapConfigId => _config.ImapId;
        public int SmtpConfigId => _config.SmtpId;

        public async Task SendAsync(MailRequest request)
        {
            try
            {
                var email = new MimeMessage
                {
                    Sender = MailboxAddress.Parse(request.From ?? _config.SmtpFromEmail),
                    Subject = request.Subject,
                    Body = new BodyBuilder
                    {
                        HtmlBody = request.Body
                    }.ToMessageBody()
                };

                foreach (var emailAddr in request.To)
                {
                    email.To.Add(MailboxAddress.Parse(emailAddr));
                }

                using var smtp = new SmtpClient();
                await smtp.ConnectAsync(_config.SmtpHost, _config.SmtpPort, _config.SmtpUseSsl ? SecureSocketOptions.StartTls : SecureSocketOptions.None);
                if (_config.SmtpUseAuthentication)
                    await smtp.AuthenticateAsync(_config.SmtpUsername, _config.SmtpPassword);
                await smtp.SendAsync(email);
                await smtp.DisconnectAsync(true);
            }
            catch (System.Exception ex)
            {
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="youngerThan">If not null, specify to take only email in the last youngerThan seconds.</param>
        /// <param name="maxCount"></param>
        /// <returns></returns>
        public IList<MailResponse> ReceiveEmailWithImap(DateTime? fromDate = null,  int? maxCount = null)
        {
            using (var emailClient = new ImapClient())
            {
                emailClient.Connect(_config.ImapHost, _config.ImapPort, true);
                emailClient.AuthenticationMechanisms.Remove("XOAUTH2");
                if (_config.ImapUseAuthentication)
                    emailClient.Authenticate(_config.ImapUsername, _config.ImapPassword);

                emailClient.Inbox.Open(MailKit.FolderAccess.ReadWrite);

                var uids = emailClient.Inbox.Search(!fromDate.HasValue ? SearchQuery.All : SearchQuery.SentSince(fromDate.Value));

                List<MailResponse> emails = new List<MailResponse>();
                for (int i = 0; i < uids.Count && (!maxCount.HasValue || i < maxCount.Value); i++)
                {
                    var message = emailClient.Inbox.GetMessage(uids[i]);
                    if (fromDate.HasValue && message.Date.DateTime < fromDate)
                    {
                        continue;
                    }

                    var emailMessage = new MailResponse
                    {
                        Body = message.TextBody,
                        Subject = message.Subject,
                        Date = message.Date.DateTime
                    };
                    emails.Add(emailMessage);
                }
                emailClient.Inbox.AddFlags(uids, MailKit.MessageFlags.Seen, true);
                emailClient.Disconnect(true);

                return emails;
            }
        }
    }
}