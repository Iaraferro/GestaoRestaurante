namespace GestaoRestaurante.Models
{
    public class ItemPedido: EntidadeBase
    {
        public int Quantidade { get; set; }
        public decimal PrecoMomento{  get; set; } // mostrar o preço na hora do pedido
        public decimal DescontoAplicado { get; set; }  // 0 ou 20% se era Sugestão do Chefe
        public decimal TotalItem { get; set; } // (PrecoMomento * Quantidade) - DescontoAplicado
        public string? Observacao { get; set; } // "sem cebola", "bem passado"...
        public int PedidoId { get; set; }
        public Pedido Pedido { get; set; } = null!;

        public int ItemCardapioId {  get; set; }
        public ItemCardapio ItemCardapio { get; set; } = null!;
    }
}
