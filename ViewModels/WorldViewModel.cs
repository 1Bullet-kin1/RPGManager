using Microsoft.EntityFrameworkCore;
using RPGManager.Data;
using RPGManager.Models;
using System.Collections.ObjectModel;
using System.Windows.Controls;

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

        private bool _isEditMode;
        public bool IsEditMode
        {
            get => _isEditMode;
            set { _isEditMode = value; OnPropertyChanged(); }
        }

        private object? _selectedItem;
        public object? SelectedItem
        {
            get => _selectedItem;
            set
            {
                _selectedItem = value;
                OnPropertyChanged();
                IsEditMode = false;
                OnPropertyChanged(nameof(IsWorldSelected));
                OnPropertyChanged(nameof(IsContinentSelected));
                OnPropertyChanged(nameof(IsRegionSelected));
                OnPropertyChanged(nameof(IsLocationSelected));
                OnPropertyChanged(nameof(CanDelete));
                OnPropertyChanged(nameof(WorldNpcCount));
                OnPropertyChanged(nameof(WorldFactionCount));
                OnPropertyChanged(nameof(WorldQuestCount));
                OnPropertyChanged(nameof(ContinentNpcCount));
                OnPropertyChanged(nameof(ContinentFactionCount));
                OnPropertyChanged(nameof(ContinentQuestCount));
                OnPropertyChanged(nameof(RegionNpcCount));
                OnPropertyChanged(nameof(RegionFactionCount));
                OnPropertyChanged(nameof(RegionQuestCount));
            }
        }

        /// <summary>
        /// Returns the total number of NPCs across all locations in the selected world.
        /// </summary>
        public int WorldNpcCount
        {
            get
            {
                if (SelectedItem is not World w) return 0;
                return w.Continents
                    .SelectMany(c => c.Regions)
                    .SelectMany(r => r.Locations)
                    .SelectMany(l => l.Npcs)
                    .Count();
            }
        }

        /// <summary>
        /// Returns the total number of factions across all locations in the selected world.
        /// </summary>
        public int WorldFactionCount
        {
            get
            {
                if (SelectedItem is not World w) return 0;
                return w.Continents
                    .SelectMany(c => c.Regions)
                    .SelectMany(r => r.Locations)
                    .SelectMany(l => l.PresentFactions)
                    .Distinct()
                    .Count();
            }
        }

        /// <summary>
        /// Returns the total number of quests across all locations in the selected world.
        /// </summary>
        public int WorldQuestCount
        {
            get
            {
                if (SelectedItem is not World w) return 0;
                return w.Continents
                    .SelectMany(c => c.Regions)
                    .SelectMany(r => r.Locations)
                    .SelectMany(l => l.Quests)
                    .Count();
            }
        }

        /// <summary>
        /// Returns the total number of NPCs across all locations in the selected continent.
        /// </summary>
        public int ContinentNpcCount
        {
            get
            {
                if (SelectedItem is not Continent c) return 0;
                return c.Regions
                    .SelectMany(r => r.Locations)
                    .SelectMany(l => l.Npcs)
                    .Count();
            }
        }

        /// <summary>
        /// Returns the total number of factions across all locations in the selected continent.
        /// </summary>
        public int ContinentFactionCount
        {
            get
            {
                if (SelectedItem is not Continent c) return 0;
                return c.Regions
                    .SelectMany(r => r.Locations)
                    .SelectMany(l => l.PresentFactions)
                    .Distinct()
                    .Count();
            }
        }

        /// <summary>
        /// Returns the total number of quests across all locations in the selected continent.
        /// </summary>
        public int ContinentQuestCount
        {
            get
            {
                if (SelectedItem is not Continent c) return 0;
                return c.Regions
                    .SelectMany(r => r.Locations)
                    .SelectMany(l => l.Quests)
                    .Count();
            }
        }

        /// <summary>
        /// Returns the total number of NPCs across all locations in the selected region.
        /// </summary>
        public int RegionNpcCount
        {
            get
            {
                if (SelectedItem is not Region r) return 0;
                return r.Locations
                    .SelectMany(l => l.Npcs)
                    .Count();
            }
        }

        /// <summary>
        /// Returns the total number of factions across all locations in the selected region.
        /// </summary>
        public int RegionFactionCount
        {
            get
            {
                if (SelectedItem is not Region r) return 0;
                return r.Locations
                    .SelectMany(l => l.PresentFactions)
                    .Distinct()
                    .Count();
            }
        }

        /// <summary>
        /// Returns the total number of quests across all locations in the selected region.
        /// </summary>
        public int RegionQuestCount
        {
            get
            {
                if (SelectedItem is not Region r) return 0;
                return r.Locations
                    .SelectMany(l => l.Quests)
                    .Count();
            }
        }

        public bool IsWorldSelected => SelectedItem is World;
        public bool IsContinentSelected => SelectedItem is Continent;
        public bool IsRegionSelected => SelectedItem is Region;
        public bool IsLocationSelected => SelectedItem is Location;
        public bool CanDelete => SelectedItem != null;

        public WorldViewModel()
        {
            LoadWorlds();
        }

        public void ReloadSelection()
        {
            if (SelectedWorld != null)
            {
                LoadWorlds();
                IsEditMode = false;
                OnPropertyChanged();
            }
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
                            .ThenInclude(l => l.PresentFactions)
                .Include(w => w.Continents)
                    .ThenInclude(c => c.Regions)
                        .ThenInclude(r => r.Locations)
                            .ThenInclude(l => l.Quests)
                .ToList();
            Worlds = new ObservableCollection<World>(worlds);
            OnPropertyChanged(nameof(Worlds));
        }

        public void AddWorld()
        {
            var newWorld = new World { Name = "Новый мир", Description = String.Empty };
            using var db = DbContextFactory.Create();
            db.Worlds.Add(newWorld);
            db.SaveChanges();
            Worlds.Add(newWorld);
            SelectedItem = newWorld;
            SelectedWorld = newWorld;
            LoadWorlds();
        }

        public void AddContinent()
        {
            if (SelectedWorld == null) return;
            var newContinent = new Continent { Name = "Новый континент", Description = String.Empty, WorldId = SelectedWorld.Id };
            using var db = DbContextFactory.Create();
            db.Continents.Add(newContinent);
            db.SaveChanges();
            SelectedWorld.Continents.Add(newContinent);
            LoadWorlds();
            SelectedItem = newContinent;

        }

        public void AddRegion()
        {
            if (SelectedItem is not Continent continent) return;
            var newRegion = new Region { Name = "Новый регион", Description = String.Empty, ContinentId = continent.Id };
            using var db = DbContextFactory.Create();
            db.Regions.Add(newRegion);
            db.SaveChanges();
            continent.Regions.Add(newRegion);
            LoadWorlds();
            SelectedItem = newRegion;
        }

        public void AddLocation()
        {
            if (SelectedItem is not Region region) return;
            var newLocation = new Location { Name = "Новое место", Description = String.Empty, RegionId = region.Id };
            using var db = DbContextFactory.Create();
            db.Locations.Add(newLocation);
            db.SaveChanges();
            region.Locations.Add(newLocation);
            LoadWorlds();
            SelectedItem = newLocation;
        }

        public void DeleteSelected()
        {
            if (SelectedItem == null) return;

            var name = SelectedItem switch
            {
                World w => $"мир '{w.Name}'",
                Continent c => $"континент '{c.Name}'",
                Region r => $"регион '{r.Name}'",
                Location l => $"место '{l.Name}'",
                _ => "элемент"
            };

            var type = SelectedItem switch
            {
                World => "мир и все его континенты, регионы и локации",
                Continent => "континент и все его регионы и локации",
                Region => "регион и все его локации",
                Location => "локацию",
                _ => "элемент"
            };

            var resulte = System.Windows.MessageBox.Show(
                $"Вы уверены, что хотите удалить {name}? Это удалит {type} без возможности восстановления.",
                "Подтверждение удаления",
                System.Windows.MessageBoxButton.YesNo,
                System.Windows.MessageBoxImage.Warning
                );

            if (resulte == System.Windows.MessageBoxResult.Yes)
            {

                using var db = DbContextFactory.Create();
                switch (SelectedItem)
                {
                    case World world:
                        var dbWorld = db.Worlds.Include(w => w.Continents)
                            .ThenInclude(c => c.Regions)
                            .ThenInclude(r => r.Locations)
                            .FirstOrDefault(w => w.Id == world.Id);
                        if (dbWorld != null) db.Worlds.Remove(dbWorld);
                        Worlds.Remove(world);
                        SelectedWorld = Worlds.FirstOrDefault();
                        break;

                    case Continent continent:
                        var dbContinent = db.Continents.FirstOrDefault(c => c.Id == continent.Id);
                        if (dbContinent != null) db.Continents.Remove(dbContinent);
                        var parentWorld = Worlds.FirstOrDefault(w => w.Id == continent.WorldId);
                        parentWorld?.Continents.Remove(continent);
                        break;

                    case Region region:
                        var dbRegion = db.Regions.FirstOrDefault(r => r.Id == region.Id);
                        if (dbRegion != null) db.Regions.Remove(dbRegion);
                        foreach (var w in Worlds)
                            foreach (var c in w.Continents)
                                if (c.Id == region.ContinentId) { c.Regions.Remove(region); break; }
                        break;

                    case Location location:
                        var dbLocation = db.Locations.FirstOrDefault(l => l.Id == location.Id);
                        if (dbLocation != null) db.Locations.Remove(dbLocation);
                        foreach (var w in Worlds)
                            foreach (var c in w.Continents)
                                foreach (var r in c.Regions)
                                    if (r.Id == location.RegionId) { r.Locations.Remove(location); break; }
                        break;
                    }
                db.SaveChanges();
                LoadWorlds();
                SelectedItem = null;
                }
            }
        public void SaveSelected()
        {
            if (SelectedItem == null) return;
            using var db = DbContextFactory.Create();
            switch (SelectedItem)
            {
                case World world:
                    var dbWorld = db.Worlds.FirstOrDefault(w => w.Id == world.Id);
                    if (dbWorld != null)
                    {
                        dbWorld.Name = world.Name;
                        dbWorld.Description = world.Description;
                    }
                    break;
                case Continent continent:
                    var dbContinent = db.Continents.FirstOrDefault(c => c.Id == continent.Id);
                    if (dbContinent != null)
                    {
                        dbContinent.Name = continent.Name;
                        dbContinent.Description = continent.Description;
                    }
                    break;
                case Region region:
                    var dbRegion = db.Regions.FirstOrDefault(r => r.Id == region.Id);
                    if (dbRegion != null)
                    {
                        dbRegion.Name = region.Name;
                        dbRegion.Description = region.Description;
                    }
                    break;
                case Location location:
                    var dbLocation = db.Locations.FirstOrDefault(l => l.Id == location.Id);
                    if (dbLocation != null)
                    {
                        dbLocation.Name = location.Name;
                        dbLocation.Description = location.Description;
                        dbLocation.Type = location.Type;
                    }
                    break;
            }
            db.SaveChanges();
        }

        }
    }
