using System.Windows;
using System.Windows.Controls;
using RPGManager.Models;
using RPGManager.ViewModels;


namespace RPGManager.Views
{
    public partial class FactionView : UserControl
    {
        public FactionView()
        {
            InitializeComponent();
        }
        private FactionViewModel? viewModel => DataContext as FactionViewModel;

        private void AddFaction_Click(object sender, RoutedEventArgs e)
        {
            if (viewModel != null)
            {
                viewModel.AddFaction();
            }
        }
        private void StartEditing_Click(object sender, RoutedEventArgs e)
        {
            if (viewModel != null)
            {
                viewModel.IsEditMode = true;
            }
        }
        private void CancelEditing_Click(object sender, RoutedEventArgs e)
        {
            if (viewModel != null)
            {
                viewModel.IsEditMode = false;
                viewModel.ReloadFaction();
            }
        }
        private void Save_Click(object sender, RoutedEventArgs e)
        {
            if (viewModel != null)
            {
                viewModel.SaveFaction();
            }
        }
        private void DeleteFaction_Click(object sender, RoutedEventArgs e)
        {
            if (viewModel != null)
            {
                viewModel.DeleteFaction();
            }
        }
        private void AddNpc_Click(object sender, RoutedEventArgs e)
        {
            if (viewModel != null)
            {
                viewModel.AddNpc();
            }
        }
        private void RemoveNpc_Click(object sender, RoutedEventArgs e)
        {
            if (viewModel != null)
            {
                if (sender is Button button && button.Tag is Npc npc)
                {
                    viewModel.RemoveNpc(npc);
                }
            }
        }
        private void AddLocation_Click(object sender, RoutedEventArgs e)
        {
            if (viewModel != null)
            {
                viewModel.AddLocation();

            }
        }
        private void RemoveLocation_Click(object sender, RoutedEventArgs e)
        {
            if (viewModel != null)
            {
                if (sender is Button button && button.Tag is Location location)
                {
                   viewModel.RemoveLocation(location);
                }
            }
        }
    }
}
