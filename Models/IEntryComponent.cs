using System.Collections.Generic;

namespace NotesApp.Models;

public interface IEntryComponent
{
    string DisplayName { get; }
    bool IsDone { get; set; }
    List<string> Tags { get; set; }
    
    // NOWE POLE: Czy element można zaznaczyć/kliknąć?
    bool IsSelectable { get; }
    
    void Display();
    void MarkAsDone();
}