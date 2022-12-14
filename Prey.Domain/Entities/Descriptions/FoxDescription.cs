using Prey.Domain.Entities.Contracts;

namespace Prey.Domain.Entities.Descriptions
{
    public abstract class FoxDescription : IAnimalDescription
    {
        public int DyingAge { get; set; }
        public int FertileStartAge { get; set; }
        public int FertileEndAge { get; set; }
        public int MaxFoodConsumptionPerTurn { get; set; }
        public int MaxEnergyStorage { get; set; }
        public int BreedingEnergy { get; set; }
        public int MovingEnergy { get; set; }
        public int BaseEnergyConsumption { get; set; }
        public double MaxMovingDistance { get; set; }
        public abstract Sex Sex { get; }
    }

    public class FemaleFox : FoxDescription
    {
        public override Sex Sex => Sex.Female;
    }

    public class MaleFox : FoxDescription
    {
        public override Sex Sex => Sex.Male;
    }
}