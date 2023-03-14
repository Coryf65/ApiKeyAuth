# ApiKeyAuth
 A sample of APi Key Authorization from Service 2 Service

Using an API Key that is set in the project which allows another request to it using the same api key in the header.

> NOTE: The key is just used as an example of implementation. It is a `GUID` stored in the `appsettings.json` for **demo purposes only**.

## Request example

`https://localhost:7203/weather` with a header of `x-api-key = 83565FE196334682A1A08665129E7B1D`

![api-request](https://user-images.githubusercontent.com/20805058/225124197-6d75eb84-10e6-4685-a1c3-f8a76ef9236c.png)

## 3 ways to handles this

1. As a Middleware

example

add into `program.cs`
```C#
app.UseMiddleware<ApiKeyAuthMiddleware>();
```

new class `ApiKeyAuthMiddleware`
```C#
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
```

Now any request sent will either send an error message like...

`missing api key`
![missing-api-key](https://user-images.githubusercontent.com/20805058/225133555-c795c126-5188-41ab-87cd-b22b52822534.png)

`invalid api key`
![invalid-api-key](https://user-images.githubusercontent.com/20805058/225133525-296b8807-ac38-4f59-828c-aa59de96c758.png)

If Authenticated it will allow us to proceed and interact with this API.

| Pros  | Cons |
| ------------- | ------------- |
| + easy to setup  | - applies to all controllers  |
| + automatically applies to any new code | - applies to all function / requests  |


2. As a Service Filter

example

| Pros  | Cons |
| ------------- | ------------- |
| + fine control  | - have to apply to any new code  |


3. As a Attribute and Service Filter

example

| Pros  | Cons |
| ------------- | ------------- |
| + fine control  | - have to apply to any new code  |
| + ease of use in code  | - harder to unit test / moq  |
