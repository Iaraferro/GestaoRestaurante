namespace GestaoRestaurante.Models
{
    public class AtendimentoPresencial : Atendimento
    {
        public override decimal CalcularTaxa(decimal valorPedido)
        {
            return 0;
        }
    }
}
