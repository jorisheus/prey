using System.Diagnostics;
using NLog;
using Prey.Domain;
using Prey.Domain.Entities;
using Prey.Domain.Entities.Contracts;
using Prey.Domain.Entities.Descriptions;

namespace Prey.ConfigurationOptimizer
{
    public class Runner
    {
        private readonly int _maxTicks;
        private readonly World _world;
        private static readonly Random Rnd = new Random();
        private readonly Stopwatch _watch;

        public Runner(Point size, int rabbitCount, int foxCount, IConfiguration configuration, int maxTicks)
        {
            _maxTicks = maxTicks;
            _watch = new Stopwatch();

            LogManager.GetCurrentClassLogger().Info("Application started.");

            var initialEntities = new List<EntityState>();
            for (int i = 0; i < rabbitCount; i++)
            {
                RabbitDescription description = Rnd.NextDouble() > 0.5 ? (RabbitDescription)configuration.MaleRabbit : configuration.FemaleRabbit;

                initialEntities.Add(new EntityState(new Point(Rnd.Next(size.X), Rnd.Next(size.Y)), new Rabbit(), description, Rnd.Next((int)(description.MaxEnergyStorage * 0.3), (int)(description.MaxEnergyStorage * 0.7))) { Age = Rnd.Next(0, description.DyingAge / 3) });
            }

            for (int i = 0; i < foxCount; i++)
            {
                FoxDescription description = Rnd.NextDouble() > 0.5 ? (FoxDescription)configuration.MaleFox : configuration.FemaleFox;

                initialEntities.Add(new EntityState(new Point(Rnd.Next(size.X), Rnd.Next(size.Y)), new Fox(), description, Rnd.Next((int)(description.MaxEnergyStorage * 0.3), (int)(description.MaxEnergyStorage * 0.7))) { Age = Rnd.Next(0, description.DyingAge / 3) });
            }

            _world = new World(size, new ActionInvoker(), initialEntities, configuration);
        }

        public World World
        {
            get { return _world; }
        }

        public Stopwatch Watch
        {
            get { return _watch; }
        }

        public Task Run()
        {
            var task = new Task(() =>
            {
                Watch.Start();
                int rabbits, foxes;
                do
                {
                    World.Tick();
                    var stats = World.GetStatistics();

                    var entities = stats.Count;
                    rabbits = entities.ContainsKey(typeof (Rabbit)) ? entities[typeof (Rabbit)] : 0;
                    foxes = entities.ContainsKey(typeof(Fox)) ? entities[typeof(Fox)] : 0;
                    /*Console.WriteLine(@"{0:000000} | {1:hh\:mm\:ss\.fff} | R{2:0000} | F{3:0000}", World.Ticks, Watch.Elapsed,
                        rabbits, foxes);*/

                } while (rabbits > 1 && foxes > 1 && World.Ticks < _maxTicks);
                Watch.Stop();
            });
            task.Start();
            return task;
        }
    }
}