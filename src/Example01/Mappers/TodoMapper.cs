using Example01.Models;
using Example01.ViewModels;

namespace Example01.Mappers;

public interface ITodoMapper
{
    TodoDto MapTo(Todo todo);
    Todo MapTo(TodoDto todo);
}

public class TodoMapper : ITodoMapper
{
    public TodoDto MapTo(Todo todo)
    {
        if (todo is null) return null;
        return new TodoDto
        {
            Id = todo.Id,
            Title = todo.Title,
            Category = todo.Category.ToString()
        };
    }

    public Todo MapTo(TodoDto todo)
    {
        if (todo is null) return null;
        return new Todo
        {
            Id = todo.Id,
            Title = todo.Title,
            Category = Enum.Parse<Category>(todo.Category, true)
        };
    }
}