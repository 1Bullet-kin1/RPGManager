using System.Windows;
using System.Windows.Controls;
using RPGManager.ViewModels;



namespace RPGManager.Views
{

    public partial class QuestView : UserControl
    {
        public QuestView()
        {
            InitializeComponent();
        }
        private QuestViewModel? _viewModel => DataContext as QuestViewModel;
       
        private void StartEditing_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (_viewModel != null)
            {
                _viewModel.IsEditMode = true;
            }
        }

        private void AddQuest_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (_viewModel != null)
            {
                _viewModel.AddQuest();
                _viewModel.IsEditMode = true;
            }
        }
        private void Save_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (_viewModel != null)
            {
                _viewModel.SaveQuest();
            }
        }
        private void CancelEditing_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (_viewModel != null)
            {
                _viewModel.ReloadQuest();
            }
        }
        private void DeleteQuest_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (_viewModel != null)
            {
                _viewModel.DeleteQuest();
            }
        }
        private void SetStatusNotTaken_Click(object sender, RoutedEventArgs e)
        {
            if (_viewModel != null)
            {
                _viewModel.SetStatus("не взят");
            }
        }
        private void SetStatusDone_Click(object sender, RoutedEventArgs e)
        {
            if (_viewModel != null)
            {
                _viewModel.SetStatus("завершён");
            }
        }
        private void SetStatusActive_Click(object sender, RoutedEventArgs e)
        {
            if (_viewModel != null)
            {
                _viewModel.SetStatus("активный");
            }
        }
        private void SetStatusFailed_Click(object sender, RoutedEventArgs e)
        {
            if (_viewModel != null)
            {
                _viewModel.SetStatus("провален");
            }
        }
    }
}
