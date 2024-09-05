using Minimal.Dominio.Entidades;

namespace TestProject1.Dominio.Entidades
{
    [TestClass]
    public class AdministradorTests
    {
        [TestMethod]
        public void Administrador_DeveTerPropriedadesDefinidasCorretamente()
        {
            // Arrange
            var administrador = new Administrador {
                Name = "John Doe",
                Email = "john.doe@example.com",
                Senha = "password123",
                Perfil = "Admin"
            };

            // Act
            // Não há ação necessária, já que estamos apenas verificando as propriedades

            // Assert
            Assert.AreEqual("John Doe",administrador.Name);
            Assert.AreEqual("john.doe@example.com",administrador.Email);
            Assert.AreEqual("password123",administrador.Senha);
            Assert.AreEqual("Admin",administrador.Perfil);
        }

        [TestMethod]
        public void Administrador_IdDeveSerDefaultInicialmente()
        {
            // Arrange
            var administrador = new Administrador();

            // Act
            var idInicial = administrador.Id;

            // Assert
            Assert.AreEqual(default(int),idInicial);
        }
    }
}