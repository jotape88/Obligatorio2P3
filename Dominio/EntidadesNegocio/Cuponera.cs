using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio
{
    public class Cuponera : FormaPago
    {
        #region Propiedades

        public int CantidadActividades { get; set; }
        public static decimal ValorActividad { get; set; }

        public static decimal DescPorTopeActiv { get; set; }

        public static int TopeActividades { get; set; }

        #endregion

        #region Metodos
        public override decimal CostoTotal(dynamic[] datos)
        {
            decimal montoTotal = 0;

            int cantActividades = datos[0];

            decimal subTotalSinDesc = cantActividades * ValorActividad;

            if(cantActividades > TopeActividades)
            {
                montoTotal = subTotalSinDesc - DescPorTopeActiv;
            }
            else
            {
                montoTotal = subTotalSinDesc;
            }
            return Decimal.Round(montoTotal); 
        }

        public override string Tipo()
        {
            return "Cuponera";
        }

        #endregion
    }
}
