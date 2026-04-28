using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RPGManager.Models;

namespace RPGManager.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        public List<NavigationItem> NavigationItems { get; } = new()
        {
            new NavigationItem { Title = "Dashboard",  Icon = "🏠", ViewModelType = typeof(DashboardViewModel)  },
            new NavigationItem { Title = "Миры",       Icon = "🌍", ViewModelType = typeof(WorldViewModel)     },
            new NavigationItem { Title = "Локации",    Icon = "📍", ViewModelType = typeof(LocationViewModel)  },
            new NavigationItem { Title = "NPC",        Icon = "👤", ViewModelType = typeof(NpcViewModel)       },
            new NavigationItem { Title = "Фракции",    Icon = "⚔️", ViewModelType = typeof(FactionViewModel)   },
            new NavigationItem { Title = "Квесты",     Icon = "📜", ViewModelType = typeof(QuestViewModel)     },
            new NavigationItem { Title = "Заметки",    Icon = "📝", ViewModelType = typeof(NotesViewModel)     },
        };

       
        private NavigationItem? _currentPage;
        public NavigationItem CurrentPage
        {
            get => _currentPage;
            set
            {
                _currentPage = value;
                OnPropertyChanged();
                CurrentViewModel = value.ViewModelType == typeof(DashboardViewModel)
                    ? new DashboardViewModel(this)
                    : Activator.CreateInstance(value.ViewModelType) as BaseViewModel;
            }
        }

        private BaseViewModel? _currentViewModel;
        public BaseViewModel CurrentViewModel
        {
            get => _currentViewModel;
            set { _currentViewModel = value; OnPropertyChanged(); }
        }

        public MainViewModel()
        {
            CurrentPage = NavigationItems[0];
        }
        public void NavigateTo<TViewModel>(Action<TViewModel>? configure = null) where TViewModel : BaseViewModel
        {
            var page = NavigationItems.FirstOrDefault(n => n.ViewModelType == typeof(TViewModel));
            if (page == null) return;

            CurrentPage = page;

            if (configure != null && CurrentViewModel is TViewModel vm)
                configure(vm);
        }
    }
}
