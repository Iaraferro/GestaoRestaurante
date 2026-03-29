namespace GestaoRestaurante.Models
{
    public class Ingrediente : EntidadeBase
    {
        public required string Nome { get; set; }
        public string? Descricao { get; set; } //ex: Tomate italiano pelado
        public decimal EstoqueAtual {  get; set; } // quanto tem em estoque agora
        public decimal EstoqueMinimo { get; set; } // alerta quando estiver abaixo da média 
        public required string UnidadeMedida { get; set; } // "kg", "litros", "unidade"
        public string? DescricaoAlergeno { get; set; }   // "contém glúten"
        public bool Alergeno { get; set; } // contém glúten, lactose etc?
 
        public List<ItemIngrediente> ItensCardapio { get; set; } = new();
        
    }
}
