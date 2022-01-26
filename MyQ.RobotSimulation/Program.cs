using CommandLine;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MyQ.Domain.Abstractions;
using MyQ.Domain.IoC;
using MyQ.Domain.Models;
using MyQ.Shared.Services.Abstractions;
using MyQ.Shared.Services.IoC;
using System;

namespace MyQ.RobotSimulation
{
    class Program
    {
        static void Main(string[] args)
        {
            Parser.Default.ParseArguments<SimulationOptions>(args)
                .WithParsed(async options => {
                    var serviceProvider = BuildServiceProvider();
                    var fileProvider = serviceProvider.GetRequiredService<IFileProvider>();
                    var jsonService = serviceProvider.GetRequiredService<IJsonService>();

                    var sourceFileContent = await fileProvider.ReadSourceFileAsync(options.Source);
                    var parameters = jsonService.Deserialize<SimulationParameters>(sourceFileContent);
                    var robotSimulator = serviceProvider.GetRequiredService<IRobotSimulator>();

                    var simulationResult = robotSimulator.Simulate(parameters);

                    var simulationResultContent = jsonService.SerializeObject(simulationResult);
                    await fileProvider.WriteAllTextAsync(simulationResultContent, options.Result);

                    Console.ReadLine();
                })
                .WithNotParsed(e => {
                    // todo: use logger
                    Console.WriteLine("Provide all required application arguments.");
                });
        }

        private static IServiceProvider BuildServiceProvider()
        {
            //setup our DI
            var serviceProvider = new ServiceCollection()
                .AddLogging(builder => builder.AddConsole())
                .AddDomain()
                .AddRobotCommands()
                .AddSharedServices()
                .BuildServiceProvider();

            return serviceProvider;
        }
    }
}
