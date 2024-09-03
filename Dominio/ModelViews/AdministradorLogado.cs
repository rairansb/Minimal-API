using Minimal.Enuns;

namespace Minimal.Dominio.ModelViews;

public record AdministradorLogado
{ 

    public string Token { get; set; } = default!;
    public string Email { get; set; } = default!;
    public string Perfil { get; set; } = default!;
}
