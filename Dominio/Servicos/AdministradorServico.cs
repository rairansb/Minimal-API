using Microsoft.EntityFrameworkCore;
using Minimal.Dominio.DTOs;
using Minimal.Dominio.Entidades;
using Minimal.Dominio.Interfaces;
using Minimal.Infraestrutura.Db;

namespace Minimal.Dominio.Servicos;

public class AdministradorServico :IAdministradorServico
{
    private readonly DbContexto _contexto;
    public AdministradorServico( DbContexto contexto )
    {
        _contexto = contexto;
    }

    public Administrador? BuscaPorId( int id )
    {
       return _contexto.Administradores.Where(a => a.Id == id).FirstOrDefault();
    }

    public Administrador Incluir( Administrador administrador )
    {
        _contexto.Administradores.Add(administrador);
        _contexto.SaveChanges();

        return administrador;
    }

    public Administrador? Login( LoginDTO loginDTO )
    {
        var adm = _contexto.Administradores.Where(a => a.Email == loginDTO.Email && a.Senha == loginDTO.Password).FirstOrDefault();
        return adm;
    }

    public List<Administrador> Todos( int? page )
    {
        var query = _contexto.Administradores.AsQueryable();

        int itensPorPagina = 10;

        if(page != null) {
            query = query.Skip(((int)page - 1) * itensPorPagina).Take(itensPorPagina);
        }


        return query.ToList();
    }
}
