
using TravelExpense.Application;
using TravelExpense.Infrastructure;
using TravelExpense.Presentation;

var builder = WebApplication.CreateBuilder(args);
builder.Services
    .AddInfrastructure(builder.Configuration)
    .AddAplication();

var app = builder.Build();

app.MapGet("/health", () => "Healthy!");

app.MapTravelEndPoints();

app.Run();

public partial class Program { }
