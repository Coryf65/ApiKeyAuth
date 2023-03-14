using ApiKeyAuth;
using ApiKeyAuth.Authentication;

var builder = WebApplication.CreateBuilder(args);

// original
//builder.Services.AddControllers();

// Filter version #2
// we have many ways to use these filters allows more flexibility
// we could do this for all controllers or certain ones down to certain functions
builder.Services.AddControllers(filter => filter.Filters.Add<ApiKeyAuthFilter>());

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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

// Minimal API example and how to use filters with this type
app.MapGet("weathermin", () =>
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