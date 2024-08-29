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

    public Administrador? Login( LoginDTO loginDTO )
    {
        var adm = _contexto.Administradores.Where(a => a.Email == loginDTO.Email && a.Senha == loginDTO.Password).FirstOrDefault();
        return adm;
    }

}
