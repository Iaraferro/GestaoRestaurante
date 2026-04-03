namespace GestaoRestaurante.Models
{
    public class Mesa : EntidadeBase
    {
        public int Numero {  get; set; }
        public required int Capacidade { get; set; }
        public List<Reserva> Reservas { get; set; } = new List<Reserva>();
    }
}
