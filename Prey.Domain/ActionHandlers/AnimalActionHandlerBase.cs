using NLog;
using Prey.Domain.Contracts;
using Prey.Domain.Entities.Contracts;

namespace Prey.Domain.ActionHandlers
{
    public class AnimalActionHandlerBase : ActionHandlerBase
    {
        protected static readonly Random Rnd = new Random();
        private readonly Logger _logger = LogManager.GetCurrentClassLogger();

        public AnimalActionHandlerBase(IChangeableWorld world)
            : base(world)
        {
        }

        protected IAnimalDescription AnimalDescription => (IAnimalDescription)CurrentState.Description;

        protected Logger Logger => _logger;

        public ActionResult Move(int x, int y)
        {
            var delta = new Point(x, y);
            var distance = delta.DistanceFromZero();
            if (distance > AnimalDescription.MaxMovingDistance)
            {
                delta.X = (int)(delta.X * AnimalDescription.MaxMovingDistance / distance);
                delta.Y = (int)(delta.Y * AnimalDescription.MaxMovingDistance / distance);
                distance = AnimalDescription.MaxMovingDistance;
            }

            CurrentState.Position += delta;
            CurrentState.Energy -= (int)(distance * AnimalDescription.MovingEnergy);
            return new ActionResult(-(int)distance);
        }

        public bool IsBreedingReady(IEntityState entityState)
        {
            var desc = entityState.Description as IAnimalDescription;
            if (desc != null)
            {
                return entityState.Age > desc.FertileStartAge && entityState.Age < desc.FertileEndAge && entityState.Energy > desc.BreedingEnergy * 1.5;
            }
            return false;
        }

        public bool BreedWith(IEntityState partner)
        {
            if (IsBreedingReady(CurrentState) && IsBreedingReady(partner) && Point.Distance(CurrentState.Position, partner.Position) < 2)
            {
                var partnerDesc = partner.Description as IAnimalDescription;
                if (partnerDesc == null) return false;
                if (partnerDesc.Sex == ((IAnimalDescription)CurrentState.Description).Sex) return false;
                if (partner.Entity.GetType() != CurrentState.Entity.GetType()) return false;

                //Ok, all is right. Make baby.
                var baby = ((IAnimal)CurrentState.Entity).CreateBaby();

                CurrentState.Energy -= ((IAnimalDescription)CurrentState.Description).BreedingEnergy;
                var partnerState = World.GetEntityState(partner.Entity);
                partnerState.Energy -= partnerDesc.BreedingEnergy;

                Logger.Info("A baby {0} was born at {1}", partner.Entity.GetType().Name, CurrentState.Position);

                World.AddEntity(new EntityState(CurrentState.Position, baby, ((IAnimal)CurrentState.Entity).GetBabyDescription()(World.Configuration), Rnd.Next(200, 600)));
                return true;
            }
            return false;
        }
    }
}