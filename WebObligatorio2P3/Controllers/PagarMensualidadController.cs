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

    public class PagarMensualidadController : Controller
    {
        #region Repositorios
        IRepoSocios repoSoc = FabricaRepositorios.ObtenerRepositorioSocios();
        IRepoPagarMensualidad repoMensualidad = FabricaRepositorios.ObtenerRepositorioPagarMensualidad();
        #endregion

        #region ActionResults
        public ActionResult Index(int anio, int mes)
        {
            if (Session["usuarioLogueado"] == null)
            {
                return View("~/Views/Shared/NoAutorizado.cshtml");
            }
            ViewBag.Anio = anio;
            ViewBag.Mes = mes;
            List<DTOMensualidad> listaDtoMensualidades = repoMensualidad.ListarFormasPagoPorMesYAnio(mes, anio);
            if(listaDtoMensualidades.Count == 0)
            {
                ViewBag.SinResultados = "No se encontraron formas de pago vigentes en la fecha proporcionada";
                return View();
            }
            List<ViewModelPagoMensualidad> listVmPagoMensualidad = ConvertirFormasPagosVigentesAModel(listaDtoMensualidades);
            return View(listVmPagoMensualidad);
        }

        public ActionResult FormasPagosVigentesXFecha()
        {
            if (Session["usuarioLogueado"] == null)
            {
                return View("~/Views/Shared/NoAutorizado.cshtml");
            }

            return View();
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

            if (ctdActiv.HasValue && tipoPago == "cuponera")
            {
                if (ctdActiv.Value >= 8 && ctdActiv.Value <= 60)
                {
                    dynamic[] datos = { ctdActiv.Value };
                    decimal totalCuponera = unaCupo.CostoTotal(datos);
                    decimal descuentoCuponera = (Cuponera.ValorActividad * ctdActiv.Value) - totalCuponera;
                    ViewBag.Cuponera = totalCuponera;

                    dynamic[] arrayDatosPagos = { TempData["tipoPago"] = tipoPago, TempData["total"] = totalCuponera, TempData["desc"] = descuentoCuponera, TempData["ctdAct"] = ctdActiv };
                    TempData["datosPago"] = arrayDatosPagos;

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
                decimal descuentoPaseLibre = PaseLibre.ValorMes - totalPaselibre;

                dynamic[] arrayDatosPagos = { TempData["tipoPago"] = tipoPago, TempData["total"] = totalPaselibre, TempData["desc"] = descuentoPaseLibre };
                TempData["datosPago"] = arrayDatosPagos;

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
            Socio unSoc = repoSoc.BuscarPorId(id);
            dynamic[] datosPagos = (dynamic[])TempData["datosPago"];

            string tipoAct = datosPagos[0];
            decimal total = datosPagos[1];
            decimal descuento = datosPagos[2];
            

            if (tipoAct == "cuponera")
            {
                int ctdAct = datosPagos[3];
                if (repoMensualidad.AltaPago(id, total, descuento, ctdAct))
                {
                    ViewBag.Success = "El pago se ha realizado correctamente";
                }
                else
                {
                    ViewBag.Error = "Hubo un error al procesar el pago";
                }
            }
            else if (tipoAct == "paseLibre")
            {
                if (repoMensualidad.AltaPago(id, total, descuento))
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

        #endregion

        #region Metodos de conversion de listas
        private List<ViewModelPagoMensualidad> ConvertirFormasPagosVigentesAModel(List<DTOMensualidad> listaDtoMens)
        {
            List<ViewModelPagoMensualidad> listaMensualidadesModel = new List<ViewModelPagoMensualidad>();
            foreach (DTOMensualidad unaMens in listaDtoMens)
            {
                ViewModelPagoMensualidad socioViewModel = new ViewModelPagoMensualidad()
                {
                    TipoPago = unaMens.TipoForma.Tipo(),
                    FechaPago = unaMens.FechaPago,
                    NombreSocio = unaMens.NombreSocio,
                    MontoPago = unaMens.MontoPago,
                    DescuentoPago = unaMens.DescuentoPago,
                    CedulaSocio = unaMens.CedulaSocio
                };
                listaMensualidadesModel.Add(socioViewModel);
            }
            return listaMensualidadesModel;
            
        }
        #endregion
    }
}
