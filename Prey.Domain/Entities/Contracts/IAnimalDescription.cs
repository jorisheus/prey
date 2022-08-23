using Prey.Domain.Entities.Descriptions;

namespace Prey.Domain.Entities.Contracts
{
    public interface IAnimalDescription : IEntityDescription
    {
        int DyingAge { get; }
        int FertileStartAge { get; }
        int FertileEndAge { get; }
        int BreedingEnergy { get; }
        int MovingEnergy { get; }
        int BaseEnergyConsumption { get; }
        double MaxMovingDistance { get; }
        Sex Sex { get; }
    }
}