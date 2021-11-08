using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dominio
{
    [Table("Actividades")]
    public class Actividad
    {
        #region Propiedades
        public int Id { get; set; }
        [Index (IsUnique = true), StringLength(20)]
        public string Nombre { get; set; }
        [Range(3, 90)]
        public int EdadMinima { get; set; }
        [Range(3, 90)]
        public int EdadMaxima { get; set; }
        #endregion

        #region Metodos
        public bool ValidarEdadActiv(int edadMinima, int edadMaxima)
        {
            return edadMinima < edadMaxima;
        }

        #endregion

    }
}
