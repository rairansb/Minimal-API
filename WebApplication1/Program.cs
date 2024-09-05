using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Minimal.Dominio.DTOs;
using Minimal.Dominio.Entidades;
using Minimal.Dominio.Interfaces;
using Minimal.Dominio.ModelViews;
using Minimal.Dominio.Servicos;
using Minimal.Enuns;
using Minimal.Infraestrutura.Db;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

#region Builder

var builder = WebApplication.CreateBuilder(args);

var key = builder.Configuration.GetValue<string>("Jwt");
if(string.IsNullOrEmpty(key)) key = "123456";

builder.Services.AddAuthentication(option => {
    option.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    option.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options => {
    options.TokenValidationParameters = new TokenValidationParameters {
        ValidateLifetime = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key)),
        ValidateIssuer = false,
        ValidateAudience = false,
    };
});

builder.Services.AddAuthorization();

builder.Services.AddScoped<IAdministradorServico,AdministradorServico>();
builder.Services.AddScoped<IVeiculoServico,VeiculoServico>();

// Add services to the container.
builder.Services.AddDbContext<DbContexto>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("ConexaoPadrao")));

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(Options => {
    Options.AddSecurityDefinition("Bearer",new OpenApiSecurityScheme {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Insira o token JWT aqui",
    });

    Options.AddSecurityRequirement(new OpenApiSecurityRequirement {
        { 
        new OpenApiSecurityScheme {
            Reference = new OpenApiReference {
                Type = ReferenceType.SecurityScheme,
                Id = "Bearer"
            }
            },

        new string[] {}

        }
    });
});

var app = builder.Build();

#endregion

// Configure the HTTP request pipeline.

#region Home
app.MapGet("/",() => Results.Json(new Home())).AllowAnonymous().WithTags("Home");
#endregion

#region Administradores
string GerarTokenJwt( Administrador administrador )
{
    if(string.IsNullOrEmpty(key))
        return string.Empty;

    var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
    var credentials = new SigningCredentials(securityKey,SecurityAlgorithms.HmacSha256);

    var claims = new List<Claim>() {
        new Claim("Email", administrador.Email),
        new Claim("Perfil", administrador.Perfil),
        new Claim(ClaimTypes.Role, administrador.Perfil)
    };

    var token = new JwtSecurityToken(
        claims: claims,
        expires: DateTime.Now.AddDays(1),
        signingCredentials: credentials
        );
    return new JwtSecurityTokenHandler().WriteToken(token);
}
app.MapPost("/administradores",( [FromBody] AdministradorDTO administradorDTO,IAdministradorServico administradorServico ) => {
    var validacao = new ErrosDeValidacao {
        Mensagens = new List<string>()
    };

    if(string.IsNullOrEmpty(administradorDTO.Name))
        validacao.Mensagens.Add("Nome não pode ser vazio");
    if(string.IsNullOrEmpty(administradorDTO.Email))
        validacao.Mensagens.Add("Email não pode ser vazio");
    if(administradorDTO.Perfil == null)
        validacao.Mensagens.Add("Perfil não pode ser vazio");
    if(string.IsNullOrEmpty(administradorDTO.Password))
        validacao.Mensagens.Add("Senha não pode ser vazia");

    if(validacao.Mensagens.Count > 0) {
        return Results.BadRequest(validacao);
    }

    var administrador = new Administrador {
        Name = administradorDTO.Name,
        Email = administradorDTO.Email,
        Perfil = administradorDTO.Perfil.ToString() ?? Perfil.Editor.ToString(),
        Senha = administradorDTO.Password
    };
    administradorServico.Incluir(administrador);

    return Results.Created($"/administrador/{administrador.Id}",new AdministradorModelViews {
        Id = administrador.Id,
        Name = administrador.Name,
        Email = administrador.Email,
        Perfil = administrador.Perfil,
    });

}).RequireAuthorization(new AuthorizeAttribute { Roles = "Adm" }).WithTags("Administradores");


app.MapPost("/administradores/login",( [FromBody] LoginDTO loginDTO,IAdministradorServico administradorServico ) => {

    var adm = administradorServico.Login(loginDTO);
    if(adm != null) {
        string token = GerarTokenJwt(adm);
        return Results.Ok(new AdministradorLogado {
            Email = adm.Email,
            Perfil = adm.Perfil,
            Token = token
        });
    } else
        return Results.Unauthorized();
}).AllowAnonymous().WithTags("Administradores");

app.MapGet("/administradores",( [FromQuery] int page,IAdministradorServico administradorServico ) => {
    var adms = new List<AdministradorModelViews>();
    var administradores = administradorServico.Todos(page);
    foreach(var adm in administradores) {
        adms.Add(new AdministradorModelViews {
            Id = adm.Id,
            Name = adm.Name,
            Email = adm.Email,
            Perfil = adm.Perfil,
        });
    }
    return Results.Ok(adms);
}).RequireAuthorization().RequireAuthorization(new AuthorizeAttribute { Roles = "Adm"}).WithTags("Administradores");

app.MapGet("/administradores/{id}",( [FromRoute] int id,IAdministradorServico administradorServico ) => {
    var administrador = administradorServico.BuscaPorId(id);
    if(administrador == null)
        return Results.NotFound();
    return Results.Ok(new AdministradorModelViews { Id = administrador.Id,Name = administrador.Name,Email = administrador.Email,Perfil = administrador.Perfil,});
}).RequireAuthorization(new AuthorizeAttribute { Roles = "Adm" }).WithTags("Administradores");

#endregion

#region Veiculos
ErrosDeValidacao validaDTO( VeiculoDTO veiculoDTO )
{
    var validacao = new ErrosDeValidacao {
        Mensagens = new List<string>()
    };

    if(string.IsNullOrEmpty(veiculoDTO.Modelo))
        validacao.Mensagens.Add("O Modelo do veiculo não pode ser vazio");

    if(string.IsNullOrEmpty(veiculoDTO.Marca))
        validacao.Mensagens.Add(" A Marca do veiculo não pode ficar em branco");

    if(veiculoDTO.Ano < 1950)
        validacao.Mensagens.Add("Veiculo muito antigo, aceito somente anos superiores a 1950");

    return validacao;

}

app.MapGet("/veiculos",( [FromQuery] int? page,IVeiculoServico veiculoServico ) => {
    var veiculos = veiculoServico.Todos(page);

    return Results.Ok(veiculos);
}).RequireAuthorization().WithTags("Administradores");


app.MapGet("/veiculos/{id}",( [FromRoute] int id,IVeiculoServico veiculoServico ) => {
    var veiculo = veiculoServico.BuscarPorId(id);

    if(veiculo == null)
        return Results.NotFound();

    return Results.Ok(veiculo);
}).RequireAuthorization(new AuthorizeAttribute { Roles = "Adm,Editor" }).WithTags("Administradores");


app.MapPut("/veiculos/{id}",( [FromRoute] int id,VeiculoDTO veiculoDTO,IVeiculoServico veiculoServico ) => {

    var veiculo = veiculoServico.BuscarPorId(id);
    if(veiculo == null)
        return Results.NotFound();

    var validacao = validaDTO(veiculoDTO);
    if(validacao.Mensagens.Count > 0) {
        return Results.BadRequest(validacao);
    }



    veiculo.Modelo = veiculoDTO.Modelo;
    veiculo.Marca = veiculoDTO.Marca;
    veiculo.Ano = veiculoDTO.Ano;

    veiculoServico.Atualizar(veiculo);

    return Results.Ok(veiculo);
}).RequireAuthorization(new AuthorizeAttribute { Roles = "Adm" }).WithTags("Administradores");


app.MapDelete("/veiculos/{id}",( [FromRoute] int id,IVeiculoServico veiculoServico ) => {
    var veiculo = veiculoServico.BuscarPorId(id);

    if(veiculo == null)
        return Results.NotFound();

    veiculoServico.Apagar(veiculo);

    return Results.NoContent();
}).RequireAuthorization(new AuthorizeAttribute { Roles = "Adm" }).WithTags("Administradores");


app.MapPost("/veiculos",( [FromBody] VeiculoDTO veiculoDTO,IVeiculoServico veiculoServico ) => {

    var validacao = validaDTO(veiculoDTO);
    if(validacao.Mensagens.Count > 0) {
        return Results.BadRequest(validacao);
    }

    var veiculo = new Veiculo {
        Modelo = veiculoDTO.Modelo,
        Marca = veiculoDTO.Marca,
        Ano = veiculoDTO.Ano,
    };
    veiculoServico.Incluir(veiculo);

    return Results.Created($"/veiculo/{veiculo.Id}",veiculo);
}).RequireAuthorization(new AuthorizeAttribute { Roles = "Adm,Editor" }).WithTags("Administradores");
#endregion

#region App
if(app.Environment.IsDevelopment()) {
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();

#endregion
