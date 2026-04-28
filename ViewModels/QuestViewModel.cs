using RPGManager.Data;
using RPGManager.Models;
using System.Collections.ObjectModel;
using Microsoft.EntityFrameworkCore;

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

        public ObservableCollection<Quest> Quests { get; set; } = new ObservableCollection<Quest>();

        public QuestViewModel()
        {
            LoadQuests();
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
    }
}
