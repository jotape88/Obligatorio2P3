using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio
{
    public interface IRepoPagarMensualidad: IRepositorio<PagarMensualidad>
    {
        bool AltaPago(int idSocio, int cantidadActiv = 0);
        DateTime BuscarUltFechaPagoXIdSocio(int idSocio);

        FormaPago BuscarUltFormaPago(int idSocio);
    }
}
