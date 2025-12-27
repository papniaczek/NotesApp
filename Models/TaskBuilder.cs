using System;
using System.Collections.Generic;

namespace NotesApp.Models;

public class TaskBuilder
{
    private Task _task = new Task();

    public TaskBuilder SetDescription(string description)
    {
        _task.Description = description;
        return this;
    }

    public TaskBuilder SetPriority(string priority)
    {
        _task.Priority = priority;
        return this;
    }

    public TaskBuilder SetCategory(string category)
    {
        _task.Category = category;
        return this;
    }
    
    public TaskBuilder SetDueDate(DateTime date)
    {
        _task.DueDate = date;
        return this;
    }

    public Task Build()
    {
        return _task;
    }
}