using F1DriversApi.External;
using F1DriversApi.Models;
using F1DriversApi.Repositories;
using F1DriversApi.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();


builder.Services.AddScoped<IDriverService, DriverService>();
builder.Services.AddScoped<IDriverRepository, DriverRepository>();

builder.Services.Configure<OpenF1Config>(builder.Configuration.GetSection(OpenF1Config.SectionName));
builder.Services.AddHttpClient<IOpenF1Client, OpenF1Client>((serviceProvider, client) =>
{
    var settings = builder.Configuration.GetSection(OpenF1Config.SectionName).Get<OpenF1Config>()
        ?? new OpenF1Config();
    
    client.BaseAddress = new Uri(settings.BaseUrl);
    client.DefaultRequestHeaders.Add("Accept", "application/json");
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.MapControllers();


app.Run();

