using Prey.Domain.Contracts;

namespace Prey.Domain.Extensions
{
    public static class EntityExtensions
    {
        public static DistanceResult Closest(this IEnumerable<IEntityState> entities, Point position)
        {
            var curDistance = 100000d;
            IEntityState closest = null;
            foreach (var entity in entities)
            {
                var dist = Point.Distance(entity.Position, position);
                if (dist < curDistance)
                {
                    closest = entity;
                    curDistance = dist;
                }
            }

            if (closest != null)
            {
                return new DistanceResult
                {
                    Entity = closest,
                    Distance = curDistance,
                    Delta = new Point(closest.Position.X - position.X, closest.Position.Y - position.Y)

                };
            }
            
            return null;
        }
    }
}