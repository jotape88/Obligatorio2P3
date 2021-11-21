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
                        EstaActivo = "1",
                        FechaRegistro = DateTime.Now
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
                ViewBag.Error = "La edad debe estar comprendida entre 3 y 90 años inclusive";
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
                if(unSoc.ValidarEdad(vmSocio.FechaNacimiento)) 
                {
                    unSoc = new Socio()
                    {
                        Id = vmSocio.Id,
                        NombreYapellido = vmSocio.NombreYapellido,
                        FechaNacimiento = vmSocio.FechaNacimiento,
                        Cedula = unSoc.Cedula,
                        EstaActivo = unSoc.EstaActivo,
                        FechaRegistro = unSoc.FechaRegistro
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
                    ViewBag.Error = "El socio debe tener por lo menos 3 años de edad";
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
        #endregion
    }
}
