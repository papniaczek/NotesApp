using System.Collections.Generic;
using CommunityToolkit.Mvvm.ComponentModel;

namespace NotesApp.Models;

public partial class Note : ObservableObject, IEntryComponent
{
    public string Description { get; set; } = string.Empty;

    [ObservableProperty]
    private bool _isDone;

    public List<string> Tags { get; set; } = new List<string>();

    public string DisplayName => $"[NOTATKA] {Description}";

    public void Display() { }

    public void MarkAsDone()
    {
        IsDone = true;
    }
}