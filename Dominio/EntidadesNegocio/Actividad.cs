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
        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)] //Usamos las ids del Archivo
        public int Id { get; set; }
        [Required, Index (IsUnique = true), MaxLength(20)]
        public string Nombre { get; set; }
        [Range(3, 90)]
        public int EdadMinima { get; set; }
        [Range(3, 90)]
        public int EdadMaxima { get; set; }
        #endregion

        //Validamos por DataAnnotations y a su vez también por métodos

        #region Metodos
        public bool ValidarEdadActiv(int edadMinima, int edadMaxima)
        {
            return edadMinima < edadMaxima;
        }

        public bool ValidarNombreAct(string nom)
        {
            return !string.IsNullOrWhiteSpace(nom); //Validamos que no hayan espacios en blanco o que sea null
        }

        #endregion

    }
}
