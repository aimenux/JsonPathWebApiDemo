using FluentValidation;
using FluentValidation.Results;

namespace Example01.Validators;

public abstract class AbstractApiValidator<T> : AbstractValidator<T>
{
    private readonly IRequestBodyHolder _requestBodyHolder;

    protected AbstractApiValidator(IRequestBodyHolder requestBodyHolder)
    {
        _requestBodyHolder = requestBodyHolder ?? throw new ArgumentNullException(nameof(requestBodyHolder));
    }
    
    public override ValidationResult Validate(ValidationContext<T> context)
    {
        var result = base.Validate(context);
        
        foreach (var error in result.Errors)
        {
            var propJsonPath = _requestBodyHolder.RequestBody.GetJsonPath(error.PropertyName);
            if (!string.IsNullOrWhiteSpace(propJsonPath))
            {
                error.PropertyName = propJsonPath;
            }
        }

        return result;
    }
}