using Microsoft.Extensions.Logging;
using MyQ.Domain.Abstractions;
using MyQ.Domain.Commands;
using MyQ.Enums;
using System.Collections.Generic;
using System.Linq;

namespace MyQ.Domain
{
    public class BackOffSequenceStrategy : IBackOffSequenceStrategy
    {
        private readonly ILogger<BackOffSequenceStrategy> _logger;
        private readonly string[][] BackOffCommandSequences = new[]
        {
            new[] { CommandType.TurnRight, CommandType.Advance, CommandType.TurnLeft },
            new[] { CommandType.TurnRight, CommandType.Advance, CommandType.TurnRight },
            new[] { CommandType.TurnRight, CommandType.Advance, CommandType.TurnRight },
            new[] { CommandType.TurnRight, CommandType.Back, CommandType.TurnRight, CommandType.Advance },
            new[] { CommandType.TurnLeft, CommandType.TurnLeft, CommandType.Advance }
        };

        private readonly IEnumerable<IRobotCommand> _commands;

        public BackOffSequenceStrategy(
            ILogger<BackOffSequenceStrategy> logger,
            IEnumerable<IRobotCommand> commands)
        {
            _logger = logger;
            _commands = commands;
        }

        public CanExecuteCommandResult TryToBackOff(IRobot robot, IRobotCommand command)
        {
            robot.LogBackOffSequence();

            foreach (var backOffCommandSequence in BackOffCommandSequences)
            {
                _logger.LogInformation($"Running back off sequence: {string.Join(", ", backOffCommandSequence)}");

                foreach (var backOffCommandName in backOffCommandSequence)
                {
                    var backOffCommand = _commands.FirstOrDefault(c => c.Name == backOffCommandName);
                    var backOffCommandResult = backOffCommand.CanExecuteCommand(robot);

                    if (backOffCommandResult == CanExecuteCommandResult.Obstacle)
                    {
                        robot.ConsumeEnergy(backOffCommand.EnergyConsumption);
                        break;
                    }

                    if (backOffCommandResult == CanExecuteCommandResult.Yes || backOffCommandResult == CanExecuteCommandResult.LowEnergy)
                    {
                        backOffCommand.Execute(robot);
                        robot.ConsumeEnergy(backOffCommand.EnergyConsumption);

                        var commandResult = command.CanExecuteCommand(robot);

                        if (commandResult == CanExecuteCommandResult.Obstacle)
                        {
                            robot.ConsumeEnergy(command.EnergyConsumption);
                            break;
                        }
                        if (commandResult == CanExecuteCommandResult.Yes)
                        {
                            robot.ConsumeEnergy(command.EnergyConsumption);
                            command.Execute(robot);
                            return commandResult;
                        }
                        if (commandResult == CanExecuteCommandResult.LowEnergy)
                        {
                            return commandResult;
                        }

                        return backOffCommandResult;
                    }
                }
            }

            return CanExecuteCommandResult.Stucked;
        }
    }
}
