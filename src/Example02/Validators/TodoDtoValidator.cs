using Example02.Models;
using Example02.ViewModels;
using FluentValidation;

namespace Example02.Validators;

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