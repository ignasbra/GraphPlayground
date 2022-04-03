using System.Windows.Controls;
using GraphPlayground2.ViewModels;

namespace GraphPlayground2.Views
{
    public partial class MainPage : Page
    {
        public MainPage(MainViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;
        }
    }
}
