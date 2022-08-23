using Prey.Domain.Contracts;

namespace Prey.Domain.Extensions
{
    public class DistanceResult
    {
        public double Distance { get; set; }
        public Point Delta { get; set; }

        public IEntityState Entity { get; set; }
    }
}