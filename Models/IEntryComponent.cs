using System.Collections.Generic;

namespace NotesApp.Models;

public interface IEntryComponent
{
    string DisplayName { get; }
    List<string> Tags { get; set; }

    void Display();
}