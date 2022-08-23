using Prey.Domain.ActionHandlers.Contracts;
using Prey.Domain.Contracts;
using Prey.Domain.Entities.Contracts;
using Prey.Domain.Entities.Descriptions;

namespace Prey.Domain.Entities
{
    public class Surface : IEntity
    {
        private int _coolingDownPeriod;
        public void Act(ISurfaceHandler actionHandler, IEntityState state, SurfaceDescription description)
        {
            if (state.Energy == 0)
            {
                if (_coolingDownPeriod < description.RegeneratePeriod)
                {
                    _coolingDownPeriod++;
                }
                else
                {
                    //Try to regenerate
                    if (actionHandler.Regenerate().EnergyChange > 0)
                        _coolingDownPeriod = 0;    
                }
                
            }
            actionHandler.SunEnergy();
            
        }
    }
}