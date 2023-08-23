using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ProductionPlan.Helpers;
using ProductionPlan.LoadServices;
using ProductionPlan.LoadServices.Interfaces;
using ProductionPlan.Middleware;
using ProductionPlan.Services;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddScoped<IProductionPlanService, ProductionPlanService>();
builder.Services.AddScoped<IValidationHelper, ValidationHelper>();
builder.Services.AddScoped<IGasPlantService, GasPlantService>();
builder.Services.AddScoped<IWindTurbineService, WindTurbineService>();
builder.Services.AddScoped<ITurbojetPlantService, TurbojetPlantService>();


builder.Services.AddControllers().AddJsonOptions(x =>
{
    x.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
});

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<ExceptionMiddleware>();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

public partial class Program { }