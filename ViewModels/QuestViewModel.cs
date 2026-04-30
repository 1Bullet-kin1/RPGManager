using RPGManager.Data;
using RPGManager.Models;
using System.Collections.ObjectModel;
using Microsoft.EntityFrameworkCore;
using System.Windows;

namespace RPGManager.ViewModels
{
    public class QuestViewModel : BaseViewModel
    {
        private Quest? _selectedQuest;
        public Quest? SelectedQuest
        {
            get => _selectedQuest;
            set
            {
                if (_selectedQuest != value)
                {
                    _selectedQuest = value;
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

        public ObservableCollection<Quest> Quests { get; set; } = new ObservableCollection<Quest>();
        public ObservableCollection<Location> Locations { get; set; } = new ObservableCollection<Location>();
        public ObservableCollection<Npc> Npcs { get; set; } = new ObservableCollection<Npc>();

        public QuestViewModel()
        {
            LoadQuests();
            LoadAllData();
        }

        private void LoadQuests()
        {
            using var db = DbContextFactory.Create();
            var quests = db.Quests
                .Include(q => q.QuestGiver)
                .Include(q => q.Location)
                .ToList();
            Quests = new ObservableCollection<Quest>(quests);
            OnPropertyChanged(nameof(Quests));
        }
        private void LoadAllData()
        {
            using var db = DbContextFactory.Create();
            Npcs = new ObservableCollection<Npc>(db.Npcs.ToList());
            Locations = new ObservableCollection<Location>(db.Locations.ToList());
            OnPropertyChanged(nameof(Npcs));
            OnPropertyChanged(nameof(Locations));
        }

        public void SaveQuest()
        {
            if (SelectedQuest == null) return;
            using var db = DbContextFactory.Create();
            var quest = db.Quests.FirstOrDefault(q => q.Id == SelectedQuest.Id);
            if (quest != null)
            {
                quest.Title = SelectedQuest.Title;
                quest.Description = SelectedQuest.Description;
                quest.Status = SelectedQuest.Status;
                quest.QuestGiver = SelectedQuest.QuestGiver;
                quest.Location = SelectedQuest.Location;
                db.SaveChanges();
                LoadQuests();
                SelectedQuest = Quests.FirstOrDefault(q => q.Id == quest.Id);
                IsEditMode = false;
            }
        }
        public void DeleteQuest()
        {
            if (SelectedQuest == null) return;

            var result = MessageBox.Show(
                $"Удалить квест «{SelectedQuest.Title}»?",
                "Подтверждение удаления",
                MessageBoxButton.YesNo,
                MessageBoxImage.Warning);

            if (result != MessageBoxResult.Yes) return;

            using var db = DbContextFactory.Create();
            var quest = db.Quests.FirstOrDefault(q => q.Id == SelectedQuest.Id);
            if (quest == null) return;

            db.Quests.Remove(quest);
            db.SaveChanges();
            Quests.Remove(SelectedQuest);
            SelectedQuest = Quests.FirstOrDefault();
        }
        public void ReloadQuest()
        {
            if (SelectedQuest == null) return;
            using var db = DbContextFactory.Create();
            var fresh = db.Quests
                .Include(q => q.QuestGiver)
                .Include(q => q.Location)
                .FirstOrDefault(q => q.Id == SelectedQuest.Id);
            if (fresh == null) return;
            var index = Quests.IndexOf(SelectedQuest);
            if (index >= 0) Quests[index] = fresh;
            SelectedQuest = fresh;
            IsEditMode = false;
        }

        public void AddQuest()
        {
            using var db = DbContextFactory.Create();
            var quest = new Quest
            {
                Title = "Новый квест",
                Description = string.Empty,
                Status = "не взят"
            };
            db.Quests.Add(quest);
            db.SaveChanges();
            Quests.Add(quest);
            SelectedQuest = quest;
            IsEditMode = true;
        }
        public void SetStatus(string status)
        {
            if (SelectedQuest == null) return;
            using var db = DbContextFactory.Create();
            var quest = db.Quests.FirstOrDefault(q => q.Id == SelectedQuest.Id);
            if (quest == null) return;
            quest.Status = status;
            db.SaveChanges();
            SelectedQuest.Status = status;
            OnPropertyChanged(nameof(SelectedQuest));
            LoadQuests();
            SelectedQuest = Quests.FirstOrDefault(q => q.Id == quest.Id);
        }
    }
}
