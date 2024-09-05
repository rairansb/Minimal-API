using Microsoft.EntityFrameworkCore;
using Minimal.Dominio.Entidades;
using Minimal.Dominio.Servicos;
using Minimal.Infraestrutura.Db;
using Xunit;
using Assert = Xunit.Assert;

namespace TestProject1.Dominio.Servicos
{
    public class VeiculoServicoTests
    {
        private DbContexto _contexto;
        private VeiculoServico _servico;

        public VeiculoServicoTests()
        {
            var options = new DbContextOptionsBuilder<DbContexto>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            _contexto = new DbContexto(options);
            _servico = new VeiculoServico(_contexto);
        }

        [Fact]
        public void BuscaPorId_DeveRetornarVeiculoCorreto()
        {
            // Arrange
            var veiculo = new Veiculo {
                Modelo = "City",
                Marca = "Honda",
                Ano = 2022

            };

            _servico.Incluir(veiculo);

            // Act
            var resultado = _servico.BuscarPorId(veiculo.Id);

            // Assert
            Assert.NotNull(resultado);
            Assert.Equal(veiculo.Id,resultado.Id);
        }

        [Fact]
        public void IncluirVeiculo_DeveAdicionarVeiculoNoBanco()
        {
            // Arrange
            var veiculo = new Veiculo {
                Modelo = "Corola",
                Marca = "Toyota",
                Ano = 2024
                
            };

            // Act
            _servico.Incluir(veiculo);

            // Assert
            var veiculoSalvo = _contexto.Veiculos.FirstOrDefault(v => v.Marca == "Toyota");
            Assert.NotNull(veiculoSalvo);
            Assert.Equal("Toyota",veiculoSalvo.Marca);
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