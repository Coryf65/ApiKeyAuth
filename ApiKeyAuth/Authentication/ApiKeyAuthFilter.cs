using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace ApiKeyAuth.Authentication;

/// <summary>
/// An example of using the Authorization Filter could be async or not
/// This will allow us to use more fine grained controls
/// </summary>
public class ApiKeyAuthFilter : IAuthorizationFilter
{
    private readonly IConfiguration _configuration;

    public ApiKeyAuthFilter(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public void OnAuthorization(AuthorizationFilterContext context)
    {
        // Check the header for a vaild api key
        if (!context.HttpContext.Request.Headers.TryGetValue(AuthConstants.ApiHeaderName, out var extractedApiKey))
        {
            context.Result = new UnauthorizedObjectResult("API Key is missing");
            return;
        }

        // the key we expect to see
        var apikey = _configuration.GetValue<string>(AuthConstants.ApiKeySectionName);

        // check if the one sent matches the one we expected
        if (!apikey.Equals(extractedApiKey))
        {
            context.Result = new UnauthorizedObjectResult("API Key is invalid");
            return;
        }
    }
}