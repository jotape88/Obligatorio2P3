using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio
{
    public interface IRepoIngresosActividades : IRepositorio<IngresoActividad>
    {
        int ValidarCupos(int idDiaYHora);

        bool YaIngresoActividad(int idSocio, int idActividad);

        List<IngresoActividad> TraerTodoPorFechasXIdSocio(DateTime fechaInicio, DateTime FechaFin, int idSocio);

    }
}
