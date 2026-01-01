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
    private string _currentTab = "Task"; 

    public bool IsTaskView => CurrentTab == "Task";

    // --- 2. LISTA DANYCH ---

    public ObservableCollection<IEntryComponent> FilteredEntries { get; } = new();

    // --- 3. FORMULARZ ---

    [ObservableProperty] 
    private string _newTaskDescription = string.Empty;
    
    [ObservableProperty] 
    private string _newTaskPriority = "Normal";

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

        RefreshList();
    }

    [RelayCommand]
    private void SwitchTab(string tabName)
    {
        CurrentTab = tabName;
        RefreshList();
    }

    private void RefreshList()
    {
        FilteredEntries.Clear();

        IEnumerable<IEntryComponent> itemsToAdd;

        if (CurrentTab == "Task")
        {
            itemsToAdd = AppManager.Instance.AllEntries.OfType<Task>();
        }
        else
        {
            itemsToAdd = AppManager.Instance.AllEntries.OfType<Note>();
        }

        foreach (var item in itemsToAdd)
        {
            FilteredEntries.Add(item);
        }
    }

    private void OnMainListChanged(object? sender, NotifyCollectionChangedEventArgs e)
    {
        RefreshList();
    }

    [RelayCommand]
    private void AddNewEntry()
    {
        if (string.IsNullOrWhiteSpace(NewTaskDescription)) return;

        IEntryComponent newEntry;

        if (CurrentTab == "Task")
        {
            newEntry = new TaskBuilder()
                .SetDescription(NewTaskDescription)
                .SetPriority(NewTaskPriority)
                .SetCategory("Ogólne")
                .Build();
        }
        else
        {
            newEntry = new Note
            {
                Description = NewTaskDescription
            };
        }

        var command = new AddEntryCommand(newEntry);
        command.Execute(null);

        NewTaskDescription = string.Empty;
    }
}