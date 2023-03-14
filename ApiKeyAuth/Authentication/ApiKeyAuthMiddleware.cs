namespace ApiKeyAuth.Authentication;

public class ApiKeyAuthMiddleware
{
    private readonly RequestDelegate _next;
    private readonly IConfiguration _configuration;

    public ApiKeyAuthMiddleware(RequestDelegate next, IConfiguration configuration)
    {
        _next = next;
        _configuration = configuration;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        // Check the header for a vaild api key
        if (!context.Request.Headers.TryGetValue(AuthConstants.ApiHeaderName, out var extractedApiKey))
        {
            context.Response.StatusCode = 401;
            await context.Response.WriteAsync("API Key is missing");
            return;
        }

        // the key we expect to see
        var apikey = _configuration.GetValue<string>(AuthConstants.ApiKeySectionName);

        // check if the one sent matches the one we expected
        if (!apikey.Equals(extractedApiKey))
        {
            context.Response.StatusCode = 401;
            await context.Response.WriteAsync("API Key is invalid");
            return;
        }

        await _next(context);
    }
}