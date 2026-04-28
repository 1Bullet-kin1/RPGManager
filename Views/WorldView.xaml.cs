using RPGManager.ViewModels;
using System.Windows.Controls;


namespace RPGManager.Views
{
    public partial class WorldView : UserControl
    {
        public WorldView()
        {
            InitializeComponent();
            DataContext = new WorldViewModel();
        }
    }
}
