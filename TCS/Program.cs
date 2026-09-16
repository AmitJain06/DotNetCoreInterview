var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

// Interview questions page (served from wwwroot/questions.html at the root URL)
app.MapGet("/", () => Results.File(
    Path.Combine(app.Environment.WebRootPath, "questions.html"),
    "text/html; charset=utf-8"));

app.Run();
