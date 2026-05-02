using Microsoft.EntityFrameworkCore;
using RPGManager.Data;
using RPGManager.Models;
using System.Collections.ObjectModel;
using System.Windows;

namespace RPGManager.ViewModels
{
    public class NpcViewModel : BaseViewModel
    {
        private Npc? _selectedNpc;
        public Npc? SelectedNpc
        {
            get => _selectedNpc;
            set { _selectedNpc = value; OnPropertyChanged(); }
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
        public bool IsPinned
        {
            get
            {
                if (SelectedNpc == null) return false;
                using var db = DbContextFactory.Create();
                return db.PinnedNpcs.Any(p => p.NpcId == SelectedNpc.Id);
            }
        }
        public ObservableCollection<Npc> NpcList { get; set; } = new();
        public ObservableCollection<Faction> AllFactions { get; set; } = new();
        public ObservableCollection<Location> AllLocations { get; set; } = new();

        public NpcViewModel()
        {
            LoadNpcs();
            LoadAllData();
        }

        private void LoadNpcs()
        {
            using var db = DbContextFactory.Create();
            var npcs = db.Npcs
                .Include(n => n.Faction)
                .Include(n => n.Location)
                .Include(n => n.QuestNpcs)
                    .ThenInclude(qn => qn.Quest)
                .Include(n => n.NpcrelationNpcId1Navigations)
                    .ThenInclude(r => r.NpcId2Navigation)
                .Include(n => n.NpcrelationNpcId2Navigations)
                    .ThenInclude(r => r.NpcId1Navigation)
                .Include(n => n.Quests)
                .ToList();
            NpcList = new ObservableCollection<Npc>(npcs);
            OnPropertyChanged(nameof(NpcList));
        }

        private void LoadAllData()
        {
            using var db = DbContextFactory.Create();
            AllFactions = new ObservableCollection<Faction>(db.Factions.ToList());
            AllLocations = new ObservableCollection<Location>(db.Locations.ToList());
            OnPropertyChanged(nameof(AllFactions));
            OnPropertyChanged(nameof(AllLocations));
        }

        public void AddNpc()
        {
            using var db = DbContextFactory.Create();
            var npc = new Npc
            {
                Name = "Новый персонаж",
                Level = 1
            };
            db.Npcs.Add(npc);
            db.SaveChanges();
            NpcList.Add(npc);
            SelectedNpc = npc;
            IsEditMode = true;
        }

        public void SaveNpc()
        {
            if (SelectedNpc == null) return;
            using var db = DbContextFactory.Create();
            var npc = db.Npcs.FirstOrDefault(n => n.Id == SelectedNpc.Id);
            if (npc == null) return;
            npc.Name = SelectedNpc.Name;
            npc.Race = SelectedNpc.Race;
            npc.Class = SelectedNpc.Class;
            npc.Level = SelectedNpc.Level;
            npc.Alignment = SelectedNpc.Alignment;
            npc.Description = SelectedNpc.Description;
            npc.FactionId = SelectedNpc.Faction?.Id;
            npc.LocationId = SelectedNpc.Location?.Id;
            db.SaveChanges();
            ReloadNpc();
            IsEditMode = false;
        }

        public void DeleteNpc()
        {
            if (SelectedNpc == null) return;

            var result = MessageBox.Show(
                $"Удалить персонажа «{SelectedNpc.Name}»?",
                "Подтверждение удаления",
                MessageBoxButton.YesNo,
                MessageBoxImage.Warning);

            if (result != MessageBoxResult.Yes) return;

            using var db = DbContextFactory.Create();
            var npc = db.Npcs.FirstOrDefault(n => n.Id == SelectedNpc.Id);
            if (npc == null) return;
            db.Npcs.Remove(npc);
            db.SaveChanges();
            NpcList.Remove(SelectedNpc);
            SelectedNpc = NpcList.FirstOrDefault();
        }

        public void ReloadNpc()
        {
            if (SelectedNpc == null) return;
            using var db = DbContextFactory.Create();
            var fresh = db.Npcs
                .Include(n => n.Faction)
                .Include(n => n.Location)
                .Include(n => n.QuestNpcs)
                    .ThenInclude(qn => qn.Quest)
                .Include(n => n.NpcrelationNpcId1Navigations)
                    .ThenInclude(r => r.NpcId2Navigation)
                .Include(n => n.NpcrelationNpcId2Navigations)
                    .ThenInclude(r => r.NpcId1Navigation)
                .Include(n => n.Quests)
                .FirstOrDefault(n => n.Id == SelectedNpc.Id);
            if (fresh == null) return;
            var index = NpcList.IndexOf(SelectedNpc);
            if (index >= 0) NpcList[index] = fresh;
            SelectedNpc = fresh;
            IsEditMode = false;
        }
        public void TogglePin()
        {
            if (SelectedNpc == null) return;
            using var db = DbContextFactory.Create();

            var existing = db.PinnedNpcs.FirstOrDefault(p => p.NpcId == SelectedNpc.Id);
            if (existing != null)
            {
                db.PinnedNpcs.Remove(existing);
                db.SaveChanges();
            }
            else
            {
                var pinned = db.PinnedNpcs.OrderBy(p => p.Slot).ToList();
                if (pinned.Count >= 4)
                {
                    MessageBox.Show(
                        "Уже закреплено 4 персонажа — максимум. Открепите одного чтобы добавить нового.",
                        "Нельзя закрепить",
                        MessageBoxButton.OK,
                        MessageBoxImage.Information);
                    return;
                }
                var usedSlots = pinned.Select(p => p.Slot).ToHashSet();
                int freeSlot = Enumerable.Range(0, 4).First(s => !usedSlots.Contains(s));

                db.PinnedNpcs.Add(new PinnedNpc { NpcId = SelectedNpc.Id, Slot = freeSlot });
                db.SaveChanges();
            }

            OnPropertyChanged(nameof(IsPinned));
        }
    }
}