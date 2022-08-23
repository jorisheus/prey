using Prey.Domain.ActionHandlers.Contracts;
using Prey.Domain.Contracts;
using Prey.Domain.Entities;
using Prey.Domain.Entities.Descriptions;

namespace Prey.Domain.ActionHandlers
{
    public class SurfaceActionHandler : ActionHandlerBase, ISurfaceHandler, IEntityActInvoker
    {
        private Surface _currentSurface;

        public SurfaceActionHandler(IChangeableWorld world) : base(world)
        {
        }

        public void Act(EntityState state)
        {
            BeginEntity(state);
            _currentSurface = (Surface)state.Entity;
            _currentSurface.Act(this, state, (SurfaceDescription) state.Description);

            EndEntity(state);
        }

        public ActionResult SunEnergy()
        {
            if (CurrentState.Energy > 0)
            {

                CurrentState.Energy += CurrentState.Description.MaxFoodConsumptionPerTurn;

                if (CurrentState.Energy > CurrentState.Description.MaxEnergyStorage)
                    CurrentState.Energy = CurrentState.Description.MaxEnergyStorage;

                return new ActionResult(CurrentState.Description.MaxFoodConsumptionPerTurn);
            }
            

            return new ActionResult(0);
        }

        public ActionResult Regenerate()
        {
            var xValues = World.GetValidXRange(CurrentState.Position.X - 1, CurrentState.Position.X + 1);
            var yValues = World.GetValidYRange(CurrentState.Position.Y - 1, CurrentState.Position.Y + 1);

            var energyfields = 0;
            for (var i = 0; i < 3; i++)
                for (var j = 0; j < 3; j++)
                    energyfields += World.GetSurfaceEnergyAt(new Point(xValues[i], yValues[j])) > SurfaceDescription.MaximumEnergy * 0.2 ? 1 : 0;

            if (energyfields > 1)
            {
                CurrentState.Energy = 1;
                return new ActionResult(CurrentState.Description.MaxFoodConsumptionPerTurn);

            }

            return new ActionResult(0);
        }
    }
}