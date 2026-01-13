using System;
using System.Windows.Input;
using NotesApp.Models;
using NotesApp.Services;

namespace NotesApp.Commands;

public class MarkAsDoneCommand : ICommand
{
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

            AppManager.Instance.NotifyObservers();
        }
    }
}