namespace MyQ.Shared.Exceptions
{
    public class UnsupportedRobotCommandException : RobotOperationException
    {
        public UnsupportedRobotCommandException(string message) : base(message) { }
    }
}
