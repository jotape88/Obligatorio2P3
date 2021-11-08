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
        [Index (IsUnique = true), StringLength(20)] //Para el punto de validar el nombre y tampoco unique, porque tira exception y no ingresa el resto de las actividades cuando la letra pide que si
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

        public bool ValidarNombreAct(string nom)
        {
            //nom = nom.Trim();
            //return nom.Length > 2;
            return !string.IsNullOrWhiteSpace(nom); //Es lo mismo que lo anterior lo seteamos en false, porque sino nos devuelve true cuando se cumple la condicion
        }

        #endregion

    }
}
