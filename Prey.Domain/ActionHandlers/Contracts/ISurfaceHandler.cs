namespace Prey.Domain.ActionHandlers.Contracts
{
    public interface ISurfaceHandler : IActionHandler
    {
        ActionResult SunEnergy();
        ActionResult Regenerate();
    }
}