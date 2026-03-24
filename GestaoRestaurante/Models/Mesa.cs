namespace GestaoRestaurante.Models
{
    public class Mesa : EntidadeBase
    {
        public int Numero {  get; set; }
        public required string Capacidade { get; set; }
    }
}
