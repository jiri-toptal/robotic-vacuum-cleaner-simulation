using MyQ.Domain.Models;

namespace MyQ.Domain.Models
{
    public class RobotInitParameters
    {
        public string[][] Map { get; set; }

        public RobotPosition Start { get; set; }

        public int Battery { get; set; }
    }
}
