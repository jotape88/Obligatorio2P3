using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dominio
{
    [Table("PagarMensualidades")]
    public class PagarMensualidad
    {
        #region Propiedades
        public int Id { get; set; }
        //[DatabaseGenerated(DatabaseGeneratedOption.None)] 
        public Socio UnSocio { get; set; }

        public FormaPago UnaFormaPago { get; set; }

        public DateTime FechaPago { get; set; }
        public decimal MontoPagado { get; set; }
        public decimal MontoDescontado { get; set; }

        #endregion

    }
}
