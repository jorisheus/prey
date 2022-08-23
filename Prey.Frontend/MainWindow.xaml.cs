using System;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Microsoft.Maui.Graphics;
using Microsoft.Maui.Graphics.Skia;
using NLog;
using NLog.Config;
using Prey.Domain;
using Prey.Domain.Entities;
using Prey.Domain.Entities.Descriptions;
using Point = Prey.Domain.Point;

namespace Prey.Frontend
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private World _world;
        private ListTarget _logTarget;
        private readonly Color[] _brushes;

        public MainWindow()
        {
            InitializeComponent();

            _logTarget = new ListTarget();
            var loggingConfiguration = new LoggingConfiguration();
            loggingConfiguration.AddTarget("list", _logTarget);
            loggingConfiguration.LoggingRules.Add(new LoggingRule("*", LogLevel.Debug, _logTarget));
            LogManager.Configuration = loggingConfiguration;

            _world = WorldFactory.GetDefaultWorld(new Point(100, 100), 200, 30, new Configuration());

            _brushes = new Color[100];
            for (var i = 0; i < 100; i++)
            {
                _brushes[i] = new Color(0.3f, 0.3f + i / 200f, 0.3f, 1f);
            }
            
            BackgroundWorker worker = new BackgroundWorker();
            worker.WorkerReportsProgress = true;
            worker.DoWork += DoWork;
            worker.ProgressChanged += RenderWorldTick;
            worker.RunWorkerAsync();
        }

        private void DoWork(object? sender, DoWorkEventArgs e)
        {
            System.Threading.Thread.Sleep(5000);
            while (true)
            {
                _world.Tick();
                ((sender as BackgroundWorker)!).ReportProgress(0);
                System.Threading.Thread.Sleep(100);
            }
        }

        private void RenderWorldTick(object? sender, ProgressChangedEventArgs e)
        {
            SkElement.InvalidateVisual();
            var stats = _world.GetStatistics();
            Age.Content = "";
            Energy.Content= "";
            Count.Content = "";
            foreach (var count in stats.Count.OrderBy(p=>p.Key.Name))
            {
                Age.Content +=
                    $"{count.Key.Name}: {stats.AverageAge[count.Key] / (count.Value == 0 ? 1 : count.Value)}{Environment.NewLine}";
                Energy.Content += 
                    $"{count.Key.Name}: {stats.AverageEnergy[count.Key] / (count.Value == 0 ? 1 : count.Value)}{Environment.NewLine}";
                Count.Content += 
                    $"{count.Key.Name}: {count.Value}{Environment.NewLine}";
            }

            while (_logTarget.List.Count > 0)
            {
                list.Items.Insert(0, _logTarget.List.Dequeue());
            }
        }
        
        private void SKElement_PaintSurface(object sender, SkiaSharp.Views.Desktop.SKPaintSurfaceEventArgs e)
        {
            ICanvas canvas = new SkiaCanvas() { Canvas = e.Surface.Canvas };

            var entities = _world.GetEntities().ToList();
            
            var width = (float)SkElement.ActualWidth / _world.Boundaries.X;
            var height = (float)SkElement.ActualHeight / _world.Boundaries.Y;

            for (int x = 0; x < _world.Boundaries.X; x++)
            {
                for (int y = 0; y < _world.Boundaries.Y; y++)
                {
                    var energy = _world.GetSurfaceEnergyAt(new Point(x, y));
                    var color = 99 * energy / SurfaceDescription.MaximumEnergy;
                    canvas.FillColor = _brushes[color];
                    canvas.FillRectangle(new RectF(x * width, y * height, width * 1.2f, height * 1.2f));
                }
            }

            var foxBrush = Colors.Black;
            var rabbitBrush = Colors.White;
            
            foreach (var entityPosition in entities)
            {
                canvas.FillColor = entityPosition.Key is Fox ? foxBrush : rabbitBrush;
                canvas.FillRectangle(new RectF(entityPosition.Value.X * width, entityPosition.Value.Y * height, width, height));
            }
        }
    }
}