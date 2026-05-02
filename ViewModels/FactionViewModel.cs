using Microsoft.EntityFrameworkCore;
using RPGManager.Data;
using RPGManager.Models;
using System.Collections.ObjectModel;
using System.Windows;

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
                    IsEditMode = false;
                }
            }
        }

        private bool _isEditMode;
        public bool IsEditMode
        {
            get => _isEditMode;
            set { _isEditMode = value; OnPropertyChanged(); }
        }

        private Npc? _npcToAdd;
        public Npc? NpcToAdd
        {
            get => _npcToAdd;
            set { _npcToAdd = value; OnPropertyChanged(); }
        }
        private Location? _locationToAdd;
        public Location? LocationToAdd
        {
            get => _locationToAdd;
            set { _locationToAdd = value; OnPropertyChanged(); }
        }

        public ObservableCollection<Faction> Factions { get; set; } = new();
        public ObservableCollection<Npc> Npcs { get; set; } = new();
        public ObservableCollection<Location> Locations { get; set; } = new();

        public FactionViewModel()
        {
            LoadFactions();
            LoadAllData();
        }

        private void LoadFactions()
        {
            using var db = DbContextFactory.Create();
            var factions = db.Factions
                .Include(f => f.BaseLocation)
                    .ThenInclude(l => l.Region)
                        .ThenInclude(r => r.Continent)
                .Include(f => f.Npcs)
                .Include(f => f.Locations)
                .ToList();
            Factions = new ObservableCollection<Faction>(factions);
            OnPropertyChanged(nameof(Factions));
        }

        private void LoadAllData()
        {
            using var db = DbContextFactory.Create();
            Npcs = new ObservableCollection<Npc>(db.Npcs.ToList());
            Locations = new ObservableCollection<Location>(db.Locations.ToList());
            OnPropertyChanged(nameof(Npcs));
            OnPropertyChanged(nameof(Locations));
        }

        public void AddFaction()
        {
            using var db = DbContextFactory.Create();
            var faction = new Faction
            {
                Name = "Новая фракция",
                Alignment = string.Empty,
                Description = string.Empty
            };
            db.Factions.Add(faction);
            db.SaveChanges();
            Factions.Add(faction);
            SelectedFaction = faction;
            IsEditMode = true;
        }

        public void SaveFaction()
        {
            if (SelectedFaction == null) return;
            using var db = DbContextFactory.Create();
            var faction = db.Factions.FirstOrDefault(f => f.Id == SelectedFaction.Id);
            if (faction == null) return;
            faction.Name = SelectedFaction.Name;
            faction.Alignment = SelectedFaction.Alignment;
            faction.Description = SelectedFaction.Description;
            faction.BaseLocationId = SelectedFaction.BaseLocation?.Id;
            db.SaveChanges();
            LoadFactions();
            SelectedFaction = Factions.FirstOrDefault(f => f.Id == faction.Id);
            IsEditMode = false;
        }

        public void DeleteFaction()
        {
            if (SelectedFaction == null) return;

            var result = MessageBox.Show(
                $"Удалить фракцию «{SelectedFaction.Name}»?",
                "Подтверждение удаления",
                MessageBoxButton.YesNo,
                MessageBoxImage.Warning);

            if (result != MessageBoxResult.Yes) return;

            using var db = DbContextFactory.Create();
            var faction = db.Factions.FirstOrDefault(f => f.Id == SelectedFaction.Id);
            if (faction == null) return;
            db.Factions.Remove(faction);
            db.SaveChanges();
            Factions.Remove(SelectedFaction);
            SelectedFaction = Factions.FirstOrDefault();
        }

        public void ReloadFaction()
        {
            if (SelectedFaction == null) return;
            using var db = DbContextFactory.Create();
            var fresh = db.Factions
                .Include(f => f.BaseLocation)
                    .ThenInclude(l => l.Region)
                        .ThenInclude(r => r.Continent)
                .Include(f => f.Npcs)
                .Include(f => f.Locations)
                .FirstOrDefault(f => f.Id == SelectedFaction.Id);
            if (fresh == null) return;
            var index = Factions.IndexOf(SelectedFaction);
            if (index >= 0) Factions[index] = fresh;
            SelectedFaction = fresh;
            IsEditMode = false;
        }

        public void AddNpc()
        {
            if (SelectedFaction == null || NpcToAdd == null) return;
            if (SelectedFaction.Npcs.Any(n => n.Id == NpcToAdd.Id)) return;

            using var db = DbContextFactory.Create();
            var npc = db.Npcs.FirstOrDefault(n => n.Id == NpcToAdd.Id);
            if (npc == null) return;
            npc.FactionId = SelectedFaction.Id;
            db.SaveChanges();
            SelectedFaction.Npcs.Add(NpcToAdd);
            NpcToAdd = null;
            ReloadFaction();
        }

        public void RemoveNpc(Npc npc)
        {
            if (SelectedFaction == null || npc == null) return;

            using var db = DbContextFactory.Create();
            var dbNpc = db.Npcs.FirstOrDefault(n => n.Id == npc.Id);
            if (dbNpc == null) return;
            dbNpc.FactionId = null;
            db.SaveChanges();
            SelectedFaction.Npcs.Remove(npc);
            ReloadFaction();
        }
        public void AddLocation()
        {
            if (SelectedFaction == null || LocationToAdd == null) return;
            if (SelectedFaction.Locations.Any(l => l.Id == LocationToAdd.Id)) return;

            using var db = DbContextFactory.Create();
            var faction = db.Factions.Include(f => f.Locations)
                                      .FirstOrDefault(f => f.Id == SelectedFaction.Id);
            var location = db.Locations.FirstOrDefault(l => l.Id == LocationToAdd.Id);
            if (faction == null || location == null) return;

            faction.Locations.Add(location);
            db.SaveChanges();
            LocationToAdd = null;
            ReloadFaction();
        }

        public void RemoveLocation(Location location)
        {
            if (SelectedFaction == null || location == null) return;

            using var db = DbContextFactory.Create();
            var faction = db.Factions.Include(f => f.Locations)
                                      .FirstOrDefault(f => f.Id == SelectedFaction.Id);
            var dbLocation = db.Locations.FirstOrDefault(l => l.Id == location.Id);
            if (faction == null || dbLocation == null) return;

            faction.Locations.Remove(dbLocation);
            db.SaveChanges();
            ReloadFaction();
        }
    }
}