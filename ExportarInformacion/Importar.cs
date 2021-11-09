using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Auxiliar;
using Dominio;
using System.Data;
using System.Data.SqlClient;

namespace ImportarInformacion
{
    public class Importar
    {
        public static bool ImportarUsuarios()
        {
            bool bandera = false;
            try
            {
                IRepoUsuarios repoUsuarios = FabricaRepositorios.ObtenerRepositorioUsuarios();

                string rutaAplicacion = System.Web.HttpRuntime.AppDomainAppPath;
                string rutaArchivoUsuario = Path.Combine(rutaAplicacion, "Archivos", "Usuarios.txt");
                Stream streamUsuarios = new FileStream(rutaArchivoUsuario, FileMode.Open); 
                StreamReader miStReader = new StreamReader(streamUsuarios); 

                string unaLinea = miStReader.ReadLine();

                while (unaLinea != null)
                {
                    Usuario unUsuario = ConvertirStringEnUsuario(unaLinea, "|");

                    if(unUsuario != null)
                    {
                        if (repoUsuarios.BuscarPorEmail(unUsuario.Email) == null)
                        {
                            bandera = repoUsuarios.Alta(unUsuario); //Al setear la bandera aca, en caso de que no se agregue ningun usuario, nos devuelve el mensaje correspondiente
                        }
                    }
                    unaLinea = miStReader.ReadLine();
                }
                miStReader.Close();
            }
            catch
            {
                throw;
            }
            return bandera;
        }

        private static Usuario ConvertirStringEnUsuario(string unaLinea, string separador)
        {
            Usuario instanciaUsu = new Usuario();

            string[] vecStrings = unaLinea.Split(separador.ToCharArray());
            if(vecStrings.Length == 2)
            {
                Usuario unUsu = new Usuario { Email = vecStrings[0], Contrasenia = vecStrings[1], ContraseniaDesencriptada = instanciaUsu.Desencriptacion(vecStrings[1]) };
                return unUsu;
            }
            return null;
        }

        public static bool ImportarActividades()
        {
            bool bandera = false;
            try
            {
                string rutaAplicacion = System.Web.HttpRuntime.AppDomainAppPath;
                IRepoActividades repoActividades = FabricaRepositorios.ObtenerRepositorioActividades();

                string rutaArchivoActividades = Path.Combine(rutaAplicacion, "Archivos", "Actividad.txt");
                Stream streamActividades = new FileStream(rutaArchivoActividades, FileMode.Open);
                StreamReader miStReader = new StreamReader(streamActividades);

                string unaLinea = miStReader.ReadLine();

                while (unaLinea != null)
                {
                    Actividad unaAct = ConvertirStringEnActividad(unaLinea, "|");

                    if (unaAct != null)
                    {
                        if (repoActividades.BuscarPorId(unaAct.Id) == null) 
                        {
                            bandera = repoActividades.Alta(unaAct);
                        }
                    }
                    unaLinea = miStReader.ReadLine();
                }
                miStReader.Close();
                ImportarDiasYHoras();
            }
            catch
            {
                throw;
            }
            return bandera;
        }

        private static Actividad ConvertirStringEnActividad(string unaLinea, string separador)
        {
            string[] vecActivs = unaLinea.Split(separador.ToCharArray());
            if (vecActivs.Length == 4)
            {
                Actividad unaAct = new Actividad { Id = Int32.Parse(vecActivs[0]), Nombre = vecActivs[1], EdadMinima = Int32.Parse(vecActivs[2]), EdadMaxima = Int32.Parse(vecActivs[3])};
                return unaAct;
            }
            return null;
        }

        public static bool ImportarDiasYHoras()
        {
            bool bandera = false;
            try
            {
                string rutaAplicacion = System.Web.HttpRuntime.AppDomainAppPath;
                IRepoDiaYHora repoDiasHrs = FabricaRepositorios.ObtenerRepositorioDiaYHora();

                string rutaArchivoDiasHrs = Path.Combine(rutaAplicacion, "Archivos", "Dia y hora.txt");
                Stream streamDiasHrs = new FileStream(rutaArchivoDiasHrs, FileMode.Open);
                StreamReader miStReader = new StreamReader(streamDiasHrs);

                string unaLinea = miStReader.ReadLine();

                while (unaLinea != null)
                {
                    DiaYHora unDiaHr = ConvertirStringEnDiaHora(unaLinea, "|");

                    if (unDiaHr != null)
                    {
                        if (repoDiasHrs.BuscarPorId(unDiaHr.Id) == null && !repoDiasHrs.BuscarActivsEnMismoDiaYHora(unDiaHr)) //Agregue buscar por id para que no se repitan los ingresos y tambien buscamos que no se repitan en el mismo dia las actividades
                        {
                            bandera = repoDiasHrs.Alta(unDiaHr); //Mismo que actividades
                        }
                    }
                    unaLinea = miStReader.ReadLine();
                }
                miStReader.Close();
            }
            catch
            {
                throw;
            }
            return bandera;
        }

        private static DiaYHora ConvertirStringEnDiaHora(string unaLinea, string separador)
        {
            IRepoActividades repoACt = FabricaRepositorios.ObtenerRepositorioActividades();
            string[] vecDiasHrs = unaLinea.Split(separador.ToCharArray());
            if (vecDiasHrs.Length == 5)
            {
                DiaYHora unDhr = new DiaYHora { Id = Int32.Parse(vecDiasHrs[0]), Activ = repoACt.BuscarPorId(Int32.Parse(vecDiasHrs[1])), Dia = vecDiasHrs[2], Hora = Decimal.Parse(vecDiasHrs[3]), CuposMaximos = Int32.Parse(vecDiasHrs[4]) };
                return unDhr;
            }
            return null;
        }

    } 
}
