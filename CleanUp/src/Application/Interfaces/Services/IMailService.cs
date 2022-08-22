using CleanUp.Application.Requests.Mail;
using CleanUp.Application.Responses.Mail;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CleanUp.Application.Interfaces.Services
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