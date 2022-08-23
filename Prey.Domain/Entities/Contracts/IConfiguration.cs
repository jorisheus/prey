using Prey.Domain.Entities.Descriptions;

namespace Prey.Domain.Entities.Contracts
{
    public interface IConfiguration
    {
        FemaleFox FemaleFox { get; set; }
        MaleFox MaleFox { get; set; }
        FemaleRabbit FemaleRabbit { get; set; }
        MaleRabbit MaleRabbit { get; set; }
    }
}