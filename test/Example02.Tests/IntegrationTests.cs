using System.Net;
using System.Net.Http.Json;
using Example02.ViewModels;
using FluentAssertions;

namespace Example02.Tests;

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
    [ClassData(typeof(TodoDtoTestCases))]
    public async Task When_Post_Todo_Then_Should_Return_Success_Response(TodoDto dto, HttpStatusCode expectedStatusCode)
    {
        // arrange
        var fixture = new WebApiTestFixture();
        var client = fixture.CreateClient();

        // act
        var response = await client.PostAsJsonAsync("/todo", dto);

        // assert
        response.StatusCode.Should().Be(expectedStatusCode);
    }
    
    private class TodoDtoTestCases : TheoryData<TodoDto, HttpStatusCode>
    {
        public TodoDtoTestCases()
        {
            Add(new TodoDto
            {
                Title = "abc",
                Category = "Home"
            }, HttpStatusCode.Created);
            
            Add(new TodoDto
            {
                Title = "xyz",
                Category = "Work"
            }, HttpStatusCode.Created);
            
            Add(new TodoDto
            {
                Title = "abc",
                Category = "home"
            }, HttpStatusCode.Created);
            
            Add(new TodoDto
            {
                Title = "xyz",
                Category = "work"
            }, HttpStatusCode.Created);
            
            Add(new TodoDto
            {
                Title = "",
                Category = "Work"
            }, HttpStatusCode.BadRequest);
            
            Add(new TodoDto
            {
                Title = "Foo",
                Category = "Bar"
            }, HttpStatusCode.BadRequest);
        }
    }
}