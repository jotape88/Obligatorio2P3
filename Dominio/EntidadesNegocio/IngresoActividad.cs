using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio
{
    public class IngresoActividad
    {
        #region Propiedades

        public int Id { get; set; }

        public DiaYHora DiaYHr { get; set; }

        public DateTime FechaYHora { get; set; }

        public Socio Soc { get; set; }
        #endregion

    }
}
