
using CleanUp.Application.Configurations;
using CleanUp.Infrastructure.Shared.Services;
using System;
using System.Data.SqlClient;
using Dapper;
using System.Linq;
using CleanUp.Domain.Entities;

namespace CleanUp.Server.Extensions
{
    internal static class ExtensionMethods
    {
        public static MailService AddEmailService(this IServiceProvider provider, string connectionString)
        {
            using var connection = new SqlConnection(connectionString);
            try
            {
                connection.Open();

                var query = @"SELECT [Id]
                                    ,[Name]
                                    ,[UseSsl]
                                    ,[Host]
                                    ,[Port]
                                    ,[UseAuthentication]
                                    ,[Username]
                                    ,[Password]
                                    ,[FromEmail]
                                    ,[LastEmailDateTime]
                                FROM [dbo].[EmailConfigs]";
                var settings = connection.Query<EmailConfig>(query);
                connection.Close();

                var mailConfig = new MailConfiguration();
                foreach (var setting in settings)
                {
                    if (setting.Name == "IMAP")
                    {
                        mailConfig.ImapId = setting.Id;
                        mailConfig.ImapUseSsl = setting.UseSsl;
                        mailConfig.ImapHost = setting.Host;
                        mailConfig.ImapPort = setting.Port;
                        mailConfig.ImapUseAuthentication = setting.UseAuthentication;
                        mailConfig.ImapUsername = setting.Username;
                        mailConfig.ImapPassword = setting.Password;
                    } else if(setting.Name == "SMTP")
                    {
                        mailConfig.SmtpId = setting.Id;
                        mailConfig.SmtpUseSsl = setting.UseSsl;
                        mailConfig.SmtpHost = setting.Host;
                        mailConfig.SmtpPort = setting.Port;
                        mailConfig.SmtpUseAuthentication = setting.UseAuthentication;
                        mailConfig.SmtpUsername = setting.Username;
                        mailConfig.SmtpPassword = setting.Password;
                        mailConfig.SmtpFromEmail = setting.FromEmail;
                    }
                }
                return new MailService(mailConfig);
            }
            catch (Exception ex)
            {
                connection.Close();
                throw;
            }
        }
    }
}