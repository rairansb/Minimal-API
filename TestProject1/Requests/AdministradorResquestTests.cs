using Microsoft.Extensions.DependencyModel;
using Minimal.Dominio.DTOs;
using Minimal.Dominio.ModelViews;
using System.Text;
using System.Text.Json;
using TestProject1.Helpers;
using Xunit;
using Assert = Xunit.Assert;

namespace TestProject1.Resquests

{
    [TestClass]
    public class AdministradorRequestTests
    {
        [ClassInitialize]
        public static void ClassInit( TestContext testContext )
        {
            Setup.ClassInit(testContext);
        }

        [ClassCleanup]
        public static void ClassCleanup()
        {
            Setup.ClassCleanup();
        }
        [TestMethod]
        public async Task Administrador_DeveTerPropriedadesDefinidasCorretamente()
        {
            // Arrange
            var loginDTO = new LoginDTO {
            
                Email = "adm@teste.com",
                Password = "123456"
               
            };
            var content = new StringContent(JsonSerializer.Serialize(loginDTO), Encoding.UTF8, "Aplication/json");

            // Act
            var response = await Setup.client.PostAsync("Administradores/login",content);

            // Assert
            Assert.Equal(200,(float)response.StatusCode);

            var result = await response.Content.ReadAsStringAsync();
            var admLogado = JsonSerializer.Deserialize<AdministradorLogado>(result,new JsonSerializerOptions {
                PropertyNameCaseInsensitive = true
            });

            Assert.NotNull(admLogado?.Perfil ?? "");
            Assert.NotNull(admLogado?.Email ?? "");
            Assert.NotNull(admLogado?.Token ?? "");
        }

    }
}