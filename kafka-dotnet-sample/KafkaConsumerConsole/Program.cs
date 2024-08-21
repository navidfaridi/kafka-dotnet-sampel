// See https://aka.ms/new-console-template for more information
using KafkaConsumerConsole;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

Console.WriteLine("Hello, World!");

Host.CreateDefaultBuilder(args)
           .ConfigureServices((hostContext, services) =>
           {
               services.AddHostedService<KafkaWorker>();
           })
           .Build()
           .Run();