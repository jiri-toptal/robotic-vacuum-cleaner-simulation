using MyQ.Enums;

namespace MyQ.Domain.Abstractions
{
    public interface IRobotCommand
    {
        string Name { get; }

        int EnergyConsumption { get; }

        void Execute(IRobot robot);

        CanExecuteCommandResult CanExecuteCommand(IRobot robot);
    }
}
