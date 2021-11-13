using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebObligatorio_2_P3.Models;
using Auxiliar;
using Dominio;

namespace WebObligatorio_2_P3.Controllers
{
    public class SocioController : Controller
    {
        #region Repositorios
        IRepoSocios repoSoc = FabricaRepositorios.ObtenerRepositorioSocios();
        IRepoPagarMensualidad repoMensualidad = FabricaRepositorios.ObtenerRepositorioPagarMensualidad();
        IRepoIngresosActividades repoIngresos = FabricaRepositorios.ObtenerRepositorioIngresosActividades();
        IRepoDiaYHora repoDyH = FabricaRepositorios.ObtenerRepositorioDiaYHora();
        #endregion
     
        #region ActionResults
        public ActionResult Index()
        {
            if (Session["usuarioLogueado"] == null)
            {
                return View("~/Views/Shared/NoAutorizado.cshtml");
            }
            List<Socio> losSoc = repoSoc.TraerTodo();
            List<ViewModelSocio> losSocModel = ConvertirListSocioAModel(losSoc);
            return View(losSocModel);  
        }
        public ActionResult Buscar()
        {
            if (Session["usuarioLogueado"] == null)
            {
                return View("~/Views/Shared/NoAutorizado.cshtml");
            }
            return View();
        }

        [HttpPost]
        public ActionResult Buscar(ViewModelSocio vMSocio)
        {
            if (Session["usuarioLogueado"] == null)
            {
                return View("~/Views/Shared/NoAutorizado.cshtml");
            }

            Socio unSoc = new Socio();
            unSoc = repoSoc.BuscarPorCedula(vMSocio.Cedula);

            if (unSoc != null && unSoc.EstaActivo != "0")
            {
                return RedirectToAction("DetalleSocio", new { unSoc.Id });
            }
            else
            {
                ViewBag.Error = "La cédula no se encuentra registrada en el sistema o el socio ya fue deshabilitado";
                return View();
            }
        }

        public ActionResult DetalleSocio(int id)
        {
            if (Session["usuarioLogueado"] == null)
            {
                return View("~/Views/Shared/NoAutorizado.cshtml");
            }
            Socio unSoc = new Socio();
            unSoc = repoSoc.BuscarPorId(id); 
            if (unSoc != null && unSoc.EstaActivo != "0")
            {
                ViewModelSocio viewModSoc = new ViewModelSocio()
                {
                    Id = unSoc.Id,
                    NombreYapellido = unSoc.NombreYapellido,
                    FechaNacimiento = unSoc.FechaNacimiento,
                    Cedula = unSoc.Cedula,
                    EstaActivo = unSoc.EstaActivo,
                    FechaRegistro = unSoc.FechaRegistro
                };
                IRepoPagarMensualidad repoPago = FabricaRepositorios.ObtenerRepositorioPagarMensualidad();
                DateTime ultFechaDePagoSocio = repoPago.BuscarUltFechaPagoXIdSocio(unSoc.Id);
                if(ultFechaDePagoSocio != DateTime.MinValue)
                {
                    if(ultFechaDePagoSocio.Date.Month == DateTime.Now.Date.Month)
                    {
                         ViewBag.TienePago = 1;
                    }
                    else
                    {
                         ViewBag.TienePago = 2;
                    }
                }

                return View(viewModSoc);
            }
            else
            {
                ViewBag.Error = "La cédula no se encuentra registrada en el sistema o el socio ya fue deshabilitado";
                return View();
            }
        }

        public ActionResult GestionPago(int id)
        {
            if (Session["usuarioLogueado"] == null)
            {
                return View("~/Views/Shared/NoAutorizado.cshtml");
            }
            Socio unSoc = new Socio();
            unSoc = repoSoc.BuscarPorId(id);
            ViewModelSocio viewModSoc = new ViewModelSocio()
            {
                Id = unSoc.Id,
                NombreYapellido = unSoc.NombreYapellido,
                FechaNacimiento = unSoc.FechaNacimiento,
                Cedula = unSoc.Cedula,
                EstaActivo = unSoc.EstaActivo,
                FechaRegistro = unSoc.FechaRegistro
            };
            return View(viewModSoc);
        }

        [HttpPost]
        public ActionResult GestionPago(int id, string tipoPago, int? ctdActiv)  
        {
            if (Session["usuarioLogueado"] == null)
            {
                return View("~/Views/Shared/NoAutorizado.cshtml");
            }
            PaseLibre unPase = new PaseLibre();
            Cuponera unaCupo = new Cuponera();
            Socio unSoc = new Socio();
            unSoc = repoSoc.BuscarPorId(id);

            ViewModelSocio viewModSoc = new ViewModelSocio()
            {
                Id = unSoc.Id,
                NombreYapellido = unSoc.NombreYapellido,
                FechaNacimiento = unSoc.FechaNacimiento,
                Cedula = unSoc.Cedula,
                EstaActivo = unSoc.EstaActivo,
                FechaRegistro = unSoc.FechaRegistro
            };

            TempData["tipoPago"] = tipoPago; 
            
            if (ctdActiv.HasValue && tipoPago == "cuponera")
            {
                if (ctdActiv.Value >= 8 && ctdActiv.Value <= 60)
                {
                    dynamic[] datos = { ctdActiv.Value };
                    decimal totalCuponera = unaCupo.CostoTotal(datos); 
                    ViewBag.Cuponera = totalCuponera;
                    TempData["ctdAct"] = ctdActiv;
                    return View(viewModSoc);
                }
                else
                {
                    ViewBag.Error = "La cantidad de actividades seleccionada debe estar comprendida entre 8 y 60";
                }
            }
            else if (tipoPago == "paseLibre")
            {
                dynamic[] datos = { unSoc.FechaRegistro };
                decimal totalPaselibre = unPase.CostoTotal(datos); 
                ViewBag.Cuponera = totalPaselibre;
                return View(viewModSoc);
            }
            return View(viewModSoc);
        }

        
        public ActionResult ConfirmarPago(int id)
        {
            if (Session["usuarioLogueado"] == null)
            {
                return View("~/Views/Shared/NoAutorizado.cshtml");
            }

            string tipoAct = TempData["tipoPago"].ToString();
            if(tipoAct == "cuponera")
            {
                int ctdAct = (int)TempData["ctdAct"];
                if (repoMensualidad.AltaPago(id, ctdAct))
                {
                    ViewBag.Success = "El pago se ha realizado correctamente";
                } 
                else
                {
                    ViewBag.Error = "Hubo un error al procesar el pago";
                }
            } else if (tipoAct == "paseLibre")
            {
                if (repoMensualidad.AltaPago(id))
                {
                    ViewBag.Success = "El pago se ha realizado correctamente";
                }
                else
                {
                    ViewBag.Error = "Hubo un error al procesar el pago";
                }
            }
            return View();
        }

        public ActionResult Create()
        {
            if (Session["usuarioLogueado"] == null)
            {
                return View("~/Views/Shared/NoAutorizado.cshtml");
            }
            return View();
        }

        [HttpPost]
        public ActionResult Create(ViewModelSocio vMSocio)
        {
            if (Session["usuarioLogueado"] == null)
            {
                return View("~/Views/Shared/NoAutorizado.cshtml");
            }
            Socio unSoc = new Socio(); 
           // if (unSoc.ValidarNomYApell(vMSocio.NombreYapellido) && unSoc.ValidarEdad(vMSocio.FechaNacimiento) && unSoc.ValidarCi(vMSocio.Cedula)) 
            if (unSoc.ValidarEdad(vMSocio.FechaNacimiento))
            {
                unSoc = repoSoc.BuscarPorCedula(vMSocio.Cedula); 
                if (unSoc == null)
                {
                    Socio socAux = new Socio()
                    {
                        Cedula = vMSocio.Cedula,
                        NombreYapellido = vMSocio.NombreYapellido,
                        FechaNacimiento = vMSocio.FechaNacimiento,
                    };

                    if (repoSoc.Alta(socAux))
                    {
                        ViewBag.Success = "El socio fue dado de alta correctamente";
                        return View();
                    }
                }
                else
                {
                    ViewBag.Error = "La cédula ya se encuentra registrada, intente nuevamente";
                    return View();
                }
            }
            else
            {
                ViewBag.Error = "La edad debe estar comprendida entre 3 y 90 años inclusive"; //Inclusive o exclusive? En la bd hay actividades de 3 y 90 años
                return View();
            }
            return View();
        }

        public ActionResult Edit(int id)
        {
            if (Session["usuarioLogueado"] == null)
            {
                return View("~/Views/Shared/NoAutorizado.cshtml");
            }

            Socio unSoc = new Socio();
            unSoc = repoSoc.BuscarPorId(id);
            ViewModelSocio vm = new ViewModelSocio()
            {
                NombreYapellido = unSoc.NombreYapellido,
                Cedula = unSoc.Cedula,
                FechaNacimiento = unSoc.FechaNacimiento
            };
            return View(vm);
        }

        [HttpPost]
        public ActionResult Edit(ViewModelSocio vmSocio)
        {
            if (Session["usuarioLogueado"] == null)
            {
                return View("~/Views/Shared/NoAutorizado.cshtml");
            }

            Socio unSoc = new Socio();
            unSoc = repoSoc.BuscarPorId(vmSocio.Id);
            if (unSoc != null && unSoc.EstaActivo != "0")
            {
                if(unSoc.ValidarEdad(vmSocio.FechaNacimiento) && unSoc.ValidarNomYApell(vmSocio.NombreYapellido))
                {
                    unSoc = new Socio()
                    {
                        Id = vmSocio.Id,
                        NombreYapellido = vmSocio.NombreYapellido,
                        FechaNacimiento = vmSocio.FechaNacimiento,
                    };
                    if (repoSoc.Modificacion(unSoc))
                    {
                        ViewBag.Success = "El socio se ha editado con éxito";
                        return View();
                    }
                    else
                    {
                        ViewBag.Error = "Hubo un error en la modificacion";
                        return View();
                    }
                }
                else
                {
                    ViewBag.Error = "El nombre y/o fecha de nacimiento ingresados no son válidos";
                    return View();
                }
            } else
            {
                ViewBag.Error = "El socio se encuentra deshabilitado";
                return View();
            }
        }

        public ActionResult Delete(int id)
        {
            if (Session["usuarioLogueado"] == null)
            {
                return View("~/Views/Shared/NoAutorizado.cshtml");
            }

            Socio unSoc = new Socio();
            unSoc = repoSoc.BuscarPorId(id);
            ViewModelSocio vm = new ViewModelSocio()
            {
                NombreYapellido = unSoc.NombreYapellido,
                Cedula = unSoc.Cedula,
                FechaNacimiento = unSoc.FechaNacimiento
            };
            return View(vm);
        }
        
        [HttpPost]
        public ActionResult Delete(ViewModelSocio vmSocio) 
        {
            if (Session["usuarioLogueado"] == null)
            {
                return View("~/Views/Shared/NoAutorizado.cshtml");
            }

            Socio unSoc = new Socio();
            unSoc = repoSoc.BuscarPorId(vmSocio.Id);
            if (unSoc != null && unSoc.EstaActivo != "0") 
            {
                ViewModelSocio viewModSoc = new ViewModelSocio() 
                {
                    Id = unSoc.Id,
                    NombreYapellido = unSoc.NombreYapellido,
                    FechaNacimiento = unSoc.FechaNacimiento,
                };
                if (repoSoc.Baja(viewModSoc.Id))
                {
                    ViewBag.Success = "El socio fue deshabilitado";
                    return View();
                }
                else
                {
                    ViewBag.Error = "Hubo un error durante la baja";
                    return View();
                }
            }
            else
            {
                ViewBag.Error = "La cédula no se encuentra registrada en el sistema o el socio ya fue deshabilitado";
                return View();
            }       
        }

        public ActionResult IngresoFechasLista(int id)
        {
            if (Session["usuarioLogueado"] == null)
            {
                return View("~/Views/Shared/NoAutorizado.cshtml");
            }

            TempData["idSocio"] = id;
            return View();
        }


        [HttpPost]
        public ActionResult ListarIngresos(DateTime? fechaInicio, DateTime? fechaFin) 
        {
            if (Session["usuarioLogueado"] == null)
            {
                return View("~/Views/Shared/NoAutorizado.cshtml");
            }

            int idSocio = (int)TempData["idSocio"];
            List<IngresoActividad> listaIngre = new List<IngresoActividad>();
            List<ViewModelIngresoActividad> listaIngrActVM = new List<ViewModelIngresoActividad>();

            var firstDayOfMonth = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1); //Trae el primer día del mes corriente

            if (fechaInicio == null && fechaFin == null)
            {
                listaIngre = repoIngresos.TraerTodoPorFechasXIdSocio(firstDayOfMonth, DateTime.Now, idSocio); //Retornamos los ingresos desde el primer dia del mes hasta el ultimo
                listaIngrActVM = ConvertirListIngActAModel(listaIngre);
                if(listaIngrActVM.Count == 0) //Si no tiene ingresos, devolvemos un mensaje
                {
                    ViewBag.Mensaje = "El socio no actividades durante las fechas proporcionadas";
                    return View();
                }
            }  if(fechaInicio.Value <= DateTime.Now) //Si la fecha de inicio es menor a la fecha actual entramos
                {
                if (fechaInicio.Value != null && fechaFin.Value != null && fechaInicio.Value <= fechaFin.Value)
                {
                    listaIngre = repoIngresos.TraerTodoPorFechasXIdSocio(fechaInicio.Value, fechaFin.Value, idSocio); //Retornamos una lista con los ingresos entre las fechas que ingreso el usuario
                    listaIngrActVM = ConvertirListIngActAModel(listaIngre);
                    if (listaIngrActVM.Count == 0)
                    {
                        ViewBag.Mensaje = "El socio no tiene actividades durante las fechas proporcionadas";
                        return View();
                    }
                }
                else if (fechaInicio.Value != null && fechaFin.Value != null && fechaInicio.Value >= fechaFin.Value)
                {
                    ViewBag.Error = "La fecha de inicio no puede ser superior a la fecha final";
                    return View();
                }
            } else
            {
                ViewBag.ErrorFechaInic = "La fecha inicial no puede ser superior a la actual";
                return View();
            }


            return View(listaIngrActVM);
        }
        #endregion

        #region Conversión Listas ViewModel
        private List<ViewModelSocio> ConvertirListSocioAModel(List<Socio> listaSocios)
        {
            List<ViewModelSocio> listaSocioModel = new List<ViewModelSocio>();
            foreach (Socio unSoc in listaSocios)
            {
                ViewModelSocio socioViewModel = new ViewModelSocio()
                {
                    Id = unSoc.Id,
                    Cedula = unSoc.Cedula,
                    NombreYapellido = unSoc.NombreYapellido,
                    FechaNacimiento = unSoc.FechaNacimiento,
                    EstaActivo = unSoc.EstaActivo,
                    FechaRegistro = unSoc.FechaRegistro
                };
                listaSocioModel.Add(socioViewModel);
            }
            return listaSocioModel;
        }

        private List<ViewModelIngresoActividad> ConvertirListIngActAModel(List<IngresoActividad> listaIngrAct)
        {
            List<ViewModelIngresoActividad> listaIngActModel = new List<ViewModelIngresoActividad>();
            foreach (IngresoActividad unIngAc in listaIngrAct)
            {
                ViewModelIngresoActividad ingresosViewModel = new ViewModelIngresoActividad()
                {
                    Id = unIngAc.Id,
                    FechaYHora = unIngAc.FechaYHora,
                    Soc = unIngAc.Soc,
                    DiaYHr = unIngAc.DiaYHr
                };
                listaIngActModel.Add(ingresosViewModel);
            }
            return listaIngActModel;
        }
        #endregion
    }
}
