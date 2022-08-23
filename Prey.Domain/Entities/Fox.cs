using Prey.Domain.ActionHandlers.Contracts;
using Prey.Domain.Contracts;
using Prey.Domain.Entities.Contracts;
using Prey.Domain.Entities.Descriptions;
using Prey.Domain.Extensions;

namespace Prey.Domain.Entities
{
    public class Fox : ICarnivore
    {
        private static readonly Random Rnd = new Random();
        private Point _moveDirection = new Point(Rnd.Next(-1, 2), Rnd.Next(-1, 2));
        private int _moveDuration;
        private int _breedingCoolingDown;
        private const int MaxMoveDuration = 30;
        private const int MinBreedingCoolingDown = 300;


        public void Act(ICarnivoreHandler actionHandler, IEntityState state, IAnimalDescription description)
        {
            _breedingCoolingDown++;

            //Breeding need
            var neighbours = actionHandler.FindNeighboursOfType<FoxDescription>(3).ToList();
            if (_breedingCoolingDown > MinBreedingCoolingDown && actionHandler.IsBreedingReady(state))
            {

                DistanceResult mate;
                if (description is FemaleFox)
                    mate = neighbours.Where(p => p.Description is MaleFox && actionHandler.IsBreedingReady(p)).Closest(state.Position);
                else
                    mate = neighbours.Where(p => p.Description is FemaleFox && actionHandler.IsBreedingReady(p)).Closest(state.Position);
                if (mate != null)
                {
                    if (mate.Distance < 2)
                    {
                        if (actionHandler.BreedWith(mate.Entity))
                            _breedingCoolingDown = 0;
                    }
                    else
                    {
                        //Move to matey
                        actionHandler.Move(mate.Delta.X, mate.Delta.Y);
                    }
                    return;
                }
            }


            //FOODD
            if (state.Energy < description.MaxEnergyStorage * 0.7)
            {
                var rabbit = actionHandler.FindNeighboursOfType<RabbitDescription>(5).Closest(state.Position);
                if (rabbit != null)
                {
                    if (rabbit.Distance < 2)
                    {
                        //Eat rabbit now something
                        actionHandler.Eat(rabbit.Entity);
                        return;
                    }

                    actionHandler.Move(rabbit.Delta.X, rabbit.Delta.Y);
                    return;
                }
            }

            //Staying close to other foxes
            var closestfriend = neighbours.Closest(state.Position);
            if (closestfriend != null && (closestfriend.Distance >= 2))
            {
                //Found a friend on a piece of land close to me.
                actionHandler.Move(closestfriend.Delta.X / 2, closestfriend.Delta.Y / 2);
                return;
            }

            //Moving for a time in same direction, try changing it.
            if (_moveDuration > MaxMoveDuration)
            {
                _moveDuration = 0;
                _moveDirection = new Point(Rnd.Next(-1, 2), Rnd.Next(-1, 2));
                if (_moveDirection.X == 0 && _moveDirection.Y == 0)
                    _moveDirection.X = Rnd.NextDouble() > 0.5 ? 1 : -1;
            }

            //No rabbit found, no friends; move random
            actionHandler.Move(_moveDirection.X, _moveDirection.Y);
            _moveDuration++;
        }


        public virtual IAnimal CreateBaby()
        {
            return new Fox();
        }
        public Func<IConfiguration, IEntityDescription> GetBabyDescription()
        {
            return p => Rnd.NextDouble() > 0.5 ? p.FemaleFox : (IEntityDescription)p.MaleFox;
        }


    }
}