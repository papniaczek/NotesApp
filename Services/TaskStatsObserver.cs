using System;
using System.Linq;
using NotesApp.Models;

namespace NotesApp.Services;

public class TaskStatsObserver : IObserver
{
    private readonly Action<string> _updateAction;

    public TaskStatsObserver(Action<string> updateAction)
    {
        _updateAction = updateAction;
    }

    public void Update()
    {
        var tasks = AppManager.Instance.AllEntries.OfType<Task>().ToList();

        int total = tasks.Count;
        int done = tasks.Count(t => t.IsDone);
        int pending = total - done;

        string report = $"Do zrobienia: {pending}\nZrobione: {done}\nWszystkie: {total}";
        
        _updateAction(report);
    }
}