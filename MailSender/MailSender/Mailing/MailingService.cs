using Microsoft.Extensions.Configuration;
using SiedliskoCommon.Models;
using SiedliskoCommon.Models.Enums;
using System.Collections.Generic;
using System.Linq;
using System;
using System.Net.Http;
using MailKit.Net.Smtp;
using MimeKit;
using MimeKit.Text;
using MailKit;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Reactive.Linq;
using Newtonsoft.Json;

namespace MailSender.Mailing
{
    public class MailingService
    {
        #region Fields and Properties
        private IConfigurationRoot _configuration;
        private List<EmailMessage> _emails = new List<EmailMessage>();
        private string _apiCredentials;
        private string _baseAddress;
        private readonly object _syncObject = new object();
        #endregion

        #region ctor
        public MailingService(IConfigurationRoot configuration)
        {
            _configuration = configuration;
            _apiCredentials = Convert.ToBase64String(Encoding.UTF8.GetBytes(string.Format("{0}:{1}", _configuration["WebApi:Login"], _configuration["WebApi:Password"])));
            _baseAddress = _configuration["WebApi:BaseAdress"];

        }
        #endregion

        #region Public API
        public async Task GatherEmails()
        {
            string response = "";
            try
            {
                //todo: logic to retrieve emails list;
                using (HttpClient client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", _apiCredentials);
                    response = await client.GetStringAsync(_baseAddress + "/GetEmailsToSend");
                    if (string.IsNullOrWhiteSpace(response))
                    {
                        return;
                    }
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

            if (string.IsNullOrWhiteSpace(response))
            {
                Console.WriteLine("Revieved empty list");
                return;
            }

            Console.WriteLine("Processing emails...");
            var tempEmails = JsonConvert.DeserializeObject<List<EmailMessage>>(response);
            lock (_syncObject)
            {
                foreach (var email in tempEmails)
                {
                    if (!_emails.Any(x => x.Id == email.Id))
                    {
                        _emails.Add(email);
                    }
                }
                var emailsToSend = _emails.Where(x => x.status == EmailStatus.ToSend);
                if (emailsToSend.Count() > 0)
                {
                    ProcessEmails(emailsToSend);
                    Console.WriteLine("Processing emails done.");
                }
                else
                {
                    Console.WriteLine("Nothing to do...");
                }
            }
        }

        public void Run()
        {
            var looper = Observable.Interval(TimeSpan.FromSeconds(double.Parse(_configuration["ServiceConfiguration:TimerInterval"])));
            using (looper.Subscribe(onNext: async (x) =>
            {
                await GatherEmails();
            },
            onError: (ex) =>
            {
                System.Diagnostics.Debug.WriteLine(string.Format($"An error occured: {ex}"));
            }, onCompleted: () =>
            {
                System.Diagnostics.Debug.WriteLine("Subscription completed");
            }
            ))
            {
                Console.WriteLine("press any key to close application...");
                Console.ReadLine();
            }
        }
        #endregion

        #region Private methods
        private async Task ProcessEmails(IEnumerable<EmailMessage> emailsToSend)
        {
            foreach (var email in emailsToSend)
            {
                if (await UpdateEmailStatus(email))
                {
                    await SendEmail(email);
                }

            }
        }

        private async Task SendEmail(EmailMessage email)
        {
            //setting up smtp client
            try
            {
                using (SmtpClient client = new SmtpClient())
                {
                    client.ServerCertificateValidationCallback = (s, a, b, c) => { return true; };
                    client.Connect(_configuration["SMPTConfiguration:Server"], int.Parse(_configuration["SMPTConfiguration:port"]), false);
                    //client.AuthenticationMechanisms.Remove("XOAUTH2");
                    client.Authenticate(_configuration["SMPTConfiguration:Login"], _configuration["SMPTConfiguration:Password"]);

                    var message = new MimeMessage();
                    message.Body = new TextPart(TextFormat.Html) { Text = email.MessageBody };
                    message.To.Add(new MailboxAddress(email.ToAdress));
                    message.Subject = "Rezerwacja - Siedlisko";
                    message.Sender = new MailboxAddress(_configuration["SMPTConfiguration:Login"]);
                    client.Send(message);
                    client.Disconnect(true);
                    Console.WriteLine("email sent seccesfully");
                }
            }
            catch (ServiceNotConnectedException e)
            {
                Console.WriteLine(e);
            }
            catch (ServiceNotAuthenticatedException e)
            {
                Console.WriteLine(e);
            }
            catch (ObjectDisposedException e)
            {
                Console.WriteLine(e);
            }
            catch (InvalidOperationException e)
            {
                Console.WriteLine(e);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }


            //if message is sent update status once again
            if (await UpdateEmailStatus(email))
            {
                //if status is correctly updated then we remove email from list to send
                _emails.Remove(email);
                Console.WriteLine("Finished updating email status");
            }
        }

        private async Task<bool> UpdateEmailStatus(EmailMessage email)
        {
            switch (email.status)
            {
                case EmailStatus.ToSend:
                    email.status = EmailStatus.Sending;
                    break;
                case EmailStatus.Sending:
                    email.status = EmailStatus.Sent;
                    break;
                case EmailStatus.Sent:
                    return false;
                default:
                    return false;
            }
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", _apiCredentials);
                using (var content = new StringContent(JsonConvert.SerializeObject(email), Encoding.UTF8, "application/json"))
                {
                    var result = await client.PutAsync(_baseAddress + "/UpdateEmail", content);
                    if (result.IsSuccessStatusCode)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
        }
        #endregion
    }
}
