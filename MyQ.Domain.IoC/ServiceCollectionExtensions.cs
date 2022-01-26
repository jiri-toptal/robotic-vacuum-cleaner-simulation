using Microsoft.Extensions.DependencyInjection;
using MyQ.Domain.Abstractions;
using MyQ.Domain.Commands;

namespace MyQ.Domain.IoC
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddRobotCommands(this IServiceCollection services)
        {
            return services
                .AddScoped<IRobotCommand, TurnLeftCommand>()
                .AddScoped<IRobotCommand, TurnRightCommand>()
                .AddScoped<IRobotCommand, CleanCommand>()
                .AddScoped<IRobotCommand, AdvanceCommand>()
                .AddScoped<IRobotCommand, BackCommand>();
        }

        public static IServiceCollection AddDomain(this IServiceCollection services)
        {
            return services
                .AddScoped<IRobot, Robot>()
                .AddScoped<IBackOffSequenceStrategy, BackOffSequenceStrategy>()
                .AddScoped<IRobotSimulator, RobotSimulator>();
        }
    }
}
