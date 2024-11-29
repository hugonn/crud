using Rotas.HttpService.Dom�nio.Rotas;
using Rotas.HttpService.Dom�nio.Rotas.Features.BuscarRota;
using Rotas.HttpService.Dom�nio.Rotas.Features.IncluirNovaRota;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<RotaRepository>();
builder.Services.AddScoped<IncluirRotaHandler>();
builder.Services.AddScoped<BuscarRotaHandler>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
