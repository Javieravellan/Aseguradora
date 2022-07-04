global using Aseguradora.Data;
global using Microsoft.EntityFrameworkCore;
using Aseguradora.Services;

var builder = WebApplication.CreateBuilder(args);
var MyAllowSpecificOrigins = "cors";


// Add services to the container.

builder.Services.AddControllers();
// Add EntityFramework
builder.Services.AddDbContext<AseguradoraContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DevCdn"));
});
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
// add cors support
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
        policy =>
        {
            policy.WithOrigins("http://localhost:4200");
            policy.AllowAnyHeader();
            policy.AllowAnyMethod();
        });
});

// add dependency injection
builder.Services.AddScoped<ServicioAseguradora, ServicioAseguradora>();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors(MyAllowSpecificOrigins);

app.UseAuthorization();

app.MapControllers();

app.Run();
