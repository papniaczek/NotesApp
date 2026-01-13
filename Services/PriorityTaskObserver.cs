using System;
using System.Linq;
using NotesApp.Models;

namespace NotesApp.Services;

public class PriorityTaskObserver : IObserver
{
    private readonly Action<string> _updateAction;

    public PriorityTaskObserver(Action<string> updateAction)
    {
        _updateAction = updateAction;
    }

    public void Update()
    {
        var tasks = AppManager.Instance.AllEntries.OfType<Task>().ToList();

        int high = tasks.Count(t => !t.IsDone && t.Priority == "High");
        int normal = tasks.Count(t => !t.IsDone && t.Priority == "Normal");
        int low = tasks.Count(t => !t.IsDone && t.Priority == "Low");

        string report = $"High: {high}\nNormal: {normal}\nLow: {low}";

        _updateAction(report);
    }
}