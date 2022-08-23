using System;
using System.Collections.Generic;
using Prey.Domain;
using Prey.Domain.Entities;
using Prey.Domain.Entities.Contracts;
using Prey.Domain.Entities.Descriptions;

namespace Prey.Frontend;

public static class WorldFactory
{
    private static readonly Random Rnd = new Random();
    public static World GetDefaultWorld(Point worldSize, int rabbitCount, int foxCount, IConfiguration configuration)
    {
        var surface = new Surface[worldSize.X, worldSize.Y];
        for (int x = 0; x < worldSize.X; x++)
        {
            for (int y = 0; y < worldSize.Y; y++)
            {
                var s = new Surface();
                surface[x, y] = s;
            }
        }

        var initialEntities = new List<EntityState>();
        var rabbitBasePoint = new Point(Rnd.Next(worldSize.X), Rnd.Next(worldSize.Y));
        for (int i = 0; i < rabbitCount; i++)
        {
            if (i % 10 == 0)
            {
                rabbitBasePoint = new Point(Rnd.Next(worldSize.X), Rnd.Next(worldSize.Y));
            }

            RabbitDescription description = Rnd.NextDouble() > 0.5 ? (RabbitDescription)configuration.MaleRabbit : configuration.FemaleRabbit;
            initialEntities.Add(new EntityState(new Point(rabbitBasePoint.X + Rnd.Next(-10, 10), rabbitBasePoint.Y + Rnd.Next(-10, 10)), new Rabbit(), description, Rnd.Next(200, 600)) { Age = Rnd.Next(0, description.DyingAge / 3) });
        }

        var foxBasePoint = new Point(Rnd.Next(worldSize.X), Rnd.Next(worldSize.Y));
        for (int i = 0; i < foxCount; i++)
        {
            FoxDescription description = Rnd.NextDouble() > 0.5 ? (FoxDescription)configuration.MaleFox : configuration.FemaleFox;
            initialEntities.Add(new EntityState(new Point(foxBasePoint.X + Rnd.Next(-10, 10), foxBasePoint.Y + Rnd.Next(-10, 10)), new Fox(), description, Rnd.Next(1600, 2000)) { Age = Rnd.Next(0, description.DyingAge / 3) });
        }

        return new World(worldSize, new ActionInvoker(), initialEntities, configuration);

    }
}