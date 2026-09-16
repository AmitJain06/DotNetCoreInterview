using TCS.Middleware;

var builder = WebApplication.CreateBuilder(args);

// Register the logging middleware as a DI service (IMiddleware pattern)
builder.Services.AddTransient<RequestLoggingMiddleware>();

var app = builder.Build();

// Add the logging middleware to the request pipeline
app.UseMiddleware<RequestLoggingMiddleware>();

// Interview questions page (served from wwwroot/questions.html at the root URL)
app.MapGet("/", () => Results.File(
    Path.Combine(app.Environment.WebRootPath, "questions.html"),
    "text/html; charset=utf-8"));

app.Run();
