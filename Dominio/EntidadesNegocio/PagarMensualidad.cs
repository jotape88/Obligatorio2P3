using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio
{
    public class PagarMensualidad
    {
        #region Propiedades
        public int Id { get; set; }

        public Socio UnSocio { get; set; }

        public FormaPago UnaFormaPago { get; set; }

        public DateTime FechaPago { get; set; }

        #endregion

    }
}
