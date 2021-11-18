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

        //[Required, Display(Name = "Fecha inicio"), DataType(DataType.Date)]
        //public DateTime FechaInicio { get; set; }

        //[Required, Display(Name = "Fecha fin"), DataType(DataType.Date)]
        //public DateTime FechaFin { get; set; }

        //public Socio Soc { get; set; }

    }
}