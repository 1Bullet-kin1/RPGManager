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

    public partial class QuestView : UserControl
    {
        public QuestView()
        {
            InitializeComponent();
            DataContext = new QuestViewModel();
        }
    }
}
