using Microsoft.Toolkit.Mvvm.ComponentModel;
using System.Windows.Media;

namespace GraphPlayground2.Models
{
    public class EdgeViewModel : ObservableObject, ICanvasObject
    {
        public double A_X { get; set; }
        public double A_Y { get; set; }
        public double B_X { get; set; }
        public double B_Y { get; set; }
        public int StrokeThickness;
        public double Weight;

        private SolidColorBrush color;
        public SolidColorBrush Color
        {
            get => color;
            set => SetProperty(ref color, value);
        }

        public EdgeViewModel(NodeViewModel a, NodeViewModel b, double weight, int strokeThickness)
        {
            A_X = a.X + 5;
            A_Y = a.Y + 5;
            B_X = b.X + 5;
            B_Y = b.Y + 5;
            Weight = weight;
            StrokeThickness = strokeThickness;
            Color = Brushes.LightBlue;
        }

        public EdgeViewModel(System.Windows.Point a, System.Windows.Point b, double weight, int strokeThickness)
        {
            A_X = a.X;
            A_Y = a.Y;
            B_X = b.X;
            B_Y = b.Y;
            Weight = weight;
            StrokeThickness = strokeThickness;
            Color = Brushes.LightBlue;
        }
    }
}
