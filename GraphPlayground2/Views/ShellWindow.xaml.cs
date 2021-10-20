using System.Windows.Controls;

using GraphPlayground2.Contracts.Views;
using GraphPlayground2.ViewModels;

using MahApps.Metro.Controls;

namespace GraphPlayground2.Views
{
    public partial class ShellWindow : MetroWindow, IShellWindow
    {
        public ShellWindow(ShellViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;
        }

        public Frame GetNavigationFrame()
            => shellFrame;

        public void ShowWindow()
            => Show();

        public void CloseWindow()
            => Close();
    }
}
