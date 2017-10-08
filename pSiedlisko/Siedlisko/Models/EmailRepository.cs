using SiedliskoCommon.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Siedlisko.Models
{
    public class EmailRepository
    {
        #region Fields and properties

        private SiedliskoContext _context;
        private object _syncObject = new object();

        #endregion

        #region .ctor
        public EmailRepository(SiedliskoContext context)
        {
            _context = context;
        }
        #endregion

        #region Public API
        public IEnumerable<EmailMessage> GetEmailMessages()
        {
            return _context.EmailMessages.ToList();
        }

        public IEnumerable<EmailMessage> GetEmailMessages(Func<EmailMessage, bool> predicate)
        {
            lock (_syncObject)
            {
                return _context.EmailMessages.Where(predicate).ToList();
            }
        }

        public EmailMessage GetemailById(int id)
        {
            return _context.EmailMessages.FirstOrDefault(x => x.Id == id);
        }

        public async Task<EmailMessage> AddEmail(EmailMessage email)
        {
            var entity = _context.EmailMessages.Add(email);
            await _context.SaveChangesAsync();
            return entity.Entity;
        }

        public async Task<EmailMessage> UpdateEmail(EmailMessage email)
        {
            EmailMessage emailMsg;
            lock (_syncObject)
            {
                emailMsg = _context.EmailMessages.Update(email).Entity;
            }
            await _context.SaveChangesAsync();
            return emailMsg;
        }
        #endregion
    }
}
