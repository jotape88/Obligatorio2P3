using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio
{
    public interface IRepoFormasPago : IRepositorio<FormaPago>
    {
        dynamic[] TraerAuxiliares();

        bool ModificacionCantCuponera(Cuponera unaCuponera);

    }
}
