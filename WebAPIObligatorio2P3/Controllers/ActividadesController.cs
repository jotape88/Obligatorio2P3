using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web.Http;
using Repositorios;


namespace WebAPIObligatorio2P3.Controllers
{
    [RoutePrefix("obligatorio/actividades")]
    public class ActividadesController : ApiController
    {
        #region Contexto
        private ClubContext db = new ClubContext();
        #endregion

        #region ActionResults
        //Consultas del punto 7
        [Route("GetActividadesPorNombre/{texto}")]
        public IHttpActionResult GetActividadesPorNombre(string texto)
        {
            return Ok(db.DiasYHoras.OrderBy(dh => dh.Activ.Nombre)
                                 .ThenBy(dh => dh.Dia)
                                 .ThenBy(dh => dh.Hora)
                                  .Where(dh => dh.Activ.Nombre.Contains(texto))
                                  .Select(dh => new  { NombreActividad = dh.Activ.Nombre, Dia = dh.Dia, Hora = dh.Hora })
                                  .ToList());                             
        }

        [Route("GetActividadesPorCotaMinimaEdad/{edadmin}")]
        public IHttpActionResult GetActividadesPorCotaMinimaEdad(int edadMin)
        {
            return Ok(db.DiasYHoras.OrderBy(dh => dh.Activ.Nombre)
                     .ThenBy(dh => dh.Dia)
                     .ThenBy(dh => dh.Hora)
                      .Where(dh => dh.Activ.EdadMinima >= edadMin)
                      .Select(dh => new  { NombreActividad = dh.Activ.Nombre, Dia = dh.Dia, Hora = dh.Hora, EdadMinima = dh.Activ.EdadMinima })
                      .ToList());
        }

        [Route("GetActividadesPorDiaHr/{dia}/{hora}")]
        public IHttpActionResult GetActividadesPorDiaHr(string dia, int hora)
        {
            return Ok(db.DiasYHoras.OrderBy(dh => dh.Activ.Nombre)
                     .ThenBy(dh => dh.Dia)
                     .ThenBy(dh => dh.Hora)
                      .Where(dh => dh.Dia == dia && dh.Hora == hora)
                      .Select(dh => new { NombreActividad = dh.Activ.Nombre, Dia = dh.Dia, Hora = dh.Hora })
                      .ToList());
        }


        //Consulta del punto 8
        [Route("GetIngresosActividadPorSocio/{cedula}/{nombreActiv}")]
        public IHttpActionResult GetIngresosActividadPorSocio(string cedula, string nombreActiv)
        {
            return Ok(db.IngresosActividades.Where(ia => ia.Soc.Cedula == cedula && ia.DiaYHr.Activ.Nombre == nombreActiv)
                                          .Select(ia => new { FechaYHoraIngreso = ia.FechaYHora, Dia = ia.DiaYHr.Dia })  
                                          .OrderByDescending(ia => ia.FechaYHoraIngreso)
                                          .ToList());
        }
        #endregion
    }
}
