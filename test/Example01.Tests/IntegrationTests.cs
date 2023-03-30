using System.Net;
using System.Net.Http.Json;
using Example01.ViewModels;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;

namespace Example01.Tests;

public class IntegrationTests
{
    [Fact]
    public async Task When_Get_Todo_Then_Should_Return_Success_Response()
    {
        // arrange
        var fixture = new WebApiTestFixture();
        var client = fixture.CreateClient();

        // act
        var response = await client.GetAsync("/todo");

        // assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }
    
    [Theory]
    [ClassData(typeof(OkTodoDtoTestCases))]
    public async Task When_Post_Todo_Then_Should_Return_Success_Response(TodoDto dto)
    {
        // arrange
        var fixture = new WebApiTestFixture();
        var client = fixture.CreateClient();

        // act
        var response = await client.PostAsJsonAsync("/todo", dto);

        // assert
        response.StatusCode.Should().Be(HttpStatusCode.Created);
    }
    
    [Theory]
    [ClassData(typeof(KoTodoDtoTestCases))]
    public async Task When_Post_Todo_Then_Should_Return_BadRequest_Response(TodoDto dto, int expectedErrors)
    {
        // arrange
        var fixture = new WebApiTestFixture();
        var client = fixture.CreateClient();

        // act
        var response = await client.PostAsJsonAsync("/todo", dto);
        var errors = await GetErrorsAsync(response);

        // assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        errors.Should().HaveCount(expectedErrors);
        errors.Keys.Should().Contain(x => x.StartsWith("$."));
    }

    private static async Task<IDictionary<string, string[]>> GetErrorsAsync(HttpResponseMessage response)
    {
        var validationProblems = await response.Content.ReadFromJsonAsync<ValidationProblemDetails>();
        return validationProblems.Errors;
    }

    private class OkTodoDtoTestCases : TheoryData<TodoDto>
    {
        public OkTodoDtoTestCases()
        {
            Add(new TodoDto
            {
                Title = "abc",
                Category = "Home"
            });
            
            Add(new TodoDto
            {
                Title = "xyz",
                Category = "Work"
            });
            
            Add(new TodoDto
            {
                Title = "abc",
                Category = "home"
            });
            
            Add(new TodoDto
            {
                Title = "xyz",
                Category = "work"
            });
        }
    }
    
    private class KoTodoDtoTestCases : TheoryData<TodoDto, int>
    {
        public KoTodoDtoTestCases()
        {
            Add(new TodoDto
            {
                Title = "A",
                Category = "Work"
            }, 1);
            
            Add(new TodoDto
            {
                Title = "Abc",
                Category = "Xyz"
            }, 1);
            
            Add(new TodoDto
            {
                Title = "Foo",
                Category = "Bar"
            }, 1);
            
            Add(new TodoDto
            {
                Title = new string('*', 50),
                Category = "Bar"
            }, 2);
        }
    }
}