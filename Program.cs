using GameStoreMono.BlazorServer.Data;
using GameStoreMono.BlazorServer.Endpoints;
using GameStoreMono.BlazorServer.Models;
using GameStoreMono.BlazorServer.Services;

var builder = WebApplication.CreateBuilder(args);

// Add Blazor Server services
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();

// Add database services
var connectionString = builder.Configuration.GetConnectionString("GameStore") ??
                       throw new InvalidOperationException("Connection string 'GameStore' not found.");

// Add SQLite database context                       
builder.Services.AddSqlite<GameStoreContext>(connectionString);

// Add your services
builder.Services.AddSingleton<GameCollectionModel>();
builder.Services.AddScoped<GameService>();
builder.Services.AddHostedService<PlcDataService>();

// Optional: Add API services for REST endpoints (can be removed if not needed)
if (builder.Environment.IsDevelopment())
{
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();
}

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

// Optional: API endpoints (can be removed if not needed)
if (app.Environment.IsDevelopment())
{
    app.MapGenresEndpoints();
    app.MapGamesEndpoints();
}

// Apply database migrations
try
{
    await app.MigrateDbAsync();
}
catch (Exception ex)
{
    var logger = app.Services.GetService<ILogger<Program>>();
    logger?.LogError(ex, "An error occurred while migrating the database");
    throw;
}

app.Run();