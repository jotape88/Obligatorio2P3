using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio
{
    public interface IRepoDiaYHora: IRepositorio<DiaYHora>
    {

        List<DiaYHora> TraerTodoFiltrado(string dia, decimal hora);

    }
}
