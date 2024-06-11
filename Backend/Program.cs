using Endpoints;
using Repository;
using Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IGameRepository, GameRepository>();
builder.Services.AddSingleton<WebSocketService>();


// //Connection to the frontend
// builder.Services.AddCors(options =>
// {
//     options.AddPolicy(name: "AllowAnyOrigin",
//         policy =>
//         {
//             policy.AllowAnyOrigin()
//             .AllowAnyMethod()
//             .AllowAnyHeader()
//             .AllowCredentials();
//         });
// });
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(builder =>
    {
        builder.WithOrigins("http://localhost:5173")
               .AllowAnyHeader()
               .AllowAnyMethod()
               .AllowCredentials();  // If you need to support credentials
    });
});

var app = builder.Build();
// app.UseCors("AllowAnyOrigin");

app.UseCors();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

// Use websocket support
app.UseWebSockets();

app.Use(async (context, next) =>
{
    if (context.Request.Path == "/tictactoe")
    {
        if (context.WebSockets.IsWebSocketRequest)
        {
            var webSocket = await context.WebSockets.AcceptWebSocketAsync();
            var webSocketService = context.RequestServices.GetService<WebSocketService>();
            await webSocketService.HandleWebSocketConnection(webSocket);
        }
        else
        {
            context.Response.StatusCode = 400;
            await context.Response.WriteAsync("WebSocket connection expected.");
        }
    }
    else
    {
        await next();
    }
});

app.UseRouting();

app.ConfigureTicTacToeEndpoints();

app.Run();
