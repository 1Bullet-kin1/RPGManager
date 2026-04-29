using System.Windows.Controls;
using RPGManager.ViewModels;

namespace RPGManager.Views
{

    public partial class LocationView : UserControl
    {
        public LocationView()
        {
            InitializeComponent();

        }
        private LocationViewModel? _viewModel => DataContext as LocationViewModel;
        private void StartEditing_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (_viewModel != null)
            {
                _viewModel.IsEditMode = true;
            }
        }

        private void CancelEditing_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (_viewModel != null)
            {
                _viewModel.IsEditMode = false;
            }
        }

        private void Save_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (_viewModel != null)
            {
                _viewModel.SaveLocation();
                _viewModel.IsEditMode = false;
            }
        }
        private void DeleteLocation_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (_viewModel != null)
            {
                _viewModel.DeleteLocation();
            }
        }
        private void RemoveNpc_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (sender is Button button && button.Tag is Models.Npc npc && _viewModel != null)
            {
                _viewModel.RemoveNpc(npc);
            }
        }
        private void RemoveFaction_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (sender is Button button && button.Tag is Models.Faction faction && _viewModel != null)
            {
                _viewModel.RemoveFaction(faction);
            }
        }
        private void RemoveQuest_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (sender is Button button && button.Tag is Models.Quest quest && _viewModel != null)
            {
                _viewModel.RemoveQuest(quest);
            }
        }
        private void AddNpc_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (_viewModel != null)
            {
                _viewModel.AddNpc();
            }
        }
        private void AddFaction_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (_viewModel != null)
            {
                _viewModel.AddFaction();
            }
        }
        private void AddQuest_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (_viewModel != null)
            {
                _viewModel.AddQuest();
            }
        }
    }
}
