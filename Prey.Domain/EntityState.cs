using Prey.Domain.Contracts;
using Prey.Domain.Entities.Contracts;

namespace Prey.Domain
{
    public class EntityState : IEntityState
    {
        private readonly IEntity _entity;
        private Point _position;

        public EntityState(Point position, IEntity entity, IEntityDescription description, int energy)
        {
            _entity = entity;
            Energy = energy;
            PositionDirty = true;
            Position = position;
            Description = description;
        }

        public Point Position
        {
            get { return _position; }
            set
            {
                if (_position == value) return;
                PositionDirty = true;
                _position = value;
            }
        }

        public IEntity Entity
        {
            get { return _entity; }
        }

        public bool PositionDirty { get; set; }

        public int Energy { get; set; }

        public int Age { get; set; }
        public IEntityDescription Description { get; set; }
    }
}