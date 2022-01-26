using System;

namespace MyQ.Shared.Exceptions
{
    public abstract class RobotOperationException : Exception
    {
        public RobotOperationException() { }

        public RobotOperationException(string message) : base(message) { }
    }
}
