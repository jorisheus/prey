using Prey.Domain.Entities.Contracts;

namespace Prey.Domain.Entities.Descriptions
{
    public class SurfaceDescription : IEntityDescription
    {
        public const int MaximumEnergy = 100;
        public int RegeneratePeriod { get { return 2; } }
        public int MaxFoodConsumptionPerTurn { get { return 7; } }
        public int MaxEnergyStorage { get { return MaximumEnergy; } }
    }
}