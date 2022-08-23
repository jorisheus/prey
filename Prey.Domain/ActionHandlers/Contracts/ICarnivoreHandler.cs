using Prey.Domain.Contracts;

namespace Prey.Domain.ActionHandlers.Contracts
{
    public interface ICarnivoreHandler : IAnimalHandler
    {
        ActionResult Eat(IEntityState prey);
        bool BreedWith(IEntityState partner);
    }
}