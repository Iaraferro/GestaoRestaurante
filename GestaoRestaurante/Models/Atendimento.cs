namespace GestaoRestaurante.Models
{
    public abstract class Atendimento : EntidadeBase
    {
        public abstract decimal CalcularTaxa(decimal valorPedido);
    }
}
