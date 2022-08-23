using Prey.Domain.Contracts;

namespace Prey.Domain.ActionHandlers.Contracts
{
    public interface IAnimalHandler : IActionHandler
    {
        ActionResult Move(int x, int y);
        bool IsBreedingReady(IEntityState entityState);
    }
}