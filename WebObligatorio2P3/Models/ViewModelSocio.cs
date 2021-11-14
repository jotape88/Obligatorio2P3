using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations; 

namespace WebObligatorio_2_P3.Models
{
    public class ViewModelSocio
    {
        #region Propiedades
        public int Id { get; set; }
        [RegularExpression(@"^[0-9]{7,9}$", ErrorMessage = "Ingrese un cédula válida de entre 7 y 9 dígitos sin puntos ni guíones")] //Modificado
        [Required, Display(Name = "Cédula"), StringLength(10)]
        public string Cedula { get; set; }

        [RegularExpression(@"^([a-zA-ZÀ-ÿ\u00f1\u00d1]+ )+[a-zA-ZÀ-ÿ\u00f1\u00d1]+$|^[a-zA-ZÀ-ÿ\u00f1\u00d1]{6,}$", ErrorMessage = "Mínimo 6 caracteres, debe ingresar solo letras y sin espacios en blanco ni al inicio ni al final")] //Modificado                                                                                                                                                                      //Modificado
        [Required, Display(Name = "Nombre y apellido")] 
        public string NombreYapellido { get; set; }

        [Required, Display(Name = "Fecha de Nacimiento"), DataType(DataType.Date)] //DataType.Date muestra una fecha corta
        public DateTime FechaNacimiento { get; set; }

        [Display(Name = "Activo")] 
        public string EstaActivo { get; set; }
        
        [Display(Name = "Fecha de Registro"), DataType(DataType.Date)] 
        public DateTime FechaRegistro { get; set; }
        #endregion
    }

}