using System;
using System.Collections.Generic;

#nullable disable

namespace Web.ApiHinojosaPrueba.Modelos
{
    public partial class Cuenta
    {
        public Cuenta()
        {
            Movimientos = new HashSet<Movimiento>();
        }

        public string CueNumero { get; set; }
        public int CliIdCliente { get; set; }
        public string CueTipo { get; set; }
        public bool CueEstado { get; set; }
        public decimal CueSaldo { get; set; }

        public virtual Cliente CliIdClienteNavigation { get; set; }
        public virtual ICollection<Movimiento> Movimientos { get; set; }
    }
}
