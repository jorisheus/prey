using System.Globalization;
using System.Text;
using Prey.Domain.Entities.Contracts;
using Prey.Domain.Entities.Descriptions;

namespace Prey.Domain
{
    public class Configuration : IConfiguration
    {
        private static readonly Random Rnd = new Random();
        private readonly List<PropertyDescription<Configuration, int>> _propertiesInt = new List<PropertyDescription<Configuration, int>>
        {
            new PropertyDescription<Configuration, int>
            {
                Description = "Fox.DyingAge",
                GetProperty = p=>p.MaleFox.DyingAge,
                SetProperty = (p,v) => { p.MaleFox.DyingAge = v; p.FemaleFox.DyingAge = v; },
                Minimum = 3000, Maximum = 12000
            },
            new PropertyDescription<Configuration, int>
            {
                Description = "Fox.FertileStartAge",
                GetProperty = p=>p.MaleFox.FertileStartAge,
                SetProperty = (p,v) => { p.MaleFox.FertileStartAge = v; p.FemaleFox.FertileStartAge = v; },
                Minimum = 100, Maximum = 200
            },
            new PropertyDescription<Configuration, int>
            {
                Description = "Fox.FertileEndAge",
                GetProperty = p=>p.MaleFox.FertileEndAge,
                SetProperty = (p,v) => { p.MaleFox.FertileEndAge = v; p.FemaleFox.FertileEndAge = v; },
                Minimum = 3000, Maximum = 10000
            },
            new PropertyDescription<Configuration, int>
            {
                Description = "Fox.MaxFoodConsumptionPerTurn",
                GetProperty = p=>p.MaleFox.MaxFoodConsumptionPerTurn,
                SetProperty = (p,v) => { p.MaleFox.MaxFoodConsumptionPerTurn = v; p.FemaleFox.MaxFoodConsumptionPerTurn = v; },
                Minimum = 500, Maximum = 3000
            },
            new PropertyDescription<Configuration, int>
            {
                Description = "Fox.MaxEnergyStorage",
                GetProperty = p=>p.MaleFox.MaxEnergyStorage,
                SetProperty = (p,v) => { p.MaleFox.MaxEnergyStorage = v; p.FemaleFox.MaxEnergyStorage = v; },
                Minimum = 80000, Maximum = 160000
            },
            new PropertyDescription<Configuration, int>
            {
                Description = "MaleFox.BreedingEnergy",
                GetProperty = p=>p.MaleFox.BreedingEnergy,
                SetProperty = (p,v) => { p.MaleFox.BreedingEnergy = v; },
                Minimum = 500, Maximum = 1000
            },
            new PropertyDescription<Configuration, int>
            {
                Description = "FemaleFox.BreedingEnergy",
                GetProperty = p=>p.FemaleFox.BreedingEnergy,
                SetProperty = (p,v) => { p.FemaleFox.BreedingEnergy = v; },
                Minimum = 1000, Maximum = 4000
            },
            new PropertyDescription<Configuration, int>
            {
                Description = "Fox.MovingEnergy",
                GetProperty = p=>p.MaleFox.MovingEnergy,
                SetProperty = (p,v) => { p.MaleFox.MovingEnergy = v; p.FemaleFox.MovingEnergy = v; },
                Minimum = 10, Maximum = 50
            },


            new PropertyDescription<Configuration, int>
            {
                Description = "Rabbit.DyingAge",
                GetProperty = p=>p.MaleRabbit.DyingAge,
                SetProperty = (p,v) => { p.MaleRabbit.DyingAge = v; p.FemaleRabbit.DyingAge = v; },
                Minimum = 600, Maximum = 4000
            },
            new PropertyDescription<Configuration, int>
            {
                Description = "Rabbit.FertileStartAge",
                GetProperty = p=>p.MaleRabbit.FertileStartAge,
                SetProperty = (p,v) => { p.MaleRabbit.FertileStartAge = v; p.FemaleRabbit.FertileStartAge = v; },
                Minimum = 100, Maximum = 400
            },
            new PropertyDescription<Configuration, int>
            {
                Description = "Rabbit.FertileEndAge",
                GetProperty = p=>p.MaleRabbit.FertileEndAge,
                SetProperty = (p,v) => { p.MaleRabbit.FertileEndAge = v; p.FemaleRabbit.FertileEndAge = v; },
                Minimum = 500, Maximum = 1500
            },
            new PropertyDescription<Configuration, int>
            {
                Description = "Rabbit.MaxFoodConsumptionPerTurn",
                GetProperty = p=>p.MaleRabbit.MaxFoodConsumptionPerTurn,
                SetProperty = (p,v) => { p.MaleRabbit.MaxFoodConsumptionPerTurn = v; p.FemaleRabbit.MaxFoodConsumptionPerTurn = v; },
                Minimum = 10, Maximum = 60
            },
            new PropertyDescription<Configuration, int>
            {
                Description = "MaleRabbit.MaxEnergyStorage",
                GetProperty = p=>p.MaleRabbit.MaxEnergyStorage,
                SetProperty = (p,v) => { p.MaleRabbit.MaxEnergyStorage = v; },
                Minimum = 10000, Maximum = 60000
            },
            new PropertyDescription<Configuration, int>
            {
                Description = "FemaleRabbit.MaxEnergyStorage",
                GetProperty = p=>p.FemaleRabbit.MaxEnergyStorage,
                SetProperty = (p,v) => { p.FemaleRabbit.MaxEnergyStorage = v; },
                Minimum = 10000, Maximum = 60000
            },
            new PropertyDescription<Configuration, int>
            {
                Description = "MaleRabbit.BreedingEnergy",
                GetProperty = p=>p.MaleRabbit.BreedingEnergy,
                SetProperty = (p,v) => { p.MaleRabbit.BreedingEnergy = v; },
                Minimum = 200, Maximum = 1000
            },
            new PropertyDescription<Configuration, int>
            {
                Description = "FemaleRabbit.BreedingEnergy",
                GetProperty = p=>p.FemaleRabbit.BreedingEnergy,
                SetProperty = (p,v) => { p.FemaleRabbit.BreedingEnergy = v; },
                Minimum = 250, Maximum = 1000
            },
            new PropertyDescription<Configuration, int>
            {
                Description = "Rabbit.MovingEnergy",
                GetProperty = p=>p.MaleRabbit.MovingEnergy,
                SetProperty = (p,v) => { p.MaleRabbit.MovingEnergy = v; p.FemaleRabbit.MovingEnergy = v; },
                Minimum = 2, Maximum = 4
            },

            new PropertyDescription<Configuration, int>
            {
                Description = "MaleRabbit.BaseEnergyConsumption",
                GetProperty = p=>p.MaleRabbit.BaseEnergyConsumption,
                SetProperty = (p,v) => { p.MaleRabbit.BaseEnergyConsumption = v; },
                Minimum = 1, Maximum = 5
            },
            new PropertyDescription<Configuration, int>
            {
                Description = "FemaleRabbit.BaseEnergyConsumption",
                GetProperty = p=>p.FemaleRabbit.BaseEnergyConsumption,
                SetProperty = (p,v) => { p.FemaleRabbit.BaseEnergyConsumption = v; },
                Minimum = 1, Maximum = 5
            },

            new PropertyDescription<Configuration, int>
            {
                Description = "MaleFox.BaseEnergyConsumption",
                GetProperty = p=>p.MaleFox.BaseEnergyConsumption,
                SetProperty = (p,v) => { p.MaleFox.BaseEnergyConsumption = v;  },
                Minimum = 3, Maximum = 10
            },

            new PropertyDescription<Configuration, int>
            {
                Description = "FemaleFox.BaseEnergyConsumption",
                GetProperty = p=>p.FemaleFox.BaseEnergyConsumption,
                SetProperty = (p,v) => { p.FemaleFox.BaseEnergyConsumption = v;},
                Minimum = 3, Maximum = 10
            }
        };


        private readonly List<PropertyDescription<Configuration, double>> _propertiesDouble = new List<PropertyDescription<Configuration, double>>
        {
            new PropertyDescription<Configuration, double>
            {
                Description = "Fox.MaxMovingDistance",
                GetProperty = p=>p.MaleFox.MaxMovingDistance,
                SetProperty = (p,v) => { p.MaleFox.MaxMovingDistance = v; p.FemaleFox.MaxMovingDistance = v; },
                Minimum = 4, Maximum = 20
            },
            new PropertyDescription<Configuration, double>
            {
                Description = "Rabbit.MaxMovingDistance",
                GetProperty = p=>p.MaleRabbit.MaxMovingDistance,
                SetProperty = (p,v) => { p.MaleRabbit.MaxMovingDistance = v; p.FemaleRabbit.MaxMovingDistance = v; },
                Minimum = 1, Maximum = 4
            }

        };

        public FemaleFox FemaleFox { get; set; }
        public MaleFox MaleFox { get; set; }

        public FemaleRabbit FemaleRabbit { get; set; }
        public MaleRabbit MaleRabbit { get; set; }

        public Configuration()
        {
            FemaleFox = new FemaleFox();
            MaleFox = new MaleFox();
            FemaleRabbit = new FemaleRabbit();
            MaleRabbit = new MaleRabbit();

            foreach (var propertyDescription in _propertiesInt)
            {
                int value = Rnd.Next(0, 16);
                var result = (int)(propertyDescription.Minimum + (propertyDescription.Maximum - propertyDescription.Minimum) * (value / 15d));
                propertyDescription.SetProperty(this, result);
            }

            foreach (var propertyDescription in _propertiesDouble)
            {
                int value = Rnd.Next(0, 16);
                var result = propertyDescription.Minimum + (propertyDescription.Maximum - propertyDescription.Minimum) * (value / 15d);
                propertyDescription.SetProperty(this, result);
            }
        }

        public Configuration(int[] propertyNumbers)
        {
            FemaleFox = new FemaleFox();
            MaleFox = new MaleFox();
            FemaleRabbit = new FemaleRabbit();
            MaleRabbit = new MaleRabbit();

            if (propertyNumbers.Length != _propertiesDouble.Count + _propertiesInt.Count)
                throw new ArgumentOutOfRangeException("propertyNumbers", propertyNumbers.Length, String.Format("propertyNumbers has incorrect length of {0}. Should be {1}", propertyNumbers.Length, _propertiesDouble.Count + _propertiesInt.Count));

            if (propertyNumbers.Any(p => p > 15 || p < 0))
                throw new ArgumentOutOfRangeException("propertyNumbers", propertyNumbers.Length, String.Format("One or more values is above 15 or below 0."));

            var pos = 0;
            foreach (var propertyDescription in _propertiesInt)
            {
                var result = (int)(propertyDescription.Minimum + (propertyDescription.Maximum - propertyDescription.Minimum) * (propertyNumbers[pos] / 15d));
                propertyDescription.SetProperty(this, result);
                pos++;
            }

            foreach (var propertyDescription in _propertiesDouble)
            {
                var result = propertyDescription.Minimum + (propertyDescription.Maximum - propertyDescription.Minimum) * (propertyNumbers[pos] / 15d);
                propertyDescription.SetProperty(this, result);
                pos++;
            }
        }

        public Configuration(string propertystring)
            : this(GetPropertyInts(propertystring))
        {
        }

        public Configuration CreateAverage(Configuration other)
        {
            var spec1 = GetPropertyInts();
            var spec2 = other.GetPropertyInts();
            var specResult = new int[spec1.Length];
            for (var i = 0; i < spec1.Length; i++)
            {
                specResult[i] = (spec1[i] + spec2[i]) / 2;
            }

            return new Configuration(specResult);
        }

        public Configuration CreateWithDistortion(int maxDeviation)
        {
            var spec1 = GetPropertyInts();
            var specResult = new int[spec1.Length];
            for (var i = 0; i < spec1.Length; i++)
            {
                specResult[i] = spec1[i] + Rnd.Next(-maxDeviation / 2, maxDeviation / 2 + 1);
                if (specResult[i] < 0)
                    specResult[i] = 0;
                if (specResult[i] > 15)
                    specResult[i] = 15;
            }

            return new Configuration(specResult);
        }

        public int[] GetPropertyInts()
        {
            var resultArray = new int[_propertiesDouble.Count + _propertiesInt.Count];
            var pos = 0;
            foreach (var propertyDescription in _propertiesInt)
            {
                var value = propertyDescription.GetProperty(this);
                var result = (int)(15 * (value - propertyDescription.Minimum) / (double)(propertyDescription.Maximum - propertyDescription.Minimum));
                resultArray[pos] = result;
                pos++;
            }

            foreach (var propertyDescription in _propertiesDouble)
            {
                var value = propertyDescription.GetProperty(this);
                var result = (int)(15 * (value - propertyDescription.Minimum) / (propertyDescription.Maximum - propertyDescription.Minimum));
                resultArray[pos] = result;
                pos++;
            }
            return resultArray;
        }

        private static int[] GetPropertyInts(string propertystring)
        {
            var resultArray = new int[propertystring.Length];
            var pos = 0;
            foreach (var character in propertystring)
            {
                int value = Convert.ToInt32(character.ToString(CultureInfo.InvariantCulture), 16);
                resultArray[pos] = value;
                pos++;
            }
            return resultArray;
        }

        public string GetPropertyString()
        {
            var sb = new StringBuilder(_propertiesDouble.Count + _propertiesInt.Count);
            var result = GetPropertyInts();
            foreach (var nr in result)
            {
                sb.Append(String.Format("{0:X1}", nr));
            }
            return sb.ToString();
        }

        public string GetPropertyNameAt(int index)
        {
            if (index < _propertiesInt.Count)
            {
                var property = _propertiesInt[index];
                return $"{property.Description} ({property.Minimum}-{property.Maximum})";
            }

            var propertyD = _propertiesDouble[index - _propertiesInt.Count];
            return $"{propertyD.Description} ({propertyD.Minimum}-{propertyD.Maximum})";
        }

        public class PropertyDescription<TConfig, T>
        {
            public T Minimum { get; set; }
            public T Maximum { get; set; }

            public Func<TConfig, T> GetProperty { get; set; }
            public Action<TConfig, T> SetProperty { get; set; }
            public string Description { get; set; }
        }
    }
}