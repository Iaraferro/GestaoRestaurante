namespace GestaoRestaurante.Models
{
    public class ItemIngrediente: EntidadeBase
    {
        public decimal Quantidade { get; set; }
        public required string UnidadeMedida { get; set; }
        public int ItemCardapioId { get; set; }
        public ItemCardapio ItemCardapio { get; set; } = null!;

        public int IngredienteId { get; set; }
        public Ingrediente Ingrediente { get; set; } = null!;
    }
}
