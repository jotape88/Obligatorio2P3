using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using Dominio;

using Repositorios;

namespace IngresoActividadWCF
{
    
    public class ServicioIngreso : IServicioIngreso
    {
        IRepoIngresosActividades repoIngAct = new RepoIngresosActividades();

        public bool AltaIngresoActividadDTO(DTOIngresoActividad nuevoIngreso)
        {
            if (nuevoIngreso == null)
            {
                return false;
            }
            else
            {
                IngresoActividad nuevoIng = new IngresoActividad()
                {
                    Soc = nuevoIngreso.Socio,
                    DiaYHr = nuevoIngreso.DiaYHr
                };
                return repoIngAct.Alta(nuevoIng);
            }
        }
    }
}

