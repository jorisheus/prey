namespace Prey.Domain.ActionHandlers
{
    public class ActionResult
    {
        public ActionResult(int energyChange)
        {
            EnergyChange = energyChange;
        }

        public int EnergyChange { get; private set; }
    }
}