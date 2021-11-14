using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;


namespace WebObligatorio_2_P3.Models
{
    public class ViewModelPagoMensualidad
    {
        public string TipoPago { get; set; }
        public DateTime FechaPago { get; set; }
        public decimal MontoPago { get; set; }
        public decimal DescuentoPago { get; set; }
        public string CedulaSocio { get; set; }
        public string NombreSocio { get; set; }
        
        [Required, Range(1,12, ErrorMessage = "Debe ingresar un mes válido")]
        public int Mes { get; set; } //Esto esta bien?
        [Required, Display(Name = "Año"), StringLength(4, MinimumLength = 4, ErrorMessage = "Debe ingresar un año válido")]
        public int Anio { get; set; } //Esto esta bien?
    }
}
