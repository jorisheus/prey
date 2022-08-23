using Prey.Domain.Contracts;
using Prey.Domain.Entities;
using Prey.Domain.Entities.Contracts;
using Prey.Domain.Entities.Descriptions;

namespace Prey.Domain
{
    public delegate void TickEvent(World sender, EventArgs e);
    public class World : IChangeableWorld
    {
        private readonly Point _boundaries;
        private readonly IActionInvoker _actionInvoker;
        private readonly IConfiguration _configuration;
        private static readonly Random Rnd = new Random();
        private IDictionary<IEntity, EntityState> Entities { get; set; }

        private readonly List<EntityState>[,] _positions;

        private readonly EntityState[,] _surface;
        private IList<EntityState> _removalList;
        private IList<EntityState> _addList;
        private int _ticks;

        public event TickEvent OnTicked;



        public int Ticks => _ticks;

        public IConfiguration Configuration => _configuration;

        public Point Boundaries => _boundaries;

        public World(Point boundaries, IActionInvoker actionInvoker, IEnumerable<EntityState> initialEntities, IConfiguration configuration)
        {
            _boundaries = boundaries;
            _actionInvoker = actionInvoker;
            _configuration = configuration;
            _actionInvoker.SetWorld(this);
            _positions = new List<EntityState>[boundaries.X, boundaries.Y];
            _surface = new EntityState[boundaries.X, boundaries.Y];
            InitializeSurfaceAndPositions();

            Entities = new Dictionary<IEntity, EntityState>();
            foreach (var initialEntity in initialEntities)
            {
                initialEntity.Position = GetValidPosition(initialEntity.Position);
                Entities.Add(initialEntity.Entity, initialEntity);
                _positions[initialEntity.Position.X, initialEntity.Position.Y].Add(initialEntity);
            }
        }

        private void InitializeSurfaceAndPositions()
        {
            for (var x = 0; x < _boundaries.X; x++)
            {
                for (var y = 0; y < _boundaries.Y; y++)
                {
                    var surface = new Surface();
                    _surface[x, y] = new EntityState(new Point(x, y), surface, new SurfaceDescription(), Rnd.Next(SurfaceDescription.MaximumEnergy));
                    _positions[x, y] = new List<EntityState>();
                }
            }
        }


        public void Tick()
        {
            _removalList = new List<EntityState>();
            _addList = new List<EntityState>();
            foreach (var state in Entities.Values)
            {
                var originalPosition = state.Position.Clone();
                _actionInvoker.InvokeAction(state);
                EnsurePosition(state, originalPosition);
                state.Age += 1;
            }

            for (var x = 0; x < _boundaries.X; x++)
            {
                for (var y = _ticks%10; y < _boundaries.Y; y+=10)
                {
                    var s = _surface[x, y];
                    s.Age += 1;
                    _actionInvoker.InvokeAction(s);
                }
            }

            foreach (var entityState in _removalList)
            {
                if (Entities.ContainsKey(entityState.Entity))
                {
                    Entities.Remove(entityState.Entity);
                    _positions[entityState.Position.X, entityState.Position.Y].Remove(entityState);
                }
            }

            foreach (var entityState in _addList)
            {
                Entities.Add(entityState.Entity, entityState);
                _positions[GetValidX(entityState.Position), GetValidY(entityState.Position)].Add(entityState);
            }

            _ticks++;

            if(OnTicked != null)
                OnTicked(this, new EventArgs());
        }

        public WorldStats GetStatistics()
        {
            var age = new Dictionary<Type, int>();
            var energy = new Dictionary<Type, int>();
            var count = new Dictionary<Type, int>();
            foreach (var state in Entities.Values)
            {
                var type = state.Entity.GetType();
                if (age.ContainsKey(type)) age[type] += state.Age; else age.Add(type, state.Age);
                if (energy.ContainsKey(type)) energy[type] += state.Energy; else energy.Add(type, state.Energy);
                if (count.ContainsKey(type)) count[type] += 1; else count.Add(type, 1);
            }

            return new WorldStats(age, energy, count);
        }

        private void EnsurePosition(EntityState entityState, Point originalPosition)
        {
            if (entityState.PositionDirty)
            {
                entityState.Position = GetValidPosition(entityState.Position);

                if (entityState.Position != originalPosition)
                {
                    _positions[originalPosition.X, originalPosition.Y].Remove(entityState);
                    _positions[entityState.Position.X, entityState.Position.Y].Add(entityState);
                }

                entityState.PositionDirty = false;
            }
        }

        public Point GetPosition(IEntity entity)
        {
            return Entities[entity].Position;
        }

        public IEnumerable<IEntityState> GetEntityList()
        {
            return Entities.Select(p => p.Value);
        }

        public IEnumerable<KeyValuePair<IEntity, Point>> GetEntities()
        {
            return Entities.Select(p => new KeyValuePair<IEntity, Point>(p.Key, p.Value.Position));
        }

        public IEnumerable<IEntity> DeletedEntities()
        {
            return _removalList.Select(p => p.Entity);
        }

        public IEnumerable<IEntity> AddedEntities()
        {
            return _addList.Select(p => p.Entity);
        }

        public void RemoveEntity(EntityState state)
        {
            _removalList.Add(state);
        }

        public void AddEntity(EntityState entityState)
        {
            _addList.Add(entityState);
        }

        public Surface GetSurfaceAt(Point position)
        {
            return (Surface)_surface[GetValidX(position), GetValidY(position)].Entity;
        }

        public int GetSurfaceEnergyAt(Point position)
        {
            return _surface[GetValidX(position), GetValidY(position)].Energy;
        }

        public void ChangeSurfaceEnergyAt(Point position, int energyChange)
        {
            var state = _surface[GetValidX(position), GetValidY(position)];
            var result = state.Energy + energyChange;
            if (result < 0) result = 0;
            state.Energy = result;
        }

        public IEnumerable<IEntityState> GetEntitiesAt(Point position)
        {
            return _positions[GetValidX(position), GetValidY(position)];
        }

        public IEnumerable<IEntityState> GetNeighbourEntitiesAt(Point position)
        {
            var e = new List<IEntityState>();
            var xValues = GetValidXRange(position.X - 1, position.X + 1);
            var yValues = GetValidYRange(position.Y - 1, position.Y + 1);

            for (var i = 0; i < 3; i++)
                for (var j = 0; j < 3; j++)
                    e.AddRange(_positions[xValues[i], yValues[j]]);

            return e;
        }

        public Point GetValidPosition(Point position)
        {
            return new Point(GetValidX(position), GetValidY(position));
        }

        public int GetValidX(Point position)
        {
            return GetValidX(position.X);
        }

        public int GetValidY(Point position)
        {
            return GetValidY(position.Y);
        }

        public int[] GetValidXRange(int min, int max)
        {
            return Enumerable.Range(min, max - min + 1).Select(GetValidX).ToArray();
        }

        public int[] GetValidYRange(int min, int max)
        {
            return Enumerable.Range(min, max - min + 1).Select(GetValidY).ToArray();
        }

        public int GetValidX(int x)
        {
            return x < 0 ? ((_boundaries.X + x) % _boundaries.X) : (x % _boundaries.X);
        }

        public int GetValidY(int y)
        {
            return y < 0 ? ((_boundaries.Y + y) % _boundaries.Y) : (y % _boundaries.Y);
        }

        public EntityState GetEntityState(IEntity entity)
        {
            return Entities[entity];
        }
    }

    public record WorldStats(
        Dictionary<Type, int> AverageAge,
        Dictionary<Type, int> AverageEnergy,
        Dictionary<Type, int> Count
    );
}