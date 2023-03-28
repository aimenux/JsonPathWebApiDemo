using Example01.Validators;
using Example01.ViewModels;
using FluentAssertions;
using NSubstitute;

namespace Example01.Tests;

public class UnitTests
{
    [Theory]
    [ClassData(typeof(TodoDtoTestCases))]
    public void Should_Validate_TodoDto(TodoDto dto, bool expectedIsValid)
    {
        // arrange
        var holder = Substitute.For<IRequestBodyHolder>();
        var validator = new TodoDtoValidator(holder);
        
        // act
        var result = validator.Validate(dto);

        // assert
        result.IsValid.Should().Be(expectedIsValid);
    }
    
    private class TodoDtoTestCases : TheoryData<TodoDto, bool>
    {
        public TodoDtoTestCases()
        {
            Add(new TodoDto
            {
                Title = "abc",
                Category = "Home"
            }, true);
            
            Add(new TodoDto
            {
                Title = "xyz",
                Category = "Work"
            }, true);
            
            Add(new TodoDto
            {
                Title = "abc",
                Category = "home"
            }, true);
            
            Add(new TodoDto
            {
                Title = "xyz",
                Category = "work"
            }, true);
            
            Add(new TodoDto
            {
                Title = "",
                Category = "Work"
            }, false);
            
            Add(new TodoDto
            {
                Title = "Foo",
                Category = "Bar"
            }, false);
        }
    }
}