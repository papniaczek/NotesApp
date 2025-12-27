using System;
using System.Collections.Generic;
using CommunityToolkit.Mvvm.ComponentModel; // Dodaj to using

namespace NotesApp.Models;

public partial class Task : ObservableObject, IEntryComponent
{
    public string Description { get; set; } = string.Empty;
    
    [ObservableProperty]
    private bool _isDone; 

    public string Priority { get; set; } = "Normal";
    public DateTime DueDate { get; set; } = DateTime.Now;
    public string Category { get; set; } = "Ogólne";
    public List<string> Tags { get; set; } = new List<string>();

    public string DisplayName => $"[ZADANIE] {Description} ({Priority})";

    public void Display()
    {
        Console.WriteLine(DisplayName);
    }

    public void MarkAsDone()
    {
        IsDone = true;
    }
}