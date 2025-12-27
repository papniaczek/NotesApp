using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    private string _newTaskDescription = string.Empty;
    
    [ObservableProperty] 
    private string _newTaskPriority = "Normal";

    public ObservableCollection<IEntryComponent> Entries => AppManager.Instance.AllEntries;
    
    public List<string> AvailablePriorities { get; } = new() { "High", "Normal", "Low" };
    
    public ICommand MarkAsDoneCommand { get; } = new MarkAsDoneCommand();
    
    [ObservableProperty]
    private string _reportText = "Oczekiwanie na dane...";
    
    public MainWindowViewModel()
    {
        var generator = new ReportGenerator((text) =>
        {
            ReportText = text;
        });

        AppManager.Instance.Attach(generator);
        
        generator.Update();
    }

    [RelayCommand]
    private void AddNewTask()
    {
        if (string.IsNullOrWhiteSpace(NewTaskDescription)) return;

        var task = new TaskBuilder()
            .SetDescription(NewTaskDescription)
            .SetPriority(NewTaskPriority)
            .SetCategory("Ogólne")
            .Build();

        var command = new AddEntryCommand(task);
        command.Execute(null);

        NewTaskDescription = string.Empty;
    }
}