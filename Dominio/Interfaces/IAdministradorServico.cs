using Minimal.Dominio.DTOs;
using Minimal.Dominio.Entidades;

namespace Minimal.Dominio.Interfaces;

public interface IAdministradorServico
{
    Administrador? Login( LoginDTO loginDTO );
    
}
