using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized; // Potrzebne do wykrywania zmian w liście
using System.Linq;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using NotesApp.Commands;
using NotesApp.Models;
using NotesApp.Services;

namespace NotesApp.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
    // --- 1. ZARZĄDZANIE WIDOKIEM (TABS) ---
    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(IsTaskView))]
    [NotifyPropertyChangedFor(nameof(IsNoteView))]
    private string _currentTab = "Task";

    public bool IsTaskView => CurrentTab == "Task";
    public bool IsNoteView => CurrentTab == "Note";

    // --- 2. LISTA DANYCH ---

    public ObservableCollection<IEntryComponent> FilteredEntries { get; } = new();
    
    // --- FILTROWANIE ---

    [ObservableProperty]
    private string _filterTagsText = string.Empty;

    [ObservableProperty]
    private string _selectedFilterPriority = "Wszystkie";

    public List<string> FilterPriorities { get; } = new() { "Wszystkie", "High", "Normal", "Low" };

    partial void OnFilterTagsTextChanged(string value) => RefreshList();
    partial void OnSelectedFilterPriorityChanged(string value) => RefreshList();
    
    public ObservableCollection<string> ExistingTags { get; } = new();
    
    [ObservableProperty]
    private string? _selectedTagFromList;
    
    partial void OnSelectedTagFromListChanged(string? value)
    {
        if (!string.IsNullOrEmpty(value))
        {
            FilterTagsText = value;
            SelectedTagFromList = null;
        }
    }

    // --- 3. FORMULARZ ---

    [ObservableProperty] 
    private string _newTaskDescription = string.Empty;
    
    [ObservableProperty] 
    private string _newTaskPriority = "Normal";
    
    [ObservableProperty]
    private string _newEntryTags = string.Empty;

    public List<string> AvailablePriorities { get; } = new() { "High", "Normal", "Low" };

    // --- 4. RAPORTY (STATYSTYKI) ---
    [ObservableProperty] private string _taskStatsText = "Zadania: ...";
    [ObservableProperty] private string _noteStatsText = "Notatki: ...";

    // --- 5. KOMENDY ---
    public ICommand MarkAsDoneCommand { get; } = new MarkAsDoneCommand();
    public ICommand RemoveEntryCommand { get; } = new RemoveEntryCommand();

    public MainWindowViewModel()
    {
        var taskObserver = new TaskStatsObserver((text) => TaskStatsText = text);
        var noteObserver = new NoteStatsObserver((text) => NoteStatsText = text);
        AppManager.Instance.Attach(taskObserver);
        AppManager.Instance.Attach(noteObserver);
        
        taskObserver.Update();
        noteObserver.Update();

        AppManager.Instance.AllEntries.CollectionChanged += OnMainListChanged;

        UpdateExistingTags();
        RefreshList();
    }

    [RelayCommand]
    private void SwitchTab(string tabName)
    {
        CurrentTab = tabName;
        FilterTagsText = string.Empty; 
        SelectedFilterPriority = "Wszystkie";
        
        RefreshList();
    }

    private void RefreshList()
    {
        FilteredEntries.Clear();

        IEnumerable<IEntryComponent> query;

        if (CurrentTab == "Task")
        {
            query = AppManager.Instance.AllEntries.OfType<Task>();
        }
        else
        {
            query = AppManager.Instance.AllEntries.OfType<Note>();
        }

        if (!string.IsNullOrWhiteSpace(FilterTagsText))
        {
            var searchTerms = FilterTagsText.ToLower().Split(new[] { ' ', ',' }, System.StringSplitOptions.RemoveEmptyEntries);

            query = query.Where(item => 
                item.Tags.Any(tag => searchTerms.Any(term => tag.ToLower().Contains(term)))
            );
        }

        if (CurrentTab == "Task" && SelectedFilterPriority != "Wszystkie")
        {
            query = query.OfType<Task>().Where(t => t.Priority == SelectedFilterPriority);
        }

        foreach (var item in query)
        {
            FilteredEntries.Add(item);
        }
    }

    private void OnMainListChanged(object? sender, NotifyCollectionChangedEventArgs e)
    {
        UpdateExistingTags();
        RefreshList();
    }

    [RelayCommand]
    private void AddNewEntry()
    {
        if (string.IsNullOrWhiteSpace(NewTaskDescription)) return;

        var tagsList = NewEntryTags
            .Split(new[] { ' ', ',' }, StringSplitOptions.RemoveEmptyEntries)
            .ToList();

        IEntryComponent newEntry;

        if (CurrentTab == "Task")
        {
            var builder = new TaskBuilder()
                .SetDescription(NewTaskDescription)
                .SetPriority(NewTaskPriority)
                .SetCategory("Ogólne");
            
            var task = builder.Build();
            task.Tags = tagsList;
            newEntry = task;
        }
        else
        {
            newEntry = new Note
            {
                Description = NewTaskDescription,
                Tags = tagsList
            };
        }

        var command = new AddEntryCommand(newEntry);
        command.Execute(null);

        NewTaskDescription = string.Empty;
        NewEntryTags = string.Empty;
    }
    
    private void UpdateExistingTags()
    {
        var allItems = AppManager.Instance.AllEntries;

        var distinctTags = allItems
            .SelectMany(x => x.Tags)
            .Distinct()
            .OrderBy(t => t)
            .ToList();

        ExistingTags.Clear();
        foreach (var tag in distinctTags)
        {
            ExistingTags.Add(tag);
        }
    }
}