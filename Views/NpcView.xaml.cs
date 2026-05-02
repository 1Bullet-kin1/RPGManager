using System.Windows;
using System.Windows.Controls;
using RPGManager.ViewModels;

namespace RPGManager.Views
{

    public partial class NpcView : UserControl
    {

        public NpcView()
        {
            InitializeComponent();
        }
        private NpcViewModel? viewModel => DataContext as NpcViewModel;
        private void AddNpc_Click(object sender, RoutedEventArgs e)
        {
            if (viewModel != null)
            {
                viewModel.AddNpc();
            }
        }
        private void StartEditing_Click(object sender, RoutedEventArgs e)
        {
            if (viewModel != null)
            {
                viewModel.IsEditMode = true;
            }
        }
        private void CancelEditing_Click(Object sender, RoutedEventArgs e)
        {
            if (viewModel != null)
            {
                viewModel.IsEditMode = false;
                viewModel.ReloadNpc();
            }
        }
        private void Save_Click(object sender, RoutedEventArgs e)
        {
            if (viewModel != null)
            {
                viewModel.SaveNpc();
            }
        }
        private void DeleteNpc_Click(Object sender, RoutedEventArgs e)
        {
            if (viewModel != null)
            {
                viewModel.DeleteNpc();
            }
        }
        private void TogglePin_Click(object sender, RoutedEventArgs e)
        {
            if (viewModel != null)
            {
                viewModel.TogglePin();

            }
        }
    }
}
