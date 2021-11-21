using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace WebObligatorio_2_P3.Models
{
    public class ViewModelDiaYHora
    {
        [Required, Display(Name = "Nombre de la actividad")]
        public string NombreActivdad { get; set; }

        [RegularExpression(@"^(?:Lunes|Martes|Miercoles|Jueves|Viernes)(?:\s*,\s*(?:Lunes|Martes|Miercoles|Jueves|Viernes))*$", ErrorMessage = "Debe ingresar un dia de la semana válido, la primer letra debe ser capitalizada")]
        [Required, Display(Name = "Día de la semana")]
        public string Dia { get; set; }
        [RegularExpression(@"^([7-9]|1[0-9]|2[0-3])$", ErrorMessage = "La hora de inicio debe estar comprendida entre las 7hrs y las 23hrs")]
        [Required, Display(Name = "Hora de inicio")]
        public int Hora { get; set; }
        [RegularExpression(@"^([3-9]|[1-8][0-9]|90)$", ErrorMessage = "La edad ingresada debe estar comprendida entre 3 y 90 años")]
        [Required, Display(Name = "Edad mínima")]
        public int EdadMinima { get; set; }
    }
}
