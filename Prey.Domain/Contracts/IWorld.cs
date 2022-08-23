using Prey.Domain.Entities;
using Prey.Domain.Entities.Contracts;

namespace Prey.Domain.Contracts
{
    public interface IWorld
    {
        void Tick();
        Point GetPosition(IEntity entity);
        IEnumerable<KeyValuePair<IEntity, Point>> GetEntities();
        Surface GetSurfaceAt(Point position);
        int GetSurfaceEnergyAt(Point position);
        IEnumerable<IEntityState> GetNeighbourEntitiesAt(Point position);
        IEnumerable<IEntityState> GetEntitiesAt(Point position);
        Point GetValidPosition(Point position);
        int GetValidX(Point position);
        int GetValidY(Point position);
        int GetValidX(int x);
        int GetValidY(int y);
        int[] GetValidXRange(int min, int max);
        int[] GetValidYRange(int min, int max);
        IConfiguration Configuration { get; }
    }
}