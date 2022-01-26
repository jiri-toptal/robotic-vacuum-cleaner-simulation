using MyQ.Enums;

namespace MyQ.Domain.Models
{
    public struct FinalPosition
    {
        public int X { get; set; }

        public int Y { get; set; }

        public Facing Facing { get; set; }

        public FinalPosition(int x, int y, Facing facing)
        {
            X = x;
            Y = y;
            Facing = facing;
        }
    }
}
