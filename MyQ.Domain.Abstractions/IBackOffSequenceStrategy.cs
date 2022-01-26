using MyQ.Enums;

namespace MyQ.Domain.Abstractions
{
    public interface IBackOffSequenceStrategy
    {
        CanExecuteCommandResult TryToBackOff(IRobot robot, IRobotCommand command);
    }
}
