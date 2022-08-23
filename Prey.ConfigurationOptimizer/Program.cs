using NLog;
using NLog.Config;
using NLog.Targets;
using Prey.Domain;

namespace Prey.ConfigurationOptimizer
{
    internal class Program
    {
        private static void Main()
        {
            var tgt = new FileTarget {FileName = @"preyresults.txt"};
            var loggingConfig = new LoggingConfiguration();
            loggingConfig.AddTarget("list", tgt);
            tgt.Layout = "${message}";
            loggingConfig.LoggingRules.Add(new LoggingRule("results", LogLevel.Info, tgt));
            LogManager.Configuration = loggingConfig;

            var logger = LogManager.GetLogger("results");

            var taskArray = new TaskThing[6];
            var configArray = new Configuration[6];
            TryLoadFromFile(configArray);

            int run = 0;
            int noImprovementRuns = 0;
            int bestResult = 0;
            string bestConfig = new Configuration().GetPropertyString();

            while (true)
            {
                run++;
                noImprovementRuns++;
                for (int i = 0; i < taskArray.Length; i++)
                {
                    var r = new ConfigurationOptimizer.Runner(new Point(100, 100), 200, 30, configArray[i], 15000);
                    taskArray[i] = new TaskThing { Configuration = configArray[i], Runner = r };
                }
                Task.WaitAll(taskArray.Select(p => p.Runner.Run()).ToArray());

                foreach (var task in taskArray.OrderByDescending(p=>p.Runner.World.Ticks))
                {
                    Console.WriteLine("RUN {0:0000} | RESULT {1:0000} | {2}", run, task.Runner.World.Ticks, task.Configuration.GetPropertyString());
                }

                var best = taskArray.OrderByDescending(p => p.Runner.World.Ticks).Take(2).ToArray();

                var first = best[0].Configuration;
                var second = best[1].Configuration;
                var avg = first.CreateAverage(second);
                var configLength = first.GetPropertyString().Length;
                var combined = new Configuration(first.GetPropertyString().Remove(configLength / 2) + second.GetPropertyString().Substring(configLength/2));
                var combined2 = new Configuration(second.GetPropertyString().Remove(configLength / 2) + first.GetPropertyString().Substring(configLength/2));

                configArray[0] = combined.CreateWithDistortion(1);
                configArray[1] = combined2.CreateWithDistortion(1);
                configArray[2] = first.CreateWithDistortion(1);
                configArray[3] = second.CreateWithDistortion(1);
                configArray[4] = avg;
                configArray[5] = avg.CreateWithDistortion(1);
                /*configArray[6] = avg.CreateWithDistortion(2);
                configArray[7] = first;
                configArray[8] = second;*/

                if (best[0].Runner.World.Ticks > bestResult)
                {
                    logger.Info("RUN {0:0000} | RESULT {1:0000} | {2}", run, best[0].Runner.World.Ticks, best[0].Configuration.GetPropertyString());
                    bestResult = best[0].Runner.World.Ticks;
                    bestConfig = best[0].Configuration.GetPropertyString();
                    File.AppendAllText("runs.txt", $"{bestConfig}\t{bestResult}\t{DateTime.Now:yyyy-MM-ddTHH:mm:ss}\r\n");

                    var values = best[0].Configuration.GetPropertyInts();
                    for (int i = 0; i < values.Length; i++)
                    {
                        if (values[i] < 2)
                        {
                            Console.WriteLine($"Property[{i}] {best[0].Configuration.GetPropertyNameAt(i)} has low value.");
                        }

                        if (values[i] > 13)
                        {
                            Console.WriteLine($"Property[{i}] {best[0].Configuration.GetPropertyNameAt(i)} has high value.");
                        }
                    }
                    

                    noImprovementRuns = 0;
                }


                Console.WriteLine("Run {0} completed.", run);
                Console.WriteLine($"Best run ({bestResult}): {bestConfig}");


                if (noImprovementRuns > 5)
                {

                    Console.WriteLine("No improvements for 15 turns. Max result {0}. Starting with new batch.", bestResult);
                    //Start over with random results
                    for (int i = 0; i < configArray.Length; i++)
                    {
                        configArray[i] = new Configuration();
                    }
                    bestResult = 0;
                    noImprovementRuns = 0;
                }
            }
            
        }

        private static void TryLoadFromFile(Configuration[] configArray)
        {
            if (File.Exists("runs.txt"))
            {
                var besties = (from line in File.ReadAllLines("runs.txt")
                    let parts = line.Split('\t')
                    orderby parts[1] descending
                    select new {config = parts[0], score = parts[1], date = parts[2]}).ToList();

                if (besties.Count >= configArray.Length)
                {
                    for (int i = 0; i < configArray.Length; i++)
                    {
                        configArray[i] = new Configuration(besties[i].config);
                    }

                    File.Delete("runs.txt");
                    File.WriteAllLines("runs.txt", besties.Select(b => $"{b.config}\t{b.score}\t{b.date}").ToArray());

                    return;
                }
            }

            for (int i = 0; i < configArray.Length; i++)
            {
                configArray[i] = new Configuration();
            }
        }

        public class TaskThing
        {
            public Configuration Configuration { get; set; }
            public ConfigurationOptimizer.Runner Runner { get; set; }

        }
    }
}
