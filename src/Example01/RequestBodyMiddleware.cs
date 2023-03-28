namespace Example01;

public class RequestBodyMiddleware
{
    private readonly RequestDelegate _next;
    private readonly IHttpContextAccessor _accessor;

    public RequestBodyMiddleware(RequestDelegate next, IHttpContextAccessor accessor)
    {
        _next = next ?? throw new ArgumentNullException(nameof(next));
        _accessor = accessor ?? throw new ArgumentNullException(nameof(accessor));
    }
    
    public async Task InvokeAsync(HttpContext context, IRequestBodyHolder holder)
    {
        holder.RequestBody = await GetRequestBodyAsync();
        await _next(context);
    }
    
    private async Task<string> GetRequestBodyAsync()
    {
        var context = _accessor.HttpContext;
        if (context is null) return null;
        var requestBody = await context.Request.GetRequestBodyAsync();
        return requestBody;
    }
}

public static class RequestBodyMiddlewareExtensions
{
    public static void UseRequestBodyMiddleware(this IApplicationBuilder app)
    {
        app.UseMiddleware<RequestBodyMiddleware>();
    }
}