using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

var ocelotConfiguration = new ConfigurationBuilder()
    .SetBasePath(AppContext.BaseDirectory) // Set the base path to the project directory
    .AddJsonFile("ocelot.json")
    .Build();

// Register Ocelot services
builder.Services.AddOcelot(ocelotConfiguration);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

await app.UseOcelot();



app.Run();
