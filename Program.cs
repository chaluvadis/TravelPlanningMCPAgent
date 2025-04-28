using TravelPlanningMCPAgent.Agents;
using TravelPlanningMCPAgent.Client;
using TravelPlanningMCPAgent.Routes;
using TravelPlanningMCPAgent.Servers;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

// Register HttpClient and servers with DI
builder.Services.AddHttpClient<IBookingServer, BookingServer>();
builder.Services.AddHttpClient<IGoogleMapsServer, GoogleMapsServer>();
builder.Services.AddHttpClient<IWeatherServer, WeatherServer>();
builder.Services.AddHttpClient<IWebSearchServer, WebSearchServer>();

// Register agents as singletons
builder.Services.AddSingleton<ILodgingAgent, LodgingAgent>();
builder.Services.AddSingleton<IItineraryAgent, ItineraryAgent>();
builder.Services.AddSingleton<IWeatherAgent, WeatherAgent>();
builder.Services.AddSingleton<IWebSearchAgent, WebSearchAgent>();

// Register MCPClient as a singleton
builder.Services.AddSingleton<IMCPClient, MCPClient>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

// Map trip-related routes
app.MapTripRoutes();

app.Run();
