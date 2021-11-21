using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio
{
    public class DTODiaYHora
    {
        public string NombreActividad { get; set; }
        public string Dia { get; set; }
        public int Hora { get; set; }
        public int EdadMinima { get; set; } //Para complementar la informacion en la vista del punto 7, cuando filtramos por edad
    }
}
