using System;
using System.Collections.Generic;

#nullable disable

namespace Web.ApiHinojosaPrueba.Modelos
{
    public partial class Cliente: Persona
    {
        public Cliente()
        {
            Cuenta = new HashSet<Cuenta>();
        }

        public int CliIdCliente { get; set; }
        public string CliContrasenia { get; set; }
        public bool CliEstado { get; set; }

        public virtual ICollection<Cuenta> Cuenta { get; set; }
    }
}
