using Prey.Domain.ActionHandlers.Contracts;
using Prey.Domain.Contracts;
using Prey.Domain.Entities.Contracts;
using Prey.Domain.Entities.Descriptions;
using Prey.Domain.Extensions;

namespace Prey.Domain.Entities
{
    public class Rabbit : IHerbivore
    {
        private static readonly Random Rnd = new Random();
        private Point _moveDirection;
        private int _moveDuration;
        private int _breedingCoolingDown;
        private const int MaxMoveDuration = 20;
        private const int MinBreedingCoolingDown = 150;

        public void Act(IHerbivoreHandler actionHandler, IEntityState state, IAnimalDescription description)
        {
            _breedingCoolingDown++;
            _moveDuration++;

            //Fleeingneed
            var nearestFox = actionHandler.FindNeighboursOfType<FoxDescription>(1).Closest(state.Position);
            if (nearestFox != null)
            {
                //Move away from closest fox
                if (nearestFox.Distance < 0.001)
                    actionHandler.Move(Rnd.NextDouble() > 0.5 ? 1 : -1, Rnd.NextDouble() > 0.5 ? 1 : -1);
                else
                    actionHandler.Move(-nearestFox.Delta.X, -nearestFox.Delta.Y);
                return;
            }

            var neighbours = actionHandler.FindNeighboursOfType<RabbitDescription>(2).ToList();
            //Breeding need
            if (_breedingCoolingDown > MinBreedingCoolingDown && actionHandler.IsBreedingReady(state))
            {
                DistanceResult mate;
                if (description is FemaleRabbit)
                    mate = neighbours.Where(p => p.Description is MaleRabbit && actionHandler.IsBreedingReady(p)).Closest(state.Position);
                else
                    mate = neighbours.Where(p => p.Description is FemaleRabbit && actionHandler.IsBreedingReady(p)).Closest(state.Position);
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

            //Eating need
            var surfaceEnergy = actionHandler.GetCurrentSurfaceEnergy();
            if (state.Energy < description.MaxEnergyStorage * 0.7 )
            {
                if (surfaceEnergy > description.MaxFoodConsumptionPerTurn / 10)
                {
                    actionHandler.Eat();
                    return;
                }

                //Search for food.
                MoveRandom(actionHandler);
            }

            //Staying close to other rabbits
            var closestfriend = neighbours.Closest(state.Position);
            if (closestfriend != null && closestfriend.Distance >= 1)
            {
                //Found a friend on a piece of land close to me.
                //If we are on same piece, this land needs to have food,  otherwise move.
                actionHandler.Move(closestfriend.Delta.X / 2, closestfriend.Delta.Y/ 2);
            }
            else
            {
                MoveRandom(actionHandler);
            }
        }

        private void MoveRandom(IHerbivoreHandler actionHandler)
        {
//Move randomly
            if (_moveDuration > MaxMoveDuration)
            {
                _moveDuration = 0;
                _moveDirection = new Point(Rnd.Next(-1, 2), Rnd.Next(-1, 2));
                if (_moveDirection.X == 0 && _moveDirection.Y == 0)
                    _moveDirection.X = Rnd.NextDouble() > 0.5 ? 1 : -1;
            }

            actionHandler.Move(_moveDirection.X, _moveDirection.Y);
        }


        public virtual IAnimal CreateBaby()
        {
            return new Rabbit();
        }
        public Func<IConfiguration, IEntityDescription> GetBabyDescription()
        {
            return p => Rnd.NextDouble() > 0.5 ? p.FemaleRabbit : (IEntityDescription)p.MaleRabbit;
        }
    }
}