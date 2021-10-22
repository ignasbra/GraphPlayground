using Microsoft.Toolkit.Mvvm.ComponentModel;
using System.Windows.Media;

namespace GraphPlayground2.Models
{
    public class NodeViewModel : ObservableObject, ICanvasObject
    {
        public double X { get; set; }
        public double Y { get; set; }
        public double Width { get; set; }
        public double Height { get; set; }

        private SolidColorBrush color;
        public SolidColorBrush Color
        {
            get => color;
            set => SetProperty(ref color, value);
        }

        public bool IsSelected { get; set; }

        public NodeViewModel(double x, double y, double width, double height)
        {
            X = x;
            Y = y;
            Width = width;
            Height = height;
            Color = Brushes.Black;
        }
    }
}
