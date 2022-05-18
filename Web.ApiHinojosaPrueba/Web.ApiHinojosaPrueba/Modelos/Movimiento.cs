using System;
using System.Collections.Generic;

#nullable disable

namespace Web.ApiHinojosaPrueba.Modelos
{
    public partial class Movimiento
    {
        public int MovIdMovimiento { get; set; }
        public string CueNumero { get; set; }
        public DateTime MovFecha { get; set; }
        public string MovTipo { get; set; }
        public decimal MovSaldoInicial { get; set; }
        public decimal MovMovimientoValor { get; set; }
        public decimal MovSaldoActual { get; set; }

        public virtual Cuenta MoNumeroCuentaNavigation { get; set; }
    }
}
