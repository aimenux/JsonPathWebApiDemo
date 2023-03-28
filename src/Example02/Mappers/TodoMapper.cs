using Example02.Models;
using Example02.ViewModels;

namespace Example02.Mappers;

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