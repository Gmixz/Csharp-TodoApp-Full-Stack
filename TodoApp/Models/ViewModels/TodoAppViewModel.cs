using System.Collections.Generic;

namespace TodoApp.Models.ViewModels
{

    public class TodoAppViewModel
    {
        public required List<TodoItem> TodoList {get; set;}
        public TodoItem? TodoApp {get; set;}
    }
}