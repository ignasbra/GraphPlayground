using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Input;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;

namespace GraphPlayground2.ViewModels
{
    public class MainViewModel : ObservableObject
    {

        public MainViewModel()
        {
            Nodes = new ObservableCollection<RectItem>();
            OnLeftCickCommand = new RelayCommand<System.Windows.Point>(x => HandleLeftClick(x));
            OnRightCickCommand = new RelayCommand<System.Windows.Point>(x => HandleRightClick(x));
        }

        public ObservableCollection<RectItem> Nodes { get; set; }

        public ICommand OnLeftCickCommand { get; }

        public ICommand OnRightCickCommand { get; }

        private void HandleLeftClick(System.Windows.Point point) =>
        Nodes.Add(new RectItem(point.X, point.Y, 30, 30));

        private void HandleRightClick(System.Windows.Point point)
        {
            RectItem nodeToRemove = null;
            foreach (var node in Nodes)
            {
                if (point.X < (node.X + node.Width) && point.X > (node.X - node.Width) &&
                    point.Y < (node.Y + node.Width) && point.Y > (node.Y - node.Width))
                {
                    nodeToRemove = node;
                    break;
                }
            }

            if (nodeToRemove != null)
            {
                Nodes.Remove(nodeToRemove);
            }
        }
    }

    public class RectItem
    {
        public double X { get; set; }
        public double Y { get; set; }
        public double Width { get; set; }
        public double Height { get; set; }

        public RectItem(double x, double y, double width, double height)
        {
            X = x;
            Y = y;
            Width = width;
            Height = height;
        }
    }

    public class CoordinateConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var e = (MouseButtonEventArgs)value;
            return e.GetPosition((System.Windows.IInputElement)parameter);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {

            throw new NotImplementedException();
        }
    }

}
