using Prey.Domain.Entities.Contracts;

namespace Prey.Domain.Contracts
{
    public interface IChangeableWorld : IWorld
    {
        void RemoveEntity(EntityState state);
        void ChangeSurfaceEnergyAt(Point position, int energyChange);
        EntityState GetEntityState(IEntity entity);
        void AddEntity(EntityState entityState);
    }
}