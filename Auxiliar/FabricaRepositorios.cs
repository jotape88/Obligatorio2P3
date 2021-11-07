using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dominio;
using Repositorios;

namespace Auxiliar
{
    public class FabricaRepositorios
    {
        public static IRepoSocios ObtenerRepositorioSocios()
        {
            return new RepoSocios();
        }

        public static IRepoActividades ObtenerRepositorioActividades()
        {
            return new RepoActividades();
        }

        public static IRepoUsuarios ObtenerRepositorioUsuarios()
        {
            return new RepoUsuarios();
        }

        public static IRepoPagarMensualidad ObtenerRepositorioPagarMensualidad()
        {
            return new RepoPagarMensualidad();
        }

        public static IRepoFormasPago ObtenerRepositorioFormasPagos()
        {
            return new RepoFormasPago();
        }

        public static IRepoDiaYHora ObtenerRepositorioDiaYHora()
        {
            return new RepoDiaYHora();
        }

        public static IRepoIngresosActividades ObtenerRepositorioIngresosActividades()
        {
            return new RepoIngresosActividades();
        }
    }
}
