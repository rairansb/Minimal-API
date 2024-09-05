using Minimal.Enuns;

namespace Minimal.Dominio.DTOs;

public class AdministradorDTO
{
    public string Name { get; set; } = default!;
    public string Email { get; set; } = default!;
    public string Password { get; set; } = default!;
    public Perfil? Perfil { get; set; } = default!;

}
