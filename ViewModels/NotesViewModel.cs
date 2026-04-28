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
                LoadLinkedObject();
            }
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
    }
}