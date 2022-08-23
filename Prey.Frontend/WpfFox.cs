using System.Windows.Controls;
using Prey.Domain.Entities;
using Prey.Domain.Entities.Contracts;

namespace Prey.Frontend
{
    public class WpfFox : Fox, IWpfAnimal
    {
        public WpfFox(Image presentation)
        {
            Presentation = presentation;
        }

        public Image Presentation { get; set; }
        public override IAnimal CreateBaby()
        {
            return new WpfFox(new Image
            {
                Source = Presentation.Source,
                Width = Presentation.Width,
                Height = Presentation.Height
            });
        }
    }
}