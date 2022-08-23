using System.Windows.Shapes;

namespace Prey.Frontend
{
    public class WpfSurface
    {
        public WpfSurface(Rectangle presentation)
        {
            Presentation = presentation;
        }

        public Rectangle Presentation { get; set; }
    }
}