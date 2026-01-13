using System;
using System.Linq;
using NotesApp.Models;

namespace NotesApp.Services;

public class WeeklyTaskObserver : IObserver
{
    private readonly Action<string> _updateAction;

    public WeeklyTaskObserver(Action<string> updateAction)
    {
        _updateAction = updateAction;
    }

    public void Update()
    {
        var tasks = AppManager.Instance.AllEntries.OfType<Task>().ToList();
        var today = DateTime.Today;
        
        var nextWeek = today.AddDays(7);

        int count = tasks.Count(t => 
            !t.IsDone && 
            t.DueDate.Date >= today && 
            t.DueDate.Date <= nextWeek);

        _updateAction($"W tym tygodniu: {count}");
    }
}