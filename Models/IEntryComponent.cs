using System.Collections.Generic;

namespace NotesApp.Models;

public interface IEntryComponent
{
    string DisplayName { get; }
    
    bool IsDone { get; set; }
    
    List<string> Tags { get; set; }
    
    void Display();
    
    void MarkAsDone();
}