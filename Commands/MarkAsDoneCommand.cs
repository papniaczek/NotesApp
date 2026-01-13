using System;
using System.Windows.Input;
using NotesApp.Models;
using NotesApp.Services;

namespace NotesApp.Commands;

public class MarkAsDoneCommand : ICommand
{
    // Fix CS0067: Wymuszamy puste akcesory, żeby kompilator wiedział, 
    // że celowo nie używamy tego zdarzenia (bo w tej apce przyciski są zawsze aktywne).
    public event EventHandler? CanExecuteChanged
    {
        add { }
        remove { }
    }

    public bool CanExecute(object? parameter) => parameter is IEntryComponent;

    public void Execute(object? parameter)
    {
        if (parameter is IEntryComponent entry)
        {
            var status = entry.IsDone ? "ZROBIONE" : "DO ZROBIENIA";
            Console.WriteLine($"[Command] Zmiana statusu zadania na: {status}");

            // Powiadomienie Obserwatorów
            AppManager.Instance.NotifyObservers();
        }
    }
}