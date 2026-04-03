using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace GestaoRestaurante.Models
{
    public class Pedido : EntidadeBase
    {
        public DateTime DataHoraPedido { get; set; }
        public StatusPedido Status {  get; set; }
        public PeriodoCardapio Periodo { get; set; }
        public decimal TotalItens { get; set; }
        public decimal TaxaAtendimento { get; set; }
        public decimal TotalFinal {  get; set; }

        public MetodoPagamento MetodoPagamento { get; set; } = new MetodoPagamento();
        //FK
        public int UsuarioId { get; set; }
        public Usuario Usuario { get; set; } = null!;
        
        public int AtendimentoId { get; set; }
        public Atendimento Atendimento { get; set; } = null!;
        public List<ItemPedido> Itens { get; set; } = new();


    }
}
