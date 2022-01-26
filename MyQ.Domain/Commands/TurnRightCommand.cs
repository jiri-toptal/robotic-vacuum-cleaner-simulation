using MyQ.Domain.Abstractions;
using MyQ.Enums;

namespace MyQ.Domain.Commands
{
    public class TurnRightCommand : IRobotCommand
    {
        public string Name => CommandType.TurnRight;

        public int EnergyConsumption => 1;

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
            switch (robot.Facing)
            {
                case Facing.North:
                    robot.Move(robot.X, robot.Y, Facing.East);
                    break;
                case Facing.East:
                    robot.Move(robot.X, robot.Y, Facing.South);
                    break;
                case Facing.South:
                    robot.Move(robot.X, robot.Y, Facing.West);
                    break;
                case Facing.West:
                    robot.Move(robot.X, robot.Y, Facing.North);
                    break;
            }
        }
    }
}
