using Prey.Domain.ActionHandlers.Contracts;
using Prey.Domain.Contracts;

namespace Prey.Domain.ActionHandlers
{
    public class DefaultActionHandler : ActionHandlerBase, IEntityActInvoker
    {

        public DefaultActionHandler(IChangeableWorld world) : base(world)
        {
        }

        public void Act(EntityState state)
        {
            BeginEntity(state);

            EndEntity(state);
        }


    }
}