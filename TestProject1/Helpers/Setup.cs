using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Minimal;
using Minimal.Dominio.Interfaces;
using TestProject1.Mocks;
using Microsoft.Extensions.DependencyInjection;

namespace TestProject1.Helpers;
internal class Setup
{
    public const string PORT = "5210";
    public static TestContext testContext = default!;
    public static WebApplicationFactory<Startup> http = default!;
    public static HttpClient client = default!;

    public static void ClassInit(TestContext testContext )
    {
        Setup.testContext = testContext;
        Setup.http = new WebApplicationFactory<Startup>();
        Setup.http = Setup.http.WithWebHostBuilder(builder => {

            builder.UseSetting("http_port",Setup.PORT).UseEnvironment("Testing");
            builder.ConfigureServices(services => {

                services.AddScoped<IAdministradorServico,AdministradorServicoMock>();
                
            });

        });
        Setup.client = Setup.http.CreateClient();
    }
     public static void ClassCleanup()
    {
        Setup.http.Dispose();
    }
    
}
