using GameStoreMono.BlazorServer.Data;
using GameStoreMono.BlazorServer.Endpoints;
using GameStoreMono.BlazorServer.Interfaces;
using GameStoreMono.BlazorServer.Models;
using GameStoreMono.BlazorServer.Services;

var builder = WebApplication.CreateBuilder(args);

// Add Blazor Server services
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();

// Add database services
var connectionString = builder.Configuration.GetConnectionString("GameStore") ??
                       throw new InvalidOperationException("Connection string 'GameStore' not found.");
builder.Services.AddSqlite<GameStoreContext>(connectionString);

// Add your services
builder.Services.AddSingleton<GameCollectionModel>();
builder.Services.AddScoped<GameService>();
builder.Services.AddHostedService<PlcDataService>();

// Add API services (if keeping REST endpoints)
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure pipeline
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI();
}
else
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.MapRazorPages();
app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

// Map API endpoints (optional)
app.MapGenresEndpoints();
app.MapGamesEndpoints();

// Apply migrations
await app.MigrateDbAsync();

app.Run();