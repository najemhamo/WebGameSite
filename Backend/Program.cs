using Endpoints;
using Repository;
using Services;
using Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IGameRepository, GameRepository>();
builder.Services.AddSingleton<WebSocketService>();

builder.Services.Configure<AzureWebPubSubSettings>(builder.Configuration.GetSection("AzureWebPubSub"));


// //Connection to the frontend
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(builder =>
    {
        builder.WithOrigins("http://localhost:5174", "https://guro18.github.io/WebGameSite/")
               .AllowAnyHeader()
               .AllowAnyMethod()
               .AllowCredentials();
    });
});

var app = builder.Build();

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

app.UseRouting();

app.UseCors();

app.ConfigureTicTacToeEndpoints();

app.Run();
