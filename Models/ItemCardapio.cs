
namespace GestaoRestaurante.Models
{
    public class ItemCardapio : EntidadeBase
    {
       public required string Nome { get; set; }
       public required string Descricao { get; set; }
       public required decimal Preco { get; set; }
       public required PeriodoCardapio Periodo { get; set; }
       public string? ImagemUrl { get; set; }

        public List<ItemIngrediente> Ingrediente { get; set; } = new();
      
       
    }
}
