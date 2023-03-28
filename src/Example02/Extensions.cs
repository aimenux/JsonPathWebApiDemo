using System.Text.Json;

namespace Example02;

public static class Extensions
{
    public static async Task<string> GetRequestBodyAsync(this HttpRequest request)
    {
        request.EnableBuffering();
        using var reader = new StreamReader(request.Body, leaveOpen: true);
        var requestBody = await reader.ReadToEndAsync();
        request.Body.Position = 0;
        return requestBody;
    }

    public static string GetJsonPath(this string requestBody, string propertyName)
    {
        if (string.IsNullOrWhiteSpace(requestBody)) return null;
        var element = JsonDocument.Parse(requestBody).RootElement;
        var paths = GetJsonPaths(element, propertyName);
        return paths.FirstOrDefault();
    }
    
    private static IEnumerable<string> GetJsonPaths(JsonElement element, string propertyName, string path = "$")
    {
        switch (element.ValueKind)
        {
            case JsonValueKind.Object:
            {
                var results = new List<string>();
                foreach (var property in element.EnumerateObject())
                {
                    var propertyPath = $"{path}.{property.Name}";
                    results.AddRange(GetJsonPaths(property.Value, propertyName, propertyPath));
                    if (HasPropertyName(property, propertyName))
                    {
                        results.Add(propertyPath);
                    }
                }
                return results;
            }
            case JsonValueKind.Array:
            {
                var results = new List<string>();
                for (var i = 0; i < element.GetArrayLength(); i++)
                {
                    results.AddRange(GetJsonPaths(element[i], propertyName, $"{path}[{i}]"));
                }
                return results;
            }
            default:
                return Array.Empty<string>();
        }
    }
    
    private static bool HasPropertyName(JsonProperty property, string propertyName)
    {
        return string.Equals(property.Name, propertyName, StringComparison.OrdinalIgnoreCase);
    }
}