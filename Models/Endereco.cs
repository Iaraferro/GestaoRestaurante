namespace GestaoRestaurante.Models
{
    public class Endereco : EntidadeBase
    {
        public required string Rua { get; set; }
        public required int Numero { get; set; }
        public string? Complemento { get; set; }
        public required string Bairro { get; set; }
        public required string Cidade { get; set; }
        public required string Estado { get; set; }
        public required string Cep { get; set; }
 
        // Chave estrangeira
        public int UsuarioId { get; set; }
        public Usuario Usuario { get; set; } = null!;
    }
}