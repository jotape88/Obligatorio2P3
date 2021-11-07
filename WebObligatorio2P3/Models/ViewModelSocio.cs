using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Dominio;
using Auxiliar;
using System.ComponentModel.DataAnnotations; 

namespace WebObligatorio_2_P3.Models
{
    public class ViewModelSocio
    {
        #region Propiedades
        public int Id { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Ingrese un número válido")]
        [Required, Display(Name = "Cédula")]
        public string Cedula { get; set; }

        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Debe ingresar solo letras")]
        [Required, Display(Name = "Nombre y apellido")] 
        public string NombreYapellido { get; set; }

        [Required, Display(Name = "Fecha de Nacimiento"), DataType(DataType.Date)] //Agregado DataType para mostrar fecha corta
        public DateTime FechaNacimiento { get; set; }

        [Display(Name = "Activo")] 
        public string EstaActivo { get; set; }
        
        [Display(Name = "Fecha de Registro"), DataType(DataType.Date)] 
        public DateTime FechaRegistro { get; set; }
        #endregion
    }

}