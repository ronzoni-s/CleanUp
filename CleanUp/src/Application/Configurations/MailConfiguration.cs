using System;

namespace CleanUp.Application.Configurations
{
    public class MailConfiguration
    {
        public int ImapId { get; set; }
        public bool ImapUseSsl { get; set; }
        public string ImapHost { get; set; }
        public int ImapPort { get; set; }
        public bool ImapUseAuthentication { get; set; }
        public string ImapUsername { get; set; }
        public string ImapPassword { get; set; }

        public int SmtpId { get; set; }
        public bool SmtpUseSsl { get; set; }
        public string SmtpHost { get; set; }
        public int SmtpPort { get; set; }
        public bool SmtpUseAuthentication { get; set; }
        public string SmtpUsername { get; set; }
        public string SmtpPassword { get; set; }
        public string SmtpFromEmail { get; set; }
    }
}