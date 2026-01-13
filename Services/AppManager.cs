using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json;
using NotesApp.Models;
using Task = NotesApp.Models.Task;

namespace NotesApp.Services;

public class AppManager
{
    private static AppManager _instance;
    
    private List<IObserver> _observers = new List<IObserver>();
    
    private readonly string _filePath = "notes_data.json";

    private AppManager()
    {
        Load();

        if (AllEntries.Count == 0)
        {
            var welcomeNote = new Note { Description = "Eluwina wariacie" };
            AddEntry(welcomeNote);
        }
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
    
    public void Attach(IObserver observer)
    {
        _observers.Add(observer);
    }

    public void NotifyObservers()
    {
        foreach (var observer in _observers)
        {
            observer.Update();
        }

        Save();
    }
    
    public void AddEntry(IEntryComponent entry)
    {
        AllEntries.Add(entry);
        Console.WriteLine($"[AppManager] Dodano: {entry.DisplayName}");
        NotifyObservers();
        Save();
    }
    
    public void RemoveEntry(IEntryComponent entry)
    {
        if (AllEntries.Contains(entry))
        {
            AllEntries.Remove(entry);
            Console.WriteLine($"[AppManager] Usunięto: {entry.DisplayName}");
            NotifyObservers();
            Save();
        }
    }
    
    public void Save()
    {
        var settings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All };
        var json = JsonConvert.SerializeObject(AllEntries, Formatting.Indented, settings);
        
        File.WriteAllText(_filePath, json);
        Console.WriteLine("[AppManager] Zapisano dane do pliku.");
    }

    public void Load()
    {
        if (File.Exists(_filePath))
        {
            var json = File.ReadAllText(_filePath);
            var settings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All };
            
            var loadedEntries = JsonConvert.DeserializeObject<ObservableCollection<IEntryComponent>>(json, settings);

            if (loadedEntries != null)
            {
                AllEntries.Clear();
                foreach (var entry in loadedEntries)
                {
                    AllEntries.Add(entry);
                }
            }
        }
    }
}