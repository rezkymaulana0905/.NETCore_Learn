using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ToDoApi.Models
{
    public class ITodoRepository
    {
        void Add(TodoItem item);
        IEnumerable<TodoItem> GetAll();
        TodoItem Find(string key);
        TodoItem Remove(string key);
        void Update(TodoItem item);
    }
}