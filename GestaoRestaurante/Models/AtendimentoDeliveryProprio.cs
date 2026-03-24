namespace GestaoRestaurante.Models
{
    public class AtendimentoDeliveryProprio : Atendimento
    {
        public decimal TaxaFixa { get; set; }

        public override decimal CalcularTaxa(decimal valorPedido)
        {
            return TaxaFixa;
        }
    }
}
