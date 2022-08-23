using Prey.Domain.ActionHandlers.Contracts;
using Prey.Domain.Contracts;

namespace Prey.Domain.Entities.Contracts
{
    public interface ICarnivore : IAnimal
    {

        void Act(ICarnivoreHandler actionHandler, IEntityState state, IAnimalDescription description);

    }
}