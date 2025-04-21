using Microsoft.Net.Http.Headers;
using Microsoft.EntityFrameworkCore;
using AfryTollApi.Data;
using AfryTollApi.Repositories.Contracts;
using AfryTollApi.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContextPool<TollDBContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("TollConnection"))
);

// Repositories
builder.Services.AddScoped<ITollRepository, TollRepository>();
builder.Services.AddScoped<ITollCostRepository, TollCostRepository>();
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowBlazor", policy =>
    {
        policy.WithOrigins("https://localhost:7268")
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseHttpsRedirection();

app.UseCors("AllowBlazor");



app.UseAuthorization();

app.MapControllers();

app.Run();
