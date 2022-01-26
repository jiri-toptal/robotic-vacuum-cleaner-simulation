using MyQ.Domain.Models;

namespace MyQ.Domain.Abstractions
{
    public interface IRobotSimulator
    {
        SimulationResult Simulate(SimulationParameters parameters);
    }
}
