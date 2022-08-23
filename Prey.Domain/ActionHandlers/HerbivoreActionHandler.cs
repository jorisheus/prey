using Prey.Domain.ActionHandlers.Contracts;
using Prey.Domain.Contracts;
using Prey.Domain.Entities.Contracts;

namespace Prey.Domain.ActionHandlers
{
    public class HerbivoreActionHandler : AnimalActionHandlerBase, IHerbivoreHandler, IEntityActInvoker
    {
        private IHerbivore _currentHerbivore;
        public HerbivoreActionHandler(IChangeableWorld world)
            : base(world)
        {
        }

        public void Act(EntityState state)
        {
            BeginEntity(state);
            _currentHerbivore = (IHerbivore)state.Entity;
            _currentHerbivore.Act(this, state, AnimalDescription);

            EndEntity(state);
        }

        public ActionResult Eat()
        {
            var surfaceEnergy = GetCurrentSurfaceEnergy();

            var energy = Rnd.Next((int)(CurrentState.Description.MaxFoodConsumptionPerTurn * 0.5), CurrentState.Description.MaxFoodConsumptionPerTurn + 1);
            if (surfaceEnergy < CurrentState.Description.MaxFoodConsumptionPerTurn)
            {
                energy = surfaceEnergy;
            }

            if (CurrentState.Energy + energy > CurrentState.Description.MaxEnergyStorage)
            {
                energy = CurrentState.Description.MaxEnergyStorage - CurrentState.Energy;
            }

            World.ChangeSurfaceEnergyAt(CurrentState.Position, -energy);
            CurrentState.Energy += energy;
            return new ActionResult(energy);
        }

    }
}