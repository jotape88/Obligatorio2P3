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
            try
            {
                List<Usuario> usus = new List<Usuario>();

                string rutaAplicacion = System.Web.HttpRuntime.AppDomainAppPath;
                string rutaArchivoUsuario = Path.Combine(rutaAplicacion, "Archivos", "Usuarios.txt");
                Stream streamUsuarios = new FileStream(rutaArchivoUsuario, FileMode.Open); //agregue el filemode.open
                StreamReader miStReader = new StreamReader(streamUsuarios); 

                 string linea = miStReader.ReadLine();

                while (linea != null)
                {

                }




                //string rutaAplicacion = System.Web.HttpRuntime.AppDomainAppPath;
                //IRepoUsuarios usuRepo = FabricaRepositorios.ObtenerRepositorioUsuarios();

                //string rutaArchivoUsuario = Path.Combine(rutaAplicacion, "Archivos", "Usuarios.txt");
                //Stream streamUsuarios = new FileStream(rutaArchivoUsuario, FileMode.Create);
                //StreamWriter swUsu = new StreamWriter(streamUsuarios);
                //List<Usuario> usuarios = usuRepo.TraerTodo();
                //foreach (Usuario usu in usuarios)
                //{
                //    swUsu.WriteLine(usu.Email + "|" + usu.Contrasenia);
                //}
                //swUsu.Close();
            }

            catch
            {
                throw;
            }
            return true;
        }

        public static bool ImportarActividades()
        {
            try
            {
                string rutaAplicacion = System.Web.HttpRuntime.AppDomainAppPath;

                IRepoActividades actividadRepo = FabricaRepositorios.ObtenerRepositorioActividades();
                string rutaArchivoActividad = Path.Combine(rutaAplicacion, "Archivos", "Actividad.txt");
                Stream streamActiv = new FileStream(rutaArchivoActividad, FileMode.Create);
                StreamWriter swActiv = new StreamWriter(streamActiv);
                List<Actividad> actividades = actividadRepo.TraerTodo();
                foreach (Actividad unaActividad in actividades)
                {
                    swActiv.WriteLine(unaActividad.Id + "|" + unaActividad.Nombre + "|" + unaActividad.EdadMinima + "|" + unaActividad.EdadMaxima);
                }
                swActiv.Close();
            }
            catch
            {
                throw;
            }
            return true;
        }
    } 
}
