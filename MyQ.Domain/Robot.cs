using Microsoft.Extensions.Logging;
using MyQ.Domain.Abstractions;
using MyQ.Domain.Models;
using MyQ.Enums;
using MyQ.Shared.Exceptions;
using System.Collections.Generic;
using System.Linq;

namespace MyQ.Domain
{
    public class Robot : IRobot
    {
        private bool _isInitialized = false;
        private readonly ILogger<Robot> _logger;
        private readonly IEnumerable<IRobotCommand> _commands;
        private readonly IBackOffSequenceStrategy _backOffSequenceStrategy;

        public string[][] Map { get; private set; }

        public int X { get; private set; }

        public int Y { get; private set; }

        public Facing Facing { get; private set; }

        public int Battery { get; private set; }

        public CanExecuteCommandResult LastCommandResult { get; private set; }

        public int BackOffSequenceNTriggered { get; private set; }

        public IList<Point> VisitedCells { get; set; } = new List<Point>();

        public IList<Point> CleanedCells { get; set; } = new List<Point>();

        public Robot(
            ILogger<Robot> logger,
            IEnumerable<IRobotCommand> commands,
            IBackOffSequenceStrategy backOffSequenceStrategy)
        {
            _logger = logger;
            _commands = commands;
            _backOffSequenceStrategy = backOffSequenceStrategy;
        }

        public void Init(RobotInitParameters parameters)
        {
            var isOutOfMap = parameters.Start.Y > parameters.Map.Length || parameters.Start.X > parameters.Map[parameters.Start.Y].Length;
            if (isOutOfMap)
            {
                throw new InvalidRobotStartPositionException();
            }

            X = parameters.Start.X;
            Y = parameters.Start.Y;
            Battery = parameters.Battery;
            Map = parameters.Map;
            Facing = parameters.Start.Facing;

            _isInitialized = true;
        }

        public SimulationResult Run(string[] commands)
        {
            if (!_isInitialized)
            {
                throw new RobotNotInitializedException();
            }

            foreach (var cmd in commands)
            {
                var command = _commands.FirstOrDefault(c => c.Name == cmd);
                if (command == null)
                {
                    throw new UnsupportedRobotCommandException($"Unsupported command name: { cmd }");
                }

                _logger.LogInformation($"BEFORE Command: {command.Name}, X: {X}, Y: {Y}, Facing: {Facing}, Battery: {Battery}");

                var canExecuteCommandResult = command.CanExecuteCommand(this);
                if (canExecuteCommandResult == CanExecuteCommandResult.Yes)
                {
                    command.Execute(this);
                    ConsumeEnergy(command.EnergyConsumption);
                }
                else if (canExecuteCommandResult == CanExecuteCommandResult.Obstacle)
                {
                    ConsumeEnergy(command.EnergyConsumption);
                    
                    var backOffResult = _backOffSequenceStrategy.TryToBackOff(this, command);
                    if (backOffResult == CanExecuteCommandResult.Stucked || backOffResult == CanExecuteCommandResult.LowEnergy)
                    {
                        LastCommandResult = backOffResult;
                        return GetSimulationResult();
                    }
                }
                else if (canExecuteCommandResult == CanExecuteCommandResult.LowEnergy)
                {
                    LastCommandResult = canExecuteCommandResult;
                    break;
                }

                _logger.LogInformation($"AFTER Command: {command.Name}, X: {X}, Y: {Y}, Facing: {Facing}, Battery: {Battery}");
            }

            return GetSimulationResult();
        }

        public void Move(int x, int y, Facing facing)
        {
            X = x;
            Y = y;
            Facing = facing;
        }

        public void ConsumeEnergy(int amount) => Battery -= amount;

        public void AddVisitedCell(int x, int y) => VisitedCells.Add(new Point(x, y));

        public void AddCleanedCell(int x, int y) => CleanedCells.Add(new Point(x, y));

        public void LogBackOffSequence() => BackOffSequenceNTriggered++;

        private SimulationResult GetSimulationResult() => new SimulationResult
        {
            Visited = VisitedCells.ToArray(),
            Cleaned = CleanedCells.ToArray(),
            Final = new FinalPosition(X, Y, Facing),
            Battery = Battery
        };
    }
}
