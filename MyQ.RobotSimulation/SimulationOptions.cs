using CommandLine;

namespace MyQ.RobotSimulation
{
    public class SimulationOptions
    {
        [Value(0)]
        public string Source { get; set; }

        [Value(1)]
        public string Result { get; set; }
    }
}
