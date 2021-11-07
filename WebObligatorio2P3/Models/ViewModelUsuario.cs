using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Dominio;
using Auxiliar;
using System.ComponentModel.DataAnnotations; 


namespace WebObligatorio_2_P3.Models
{
    public class ViewModelUsuario
    {
        #region Propiedades
        [Required, DataType(DataType.EmailAddress)] //Validamos mail por data annotations
        public string Email { get; set; }

        [Required, Display(Name = "Contraseña"), DataType(DataType.Password)]
        public string Contrasenia { get; set; }
        #endregion
    }
}