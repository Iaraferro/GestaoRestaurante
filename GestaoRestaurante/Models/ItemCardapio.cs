
namespace GestaoRestaurante.Models
{
    public class ItemCardapio : EntidadeBase
    {
       public required string Nome { get; set; }
       public required string Descricao { get; set; }
       public required double Preco { get; set; }
       public required string Periodo { get; set; } // Almoço ou Jantar
       
    }
}
