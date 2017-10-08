using Microsoft.Extensions.Configuration;
using SiedliskoCommon.Models;
using SiedliskoCommon.Models.Enums;
using System.Collections.Generic;
using System.Linq;
using System;
using System.Net.Http;
using MailKit.Net.Smtp;

namespace MailSender.Mailing
{
    public class MailingService
    {
        #region Fields and Properties
        private IConfigurationRoot _configuration;
        private List<EmailMessage> _emails = new List<EmailMessage>();
        private string _apiCredentials;
        #endregion

        #region ctor
        public MailingService(IConfigurationRoot configuration)
        {
            _configuration = configuration;
        }
        #endregion

        #region Public API
        public void GatherEmails()
        {
            //todo: logic to retrieve emails list;


            var emailsToSend = _emails.Where(x => x.status == EmailStatus.ToSend);
            if (emailsToSend.Count() > 0)
            {
                ProcessEmails(emailsToSend);
            }
        }
        #endregion

        #region Private methods
        private void ProcessEmails(IEnumerable<EmailMessage> emailsToSend)
        {
            foreach (var email in emailsToSend)
            {
                if (UpdateEmailStatus(email))
                {
                    SendEmail(email);
                }

            }
        }

        private void SendEmail(EmailMessage email)
        {
            //setting up smtp client
            SmtpClient client = new SmtpClient();
            client.Connect("smtp.google.com", 587, false);
            client.AuthenticationMechanisms.Remove("XOAUTH2");
            client.Authenticate();
            client.Send();
            client.Disconnect(true);


            //if message is sent update status once again
            if (UpdateEmailStatus(email))
            {
                //if status is correctly updated then we remove email from list to send
                _emails.Remove(email);
            }
        }

        private bool UpdateEmailStatus(EmailMessage email)
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
