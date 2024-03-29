﻿using Diploma.Domain.Models;
using Diploma.EntityFramework.Services.NotebookProviders;
using Diploma.EntityFramework.Services.NoteProviders;
using Diploma.WPF.Commands;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;

namespace Diploma.WPF.ViewModels
{
    public class EvernoteViewModel : ViewModelBase, INotifyPropertyChanged
    {
        private readonly INotebookService _notebookService;
        private readonly INoteService _noteService;

        private Notebook selectedNotebook;

        public Notebook SelectedNotebook
        {
            get { return selectedNotebook; }
            set
            {
                selectedNotebook = value;
                OnPropertyChanged(nameof(SelectedNotebook));
                GetNotes();
            }

        }
        private Note selectedNote;

        public Note SelectedNote
        {
            get { return selectedNote; }
            set
            {
                selectedNote = value;
                OnPropertyChanged(nameof(SelectedNote));
                SelectedNoteChanged?.Invoke(this, new EventArgs());
            }
        }

        private Visibility isVisible;

        public Visibility IsVisible
        {
            get { return isVisible; }
            set { isVisible = value; OnPropertyChanged(nameof(IsVisible)); }
        }


        public ObservableCollection<Notebook> Notebooks { get; set; }
        public ObservableCollection<Note> Notes { get; set; }

        public NewNotebookCommand NewNotebookCommand { get; set; }
        public NewNoteCommand NewNoteCommand { get; set; }
        public EditCommand EditCommand { get; set; }
        public EndEditCommand EndEditingCommand { get; set; }

        public event PropertyChangedEventHandler? PropertyChanged;
        public event EventHandler SelectedNoteChanged;


        public EvernoteViewModel(INotebookService notebookService, INoteService noteService)
        {
            _notebookService = notebookService;
            _noteService = noteService;

            NewNoteCommand = new NewNoteCommand(this);
            NewNotebookCommand = new NewNotebookCommand(this);
            EditCommand = new EditCommand(this);
            EndEditingCommand = new EndEditCommand(this);

            Notebooks = new ObservableCollection<Notebook>();
            Notes = new ObservableCollection<Note>();

            IsVisible = Visibility.Collapsed;

            GetNotebooks();
        }

        public void CreateNotebook()
        {
            var notebookCounter = _notebookService.GetAllNotebooks().Count();
            Notebook notebook = new Notebook
            {
                Name = $"New notebook {notebookCounter + 1}",
            };

            _notebookService.InsertNotebook(notebook);

            GetNotebooks();
        }

        public void CreateNote(int noteBookID)
        {
            Note newNote = new Note()
            {
                NotebookId = noteBookID,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
                Title =  $"Note for {DateTime.Now}"
            };

            _noteService.InsertNote(newNote);

            GetNotes();
        }

        private void GetNotebooks()
        {
            var notebooks = _notebookService.GetAllNotebooks();

            Notebooks.Clear();
            foreach (var notebook in notebooks)
            {
                Notebooks.Add(notebook);
            }
        }

        private void GetNotes()
        {
            if (SelectedNotebook != null)
            {
                var notes = _noteService.GetAllNotes()
                    .Where(n => n.NotebookId == SelectedNotebook.Id).ToList();

                Notes.Clear();
                foreach (var note in notes)
                {
                    Notes.Add(note);
                }
            }
        }




        public void StartEditing()
        {
            IsVisible = Visibility.Visible;
        }

        public void StopEditing(Notebook notebook)
        {
            IsVisible = Visibility.Collapsed;
            _notebookService.UpdateNotebook(notebook);
            GetNotebooks();
        }

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
