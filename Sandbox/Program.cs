using CrossBorder.Dataverse.Bootstrappers;
using Microsoft.AspNetCore.Mvc;
using Sandbox;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddOdataDbContext<SandboxDb>(options =>
{
    options.BaseUrl = "https://pprojjustsignaltst.api.crm4.dynamics.com";
    options.TokenRequestContext = new Azure.Core.TokenRequestContext(new string[] { $"https://pprojjustsignaltst.api.crm4.dynamics.com/.default" });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet("/weatherforecast", async ([FromServices] SandboxDb db) =>
{
var persons = await db.Persons.ToListAsync();
  
    return persons;
})
.WithName("GetWeatherForecast");

app.Run();

internal record WeatherForecast(DateTime Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}