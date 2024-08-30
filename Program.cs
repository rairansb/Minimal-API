using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Minimal.Dominio.DTOs;
using Minimal.Dominio.Entidades;
using Minimal.Dominio.Interfaces;
using Minimal.Dominio.ModelViews;
using Minimal.Dominio.Servicos;
using Minimal.Infraestrutura.Db;

#region Bieuder

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<IAdministradorServico,AdministradorServico>();
builder.Services.AddScoped<IVeiculoServico,VeiculoServico>();

// Add services to the container.
builder.Services.AddDbContext<DbContexto>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("ConexaoPadrao")));

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

#endregion

// Configure the HTTP request pipeline.

#region Home
app.MapGet("/",() => Results.Json(new Home())).WithTags("Home");
#endregion

#region Administradores
app.MapPost("/administradores/login",( [FromBody] LoginDTO loginDTO,IAdministradorServico administradorServico ) => {
    if(administradorServico.Login(loginDTO) != null)
        return Results.Ok("Login com sucesso");
    else
        return Results.Unauthorized();
}).WithTags("Administradores");
#endregion

#region Veiculos
app.MapGet("/veiculos",( [FromQuery] int? page,IVeiculoServico veiculoServico ) => {
    var veiculos = veiculoServico.Todos(page);

    return Results.Ok(veiculos);
}).WithTags("Veiculos");
;

app.MapGet("/veiculos/{id}",( [FromRoute] int id,IVeiculoServico veiculoServico ) => {
    var veiculo = veiculoServico.BuscarPorId(id);

    if(veiculo == null)
        return Results.NotFound();

    return Results.Ok(veiculo);
}).WithTags("Veiculos");
;

app.MapPost("/veiculos",( [FromBody] VeiculoDTO veiculoDTO,IVeiculoServico veiculoServico ) => {
    var veiculo = new Veiculo {
        Modelo = veiculoDTO.Modelo,
        Marca = veiculoDTO.Marca,
        Ano = veiculoDTO.Ano,
    };
    veiculoServico.Incluir(veiculo);

    return Results.Created($"/veiculo/{veiculo.Id}",veiculo);
}).WithTags("Veiculos");
;
#endregion

#region App
if(app.Environment.IsDevelopment()) {
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

#endregion
