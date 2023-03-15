using ApiKeyAuth;
using ApiKeyAuth.Authentication;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// original
//builder.Services.AddControllers();

// Filter version #2
// we have many ways to use these filters allows more flexibility
// we could do this for all controllers or certain ones down to certain functions
builder.Services.AddControllers(filter => filter.Filters.Add<ApiKeyAuthFilter>());

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

// Add the ApiKey authentication into Swagger
builder.Services.AddSwaggerGen(c =>
{
    c.AddSecurityDefinition("ApiKey", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        Description = "The API Key to access the API",
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.ApiKey,
        Name = "x-api-key",
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Scheme = "ApiKeyScheme"
    });
    var scheme = new OpenApiSecurityScheme
    {
        Reference = new OpenApiReference
        {
            Type = ReferenceType.SecurityScheme,
            Id = "ApiKey"
        },
        In = ParameterLocation.Header
    };
    var requirement = new OpenApiSecurityRequirement
    {
        { scheme, new List<string>() }
    };
    c.AddSecurityRequirement(requirement);
});

// Filter version #2, register the filter, turn on per controller at each one
builder.Services.AddScoped<ApiKeyAuthFilter>();
// Filter as a Attribute version #3, use an attribute flag another way to do the filter
builder.Services.AddScoped<ApiKeyAuthFilterAttribute>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Middleware version #1
// This way is great for only allowing everything in the api to require this api key
app.UseMiddleware<ApiKeyAuthMiddleware>();

app.UseAuthorization();

app.MapControllers();

// #4 endpoint filter, use as a group or by method
var group = app.MapGroup("weather").AddEndpointFilter<ApiKeyEndpointFilter>();

// Minimal API example and how to use filters with this type
group.MapGet("weathermin", () =>
{
    string[] Summaries = new[]
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

    return Enumerable.Range(1, 5).Select(index => new WeatherForecast
    {
        Date = DateTime.Now.AddDays(index),
        TemperatureC = Random.Shared.Next(-20, 55),
        Summary = Summaries[Random.Shared.Next(Summaries.Length)]
    }).ToArray();
});

app.Run();