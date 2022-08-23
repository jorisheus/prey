using Prey.Domain.Contracts;
using Prey.Domain.Entities;
using Prey.Domain.Entities.Contracts;

namespace Prey.Domain.ActionHandlers.Contracts
{
    public interface IActionHandler
    {
        Surface GetCurrentSurface();
        int GetCurrentSurfaceEnergy();

        IEnumerable<IEntityState> GetNeigbours();

        IEnumerable<IEntityState> FindNeighboursOfType<T>(int areaSize) where T : IAnimalDescription;
    }
}