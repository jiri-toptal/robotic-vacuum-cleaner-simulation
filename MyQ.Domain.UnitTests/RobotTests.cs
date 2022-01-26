using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;
using MyQ.Domain.Abstractions;
using MyQ.Domain.IoC;
using MyQ.Domain.Models;
using MyQ.Enums;
using MyQ.Shared.Exceptions;
using NUnit.Framework;
using System.Collections.Generic;

namespace MyQ.Domain.UnitTests
{
    public class RobotTests
    {
        private ServiceProvider _serviceProvider;

        [SetUp]
        public void Setup()
        {
            var serviceCollection = new ServiceCollection()
                .AddDomain()
                .AddRobotCommands();

            _serviceProvider = serviceCollection.BuildServiceProvider();
        }

        [Test]
        public void InvalidRobotInput()
        {
            // Arrange
            var robot = new Robot(null, null, null);
            var robotInitParameters = new RobotInitParameters
            {
                Map = new[]
                {
                    new[] { "S", "S", "S" },
                    new[] { "S", "S", "S" }
                },
                Start = new RobotPosition
                {
                    X = 15,
                    Y = 8
                }
            };

            // Act & Assert
            Assert.Throws<InvalidRobotStartPositionException>(() => robot.Init(robotInitParameters));
        }

        [Test]
        public void RobotOutOfBattery()
        {
            // Arrange
            var loggerMock = new Mock<ILogger<Robot>>();
            var commands = _serviceProvider.GetRequiredService<IEnumerable<IRobotCommand>>();
            var robot = new Robot(loggerMock.Object, commands, null);

            robot.Init(new RobotInitParameters
            {
                Map = new[]
                {
                    new[] { "S", "S", "S", "S" },
                    new[] { "S", "S", "S", "S" }
                },
                Start = new RobotPosition
                {
                    X = 0,
                    Y = 0,
                    Facing = Facing.East
                },
                Battery = 8
            });

            // Act
            robot.Run(new[] { "C", "A", "C", "A" });

            // Assert
            var expectedLastCommandResult = CanExecuteCommandResult.LowEnergy;

            Assert.AreEqual(expectedLastCommandResult, robot.LastCommandResult);
        }

        [Test]
        public void RobotIsStucked()
        {
            // Arrange
            var robotLoggerMock = new Mock<ILogger<Robot>>();
            var backOffStrategyLoggerMock = new Mock<ILogger<BackOffSequenceStrategy>>();
            var commands = _serviceProvider.GetRequiredService<IEnumerable<IRobotCommand>>();
            var backOffStrategy = new BackOffSequenceStrategy(backOffStrategyLoggerMock.Object, commands);
            var robot = new Robot(robotLoggerMock.Object, commands, backOffStrategy);

            robot.Init(new RobotInitParameters
            {
                Map = new[]
                {
                    new[] { "S", "C" },
                    new[] { "C", "C" }
                },
                Start = new RobotPosition
                {
                    X = 0,
                    Y = 0,
                    Facing = Facing.East
                },
                Battery = 1050
            });

            // Act
            robot.Run(new[] { "C", "A", "C", "A" });

            // Assert
            var expectedLastCommandResult = CanExecuteCommandResult.Stucked;

            Assert.AreEqual(expectedLastCommandResult, robot.LastCommandResult);
        }
    }
}