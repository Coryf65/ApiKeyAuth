using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace ApiKeyAuth.Authentication;

/// <summary>
/// Used as an Attribute, needs to use Configuration a bit differently since it is an Attribute
/// Can make Unit testing a bit more tricky
/// </summary>
public class ApiKeyAuthFilterAttribute : Attribute, IAuthorizationFilter
{
    public void OnAuthorization(AuthorizationFilterContext context)
    {
        // Check the header for a vaild api key
        if (!context.HttpContext.Request.Headers.TryGetValue(AuthConstants.ApiHeaderName, out var extractedApiKey))
        {
            context.Result = new UnauthorizedObjectResult("API Key is missing");
            return;
        }

        var configuration = context.HttpContext.RequestServices.GetRequiredService<IConfiguration>();
        // the key we expect to see
        var apikey = configuration.GetValue<string>(AuthConstants.ApiKeySectionName);

        // check if the one sent matches the one we expected
        if (!apikey.Equals(extractedApiKey))
        {
            context.Result = new UnauthorizedObjectResult("API Key is invalid");
            return;
        }
    }
}