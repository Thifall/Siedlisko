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
                .AddSingleton(_config)
                .BuildServiceProvider();

            var service = (MailingService)serviceProvide.GetService(typeof(MailingService));
            service.Run();
            Console.ReadLine();
            
        }
    }
}
