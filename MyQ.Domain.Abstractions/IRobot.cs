using MyQ.Domain.Models;
using MyQ.Enums;

namespace MyQ.Domain.Abstractions
{
    public interface IRobot
    {
        string[][] Map { get; }

        int X { get; }

        int Y { get; }

        Facing Facing { get; }

        int Battery { get; }

        void Init(RobotInitParameters parameters);

        void Move(int x, int y, Facing facing);

        void ConsumeEnergy(int amount);

        void AddVisitedCell(int x, int y);

        void AddCleanedCell(int x, int y);

        void LogBackOffSequence();

        SimulationResult Run(string[] commands);
    }
}
