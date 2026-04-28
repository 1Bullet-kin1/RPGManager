using Microsoft.EntityFrameworkCore;
using RPGManager.Data;
using RPGManager.Models;
using System.Collections.ObjectModel;

namespace RPGManager.ViewModels
{
    public class FactionViewModel : BaseViewModel
    {
        private Faction? _selectedFaction;
        public Faction? SelectedFaction
        {
            get => _selectedFaction;
            set
            {
                if (_selectedFaction != value)
                {
                    _selectedFaction = value;
                    OnPropertyChanged();
                }
            }
        }
        public ObservableCollection<Faction> Factions { get; set; } = new ObservableCollection<Faction>();
        public FactionViewModel()
        {
            LoadFactions();
        }
        private void LoadFactions()
        {
            using var db = DbContextFactory.Create();
            var factions = db.Factions
                .Include(f => f.BaseLocation)
                    .ThenInclude(l => l.Region)
                        .ThenInclude(r => r.Continent)
                .Include(f => f.Npcs)
                .ToList();
            Factions = new ObservableCollection<Faction>(factions);
            OnPropertyChanged(nameof(Factions));
        }
    }
}
