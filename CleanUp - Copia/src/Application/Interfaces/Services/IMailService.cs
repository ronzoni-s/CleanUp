using ErbertPranzi.Application.Requests.Mail;
using ErbertPranzi.Application.Responses.Mail;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ErbertPranzi.Application.Interfaces.Services
{
    public interface IMailService
    {
        int SmtpConfigId { get; }
        int ImapConfigId { get; }

        Task SendAsync(MailRequest request);

        //IList<MailResponse> ReceiveEmailWithPop(int? maxCount = null);

        IList<MailResponse> ReceiveEmailWithImap(DateTime? fromDate = null, int? maxCount = null);
    }
}