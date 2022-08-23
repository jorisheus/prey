using Prey.Domain.ActionHandlers.Contracts;
using Prey.Domain.Contracts;

namespace Prey.Domain.Entities.Contracts
{
    public interface IHerbivore : IAnimal
    {

        void Act(IHerbivoreHandler actionHandler, IEntityState state, IAnimalDescription description);

    }
}