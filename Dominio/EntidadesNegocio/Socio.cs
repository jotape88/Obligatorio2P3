using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dominio
{
    [Table("Socios")]
    public class Socio
    {
        #region Propiedades

        public int Id { get; set; }
        [RegularExpression(@"^[0-9]{7,9}$")] //Solo admitimos de 7 a 9 digitos
        [Required, MaxLength(10)]
        public string Cedula { get; set; }
        [RegularExpression(@"^([a-zA-ZÀ-ÿ\u00f1\u00d1]+ )+[a-zA-ZÀ-ÿ\u00f1\u00d1]+$|^[a-zA-ZÀ-ÿ\u00f1\u00d1]{6,}$")] //Validamos que sean minimo 6 letras, unicamente espacios embebidos y admitimos caracteres especiales para nombres en español
        [Required, MaxLength(50)]
        public string NombreYapellido { get; set; }

        public DateTime FechaNacimiento { get; set; }
        [MaxLength(1)]
        public string EstaActivo { get; set; }

        public DateTime FechaRegistro { get; set; }

        #endregion

        #region Validaciones
        public bool ValidarCi (string numero)
        {
            return int.TryParse(numero, out int numeroInt) && numeroInt > 0 && numero.Length >= 7 && numero.Length <= 9;
        }

        public bool ValidarNomYApell(string nomYAp)
        {
            string nomYApTrim = nomYAp.Trim();
            int bandera = 0;
            int i = 0;           
            while (bandera <= 1 && i < nomYApTrim.Length)
            {
                if (nomYApTrim[i] == ' ') 
                {
                    bandera++;
                };
                i++;
            }
            if(bandera == 1)
            {
                return nomYAp.Length >= 6 && !nomYAp.Any(char.IsDigit);
            } else
            {
                return false;
            }
        }

        public bool ValidarEdad(DateTime fechaNacimiento)
        {
            int edad = DateTime.Today.Year - fechaNacimiento.Year;

            //si el mes es menor restamos un año directamente
            if (DateTime.Today.Month < fechaNacimiento.Month)
            {
                --edad;
            }
            //sino preguntamos si estamos en el mismo mes, si es el mismo preguntamos si el dia de hoy es menor al de la fecha de nacimiento
            else if (DateTime.Today.Month == fechaNacimiento.Month && DateTime.Today.Day < fechaNacimiento.Day)
            {
                --edad;
            }
            return edad >= 3 && edad <= 90; //lo cambie porque en la bd hay edades de 3 y 90
        }
        #endregion

    }
}
