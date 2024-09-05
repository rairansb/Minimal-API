using Minimal.Dominio.DTOs;
using Minimal.Dominio.Entidades;

namespace Minimal.Dominio.Interfaces;

public interface IAdministradorServico
{
    Administrador? Login( LoginDTO loginDTO );
    Administrador Incluir( Administrador administrador );
    Administrador? BuscaPorId( int id );
    List<Administrador> Todos( int? page );
}
