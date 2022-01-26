using Microsoft.Extensions.Logging;
using MyQ.Domain.Abstractions;
using MyQ.Domain.Models;

namespace MyQ.Domain
{
    public class RobotSimulator : IRobotSimulator
    {
        private readonly ILogger<RobotSimulator> _logger;
        private readonly IRobot _robot;

        public RobotSimulator(
            ILogger<RobotSimulator> logger,
            IRobot robot)
        {
            _logger = logger;
            _robot = robot;
        }

        public SimulationResult Simulate(SimulationParameters parameters)
        {
            _robot.Init(parameters);
            return _robot.Run(parameters.Commands);
        }
    }
}
