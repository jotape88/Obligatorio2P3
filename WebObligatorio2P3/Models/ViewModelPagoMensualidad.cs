using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;


namespace WebObligatorio_2_P3.Models
{
    public class ViewModelPagoMensualidad
    {
        [Display(Name = "Tipo de pase")]
        public string TipoPago { get; set; }
        [Display(Name = "Fecha de pago"), DataType(DataType.Date)] //Acorta la fecha
        public DateTime FechaPago { get; set; }
        [Display(Name = "Monto pagado"), DisplayFormat(DataFormatString = "{0:n0}")] //Solo muestra decimales en el monto
        public decimal MontoPago { get; set; }
        [Display(Name = "Monto de descuento aplicado"), DisplayFormat(DataFormatString = "{0:n0}")]
        public decimal DescuentoPago { get; set; }
        [Display(Name = "Cédula del socio")]
        public string CedulaSocio { get; set; }
        [Display(Name ="Nombres y apellidos del socio")]
        public string NombreSocio { get; set; }
   
        [Required, Range(1,12, ErrorMessage = "Debe ingresar un mes válido")]
        public int Mes { get; set; } //Esto esta bien?
        [Required, Display(Name = "Año"), StringLength(4, MinimumLength = 4, ErrorMessage = "Debe ingresar un año válido")]
        public int Anio { get; set; } //Esto esta bien?
    }
}
