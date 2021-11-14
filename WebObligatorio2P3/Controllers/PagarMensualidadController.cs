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
        public ActionResult Index(int anio, int mes)
        {
            if (Session["usuarioLogueado"] == null)
            {
                return View("~/Views/Shared/NoAutorizado.cshtml");
            }
            IRepoPagarMensualidad repoPagarM = FabricaRepositorios.ObtenerRepositorioPagarMensualidad();
            List<DTOMensualidad> listaDtoMensualidades = repoPagarM.ListarFormasPagoPorMesYAnio(mes, anio);
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
    }
}
