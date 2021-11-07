using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Auxiliar;
using Dominio;
using WebObligatorio2P3.ServicioIngresoActividad;

namespace WebObligatorio_2_P3.Controllers
{
    public class ActividadController : Controller
    {
        #region Repositorios
        IRepoSocios repoSoc = FabricaRepositorios.ObtenerRepositorioSocios();
        IRepoActividades repoAct = FabricaRepositorios.ObtenerRepositorioActividades();
        IRepoIngresosActividades repoIngreso = FabricaRepositorios.ObtenerRepositorioIngresosActividades();
        IRepoPagarMensualidad repoMensualidad = FabricaRepositorios.ObtenerRepositorioPagarMensualidad();
        IRepoDiaYHora repoDyHr = FabricaRepositorios.ObtenerRepositorioDiaYHora();
        IRepoFormasPago repoFormasPago = FabricaRepositorios.ObtenerRepositorioFormasPagos();
        #endregion

        #region ActionResults
        // GET: Actividad
        public ActionResult Index(int id)
        {
            if (Session["usuarioLogueado"] == null)
            {
                return View("~/Views/Shared/NoAutorizado.cshtml");
            }
            decimal hora = DateTime.Now.Hour;
            string dia = DateTime.Now.ToString("dddd");
            String diaMayus = dia.Substring(0, 1).ToUpper() + dia.Substring(1);
            List<DiaYHora> lashrsYdias = repoDyHr.TraerTodoFiltrado(diaMayus, hora);

            TempData["idSocio"] = id;

            return View(lashrsYdias);
        }

        // GET: Actividad/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Actividad/Create
        public ActionResult Create()
        {
            return View();
        }

        public ActionResult ConfirmarIngreso(int idFechaYhora)
        {
            if (Session["usuarioLogueado"] == null)
            {
                return View("~/Views/Shared/NoAutorizado.cshtml");
            }

            int idSoc = (int)TempData["idSocio"];
            Socio unSoc = repoSoc.BuscarPorId(idSoc);        
            DiaYHora unDyH = repoDyHr.BuscarPorId(idFechaYhora);
            Actividad unaAct = repoAct.BuscarPorId(unDyH.Activ.Id);
            int edad = DateTime.Today.Year - unSoc.FechaNacimiento.Year;

            if (DateTime.Today.Month < unSoc.FechaNacimiento.Month)
            {
                --edad;
            }
            else if (DateTime.Today.Month == unSoc.FechaNacimiento.Month && DateTime.Today.Day < unSoc.FechaNacimiento.Day)
            {
                --edad;
            }

            if (edad >= unaAct.EdadMinima && edad <= unaAct.EdadMaxima)
            {
                if ((unDyH.CuposMaximos - repoIngreso.ValidarCupos(unDyH.Id)) >  0)
                {
                    if(!repoIngreso.YaIngresoActividad(idSoc, unDyH.Activ.Id))
                    {
                        FormaPago unaForma = repoMensualidad.BuscarUltFormaPago(idSoc);

                        if (unaForma is PaseLibre)
                        {
                            DTOIngresoActividad nuevoIngDTO = new DTOIngresoActividad()
                            {
                                DiaYHr = unDyH,
                                Socio = unSoc
                            };

                            try
                            {
                                ServicioIngresoClient proxy = new ServicioIngresoClient();
                                proxy.AltaIngresoActividadDTO(nuevoIngDTO);

                                ViewBag.Success = "Se ingreso correctamente a la actividad";
                            }
                            catch (Exception laEx)
                            {
                                Console.WriteLine("No se pudo dar de alta la actividad, error: " + laEx.Message);
                                
                            }

                        } else if(unaForma is Cuponera)
                        {
                            Cuponera cuponera = (Cuponera)unaForma;                
                            if (cuponera.CantidadActividades > 0)
                            {
                                if (repoFormasPago.ModificacionCantCuponera(cuponera))
                                {
                                    DTOIngresoActividad nuevoIngDTO = new DTOIngresoActividad()
                                    {
                                        DiaYHr = unDyH,
                                        Socio = unSoc
                                    };
                                    try
                                    {
                                        ServicioIngresoClient proxy = new ServicioIngresoClient();
                                        proxy.AltaIngresoActividadDTO(nuevoIngDTO);

                                        ViewBag.Success = "Se ingreso correctamente a la actividad";
                                    }
                                    catch (Exception laEx)
                                    {
                                        Console.WriteLine("No se pudo dar de alta la actividad, error: " + laEx.Message);

                                    }
                                }
                            } 
                            else
                            {
                                ViewBag.Error = "No le quedan cupos disponibles al socio";
                            }
                        }         
                    } else
                    {
                        ViewBag.Error = "El socio ya realizó esta actividad el dia de hoy";
                    }
                } else
                {
                    ViewBag.Error = "No le quedan cupos disponibles a la actividad";
                }

            } else
            {
                ViewBag.Error = "No cumple con la edad necesaria";
            }
         
            return View();
        }


        // POST: Actividad/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Actividad/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Actividad/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Actividad/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Actividad/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
        #endregion
    }
}
