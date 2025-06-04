using GameStoreMono.BlazorServer.Data;
using GameStoreMono.BlazorServer.Endpoints;
using GameStoreMono.BlazorServer.Hubs;
using GameStoreMono.BlazorServer.Interfaces;
using GameStoreMono.BlazorServer.Services;

var builder = WebApplication.CreateBuilder(args);

// Add Blazor Server services
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();

// Add database services
var connectionString = builder.Configuration.GetConnectionString("GameStore") ??
                       throw new InvalidOperationException("Connection string 'GameStore' not found.");
builder.Services.AddSqlite<GameStoreContext>(connectionString);

// Add SignalR
builder.Services.AddSignalR(options =>
{
    options.EnableDetailedErrors = true;
    options.KeepAliveInterval = TimeSpan.FromSeconds(15);
    options.ClientTimeoutInterval = TimeSpan.FromSeconds(30);
});

// Add your services
builder.Services.AddScoped<GameService>();
builder.Services.AddHostedService<PlcDataService>();
builder.Services.AddScoped<IPlcDataService, PlcDataService>();

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

// Map SignalR Hub
app.MapHub<DataUpdateHub>("/datahub");

// Map API endpoints (optional)
app.MapGenresEndpoints();
app.MapGamesEndpoints();

// Apply migrations
await app.MigrateDbAsync();

app.Run();