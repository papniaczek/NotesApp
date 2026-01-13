using System;
using System.Collections.Generic;
using CommunityToolkit.Mvvm.ComponentModel;

namespace NotesApp.Models;

public partial class Task : ObservableObject, IEntryComponent
{
    [ObservableProperty]
    private string _description = string.Empty;
    
    [ObservableProperty]
    private bool _isDone; 

    [ObservableProperty]
    private string _priority = "Normal";

    public DateTime DueDate { get; set; } = DateTime.Now;
    public string Category { get; set; } = "Ogólne";
    public List<string> Tags { get; set; } = new List<string>();

    public string DisplayName => $"[ZADANIE] {Description} ({Priority})";
    public string Title => Description;

    public bool IsSelectable => true;

    partial void OnDescriptionChanged(string value)
    {
        OnPropertyChanged(nameof(DisplayName));
        OnPropertyChanged(nameof(Title));
    }

    partial void OnPriorityChanged(string value) => OnPropertyChanged(nameof(DisplayName));

    public void Display() { }
    public void MarkAsDone() => IsDone = true;
}