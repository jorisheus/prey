using Prey.Domain.ActionHandlers;
using Prey.Domain.Contracts;
using Prey.Domain.Entities;
using Prey.Domain.Entities.Contracts;

namespace Prey.Domain
{
    public class ActionInvoker : IActionInvoker
    {
        private IChangeableWorld? _world;

        public void SetWorld(IChangeableWorld world)
        {
            _world = world;
        }

        public void InvokeAction(EntityState entity)
        {
            if (_world == null) return;
            
            if (entity.Entity is IHerbivore)
            {
                new HerbivoreActionHandler(_world).Act(entity);
            }
            else if (entity.Entity is ICarnivore)
            {
                new CarnivoreActionHandler(_world).Act(entity);
            }
            else if (entity.Entity is Surface)
            {
                new SurfaceActionHandler(_world).Act(entity);
            }
            else
            {
                new DefaultActionHandler(_world).Act(entity);
            }
            
        }
    }
}