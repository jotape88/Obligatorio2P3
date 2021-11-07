using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio
{
    public class Actividad
    {
        #region Propiedades

        public int Id { get; set; }

        public string Nombre { get; set; }

        public int EdadMinima { get; set; }
        public int EdadMaxima { get; set; }

        #endregion

    }
}
