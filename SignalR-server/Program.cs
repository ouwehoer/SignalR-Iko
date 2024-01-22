using SignalR.Hubs;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services
    .AddLogging()
    .AddCors()
    .AddSignalR()
    ;

var app = builder.Build();

app.UseCors(builder =>
    builder
    .AllowAnyHeader()
    .AllowAnyOrigin()
    .AllowAnyMethod()
    );

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapHub<ChatHub>("/chathub");

app.Run();