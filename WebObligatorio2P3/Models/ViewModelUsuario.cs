using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations; 


namespace WebObligatorio_2_P3.Models
{
    public class ViewModelUsuario
    {
        #region Propiedades
        [RegularExpression(@"^[\w-]+(\.[\w-]+)*@([a-z0-9-]+(\.[a-z0-9-]+)*?\.[a-z]{2,6}|(\d{1,3}\.){3}\d{1,3})(:\d{4})?$", ErrorMessage = "Por favor, ingrese una dirección de correo válida")]
        [Required, Display(Name = "E-mail")]
        public string Email { get; set; }

        [Required, Display(Name = "Contraseña"), DataType(DataType.Password)]
        public string Contrasenia { get; set; }
        #endregion
    }
}