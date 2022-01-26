using MyQ.Domain.Abstractions;
using MyQ.Enums;

namespace MyQ.Domain.Commands
{
    public class BackCommand : IRobotCommand
    {
        public string Name => CommandType.Back;

        public int EnergyConsumption => 3;

        public CanExecuteCommandResult CanExecuteCommand(IRobot robot)
        {
            if (robot.Battery < EnergyConsumption)
            {
                return CanExecuteCommandResult.LowEnergy;
            }

            if (robot.Facing == Facing.South && (robot.Y == 0 || robot.Map[robot.Y - 1][robot.X] != "S"))
            {
                return CanExecuteCommandResult.Obstacle;
            }
            if (robot.Facing == Facing.East && (robot.X == 0 || robot.Map[robot.Y][robot.X - 1] != "S"))
            {
                return CanExecuteCommandResult.Obstacle;
            }
            if (robot.Facing == Facing.North && (robot.Y + 1 >= robot.Map.GetLength(0) || robot.Map[robot.Y + 1][robot.X] != "S"))
            {
                return CanExecuteCommandResult.Obstacle;
            }
            if (robot.Facing == Facing.West && (robot.X + 1 >= robot.Map[robot.Y].GetLength(0) || robot.Map[robot.Y][robot.X + 1] != "S"))
            {
                return CanExecuteCommandResult.Obstacle;
            }

            return CanExecuteCommandResult.Yes;
        }

        public void Execute(IRobot robot)
        {
            switch (robot.Facing)
            {
                case Facing.North:
                    robot.Move(robot.X, robot.Y + 1, robot.Facing);
                    break;
                case Facing.East:
                    robot.Move(robot.X - 1, robot.Y, robot.Facing);
                    break;
                case Facing.South:
                    robot.Move(robot.X, robot.Y - 1, robot.Facing);
                    break;
                case Facing.West:
                    robot.Move(robot.X + 1, robot.Y, robot.Facing);
                    break;
            }

            robot.AddVisitedCell(robot.X, robot.Y);
        }
    }
}
