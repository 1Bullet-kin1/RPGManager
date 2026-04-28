using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using RPGManager.ViewModels;

namespace RPGManager.Views
{
    /// <summary>
    /// Логика взаимодействия для NpcView.xaml
    /// </summary>
    public partial class NpcView : UserControl
    {

        public NpcView()
        {
            InitializeComponent();
            DataContext = new ViewModels.NpcViewModel();
        }
    }
}
