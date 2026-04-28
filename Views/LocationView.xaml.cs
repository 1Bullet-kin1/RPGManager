using System.Windows.Controls;
using RPGManager.ViewModels;

namespace RPGManager.Views
{

    public partial class LocationView : UserControl
    {
        public LocationView()
        {
            InitializeComponent();
            DataContext = new LocationViewModel();
        }
    }
}
