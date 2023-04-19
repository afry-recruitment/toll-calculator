using Infrastructure.ServiceRegistration;
using Microsoft.Extensions.Hosting;
using System;
using TollCalculator.Infrastructure.Context;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddPersistenceInfrastructure();
builder.Services.AddSwaggerGen();
builder.Services.AddApplicationLayer();

var app = builder.Build();
// Configure the HTTP request pipeline.
using(var scope = app.Services.CreateScope())
{
    var service = scope.ServiceProvider;
    var context = service.GetRequiredService<InMemoryDbContext>();
    AddData.Initialize(service);
}
if(app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
