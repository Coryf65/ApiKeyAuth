namespace ApiKeyAuth;

public class UnauthorizedHttpObjectResult : IResult, IStatusCodeHttpResult
{
    private readonly object _body;

    public UnauthorizedHttpObjectResult(object body)
    {
        _body = body;
    }

    /// <summary>
    /// Gets the HTTP status code: <see cref="StatusCodes.Status401Unauthorized"/>
    /// </summary>
    public int StatusCode => StatusCodes.Status401Unauthorized;

    int? IStatusCodeHttpResult.StatusCode => StatusCode;

    /// <inheritdoc />
    public async Task ExecuteAsync(HttpContext httpContext)
    {
        ArgumentNullException.ThrowIfNull(httpContext);

        httpContext.Response.StatusCode = StatusCode;

        // check if it is a string
        if (_body is string s)
        {
            // return as a string
            await httpContext.Response.WriteAsync(s);
            return;
        }

        // otherwise serialize as json
        await httpContext.Response.WriteAsJsonAsync(_body);
    }
}