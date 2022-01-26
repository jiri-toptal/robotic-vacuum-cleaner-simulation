using MyQ.Enums;

namespace MyQ.Domain.Models
{
    public class RobotPosition
    {
        public int X { get; set; }

        public int Y { get; set; }

        public Facing Facing { get; set; }
    }
}
