using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using RPGManager.ViewModels;


namespace RPGManager.Views
{
    public partial class NotesView : UserControl
    {
        public NotesView()
        {
            InitializeComponent();
        }
        private NotesViewModel? Vm => DataContext as NotesViewModel;

        private void SaveNote_Click(object sender, System.Windows.RoutedEventArgs e)
            => Vm?.SaveNote();

        private void AddNote_Click(object sender, System.Windows.RoutedEventArgs e)
            => Vm?.AddNote();

        private void DeleteNote_Click(object sender, System.Windows.RoutedEventArgs e)
            => Vm?.DeleteNote();


        private void StartEditing_Click(object sender, MouseButtonEventArgs e)
        {
            if (Vm != null) Vm.IsEditMode = true;
        }

        private void TextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (Vm == null || !Vm.IsEditMode) return;

            if (sender is TextBox tb)
            {
                var focused = FocusManager.GetFocusedElement(FocusManager.GetFocusScope(tb));
                if (focused is TextBox) return;
            }

            var result = MessageBox.Show(
                "Сохранить изменения?",
                "Редактирование заметки",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
                Vm.SaveNote();
            else
                Vm.ReloadSelectedNote();

            Vm.IsEditMode = false;
        }
    }
}
