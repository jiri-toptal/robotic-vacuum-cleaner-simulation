namespace MyQ.Domain.Models
{
    public class SimulationResult
    {
        public Point[] Visited { get; set; }

        public Point[] Cleaned { get; set; }

        public FinalPosition Final { get; set; }

        public int Battery { get; set; }
    }
}
