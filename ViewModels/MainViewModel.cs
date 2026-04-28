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
        public static MainViewModel Instance { get; private set; } = null!;
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
        public BaseViewModel? CurrentViewModel
        {
            get => _currentViewModel;
            set { _currentViewModel = value; OnPropertyChanged(); }
        }

        public MainViewModel()
        {
            Instance = this;
            CurrentPage = NavigationItems[0];
        }
        public void NavigateTo<TViewModel>(Action<TViewModel>? configure = null) where TViewModel : BaseViewModel
        {
            var page = NavigationItems.FirstOrDefault(n => n.ViewModelType == typeof(TViewModel));
            if (page == null) return;

            // Сначала переключаем страницу
            _currentPage = page;
            OnPropertyChanged(nameof(CurrentPage));

            // Создаём VM в зависимости от типа
            var vm = page.ViewModelType == typeof(DashboardViewModel)
                ? new DashboardViewModel(this)
                : Activator.CreateInstance(page.ViewModelType) as BaseViewModel;

            CurrentViewModel = vm;

            // Применяем configure ПОСЛЕ того как VM создана и данные загружены
            if (configure != null && vm is TViewModel typedVm)
                configure(typedVm);
        }
    }
}
