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
    
    [Theory]
    [InlineData("a", "home", 1)]
    [InlineData("abc", "rome", 1)]
    [InlineData("a", "homework", 2)]
    public void Should_Get_Validation_Errors(string title, string category, int expectedErrors)
    {
        // arrange
        var holder = Substitute.For<IRequestBodyHolder>();
        var validator = new TodoDtoValidator(holder);
        var dto = new TodoDto
        {
            Title = title,
            Category = category
        };
        
        // act
        var result = validator.Validate(dto);

        // assert
        result.IsValid.Should().Be(false);
        result.Errors.Should().HaveCount(expectedErrors);
    }
    
    [Theory]
    [ClassData(typeof(JsonPathTestCases))]
    public void Should_Get_Json_Path_For_Property_Name(string propertyName, string expectedPath)
    {
        // arrange
        const string json =
        """
        {
            "firstName": "John",
            "lastName": "doe",
            "age": 26,
            "address": 
            {
                "streetAddress": "15 street",
                "city": "Nara",
                "postalCode": "630-0192"
            },
            "phoneNumbers": 
            [
             {
                "type": "iPhone",
                "number": "0123-4567-8888"
             },
             {
                "type": "home",
                "number": "0123-4567-8910"
             }
            ]
        }
        """;

        // act
        var paths = json.GetJsonPath(propertyName);

        // assert
        paths.Should().Be(expectedPath);
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
    
    private class JsonPathTestCases : TheoryData<string, string>
    {
        public JsonPathTestCases()
        {
            Add("age", "$.age");
            Add("city", "$.address.city");
            Add("firstName", "$.firstName");
            Add("streetAddress", "$.address.streetAddress");
            Add("number", "$.phoneNumbers[0].number");
        }
    }
}