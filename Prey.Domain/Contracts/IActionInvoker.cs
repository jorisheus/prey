namespace Prey.Domain.Contracts
{
    public interface IActionInvoker
    {
        void InvokeAction(EntityState entity);
        void SetWorld(IChangeableWorld world);
    }
}