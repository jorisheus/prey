namespace Prey.Domain.Entities.Contracts
{
    public interface IEntityDescription
    {
        int MaxFoodConsumptionPerTurn { get; }
        int MaxEnergyStorage { get; }
    }
}