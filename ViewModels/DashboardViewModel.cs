using Microsoft.EntityFrameworkCore;
using RPGManager.Models;
using System.Collections.ObjectModel;
using RPGManager.Data;
using System.Runtime.CompilerServices;

namespace RPGManager.ViewModels
{
    public class DashboardViewModel : BaseViewModel
    {
        private readonly MainViewModel _mainViewModel;
        public DashboardViewModel(MainViewModel mainViewModel)
        {
            _mainViewModel = mainViewModel;
            LoadData();
        }

        private int _worldCount;
        public int WorldCount
        {
            get => _worldCount;
            set
            {
                if (_worldCount != value)
                {
                    _worldCount = value;
                    OnPropertyChanged();
                }
            }
        }

        private int _npcCount;
        public int NpcCount
        {
            get => _npcCount;
            set
            {
                if (_npcCount != value)
                {
                    _npcCount = value;
                    OnPropertyChanged();
                }
            }
        }
        private int _questCount;
        public int QuestCount
        {
            get => _questCount;
            set
            {
                if (_questCount != value)
                {
                    _questCount = value;
                    OnPropertyChanged();
                }
            }
        }
        private int _factionCount;
        public int FactionCount
        {
            get => _factionCount;
            set
            {
                if (_factionCount != value)
                {
                    _factionCount = value;
                    OnPropertyChanged();
                }
            }
        }
        private int _locationsCount;
        public int LocationCount
        {
            get => _locationsCount;
            set
            {
                if (_locationsCount != value)
                {
                    _locationsCount = value;
                    OnPropertyChanged();
                }
            }
        }
        private int _noteCount;
        public int NoteCount
        {
            get => _noteCount;
            set
            {
                if (_noteCount != value)
                {
                    _noteCount = value;
                    OnPropertyChanged();
                }
            }
        }

        public ObservableCollection<Quest> ActiveQuests { get; set; } = new ObservableCollection<Quest>();
        public ObservableCollection<Npc?> PinnedSlots { get; set; } = new ObservableCollection<Npc?>();
        public ObservableCollection<Note> RecentNotes { get; set; } = new ObservableCollection<Note>();

        public DashboardViewModel()
        {
            LoadData();
        }

        private void LoadData() {
            using var db = DbContextFactory.Create();

            WorldCount = db.Worlds.Count();
            NpcCount = db.Npcs.Count();
            QuestCount = db.Quests.Count();
            FactionCount = db.Factions.Count();
            LocationCount = db.Locations.Count();
            NoteCount = db.Notes.Count();

            var activeQuests = db.Quests
                .Include(q => q.Location)
                .Where(q => q.Status == "активный")
                .ToList();
            ActiveQuests = new ObservableCollection<Quest>(activeQuests);
            OnPropertyChanged(nameof(ActiveQuests));

            var notes = db.Notes
                .OrderByDescending(n => n.CreatedAt)
                .Take(5)
                .ToList();
            RecentNotes = new ObservableCollection<Note>(notes);
            OnPropertyChanged(nameof(RecentNotes));


            var pinnedNpcs = db.PinnedNpcs
                .Include(p => p.Npc)
                .ToList();

            for (int i = 0; i < 4; i++)
            {
                var pin = pinnedNpcs.FirstOrDefault(p => p.Slot == i);
                PinnedSlots.Add(pin?.Npc);
            }
        }
        public void OpenNote(Note note)
        {
            _mainViewModel.NavigateTo<NotesViewModel>(vm =>
            {
                vm.SelectedNote = vm.Notes.FirstOrDefault(n => n.Id == note.Id);
            });
        }

        public void OpenQuest(Quest quest)
        {
            _mainViewModel.NavigateTo<QuestViewModel>(vm =>
            {
                vm.SelectedQuest = vm.Quests.FirstOrDefault(q => q.Id == quest.Id);
            });
        }
    }
}
