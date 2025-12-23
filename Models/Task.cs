using System;
using System.Collections.Generic;

namespace NotesApp.Models;

public class Task : IEntryComponent
{
    public string Description { get; set; } = string.Empty;
    public bool IsDone { get; set; }
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