using System;
using System.Windows.Input;
using NotesApp.Models;
using NotesApp.Services;

namespace NotesApp.Commands;

public class AddEntryCommand : ICommand
{
    private readonly IEntryComponent _entryToAdd;

    public AddEntryCommand(IEntryComponent entry)
    {
        _entryToAdd = entry;
    }

    public event EventHandler? CanExecuteChanged;
    public bool CanExecute(object? parameter) => true;

    public void Execute(object? parameter)
    {
        AppManager.Instance.AddEntry(_entryToAdd);
    }
}