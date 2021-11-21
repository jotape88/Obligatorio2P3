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




        /// <summary>
        /// /////////////////////////////////////////////////////////////

        [Route("GetIngresosActividadPorSocio/{cedula}/{nombreActiv}")]
        public IQueryable<DTOIngresos> GetIngresosActividadPorSocio(string cedula, string nombreActiv)
        {
            return db.IngresosActividades.Where(ia => ia.Soc.Cedula == cedula && ia.DiaYHr.Activ.Nombre == nombreActiv)
                                          .Select(ia => new DTOIngresos { FechaYHoraIngreso = ia.FechaYHora, Dia = ia.DiaYHr.Dia })  
                                          .OrderByDescending(ia => ia.FechaYHoraIngreso); //el dto tiene fecha de ingreso, ordenamos descendentemente por eso (sino hay que ponerlo arriba del dto y ahi ordenar por FechaYHora)
        }

        #endregion

        #region ActionResults sin uso
        // GET: api/Actividades/5
        [ResponseType(typeof(Actividad))]
        public IHttpActionResult GetActividad(int id)
        {
            Actividad actividad = db.Actividades.Find(id);
            if (actividad == null)
            {
                return NotFound();
            }

            return Ok(actividad);
        }

        // PUT: api/Actividades/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutActividad(int id, Actividad actividad)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != actividad.Id)
            {
                return BadRequest();
            }

            db.Entry(actividad).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ActividadExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/Actividades
        [ResponseType(typeof(Actividad))]
        public IHttpActionResult PostActividad(Actividad actividad)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Actividades.Add(actividad);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (ActividadExists(actividad.Id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = actividad.Id }, actividad);
        }

        // DELETE: api/Actividades/5
        [ResponseType(typeof(Actividad))]
        public IHttpActionResult DeleteActividad(int id)
        {
            Actividad actividad = db.Actividades.Find(id);
            if (actividad == null)
            {
                return NotFound();
            }

            db.Actividades.Remove(actividad);
            db.SaveChanges();

            return Ok(actividad);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ActividadExists(int id)
        {
            return db.Actividades.Count(e => e.Id == id) > 0;
        }
        #endregion
    }
}