using Microsoft.EntityFrameworkCore;
using RPGManager.Data;
using RPGManager.Models;
using System.Collections.ObjectModel;

namespace RPGManager.ViewModels
{
    public class LocationViewModel : BaseViewModel
    {
        private Location? _selectedLocation;
        public Location? SelectedLocation
        {
            get => _selectedLocation;
            set
            {
                if (_selectedLocation != value)
                {
                    _selectedLocation = value;
                    OnPropertyChanged();
                }
            }
        }
        public ObservableCollection<Location> Locations { get; set; } = new ObservableCollection<Location>();

        public LocationViewModel()
        {
            LoadLocations();
        }

        private void LoadLocations()
        {
            using var db = DbContextFactory.Create();
            var locations = db.Locations
                .Include(l => l.Region)
                    .ThenInclude(r => r.Continent)
                        .ThenInclude(c => c.World)
                .Include(l => l.Npcs)
                .Include(l => l.Factions)
                .Include(l => l.Quests)
                .ToList();
            Locations = new ObservableCollection<Location>(locations);
            OnPropertyChanged(nameof(Locations));
        }
    }
}
