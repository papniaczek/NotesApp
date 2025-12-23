using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using NotesApp.Models;
using Task = NotesApp.Models.Task;

namespace NotesApp.Services;

public class AppManager
{
    private static AppManager _instance;

    private AppManager()
    {
        var demoTask = new Task
        {
            Description = "Zainstalować AvaloniaUI",
            Priority = "High",
            Category = "Project"
        };
        AddEntry(demoTask);
    }
    
    public static AppManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new AppManager();
            }
            return _instance;
        }
    }
    
    public ObservableCollection<IEntryComponent> AllEntries { get; private set; } = new ObservableCollection<IEntryComponent>();
    
    public void AddEntry(IEntryComponent entry)
    {
        AllEntries.Add(entry);
        Console.WriteLine($"[AppManager] Dodano: {entry.DisplayName}");
        
        // NotifyObservers();
    }
    
    public void RemoveEntry(IEntryComponent entry)
    {
        if (AllEntries.Contains(entry))
        {
            AllEntries.Remove(entry);
            Console.WriteLine($"[AppManager] Usunięto: {entry.DisplayName}");
        }
    }
}