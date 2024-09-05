using Minimal.Enuns;

namespace Minimal.Dominio.ModelViews;

public record AdministradorModelViews
{
    public int Id { get; set; } = default!;
    public string Name { get; set; } = default!;
    public string Email { get; set; } = default!;
    public string Perfil { get; set; } = default!;
}
