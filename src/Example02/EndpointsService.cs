using Example02.Mappers;
using Example02.Models;
using Example02.ViewModels;

namespace Example02;

public interface IEndpointsService
{
    IEnumerable<TodoDto> GetTodos();
    TodoDto GetTodo(int id);
    TodoDto CreateTodo(TodoDto dto);
}

public class EndpointsService : IEndpointsService
{
    private static readonly ICollection<Todo> Todos = new List<Todo>();

    private readonly ITodoMapper _mapper;

    public EndpointsService(ITodoMapper mapper)
    {
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    public IEnumerable<TodoDto> GetTodos()
    {
        return Todos.Select(x => _mapper.MapTo(x));
    }

    public TodoDto GetTodo(int id)
    {
        var todo = Todos.FirstOrDefault(x => x.Id == id);
        return _mapper.MapTo(todo);
    }

    public TodoDto CreateTodo(TodoDto dto)
    {
        dto.Id = Todos.Count + 1;
        Todos.Add(_mapper.MapTo(dto));
        return dto;
    }
}