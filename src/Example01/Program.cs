using Example01;
using Example01.Mappers;
using Example01.Validators;
using Example01.ViewModels;
using FluentValidation;
using FluentValidation.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddScoped<IValidator<TodoDto>, TodoDtoValidator>();
builder.Services.AddScoped<IRequestBodyHolder, RequestBodyHolder>();
builder.Services.AddSingleton<ITodoMapper, TodoMapper>();
builder.Services.AddHttpContextAccessor();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseRequestBodyMiddleware();

app.MapControllers();

app.Run();
