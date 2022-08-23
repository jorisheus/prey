using Prey.Domain.ActionHandlers.Contracts;
using Prey.Domain.Contracts;
using Prey.Domain.Entities.Contracts;

namespace Prey.Domain.ActionHandlers
{
    public class CarnivoreActionHandler : AnimalActionHandlerBase, ICarnivoreHandler, IEntityActInvoker
    {
        private ICarnivore _currentCarnivore;

        public CarnivoreActionHandler(IChangeableWorld world)
            : base(world)
        {
        }

        public void Act(EntityState state)
        {
            BeginEntity(state);
            _currentCarnivore = (ICarnivore)state.Entity;
            _currentCarnivore.Act(this, state, AnimalDescription);

            EndEntity(state);
        }

        public ActionResult Eat(IEntityState prey)
        {
            if (prey.Entity is IHerbivore)
            {
                var energy = (int)((Rnd.NextDouble() * 0.4 + 0.5) * prey.Energy);

                if (energy + CurrentState.Energy > CurrentState.Description.MaxEnergyStorage)
                {
                    energy = CurrentState.Description.MaxEnergyStorage - CurrentState.Energy;
                }

                CurrentState.Energy += energy;

                World.RemoveEntity(World.GetEntityState(prey.Entity));
                Logger.Info("{0} eaten by {1} at {2}", prey.Entity.GetType().Name, _currentCarnivore.GetType().Name, CurrentState.Position);
                return new ActionResult(energy);
            }
            return new ActionResult(0);
        }
    }
}