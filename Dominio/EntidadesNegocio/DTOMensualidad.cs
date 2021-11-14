using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio
{
    public class DTOMensualidad
    {
        public FormaPago TipoForma { get; set; }
        public DateTime FechaPago { get; set; }
        public decimal MontoPago { get; set; }
        public decimal DescuentoPago { get; set; }
        public string CedulaSocio { get; set; }
        public string NombreSocio { get; set; }  
    }
}
