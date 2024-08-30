using Minimal.Dominio.Entidades;

namespace Minimal.Dominio.Interfaces;

public interface IVeiculoServico
{
    List<Veiculo> Todos( int? page = 1, string? modelo = null, string? marca = null );

    Veiculo? BuscarPorId( int id );

    void Incluir(Veiculo veiculo );

    void Atualizar(Veiculo veiculo );

    void Apagar( Veiculo veiculo );

    
}
