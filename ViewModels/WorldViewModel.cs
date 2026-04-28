using Microsoft.EntityFrameworkCore;
using RPGManager.Data;
using RPGManager.Models;
using System.Collections.ObjectModel;

namespace RPGManager.ViewModels
{
    public class WorldViewModel : BaseViewModel
    {
        public ObservableCollection<World> Worlds { get; set; } = new ObservableCollection<World>();

        private World? _selectedWorld;
        public World? SelectedWorld
        {
            get => _selectedWorld;
            set { _selectedWorld = value; OnPropertyChanged(); }
        }

        public WorldViewModel()
        {
            LoadWorlds();
        }

        private void LoadWorlds()
        {
            using var db = DbContextFactory.Create();
            var worlds = db.Worlds
                .Include(w => w.Continents)
                    .ThenInclude(c => c.Regions)
                        .ThenInclude(r => r.Locations)
                            .ThenInclude(l => l.Npcs)
                .Include(w => w.Continents)
                    .ThenInclude(c => c.Regions)
                        .ThenInclude(r => r.Locations)
                            .ThenInclude(l => l.Factions)
                .Include(w => w.Continents)
                    .ThenInclude(c => c.Regions)
                        .ThenInclude(r => r.Locations)
                            .ThenInclude(l => l.Quests)
                .ToList();
            Worlds = new ObservableCollection<World>(worlds);
            OnPropertyChanged(nameof(Worlds));
        }
    }
}
