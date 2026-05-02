using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using RPGManager.Models;
using RPGManager.ViewModels;


namespace RPGManager.Views
{
    public partial class NotesView : UserControl
    {
        public NotesView()
        {
            InitializeComponent();
        }
        private NotesViewModel? _viewModel => DataContext as NotesViewModel;

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            if (_viewModel != null)
            {
                _viewModel.SaveNote();
            }
        }

        private void AddNote_Click(object sender, RoutedEventArgs e)
        {
            if (_viewModel != null)
            {
                _viewModel.AddNote();
            }
        }
        private void DeleteNote_Click(object sender, RoutedEventArgs e)
        {
            if (_viewModel != null)
            {
                _viewModel.DeleteNote();
            }
        }

        private void StartEditing_Click(object sender, RoutedEventArgs e)
        {
            if (_viewModel != null)
            {
                _viewModel.IsEditMode = true;
            }
        }
        private void CancelEditing_Click(Object sender, RoutedEventArgs e)
        {
            if (_viewModel != null)
            {
                _viewModel.ReloadSelectedNote();
                _viewModel.IsEditMode = false;
            }
        }
        private void LinkedNpc_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (_viewModel != null)
            {
                if (sender is ComboBox cb && cb.SelectedItem is Npc npc && _viewModel.SelectedNote != null)
                {
                    _viewModel.SelectedNote.LinkedId = npc.Id;
                }
            }
        }
        private void LinkedLocation_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (_viewModel != null)
            {
                if (sender is ComboBox cb && cb.SelectedItem is Location loc && _viewModel.SelectedNote != null)
                {
                    _viewModel.SelectedNote.LinkedId = loc.Id;
                }
            }
        }
        private void LinkedQuest_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (_viewModel != null)
            {
                if (sender is ComboBox cb && cb.SelectedItem is Quest quest && _viewModel.SelectedNote != null)
                {
                    _viewModel.SelectedNote.LinkedId = quest.Id;
                }
            }
        }
        private void LinkedFaction_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (_viewModel != null)
            {
                if (sender is ComboBox cb && cb.SelectedItem is Faction faction && _viewModel.SelectedNote != null)
                {
                    _viewModel.SelectedNote.LinkedId = faction.Id;
                }
            }
        }
    }
}
