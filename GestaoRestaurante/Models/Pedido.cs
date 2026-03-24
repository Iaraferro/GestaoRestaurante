using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace GestaoRestaurante.Models
{
    public class Pedido : EntidadeBase
    {
     
        //FK
        public int UsuarioId { get; set; }
        public Usuario usuario { get; set; }
        


    }
}
