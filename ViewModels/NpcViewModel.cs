using RPGManager.Data;
using RPGManager.Models;
using System.Collections.ObjectModel;

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

        public ObservableCollection<Npc> NpcList { get; set; } = new();

        public NpcViewModel()
        {
            LoadNpcs();
        }

        private void LoadNpcs()
        {
            using var db = DbContextFactory.Create();
            var npcs = db.Npcs.ToList();
            NpcList = new ObservableCollection<Npc>(npcs);
            OnPropertyChanged(nameof(NpcList));
        }
    }
}