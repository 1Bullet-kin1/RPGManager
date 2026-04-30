using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using RPGManager.Data;
using RPGManager.Models;
using System.Collections.ObjectModel;
using System.Windows;

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

        private bool _isEditMode;
        public bool IsEditMode
        {
            get => _isEditMode;
            set
            {
                if (_isEditMode != value)
                {
                    _isEditMode = value;
                    OnPropertyChanged();
                }
            }
        }

        private Npc? _npcToAdd;
        public Npc? NpcToAdd
        {
            get => _npcToAdd;
            set
            {
                if (_npcToAdd != value)
                {
                    _npcToAdd = value;
                    OnPropertyChanged();
                }
            }
        }
        private Faction? _factionToAdd;
        public Faction? FactionToAdd
        {
            get => _factionToAdd;
            set
            {
                if (_factionToAdd != value)
                {
                    _factionToAdd = value;
                    OnPropertyChanged();
                }
            }
        }
        private Quest? _questToAdd;
        public Quest? QuestToAdd
        {
            get => _questToAdd;
            set
            {
                if (_questToAdd != value)
                {
                    _questToAdd = value;
                    OnPropertyChanged();
                }
            }
        }

        public ObservableCollection<Npc> AllNpcs { get; set; } = new ObservableCollection<Npc>();
        public ObservableCollection<Faction> AllFactions { get; set; } = new ObservableCollection<Faction>();
        public ObservableCollection<Quest> AllQuests { get; set; } = new ObservableCollection<Quest>();
        public ObservableCollection<Location> Locations { get; set; } = new ObservableCollection<Location>();

        public LocationViewModel()
        {
            LoadLocations();
            LoadAllData();
        }

        private void LoadLocations()
        {
            try
            {
                using var db = DbContextFactory.Create();
                var locations = db.Locations
                    .Include(l => l.Region)
                        .ThenInclude(r => r.Continent)
                            .ThenInclude(c => c.World)
                    .Include(l => l.Npcs)
                    .Include(l => l.Factions)
                    .Include(l => l.PresentFactions)
                    .Include(l => l.Quests)
                    .ToList();
                Locations = new ObservableCollection<Location>(locations);
                OnPropertyChanged(nameof(Locations));
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(ex.Message, "Ошибка загрузки локаций");
            }
        }

        private void LoadAllData()
        {
            using var db = DbContextFactory.Create();
            var npcs = db.Npcs.ToList();
            AllNpcs = new ObservableCollection<Npc>(npcs);
            OnPropertyChanged(nameof(AllNpcs));
            var factions = db.Factions.ToList();
            AllFactions = new ObservableCollection<Faction>(factions);
            OnPropertyChanged(nameof(AllFactions));
            var quests = db.Quests.ToList();
            AllQuests = new ObservableCollection<Quest>(quests);
            OnPropertyChanged(nameof(AllQuests));
        }

        public void SaveLocation()
        {
            if (SelectedLocation != null)
            {
                using var db = DbContextFactory.Create();
                var loc = db.Locations.FirstOrDefault(l => l.Id == SelectedLocation.Id);
                if (loc != null)
                {
                    loc.Name = SelectedLocation.Name;
                    loc.Description = SelectedLocation.Description;
                    loc.Type = SelectedLocation.Type;
                    db.SaveChanges();
                    LoadLocations();
                    IsEditMode = false;
                }
            }
        }

        public void DeleteLocation()
        {
            if (SelectedLocation != null)
            {
                var result = MessageBox.Show($"Are you sure you want to delete {SelectedLocation.Name}?",
                    "Confirm Delete", MessageBoxButton.YesNo,
                    MessageBoxImage.Warning);
                if (result == MessageBoxResult.Yes)
                {
                    using var db = DbContextFactory.Create();
                    var loc = db.Locations.FirstOrDefault(l => l.Id == SelectedLocation.Id);
                    if (loc != null)
                    {
                        db.Locations.Remove(loc);
                        db.SaveChanges();
                        Locations.Remove(SelectedLocation);
                        LoadLocations();
                        SelectedLocation = null;
                    }
                }
            }
        }
        public void ReloadLocation()
        {
            if (SelectedLocation != null)
            {
                using var db = DbContextFactory.Create();
                var loc = db.Locations
                    .Include(l => l.Region)
                        .ThenInclude(r => r.Continent)
                            .ThenInclude(c => c.World)
                    .Include(l => l.Npcs)
                    .Include(l => l.Factions)
                    .Include(l => l.PresentFactions)
                    .Include(l => l.Quests)
                    .FirstOrDefault(l => l.Id == SelectedLocation.Id);
                if (loc != null)
                {
                    var index = Locations.IndexOf(SelectedLocation);
                    if (index >= 0) { Locations[index] = loc; }
                    SelectedLocation = loc;
                    IsEditMode = false;
                }
            }
        }
        public void AddNpc()
        {
            if (SelectedLocation == null || NpcToAdd == null) return;
            if (SelectedLocation.Npcs.Any(n => n.Id == NpcToAdd.Id)) return;
            using var db = DbContextFactory.Create();
                    var npc = db.Npcs.FirstOrDefault(n => n.Id == NpcToAdd.Id);
                    if (npc != null)
                    {
                        npc.LocationId = SelectedLocation.Id;
                        db.SaveChanges();
                        ReloadLocation();
                        NpcToAdd = null;
                        
                    }    
            
        }
        public void RemoveNpc(Npc npc)
        {
            if (SelectedLocation == null || npc == null) return;
            
                using var db = DbContextFactory.Create();
                var npcToRemove = db.Npcs.FirstOrDefault(n => n.Id == npc.Id);
                if (npcToRemove != null)
                {
                    npcToRemove.LocationId = null;
                    db.SaveChanges();
                    ReloadLocation();
                }
            
        }
        public void AddFaction()
        {
            try
            {
                if (SelectedLocation == null || FactionToAdd == null) return;
                if (SelectedLocation.PresentFactions.Any(f => f.Id == FactionToAdd.Id)) return;
                using var db = DbContextFactory.Create();
                var loc = db.Locations.Include(l => l.PresentFactions).FirstOrDefault(l => l.Id == SelectedLocation.Id);
                var faction = db.Factions.FirstOrDefault(f => f.Id == FactionToAdd.Id);
                if (faction != null && loc != null)
                {
                    loc.PresentFactions.Add(faction);
                    db.SaveChanges();  
                    FactionToAdd = null;
                    ReloadLocation();
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка добавления фракции");
            }
        }
        public void RemoveFaction(Faction faction)
        {
            if (SelectedLocation == null || faction == null) return;
            
                using var db = DbContextFactory.Create();
                var loc = db.Locations.Include(l => l.PresentFactions).FirstOrDefault(l => l.Id == SelectedLocation.Id);
                var factionToRemove = db.Factions.FirstOrDefault(f => f.Id == faction.Id);
                if (factionToRemove != null && loc != null)
                {
                    loc.PresentFactions.Remove(factionToRemove);
                    db.SaveChanges();
                    ReloadLocation();
            }
        }
        
        public void AddQuest()
        {
            if (SelectedLocation == null || QuestToAdd == null) return;
            if (SelectedLocation.Quests.Any(q => q.Id == QuestToAdd.Id)) return;

                using var db = DbContextFactory.Create();
                    var quest = db.Quests.FirstOrDefault(q => q.Id == QuestToAdd.Id);
                    if (quest != null)
                    {
                        quest.LocationId = SelectedLocation.Id;
                        db.SaveChanges();
                        QuestToAdd = null;
                        ReloadLocation();
                    }
                
            
        }
        public void RemoveQuest(Quest quest)
        {
            if (SelectedLocation != null && quest != null)
            {
                using var db = DbContextFactory.Create();
                var questToRemove = db.Quests.FirstOrDefault(q => q.Id == quest.Id);
                if (questToRemove != null)
                {
                    questToRemove.LocationId = null;
                    db.SaveChanges();
                    ReloadLocation();
                }
            }
        }
    }
}
