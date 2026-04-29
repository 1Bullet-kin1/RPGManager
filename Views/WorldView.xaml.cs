using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using RPGManager.Models;
using RPGManager.ViewModels;


namespace RPGManager.Views
{
    public partial class WorldView : UserControl
    {
        public WorldView()
        {
            InitializeComponent();
        }

        private WorldViewModel? _viewModel => DataContext as WorldViewModel;

        private void World_Click(object sender, MouseButtonEventArgs e) 
        {
            if (sender is FrameworkElement fe && fe.Tag is World world && _viewModel != null) 
            {
                _viewModel.SelectedItem = world;
                _viewModel.SelectedWorld = world;
                e.Handled = true;

            }
        
        }

        private void Continent_Click(object sender, MouseButtonEventArgs e)
        {
            if (sender is FrameworkElement fe && fe.Tag is Continent continent && _viewModel != null)
            {
                _viewModel.SelectedItem = continent;
                e.Handled = true;
            }
        }

        private void Region_Click(object sender, MouseButtonEventArgs e)
        {
            if (sender is FrameworkElement fe && fe.Tag is Region region && _viewModel != null)
            {
                _viewModel.SelectedItem = region;
                e.Handled = true;
            }
        }

        private void Location_Click(object sender, MouseButtonEventArgs e)
        {
            if (sender is FrameworkElement fe && fe.Tag is Location location && _viewModel != null)
            {
                _viewModel.SelectedItem = location;
                e.Handled = true;
            }
        }

        private void AddWorld_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (_viewModel != null)
            {
                _viewModel.AddWorld();
            }
        }

        private void AddContinent_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (_viewModel != null)
            {
                _viewModel.AddContinent();
            }
        }

        private void AddRegion_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (_viewModel != null)
            {
                _viewModel.AddRegion();
            }
        }
        private void AddLocation_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (_viewModel != null)
            {
                _viewModel.AddLocation();
            }
        }
        private void Delete_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (_viewModel != null)
            {
                _viewModel.DeleteSelected();
            }
        }

        private void Save_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (_viewModel != null)
            {
                _viewModel.SaveSelected();
            }
        }
      }
}
