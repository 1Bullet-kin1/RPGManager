using System.Windows;
using System.Windows.Controls;
using RPGManager.Models;
using RPGManager.ViewModels;


namespace RPGManager.Views
{
    public partial class DashboardView : UserControl
    {
        public DashboardView()
        {
            InitializeComponent();
            DataContext = new DashboardViewModel();
        }
        private void NoteClicked(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (sender is FrameworkElement fe &&
                fe.DataContext is Note note &&
                DataContext is DashboardViewModel vm)
            {
                vm.OpenNote(note);
            }
        }

        private void QuestClicked(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (sender is FrameworkElement fe &&
                fe.DataContext is Quest quest &&
                DataContext is DashboardViewModel vm)
            {
                vm.OpenQuest(quest);
            }
        }
    }
}
