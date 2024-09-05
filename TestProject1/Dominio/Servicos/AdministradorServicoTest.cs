using Microsoft.EntityFrameworkCore;
using Minimal.Dominio.Entidades;
using Minimal.Dominio.Servicos;
using Minimal.Infraestrutura.Db;
using Xunit;
using Assert = Xunit.Assert;

namespace TestProject1.Dominio.Servicos
{
    public class AdministradorServicoTest
    {
        private DbContexto _contexto;
        private AdministradorServico _servico;

        public AdministradorServicoTest()
        {
            var options = new DbContextOptionsBuilder<DbContexto>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            _contexto = new DbContexto(options);
            _servico = new AdministradorServico(_contexto);
        }

        [Fact]
        public void BuscaPorId_DeveRetornarAdministradorCorreto()
        {
            // Arrange
            var administrador = new Administrador {
                Name = "Teste Admin",
                Email = "admin@teste.com",
                Senha = "123456",
                Perfil = "Admin"
            };

            _servico.Incluir(administrador);

            // Act
            var resultado = _servico.BuscaPorId(administrador.Id);

            // Assert
            Assert.NotNull(resultado);
            Assert.Equal(administrador.Id,resultado.Id);
        }

        [Fact]
        public void IncluirAdministrador_DeveAdicionarAdministradorNoBanco()
        {
            // Arrange
            var administrador = new Administrador {
                Name = "Teste Admin",
                Email = "admin@teste.com",
                Senha = "123456",
                Perfil = "Admin"
            };

            // Act
            _servico.Incluir(administrador);

            // Assert
            var administradorSalvo = _contexto.Administradores.FirstOrDefault(a => a.Email == "admin@teste.com");
            Assert.NotNull(administradorSalvo);
            Assert.Equal("Teste Admin",administradorSalvo.Name);
        }

        [TestCleanup]
        [Fact]
        public void Cleanup()
        {
            if(_contexto != null) {
                _contexto.Database.EnsureDeleted();
                _contexto.Dispose();
            }
        }
    }
}