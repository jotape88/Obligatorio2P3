using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Auxiliar;
using Dominio;

namespace WebObligatorio_2_P3
{
    public class Global : System.Web.HttpApplication
    {
        protected void Application_Start(object sender, EventArgs e)
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            Utilidades.GenerarBDAlInicio(); //Llamamos al metodo estatico que se encarga de crear la BD mediante E.F si esta aún no existe

            IRepoPagarMensualidad repoPagarMens = FabricaRepositorios.ObtenerRepositorioPagarMensualidad();
            dynamic[] arrayDeParametros = repoPagarMens.TraerAuxiliares();
            if(arrayDeParametros.Length > 0)
            {
                foreach (Parametros x in arrayDeParametros)
                {
                    PaseLibre.DescuentoPorAntiguedad = x.DescuentoPorAntiguedad;
                    PaseLibre.TopeAntiguedad = x.TopeAntiguedad;
                    Cuponera.DescPorTopeActiv = x.DescPorTopeActiv;
                    Cuponera.TopeActividades = x.TopeActividades;
                    PaseLibre.ValorMes = x.ValorMes;
                    Cuponera.ValorActividad = x.ValorActividad;
                }
            } else
            {
                Parametros unParam = new Parametros
                {
                    DescuentoPorAntiguedad = 10,
                    TopeAntiguedad = 2,
                    DescPorTopeActiv = 200,
                    TopeActividades = 30,
                    ValorMes = 1200,
                    ValorActividad = 30
                };

                repoPagarMens.CargaDeParametrosSiNoExisten(unParam);
            }
        }

        protected void Session_Start(object sender, EventArgs e)
        {

        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {

        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {

        }

        protected void Application_Error(object sender, EventArgs e)
        {

        }

        protected void Session_End(object sender, EventArgs e)
        {

        }

        protected void Application_End(object sender, EventArgs e)
        {

        }
    }
}