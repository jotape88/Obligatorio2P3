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


            IRepoFormasPago repoFormas = FabricaRepositorios.ObtenerRepositorioFormasPagos();
            dynamic[] arrayAuxiliares = repoFormas.TraerAuxiliares();
            PaseLibre.DescuentoPorAntiguedad = arrayAuxiliares[0];     
            PaseLibre.TopeAntiguedad = arrayAuxiliares[1];  
            Cuponera.DescPorTopeActiv = arrayAuxiliares[2];  
            Cuponera.TopeActividades = arrayAuxiliares[3];       
            PaseLibre.ValorMes = arrayAuxiliares[4];         
            Cuponera.ValorActividad = arrayAuxiliares[5];

            Utilidades.GenerarBDAlInicio(); //Llamamos al metodo estatico que se encarga de crear la BD mediante E.F si esta aún no existe
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