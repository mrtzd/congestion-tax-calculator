using System.Text.Json.Serialization;
using CongestionTaxCalculator.Application.Interfaces;
using CongestionTaxCalculator.Application.Services;
using CongestionTaxCalculator.Domain.Policies;
using CongestionTaxCalculator.Domain.Services;
using CongestionTaxCalculator.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

// Policies are stateless, so they can be singletons
builder.Services.AddSingleton<DateTollPolicy>();
builder.Services.AddSingleton<TollExemptionPolicy>();
builder.Services.AddSingleton<TollFeePolicy>();

// Services are scoped to the HTTP request
builder.Services.AddScoped<ICongestionTaxService, CongestionTaxService>();
builder.Services.AddScoped<ICongestionTaxAppService, CongestionTaxAppService>();

// Add services to the container.
builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Congestion Tax Calculator API",
        Version = "v1",
        Description = "API for calculating congestion tax"
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();

app.Run();
