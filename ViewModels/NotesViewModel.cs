using Microsoft.EntityFrameworkCore;
using RPGManager.Data;
using RPGManager.Models;
using System.Collections.ObjectModel;

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

        public ObservableCollection<Note> Notes { get; set; } = new();

        public NotesViewModel()
        {
            LoadNotes();
        }

        private void LoadNotes()
        {
            using var db = DbContextFactory.Create();
            var notes = db.Notes.ToList();
            Notes = new ObservableCollection<Note>(notes);
            OnPropertyChanged(nameof(Notes));
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
                    var location = db.Locations
                        .Include(l => l.Region)
                        .FirstOrDefault(l => l.Id == SelectedNote.LinkedId);
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
            db.SaveChanges();
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
        }

        public void DeleteNote()
        {
            if (SelectedNote == null) return;

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
            var note = db.Notes.AsNoTracking().FirstOrDefault(n => n.Id == SelectedNote.Id);
            if (note == null) return;
            SelectedNote.Title = note.Title;
            SelectedNote.Content = note.Content;
            OnPropertyChanged(nameof(SelectedNote));
            IsEditMode = false;
        }
    }
}
