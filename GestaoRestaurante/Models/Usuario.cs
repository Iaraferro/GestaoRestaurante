namespace GestaoRestaurante.Models
{
    public class Usuario: EntidadeBase
    {
        public required string UserName { get; set; }
        public required string Password { get; set; }
        public required string Email { get; set; }

        public List<Pedido> Pedidos { get; set; } = new List<Pedido>();
        
        public List<Endereco> Enderecos { get; set; } = new List<Endereco>();
        public List<Reserva> Reservas { get; set; } = new List<Reserva>();
    }
}
