using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dominio
{
    [Table("DiasYHoras")]
    public class DiaYHora
    {
        #region Propiedades
        public int Id { get; set; }
        public string Dia { get; set; }
        public decimal Hora { get; set; }
        public Actividad Activ { get; set; }


        public int CuposMaximos { get; set; }
        #endregion

        #region Metodos
        public bool ValidarDiaYHora(string unDia, decimal unaHora)
        {
            bool bandera = false;

            if(unDia == "Lunes" || unDia == "Martes" || unDia == "Miercoles" || unDia == "Jueves" || unDia == "Viernes")
            {
                decimal contador = 7;
                while (contador <= 23)
                {
                    if ((Int32)unaHora == contador)
                    {
                        return true;
                    }
                    contador++;
                }
            }
            return bandera;
        }
        #endregion
    }
}
