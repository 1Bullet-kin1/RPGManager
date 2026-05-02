using Microsoft.EntityFrameworkCore;
using RPGManager.Data;
using RPGManager.Models;
using System.Collections.ObjectModel;
using System.Windows;

namespace RPGManager.ViewModels
{
    public class NotesViewModel : BaseViewModel
    {
        private Note? _selectedNote;
        public Note? SelectedNote
        {
            get => _selectedNote;
            set
            {
                _selectedNote = value;
                OnPropertyChanged();
                IsEditMode = false;
                LoadLinkedObject();
            }
        }

        private bool _isEditMode;
        public bool IsEditMode
        {
            get => _isEditMode;
            set { _isEditMode = value; OnPropertyChanged(); }
        }

        private string _linkedObjectName = string.Empty;
        public string LinkedObjectName
        {
            get => _linkedObjectName;
            set { _linkedObjectName = value; OnPropertyChanged(); }
        }

        private string _linkedObjectDescription = string.Empty;
        public string LinkedObjectDescription
        {
            get => _linkedObjectDescription;
            set { _linkedObjectDescription = value; OnPropertyChanged(); }
        }

        public List<string> LinkedTypes { get; } = new()
        {
            "NPC", "Location", "Quest", "Faction"
        };
        public ObservableCollection<Npc> AllNpcs { get; set; } = new();
        public ObservableCollection<Location> AllLocations { get; set; } = new();
        public ObservableCollection<Quest> AllQuests { get; set; } = new();
        public ObservableCollection<Faction> AllFactions { get; set; } = new();

        public ObservableCollection<Note> Notes { get; set; } = new();

        public NotesViewModel()
        {
            LoadNotes();
            LoadAllData();
        }

        private void LoadNotes()
        {
            using var db = DbContextFactory.Create();
            Notes = new ObservableCollection<Note>(db.Notes.ToList());
            OnPropertyChanged(nameof(Notes));
        }

        private void LoadAllData()
        {
            using var db = DbContextFactory.Create();
            AllNpcs = new ObservableCollection<Npc>(db.Npcs.ToList());
            AllLocations = new ObservableCollection<Location>(db.Locations.ToList());
            AllQuests = new ObservableCollection<Quest>(db.Quests.ToList());
            AllFactions = new ObservableCollection<Faction>(db.Factions.ToList());
            OnPropertyChanged(nameof(AllNpcs));
            OnPropertyChanged(nameof(AllLocations));
            OnPropertyChanged(nameof(AllQuests));
            OnPropertyChanged(nameof(AllFactions));
        }

        private void LoadLinkedObject()
        {
            if (SelectedNote == null)
            {
                LinkedObjectName = string.Empty;
                LinkedObjectDescription = string.Empty;
                return;
            }

            using var db = DbContextFactory.Create();

            switch (SelectedNote.LinkedType)
            {
                case "NPC":
                    var npc = db.Npcs.FirstOrDefault(n => n.Id == SelectedNote.LinkedId);
                    LinkedObjectName = npc?.Name ?? "Не найдено";
                    LinkedObjectDescription = npc?.Description ?? string.Empty;
                    break;
                case "Location":
                    var location = db.Locations.FirstOrDefault(l => l.Id == SelectedNote.LinkedId);
                    LinkedObjectName = location?.Name ?? "Не найдено";
                    LinkedObjectDescription = location?.Description ?? string.Empty;
                    break;
                case "Quest":
                    var quest = db.Quests.FirstOrDefault(q => q.Id == SelectedNote.LinkedId);
                    LinkedObjectName = quest?.Title ?? "Не найдено";
                    LinkedObjectDescription = quest?.Description ?? string.Empty;
                    break;
                case "Faction":
                    var faction = db.Factions.FirstOrDefault(f => f.Id == SelectedNote.LinkedId);
                    LinkedObjectName = faction?.Name ?? "Не найдено";
                    LinkedObjectDescription = faction?.Description ?? string.Empty;
                    break;
                default:
                    LinkedObjectName = "Не указан";
                    LinkedObjectDescription = string.Empty;
                    break;
            }
        }

        public void SaveNote()
        {
            if (SelectedNote == null) return;

            using var db = DbContextFactory.Create();
            var note = db.Notes.FirstOrDefault(n => n.Id == SelectedNote.Id);
            if (note == null) return;

            note.Title = SelectedNote.Title;
            note.Content = SelectedNote.Content;
            note.LinkedType = SelectedNote.LinkedType;
            note.LinkedId = SelectedNote.LinkedId;
            note.CreatedAt = DateTime.Now; 
            db.SaveChanges();

            SelectedNote.CreatedAt = note.CreatedAt;
            OnPropertyChanged(nameof(SelectedNote));
            LoadLinkedObject();
            IsEditMode = false;
        }

        public void AddNote()
        {
            using var db = DbContextFactory.Create();
            var note = new Note
            {
                Title = "Новая заметка",
                Content = string.Empty,
                CreatedAt = DateTime.Now,
                LinkedType = null,
                LinkedId = null
            };
            db.Notes.Add(note);
            db.SaveChanges();
            Notes.Add(note);
            SelectedNote = note;
            IsEditMode = true;
        }

        public void DeleteNote()
        {
            if (SelectedNote == null) return;

            var result = MessageBox.Show(
                $"Удалить заметку «{SelectedNote.Title}»?",
                "Подтверждение удаления",
                MessageBoxButton.YesNo,
                MessageBoxImage.Warning);

            if (result != MessageBoxResult.Yes) return;

            using var db = DbContextFactory.Create();
            var note = db.Notes.FirstOrDefault(n => n.Id == SelectedNote.Id);
            if (note == null) return;
            db.Notes.Remove(note);
            db.SaveChanges();
            Notes.Remove(SelectedNote);
            SelectedNote = Notes.FirstOrDefault();
        }

        public void ReloadSelectedNote()
        {
            if (SelectedNote == null) return;
            using var db = DbContextFactory.Create();
            var fresh = db.Notes.FirstOrDefault(n => n.Id == SelectedNote.Id);
            if (fresh == null) return;
            var index = Notes.IndexOf(SelectedNote);
            if (index >= 0) Notes[index] = fresh;
            SelectedNote = fresh;
            IsEditMode = false;
        }
    }
}