using Minimal.Dominio.DTOs;
using Minimal.Dominio.Entidades;
using Minimal.Dominio.Interfaces;
using Minimal.Infraestrutura.Db;

namespace Minimal.Dominio.Servicos;

public class VeiculoServico :IVeiculoServico
{
    private readonly DbContexto _contexto;
    public VeiculoServico( DbContexto contexto )
    {
        _contexto = contexto;
    }

    public void Apagar( Veiculo veiculo )
    {
        _contexto.Veiculos.Remove(veiculo);
        _contexto.SaveChanges();
    }

    public void Atualizar( Veiculo veiculo )
    {
        _contexto.Veiculos.Update(veiculo);
        _contexto.SaveChanges();
    }

    public Veiculo? BuscarPorId( int id )
    {
        return _contexto.Veiculos.Where(v => v.Id == id).FirstOrDefault();
    }

    public void Incluir( Veiculo veiculo )
    {
        _contexto.Veiculos.Add(veiculo);
        _contexto.SaveChanges();
    }

    public Administrador? Login( LoginDTO loginDTO )
    {
        throw new NotImplementedException();
    }

    public List<Veiculo> Todos( int? page = 1,string? modelo = null,string? marca = null )
    {
        var query = _contexto.Veiculos.AsQueryable();
        if(!string.IsNullOrEmpty(modelo)) {
            query = query.Where(v => v.Modelo.ToLower().Contains(modelo.ToLower()));
        }

        int itensPorPagina = 10;

        if(page != null) {
            query = query.Skip(((int)page - 1) * itensPorPagina).Take(itensPorPagina);
        }


        return query.ToList();
    }
}
