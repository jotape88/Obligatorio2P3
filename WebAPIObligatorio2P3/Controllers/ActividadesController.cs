using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using Dominio;
using Repositorios;


namespace WebAPIObligatorio2P3.Controllers
{
    [RoutePrefix("obligatorio/actividades")]
    public class ActividadesController : ApiController
    {
        private ClubContext db = new ClubContext();

        #region ActionResults
        //Consultas del punto 7
        [Route("GetActividadesPorNombre/{texto}")]
        public IQueryable<DTODiaYHora> GetActividadesPorNombre(string texto)
        {
            return db.DiasYHoras.OrderBy(dh => dh.Activ.Nombre)
                                 .ThenBy(dh => dh.Dia)
                                 .ThenBy(dh => dh.Hora)
                                  .Where(dh => dh.Activ.Nombre.Contains(texto))
                                  .Select(dh => new DTODiaYHora { NombreActividad = dh.Activ.Nombre, Dia = dh.Dia, Hora = dh.Hora })
                                  .ToList().AsQueryable();                             
        }

        [Route("GetActividadesPorCotaMinimaEdad/{edadmin}")]
        public IQueryable<DTODiaYHora> GetActividadesPorCotaMinimaEdad(int edadMin)
        {

            var weekDayList = new List<string> { "Lunes", "Martes", "Miercoles", "Jueves", "Viernes" };

            return db.DiasYHoras.OrderBy(dh => dh.Activ.Nombre)
                     .ThenBy(dh => dh.Dia)
                     .ThenBy(dh => dh.Hora)
                      .Where(dh => dh.Activ.EdadMinima > edadMin)
                      .Select(dh => new DTODiaYHora { NombreActividad = dh.Activ.Nombre, Dia = dh.Dia, Hora = dh.Hora, EdadMinima = dh.Activ.EdadMinima })
                      .ToList().AsQueryable();
        }

        [Route("GetActividadesPorDiaHr/{dia}/{hora}")]
        public IQueryable<DTODiaYHora> GetActividadesPorDiaHr(string dia, int hora)
        {
            return db.DiasYHoras.OrderBy(dh => dh.Activ.Nombre)
                     .ThenBy(dh => dh.Dia)
                     .ThenBy(dh => dh.Hora)
                      .Where(dh => dh.Dia == dia && dh.Hora == hora)
                      .Select(dh => new DTODiaYHora { NombreActividad = dh.Activ.Nombre, Dia = dh.Dia, Hora = dh.Hora })
                      .ToList().AsQueryable();
        }


        //Consulta del punto 8
        [Route("GetIngresosActividadPorSocio/{cedula}/{nombreActiv}")]
        public IQueryable<DTOIngresos> GetIngresosActividadPorSocio(string cedula, string nombreActiv)
        {
            return db.IngresosActividades.Where(ia => ia.Soc.Cedula == cedula && ia.DiaYHr.Activ.Nombre == nombreActiv)
                                          .Select(ia => new DTOIngresos { FechaYHoraIngreso = ia.FechaYHora, Dia = ia.DiaYHr.Dia })  
                                          .OrderByDescending(ia => ia.FechaYHoraIngreso);
        }

        #endregion
    }
}