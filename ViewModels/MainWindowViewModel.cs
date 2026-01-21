using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
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
    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(IsTaskView))]
    [NotifyPropertyChangedFor(nameof(IsNoteView))]
    [NotifyPropertyChangedFor(nameof(EntryDescriptionWatermark))]
    private string _currentTab = "Note"; 

    public bool IsTaskView => CurrentTab == "Task";
    public bool IsNoteView => CurrentTab == "Note";
    public string EntryDescriptionWatermark => IsNoteView ? "Treść notatki..." : "Treść zadania...";

    public ObservableCollection<IEntryComponent> FilteredEntries { get; } = new();

    [ObservableProperty]
    private IEntryComponent? _selectedEntry;

    [ObservableProperty] private string _newTaskDescription = string.Empty;
    [ObservableProperty] private string _newNoteTitle = string.Empty; 
    [ObservableProperty] private string _newTaskPriority = "Normal";
    [ObservableProperty] private string _newEntryTags = string.Empty;
    [ObservableProperty] private DateTimeOffset? _newTaskDueDate = DateTimeOffset.Now;

    public List<string> AvailablePriorities { get; } = new() { "High", "Normal", "Low" };
    public ObservableCollection<string> ExistingTags { get; } = new();

    [ObservableProperty] private string _filterTagsText = string.Empty;
    [ObservableProperty] private string _selectedFilterPriority = "Wszystkie";
    [ObservableProperty] private string? _selectedTagFromList;
    public List<string> FilterPriorities { get; } = new() { "Wszystkie", "High", "Normal", "Low" };

    public ICommand MarkAsDoneCommand { get; } = new MarkAsDoneCommand();
    public ICommand RemoveEntryCommand { get; } = new RemoveEntryCommand();

    [ObservableProperty] private string _taskStatsText = "...";
    [ObservableProperty] private string _noteStatsText = "...";
    [ObservableProperty] private string _weeklyStatsText = "...";
    [ObservableProperty] private string _priorityStatsText = "...";


    public MainWindowViewModel()
    {
        var taskObserver = new TaskStatsObserver((t) => TaskStatsText = t);
        var noteObserver = new NoteStatsObserver((t) => NoteStatsText = t);
        var weeklyObserver = new WeeklyTaskObserver((t) => WeeklyStatsText = t);
        var priorityObserver = new PriorityTaskObserver((t) => PriorityStatsText = t);

        AppManager.Instance.Attach(taskObserver);
        AppManager.Instance.Attach(noteObserver);
        AppManager.Instance.Attach(weeklyObserver);
        AppManager.Instance.Attach(priorityObserver);
        
        taskObserver.Update();
        noteObserver.Update();
        weeklyObserver.Update();
        priorityObserver.Update();

        AppManager.Instance.AllEntries.CollectionChanged += OnMainListChanged;
        
        UpdateExistingTags();
        RefreshList();
    }

    [RelayCommand]
    private void SwitchTab(string tabName)
    {
        CurrentTab = tabName;
        SelectedEntry = null;
        RefreshList();
    }

    [RelayCommand]
    private void AddNewEntry()
    {
        var tagsList = NewEntryTags.Split(new[] { ' ', ',' }, StringSplitOptions.RemoveEmptyEntries).ToList();
        IEntryComponent newEntry;

        if (CurrentTab == "Task")
        {
            if (string.IsNullOrWhiteSpace(NewTaskDescription)) return;
            var builder = new TaskBuilder().SetDescription(NewTaskDescription).SetPriority(NewTaskPriority).SetCategory("Ogólne");
            if (NewTaskDueDate.HasValue) builder.SetDueDate(NewTaskDueDate.Value.DateTime);
            var task = builder.Build();
            task.Tags = tagsList;
            newEntry = task;
        }
        else
        {
            string finalTitle = string.IsNullOrWhiteSpace(NewNoteTitle) ? "Bez tytułu" : NewNoteTitle;
            newEntry = new Note { Title = finalTitle, Description = NewTaskDescription, Tags = tagsList };
        }
        var command = new AddEntryCommand(newEntry);
        command.Execute(null);
        NewTaskDescription = string.Empty; NewNoteTitle = string.Empty; NewEntryTags = string.Empty;
    }

    private void RefreshList()
    {
        FilteredEntries.Clear();
        IEnumerable<IEntryComponent> query;
        if (CurrentTab == "Task") query = AppManager.Instance.AllEntries.OfType<Task>();
        else query = AppManager.Instance.AllEntries.OfType<Note>();

        if (!string.IsNullOrWhiteSpace(FilterTagsText)) {
            var searchTerms = FilterTagsText.ToLower().Split(new[] { ' ', ',' }, StringSplitOptions.RemoveEmptyEntries);
            query = query.Where(item => item.Tags.Any(tag => searchTerms.Any(term => tag.ToLower().Contains(term))) || item.DisplayName.ToLower().Contains(FilterTagsText.ToLower()));
        }

        if (CurrentTab == "Task") {
            if (SelectedFilterPriority != "Wszystkie") query = query.OfType<Task>().Where(t => t.Priority == SelectedFilterPriority);
            var tasks = query.OfType<Task>().ToList();
            var grouped = tasks.GroupBy(t => t.DueDate.Date).OrderBy(g => g.Key);
            foreach (var group in grouped) {
                string headerText;
                if (group.Key == DateTime.MaxValue.Date) headerText = "Bez terminu";
                else if (group.Key == DateTime.Today) headerText = "Dzisiaj";
                else if (group.Key == DateTime.Today.AddDays(1)) headerText = "Jutro";
                else headerText = group.Key.ToString("dd.MM.yyyy (dddd)");
                FilteredEntries.Add(new DateHeader { Title = headerText });
                foreach (var task in group) FilteredEntries.Add(task);
            }
        }
        else { foreach (var item in query) FilteredEntries.Add(item); }
    }

    private void UpdateExistingTags() {
        var distinctTags = AppManager.Instance.AllEntries.SelectMany(x => x.Tags).Distinct().OrderBy(t => t).ToList();
        ExistingTags.Clear(); foreach (var tag in distinctTags) ExistingTags.Add(tag);
    }

    private void OnMainListChanged(object? sender, NotifyCollectionChangedEventArgs e) { UpdateExistingTags(); RefreshList(); }
    partial void OnFilterTagsTextChanged(string value) => RefreshList();
    partial void OnSelectedFilterPriorityChanged(string value) => RefreshList();
    partial void OnSelectedTagFromListChanged(string? value) { if(value!=null) { FilterTagsText = value; SelectedTagFromList = null; } }
}

public class DateHeader : IEntryComponent
{
    public string Title { get; set; } = string.Empty;
    public string DisplayName => Title;
    public bool IsDone { get; set; }
    public List<string> Tags { get; set; } = new();
    public bool IsSelectable => false;
    public void Display() { }
    public void MarkAsDone() { }
}