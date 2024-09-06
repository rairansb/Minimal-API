using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Minimal.Dominio.Entidades;

namespace Minimal.Infraestrutura.Db;

public class DbContexto :DbContext {
  
    public DbContexto( DbContextOptions options ) : base(options) 
    {
    } 

    public DbSet<Administrador> Administradores { get; set; } = default!;

    public DbSet<Veiculo> Veiculos { get; set; } = default!;


    protected override void OnModelCreating( ModelBuilder modelBuilder ) {
        modelBuilder.Entity<Administrador>().HasData(
            new Administrador {
                Id = 1,
                Name = "Administrador",
                Email = "administrador@teste.com",
                Senha = "123456",
                Perfil = "Adm"
            }
            );
    }


}
