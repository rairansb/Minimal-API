using Minimal.Dominio.DTOs;
using Minimal.Dominio.Entidades;
using Minimal.Dominio.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace TestProject1.Mocks
{
    internal class VeiculoServicoMock :IVeiculoServico
    {
        private static List<Veiculo> veiculos = new List<Veiculo>() {
            new Veiculo {
                Id = 1,
                Modelo = "Civic",
                Marca = "Honda",
                Ano = 2022
            },
            new Veiculo {
                Id = 2,
                Modelo = "Corolla", // Corrigido "Corola" para "Corolla"
                Marca = "Toyota",
                Ano = 2023
            }
        };

        public void Apagar( Veiculo veiculo )
        {
            var veiculoExistente = BuscarPorId(veiculo.Id);
            if(veiculoExistente != null) {
                veiculos.Remove(veiculoExistente);
            }
        }

        public void Atualizar( Veiculo veiculo )
        {
            var veiculoExistente = BuscarPorId(veiculo.Id);
            if(veiculoExistente != null) {
                veiculoExistente.Modelo = veiculo.Modelo;
                veiculoExistente.Marca = veiculo.Marca;
                veiculoExistente.Ano = veiculo.Ano;
            }
        }

        public Veiculo? BuscarPorId( int id )
        {
            return veiculos.FirstOrDefault(a => a.Id == id);
        }

        public List<Veiculo> Todos( int? page = 1,string? modelo = null,string? marca = null )
        {
            return veiculos;
        }

        public void Incluir( Veiculo veiculo )
        {
            veiculo.Id = veiculos.Any() ? veiculos.Max(v => v.Id) + 1 : 1; // Garante um novo ID válido
            veiculos.Add(veiculo);
        }
    }
}
