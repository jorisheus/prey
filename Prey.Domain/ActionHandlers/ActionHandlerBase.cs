
using NLog;
using Prey.Domain.ActionHandlers.Contracts;
using Prey.Domain.Contracts;
using Prey.Domain.Entities;
using Prey.Domain.Entities.Contracts;

namespace Prey.Domain.ActionHandlers
{
    public abstract class ActionHandlerBase : IActionHandler
    {
        private readonly IChangeableWorld _world;
        private EntityState _currentState;
        private readonly Logger _logger = LogManager.GetCurrentClassLogger();

        protected ActionHandlerBase(IChangeableWorld world)
        {
            _world = world;
        }

        protected EntityState CurrentState => _currentState;

        protected IChangeableWorld World => _world;

        protected void BeginEntity(EntityState state)
        {
            _currentState = state;
        }

        protected void EndEntity(EntityState state)
        {
            if (state.Description is IAnimalDescription)
            {
                state.Energy -= ((IAnimalDescription) state.Description).BaseEnergyConsumption;
            }

            if (state.Energy < 0)
            {
                //Died...
                _world.RemoveEntity(state);

                _logger.Info("A {0} died because of starvation.", state.Entity.GetType().Name);
            }
            else if (state.Description is IAnimalDescription)
            {
                //Died...
                if (state.Age > ((IAnimalDescription)state.Description).DyingAge)
                {
                    _world.RemoveEntity(state);
                    _logger.Info("A {0} died because of old age ({1}).", state.Entity.GetType().Name, state.Age);
                }
            }

            _currentState = null;
        }

        public Surface GetCurrentSurface()
        {
            return _world.GetSurfaceAt(_currentState.Position);
        }

        public int GetCurrentSurfaceEnergy()
        {
            return _world.GetSurfaceEnergyAt(_currentState.Position);
        }

        public IEnumerable<IEntityState> GetNeigbours()
        {
            return _world.GetNeighbourEntitiesAt(_currentState.Position);
        }

        public IEnumerable<IEntityState> FindNeighboursOfType<T>(int areaSize) where T : IAnimalDescription
        {
            var rangeX = _world.GetValidXRange(CurrentState.Position.X - areaSize, CurrentState.Position.X + areaSize);
            var rangeY = _world.GetValidYRange(CurrentState.Position.Y - areaSize, CurrentState.Position.Y + areaSize);

            var result = new List<IEntityState>();

            foreach (int x in rangeX)
                foreach (int y in rangeY)
                {
                    var ent = _world.GetEntitiesAt(new Point(x, y));
                    result.AddRange(ent.Where(p => p != CurrentState && p.Description is T));
                }
            return result;
        }
    }
}