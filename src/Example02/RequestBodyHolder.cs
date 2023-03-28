namespace Example02;

public interface IRequestBodyHolder
{
    public string RequestBody { get; set; }
}

public class RequestBodyHolder : IRequestBodyHolder
{
    public string RequestBody { get; set; }
}