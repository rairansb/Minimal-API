using Minimal.Dominio.Entidades;

namespace TestProject1.Dominio.Entidades
{
    [TestClass]
    public class VeiculoTests
    {
        [TestMethod]
        public void Veiculo_DeveTerPropriedadesDefinidasCorretamente()
        {
            // Arrange
            var veiculo = new Veiculo {

                Modelo = "Civic",
                Marca = "Honda",
                Ano = 2022
            
            };

            // Act
            // Não há ação necessária, já que estamos apenas verificando as propriedades

            // Assert
            Assert.AreEqual("Civic",veiculo.Modelo);
            Assert.AreEqual("Honda",veiculo.Marca);
            Assert.AreEqual(2022,veiculo.Ano);
           
        }

        [TestMethod]
        public void Veiculo_IdDeveSerDefaultInicialmente()
        {
            // Arrange
            var veiculo = new Veiculo();

            // Act
            var idInicial = veiculo.Id;

            // Assert
            Assert.AreEqual(default,idInicial);
        }
    }
}