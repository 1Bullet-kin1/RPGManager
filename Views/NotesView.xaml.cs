using RPGManager.ViewModels;
using System.Windows.Controls;


namespace RPGManager.Views
{
    /// <summary>
    /// Логика взаимодействия для NotesView.xaml
    /// </summary>
    public partial class NotesView : UserControl
    {
        public NotesView()
        {
            InitializeComponent();
            DataContext = new NotesViewModel();
        }
    }
}
