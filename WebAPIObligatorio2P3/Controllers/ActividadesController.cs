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
                                 .ThenBy(a => a.Hora)
                                  .Where(a => a.Activ.Nombre.Contains(texto))
                                  .Select(a => new DTODiaYHora { NombreActividad = a.Activ.Nombre, Dia = a.Dia, Hora = a.Hora })
                                  .ToList().AsQueryable();                             
        }

        [Route("GetActividadesPorCotaMinimaEdad/{edadmin}")]
        public IQueryable<DTODiaYHora> GetActividadesPorCotaMinimaEdad(int edadMin)
        {

            var weekDayList = new List<string> { "Lunes", "Martes", "Miercoles", "Jueves", "Viernes" };

            return db.DiasYHoras.OrderBy(dh => dh.Activ.Nombre)
                     .ThenBy(dh => dh.Dia)
                     .ThenBy(a => a.Hora)
                      .Where(a => a.Activ.EdadMinima > edadMin)
                      .Select(a => new DTODiaYHora { NombreActividad = a.Activ.Nombre, Dia = a.Dia, Hora = a.Hora })
                      .ToList().AsQueryable();
        }

        [Route("GetActividadesPorDiaHr/{dia}/{hora}")]
        public IQueryable<DTODiaYHora> GetActividadesPorDiaHr(string dia, int hora)
        {
            return db.DiasYHoras.OrderBy(dh => dh.Activ.Nombre)
                     .ThenBy(dh => dh.Dia)
                     .ThenBy(a => a.Hora)
                      .Where(a => a.Dia == dia && a.Hora == hora)
                      .Select(a => new DTODiaYHora { NombreActividad = a.Activ.Nombre, Dia = a.Dia, Hora = a.Hora })
                      .ToList().AsQueryable();
        }


        //Consulta del punto 8
        [Route("GetIngresosActividadPorSocio/{cedula}/{nombreActiv}")]
        public IQueryable<DTOIngresos> GetIngresosActividadPorSocio(string cedula, string nombreActiv)
        {
            return db.IngresosActividades.Where(ia => ia.Soc.Cedula == cedula && ia.DiaYHr.Activ.Nombre == nombreActiv)
                                          .Select(ia => new DTOIngresos { FechaYHoraIngreso = ia.FechaYHora, Dia = ia.DiaYHr.Dia })  
                                          .OrderByDescending(ia => ia.FechaYHoraIngreso); //el dto tiene fecha de ingreso, ordenamos descendentemente por eso (sino hay que ponerlo arriba del dto y ahi ordenar por FechaYHora)
        }

        #endregion
    }
}