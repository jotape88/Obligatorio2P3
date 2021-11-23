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
        public ActionResult BusquedaActividadPorParametros(string NombreActivdad, string Dia, int? Hora, int? EdadMinima)
        {
            if (Session["usuarioLogueado"] == null)
            {
                return View("~/Views/Shared/NoAutorizado.cshtml");
            }
            List<DTODiaYHora> dtoDiasYHrs = new List<DTODiaYHora>();
            ViewBag.MostrarEdadMin = 0;
            string laUbicacion = ConfigurationManager.AppSettings["UbicacionWebAPI"];
            Uri laUri = null;

            if (NombreActivdad != "")
            {
                string laUrl = laUbicacion + "obligatorio/actividades/GetActividadesPorNombre/";
                laUri = new Uri(laUrl + NombreActivdad);
            }
            else if (Dia != "" && Hora != null)
            {
                string laUrl = laUbicacion + "obligatorio/actividades/GetActividadesPorDiaHr/";
                 laUri = new Uri(laUrl + Dia + "/" + Hora);
            }
            else if (EdadMinima != null)
            {
                string laUrl = laUbicacion + "obligatorio/actividades/GetActividadesPorCotaMinimaEdad/";
                ViewBag.MostrarEdadMin = EdadMinima;
                 laUri = new Uri(laUrl + EdadMinima);
            }

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
            if(listaIngresos.Count == 0)
            {
                ViewBag.Vacia = "No hay actividades que cumplan el criterio de busqueda detallado";
            }
           
            return View("BusquedaActividadPorParametrosLista", listaIngresos);
        }

        private List<ViewModelDiaYHora> ConvertirDiasYHrsAModel(List<DTODiaYHora> ingresosDiasYHrs)
        {
            List<ViewModelDiaYHora> diasHrsViewModel = new List<ViewModelDiaYHora>();
            foreach (DTODiaYHora unDhr in ingresosDiasYHrs)
            {
                ViewModelDiaYHora vmDiaHr = new ViewModelDiaYHora()
                {
                    NombreActivdad = unDhr.NombreActividad,
                    Dia = unDhr.Dia,
                    Hora = unDhr.Hora,
                    EdadMinima = unDhr.EdadMinima
                };
                diasHrsViewModel.Add(vmDiaHr);
            }
            return diasHrsViewModel;
        }



        //Punto 8
        public ActionResult BusquedaIngresosActXSocio()
        {
            if (Session["usuarioLogueado"] == null)
            {
                return View("~/Views/Shared/NoAutorizado.cshtml");
            }
            return View();
        }
        [HttpPost]
        public ActionResult BusquedaIngresosActXSocio(string CedulaSocio, string NombreActiv)
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
            if (listaIngresos.Count == 0)
            {
                ViewBag.Vacia = "No hay actividades que cumplan el criterio de busqueda detallado";
            }
            return View("ListarIngresosActXSocio", listaIngresos);

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
