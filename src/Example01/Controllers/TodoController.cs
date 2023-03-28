using Example01.Mappers;
using Example01.Models;
using Example01.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Example01.Controllers;

[ApiController]
[Route("[controller]")]
public class TodoController : ControllerBase
{
    private static readonly ICollection<Todo> Todos = new List<Todo>();

    private readonly ITodoMapper _mapper;

    public TodoController(ITodoMapper mapper)
    {
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    [HttpGet]
    public IEnumerable<TodoDto> GetTodos()
    {
        return Todos.Select(x => _mapper.MapTo(x));
    }
    
    [HttpGet("{id}")]
    public TodoDto GetTodo(int id)
    {
        var todo = Todos.FirstOrDefault(x => x.Id == id);
        return _mapper.MapTo(todo);
    }
    
    [HttpPost]
    public IActionResult CreateTodo([FromBody] TodoDto dto)
    {
        dto.Id = Todos.Count + 1;
        Todos.Add(_mapper.MapTo(dto));
        return CreatedAtAction(nameof(GetTodo), new { id = dto.Id }, dto);
    }
}