using Example02;
using Example02.Mappers;
using Example02.Validators;
using Example02.ViewModels;
using FluentValidation;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IValidator<TodoDto>, TodoDtoValidator>();
builder.Services.AddScoped<IRequestBodyHolder, RequestBodyHolder>();
builder.Services.AddSingleton<IEndpointsService, EndpointsService>();
builder.Services.AddSingleton<ITodoMapper, TodoMapper>();
builder.Services.AddHttpContextAccessor();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseRequestBodyMiddleware();

app
    .MapGet("/todo", (IEndpointsService service) => service.GetTodos())
    .WithName("GetTodos");

app
    .MapGet("/todo/{id}", (IEndpointsService service, int id) => service.GetTodo(id))
    .WithName("GetTodo");

app
    .MapPost("/todo", (IEndpointsService service, IValidator<TodoDto> validator, TodoDto dto) =>
    {
        var validationResult = validator.Validate(dto);
        if (!validationResult.IsValid) 
        {
            return Results.ValidationProblem(validationResult.ToDictionary());
        }
        
        service.CreateTodo(dto);
        return Results.Created($"/{dto.Id}", dto);
    })
    .WithName("CreateTodo");

app.Run();