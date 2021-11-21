using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Dominio;
using System.ComponentModel.DataAnnotations;

namespace WebObligatorio_2_P3.Models
{
    public class ViewModelIngresoActividad
    {
        public int Id { get; set; }

        [Required, Display(Name = "Fecha"), DataType(DataType.Date)] //Requeried es para que sea necesario ingresarlo, DataType.Date es para acortar la fecha en la vista
        public DateTime FechaYHora { get; set; }

        public string Dia { get; set; }

        public TimeSpan Hora { get; set; }

        [Required, Display(Name = "Nombre de la actividad")]
        public string NombreActiv { get; set; }

        [RegularExpression(@"^[0-9]{7,9}$", ErrorMessage = "Ingrese un cédula válida de entre 7 y 9 dígitos sin puntos ni guíones")]
        [Required, MaxLength(10), Display(Name = "Cédula del socio")]
        public string CedulaSocio { get; set; }   
    }
}