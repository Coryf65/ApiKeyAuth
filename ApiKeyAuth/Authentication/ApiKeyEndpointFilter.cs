using Microsoft.AspNetCore.Http;

namespace ApiKeyAuth.Authentication;

/// <summary>
/// NOTE: ONLY WORKS IN .NET 7
/// </summary>
public class ApiKeyEndpointFilter : IEndpointFilter
{
    private readonly IConfiguration _configuration;

    public ApiKeyEndpointFilter(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async ValueTask<object> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
    {
        // Check the header for a vaild api key
        if (!context.HttpContext.Request.Headers.TryGetValue(AuthConstants.ApiHeaderName, out var extractedApiKey))
        {
            // normally we cannot pass in the message "missing api key"
            // return TypedResults.Unauthorized();
            // So we create our own method
            return new UnauthorizedHttpObjectResult("API Key is missing");
        }

        // the key we expect to see
        var apikey = _configuration.GetValue<string>(AuthConstants.ApiKeySectionName);

        // check if the one sent matches the one we expected
        if (!apikey.Equals(extractedApiKey))
        {
            return new UnauthorizedHttpObjectResult("Invalid API Key");
        }

        return await next(context);
    }
}