using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SiedliskoCommon.Models;

namespace Siedlisko.Models.Interfaces
{
    public interface IEmailRepository
    {
        EmailMessage AddEmail(EmailMessage email);
        EmailMessage GetemailById(int id);
        IEnumerable<EmailMessage> GetEmailMessages();
        IEnumerable<EmailMessage> GetEmailMessages(Func<EmailMessage, bool> predicate);
        Task<EmailMessage> UpdateEmail(EmailMessage email);
    }
}