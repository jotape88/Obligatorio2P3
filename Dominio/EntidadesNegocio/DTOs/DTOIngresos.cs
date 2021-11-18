using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Dominio
{
    public class DTOIngresos
    {
        public DateTime FechaYHoraIngreso { get; set; }
        public string Dia { get; set; }
    }
}
