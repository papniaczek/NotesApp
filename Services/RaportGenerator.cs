using System;
using System.Linq;
using NotesApp.Models;

namespace NotesApp.Services;

public class ReportGenerator : IObserver
{
    private readonly Action<string> _onReportGenerated;

    public ReportGenerator(Action<string> onReportGenerated)
    {
        _onReportGenerated = onReportGenerated;
    }

    public void Update()
    {
        var allEntries = AppManager.Instance.AllEntries;

        int total = allEntries.Count;
        int done = allEntries.Count(e => e.IsDone);
        int pending = total - done;

        string report = $"Zadania: {total} | Zrobione: {done} | Do zrobienia: {pending}";

        _onReportGenerated(report);
    }
}