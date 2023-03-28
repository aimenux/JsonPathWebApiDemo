using Example01.Models;
using Example01.ViewModels;
using FluentValidation;

namespace Example01.Validators;

public class TodoDtoValidator : AbstractApiValidator<TodoDto>
{
    public TodoDtoValidator(IRequestBodyHolder requestBodyHolder) : base(requestBodyHolder)
    {
        RuleFor(x => x.Title)
            .MinimumLength(3)
            .MaximumLength(20);

        RuleFor(x => x.Category)
            .IsEnumName(typeof(Category), false);
    }
}