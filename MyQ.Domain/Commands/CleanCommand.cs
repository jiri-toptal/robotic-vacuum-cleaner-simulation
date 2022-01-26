using MyQ.Domain.Abstractions;
using MyQ.Enums;

namespace MyQ.Domain.Commands
{
    public class CleanCommand : IRobotCommand
    {
        public string Name => CommandType.Clean;

        public int EnergyConsumption => 5;

        public CanExecuteCommandResult CanExecuteCommand(IRobot robot)
        {
            if (robot.Battery < EnergyConsumption)
            {
                return CanExecuteCommandResult.LowEnergy;
            }

            return CanExecuteCommandResult.Yes;
        }

        public void Execute(IRobot robot)
        {
            robot.AddCleanedCell(robot.X, robot.Y);
        }
    }
}
