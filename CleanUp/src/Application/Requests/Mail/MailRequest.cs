using System.Collections.Generic;

namespace CleanUp.Application.Requests.Mail
{
    public class MailRequest
    {
        public List<string> To { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public string From { get; set; }
    }
}