using System;
using System.Linq;
using NotesApp.Models;

namespace NotesApp.Services;

public class NoteStatsObserver : IObserver
{
    private readonly Action<string> _updateAction;

    public NoteStatsObserver(Action<string> updateAction)
    {
        _updateAction = updateAction;
    }

    public void Update()
    {
        var notes = AppManager.Instance.AllEntries.OfType<Note>().ToList();

        int total = notes.Count;

        string report = $"Notatki: {total}";

        _updateAction(report);
    }
}