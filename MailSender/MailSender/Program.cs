using MailSender.Mailing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Reactive.Linq;

namespace MailSender
{
    class Program
    {
        static void Main(string[] args)
        {

            IConfigurationRoot _config = null;
            var builder = new ConfigurationBuilder()
                .AddJsonFile("config.json");
            _config = builder.Build();

            var serviceProvide = new ServiceCollection()
                .AddSingleton<MailingService>()
                .AddSingleton(_config);

            var looper = Observable.Interval(TimeSpan.FromSeconds(5));
            using (looper.Subscribe(onNext: (x) => 
            {
                Console.WriteLine(x);
            },
            onError: (ex) =>
            {
                Console.WriteLine(ex);
            }, onCompleted: () =>
            {

            }
            ))
            {
                Console.ReadLine();
            }
        }
    }
}
