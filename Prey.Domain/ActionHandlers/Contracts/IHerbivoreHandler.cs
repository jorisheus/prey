using Prey.Domain.Contracts;

namespace Prey.Domain.ActionHandlers.Contracts
{
    public interface IHerbivoreHandler : IAnimalHandler
    {
        ActionResult Eat();
        bool BreedWith(IEntityState partner);
    }
}