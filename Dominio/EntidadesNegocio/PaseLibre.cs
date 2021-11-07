using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio
{
    public class PaseLibre : FormaPago
    {
        #region Propiedades
        public static decimal ValorMes { get; set; }

        public static int TopeAntiguedad { get; set; }

        public static decimal DescuentoPorAntiguedad { get; set; }
        #endregion

        #region Metodos
        public override decimal CostoTotal(dynamic[] datos)
        {
            decimal montoTotal = 0;

            DateTime fechaReg = datos[0];

            DateTime periodoTope = fechaReg.AddYears(TopeAntiguedad); //A la fecha de registro le agregamos dos anios (tope antiguedad), y si comparamos la fecha de ahora con la de registro + los dos anios,
                                                                      //si el if nos da ok es porque la fecha de registro es superior a dos anios
            if (DateTime.Now > periodoTope)
            {
                decimal descuento = ValorMes * (DescuentoPorAntiguedad / 100);
                montoTotal = ValorMes - descuento;
            }
            else
            {
                montoTotal = ValorMes;
            }
            return Decimal.Round(montoTotal); 
        }

        public override string Tipo()
        {
            return "PaseLibre";
        }
        #endregion
    }
}
