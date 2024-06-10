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

//Connection to the frontend
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: "AllowAnyOrigin",
        policy =>
        {
            policy.AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader()
            .WithOrigins("https://backend20240610112356.azurewebsites.net")
            .AllowCredentials();
        });
});


var app = builder.Build();
app.UseCors("AllowAnyOrigin");




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

app.ConfigureTicTacToeEndpoints();

app.Run();
