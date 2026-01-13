using System.Collections.Generic;
using CommunityToolkit.Mvvm.ComponentModel;

namespace NotesApp.Models;

public partial class Note : ObservableObject, IEntryComponent
{
    [ObservableProperty] 
    private string _title = "Bez tytułu";

    [ObservableProperty]
    private string _description = string.Empty;

    [ObservableProperty]
    private bool _isDone;

    public List<string> Tags { get; set; } = new();

    public string DisplayName => Title;

    // NOWE: Notatki są klikalne
    public bool IsSelectable => true;

    public void Display() { }
    public void MarkAsDone() => IsDone = true;
}