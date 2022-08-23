namespace Prey.Domain.ActionHandlers.Contracts
{
    public interface IEntityActInvoker
    {
        void Act(EntityState state);
    }
}