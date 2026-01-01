using System;
using System.Windows.Input;
using NotesApp.Models;
using NotesApp.Services;

namespace NotesApp.Commands;

public class RemoveEntryCommand : ICommand
{
    public event EventHandler? CanExecuteChanged;

    public bool CanExecute(object? parameter)
    {
        return parameter is IEntryComponent;
    }

    public void Execute(object? parameter)
    {
        if (parameter is IEntryComponent entry)
        {
            AppManager.Instance.RemoveEntry(entry);
        }
    }
}