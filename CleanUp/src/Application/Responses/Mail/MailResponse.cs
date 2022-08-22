using System;

namespace CleanUp.Application.Responses.Mail
{
    public class MailResponse
    {
        public string To { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public string From { get; set; }
        public DateTime Date { get; set; }
    }
}