using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebObligatorio_2_P3.Models;
using Auxiliar;
using Dominio;

using System.Configuration;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace WebObligatorio_1_P3.Controllers
{
    public class ActividadController : Controller
    {
        //Punto 7
        public ActionResult BusquedaActividadPorParametros()
        {
            if (Session["usuarioLogueado"] == null)
            {
                return View("~/Views/Shared/NoAutorizado.cshtml");
            }
            return View();
        }
        [HttpPost]
        public ActionResult ListarActividadesPorTextoEnNombre(string NombreActivdad, string Dia, int? Hora, int? EdadMinima)
        {
            if (Session["usuarioLogueado"] == null)
            {
                return View("~/Views/Shared/NoAutorizado.cshtml");
            }
            List<DTODiaYHora> dtoDiasYHrs = new List<DTODiaYHora>();

            string laUbicacion = ConfigurationManager.AppSettings["UbicacionWebAPI"];
            string laUrl = laUbicacion + "obligatorio/actividades/GetActividadesPorNombre/";
            Uri laUri = new Uri(laUrl + NombreActivdad);

            HttpClient proxy = new HttpClient();
            Task<HttpResponseMessage> tarea1 = proxy.GetAsync(laUri);

            tarea1.Wait();

            HttpResponseMessage laRespuesta = tarea1.Result;

            if (laRespuesta.IsSuccessStatusCode)
            {
                Task<string> tarea2 = laRespuesta.Content.ReadAsStringAsync();
                tarea2.Wait();

                string contenidoJson = tarea2.Result;

                dtoDiasYHrs = JsonConvert.DeserializeObject<List<DTODiaYHora>>(contenidoJson);
            }
            else
            {
                ViewBag.Error = "No se pudieron obtener los ingresos: " + laRespuesta.StatusCode;
            }
            List<ViewModelDiaYHora> listaIngresos = ConvertirDiasYHrsAModel(dtoDiasYHrs);
            return View(listaIngresos);

        }

        private List<ViewModelDiaYHora> ConvertirDiasYHrsAModel(List<DTODiaYHora> ingresosDiasYHrs)
        {
            List<ViewModelDiaYHora> diasHrsViewModel = new List<ViewModelDiaYHora>();
            foreach (DTODiaYHora unDhr in ingresosDiasYHrs)
            {
                //TimeSpan horaYMins = new TimeSpan(unDto.FechaYHoraIngreso.Hour, unDto.FechaYHoraIngreso.Minute, unDto.FechaYHoraIngreso.Second); //A partir de la fecha (que contiene fecha hora y minutos) creamos un timespan con solo la hora y los minutos
                ViewModelDiaYHora vmDiaHr = new ViewModelDiaYHora()
                {
                    NombreActivdad = unDhr.NombreActividad,
                    Dia = unDhr.Dia,
                    Hora = unDhr.Hora
                };
                diasHrsViewModel.Add(vmDiaHr);
            }
            return diasHrsViewModel;
        }



        //Punto 8
        public ActionResult IngresoNombreActYCedulaSocio()
        {
            if (Session["usuarioLogueado"] == null)
            {
                return View("~/Views/Shared/NoAutorizado.cshtml");
            }
            return View();
        }
        [HttpPost]
        public ActionResult ListarIngresosActXSocio(string CedulaSocio, string NombreActiv)
        {
            if (Session["usuarioLogueado"] == null)
            {
                return View("~/Views/Shared/NoAutorizado.cshtml");
            }
            ViewBag.CedulaSocio = CedulaSocio;
            ViewBag.NombreActividad = NombreActiv;

            List<DTOIngresos> ingresosActivs = new List<DTOIngresos>();
            string laUbicacion = ConfigurationManager.AppSettings["UbicacionWebAPI"];
            string laUrl = laUbicacion + "obligatorio/actividades/GetIngresosActividadPorSocio/";
            Uri laUri = new Uri(laUrl+ CedulaSocio + "/"+ NombreActiv);

            HttpClient proxy = new HttpClient();
            Task<HttpResponseMessage> tarea1 = proxy.GetAsync(laUri);

            tarea1.Wait();

            HttpResponseMessage laRespuesta = tarea1.Result;

            if (laRespuesta.IsSuccessStatusCode)
            {
                Task<string> tarea2 = laRespuesta.Content.ReadAsStringAsync();
                tarea2.Wait();

                string contenidoJson = tarea2.Result;

                ingresosActivs = JsonConvert.DeserializeObject<List<DTOIngresos>>(contenidoJson);
            }
            else
            {
                ViewBag.Error = "No se pudieron obtener los ingresos: " + laRespuesta.StatusCode;
            }
            List<ViewModelIngresoActividad> listaIngresos = ConvertirListIngresosAModel(ingresosActivs);
            return View(listaIngresos);

        }


        #region Metodos de conversion de listas
        private List<ViewModelIngresoActividad> ConvertirListIngresosAModel(List<DTOIngresos> ingresosActivs)
        {
            List<ViewModelIngresoActividad> ingresosActViewModel = new List<ViewModelIngresoActividad>();
            foreach (DTOIngresos unDto in ingresosActivs)
            {
                TimeSpan horaYMins = new TimeSpan(unDto.FechaYHoraIngreso.Hour, unDto.FechaYHoraIngreso.Minute, unDto.FechaYHoraIngreso.Second); //A partir de la fecha (que contiene fecha hora y minutos) creamos un timespan con solo la hora y los minutos
                ViewModelIngresoActividad vmDelDto = new ViewModelIngresoActividad()
                {
                    FechaYHora = unDto.FechaYHoraIngreso, //Por medio del data annotation DataType.Datetime del ViewModel mostramos solo el formato dd/mm/aaa
                    Dia = unDto.Dia,
                    Hora = horaYMins
                };
                ingresosActViewModel.Add(vmDelDto);
            }
            return ingresosActViewModel;
        }
        #endregion
    }
}
