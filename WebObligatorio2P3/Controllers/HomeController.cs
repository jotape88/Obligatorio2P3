using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebObligatorio_2_P3.Models;
using Auxiliar;
using Dominio;
using ImportarInformacion;


namespace WebObligatorio_2_P3.Controllers
{
    public class HomeController : Controller
    {
        #region Repositorios
        IRepoUsuarios repousu = FabricaRepositorios.ObtenerRepositorioUsuarios();
        #endregion

        #region ActionResults
        public ActionResult Index()
        {
            if (Session["usuarioLogueado"] == null)
            {
                return View("~/Views/Shared/NoAutorizado.cshtml");
            }
            return View();
        }

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(ViewModelUsuario vMUsu)
        {
            try
            {
                Usuario unUsu = new Usuario();

                if (unUsu.ValidarMail(vMUsu.Email) && unUsu.ValidarContrasenia(vMUsu.Contrasenia))
                {
                    unUsu = repousu.BuscarPorEmail(vMUsu.Email); //Buscamos (por el mail que es unico) si existe el usuario
                    if (unUsu != null)
                    {
                        string passEncriptada = unUsu.Encriptacion(vMUsu.Contrasenia); //Encripto la contrasenia recibida desde el viewModel
                        if (unUsu.Contrasenia == passEncriptada) //La comparo con la contraseña encriptada en la BD
                        {
                            Session["usuarioLogueado"] = vMUsu.Email;
                            return RedirectToAction("Index", "Home");
                        }
                        ViewBag.Warning = "Los datos ingresados no son correctos, intente nuevamente y/o verifique que se hayan importado los usuarios";
                        return View();
                    }
                    else
                    {
                        ViewBag.Warning = "Los datos ingresados no son correctos, intente nuevamente y/o verifique que se hayan importado los usuarios"; 
                        return View();
                    }
                }
                else
                {
                    ViewBag.Warning = "Los datos ingresados no son correctos, intente nuevamente y/o verifique que se hayan importado los usuarios"; //El mensaje se repite porque validamos mail y contraseña en forma separada, y por seguridad no decimos que es lo que falló
                    return View();
                }
            }
            catch
            {
                return View("~/Views/Shared/Error.cshtml"); //Al ser el Login un campo sensisble con respecto a la seguridad, no devolvemos ningún mensaje proveniente de la exception/innerException
            }
        }

        public ActionResult Logout()
        {
            if (Session["usuarioLogueado"] == null)
            {
                return View("~/Views/Shared/NoAutorizado.cshtml");
            }
            Session.Clear();
            return RedirectToAction("Login", "Home");
        }

        public ActionResult ImportarUsuarios()
        {
            try
            {
                if (Importar.ImportarUsuarios())
                {
                    ViewBag.Success = "Los usuarios se importaron con éxito";
                }
                else
                {
                    ViewBag.Warning = "No se encontraron usuarios nuevos en el archivo y/o los datos no cumplen con el formato requerido";
                }
            }
            catch (Exception laExc)
            {
                if (laExc.InnerException == null)
                {
                    ViewBag.Error = "Error de sistema: " + laExc.Message;
                }
                while (laExc.InnerException != null)
                {
                    ViewBag.Error = "Error de sistema: " + laExc.InnerException.Message;
                    laExc = laExc.InnerException;
                }
            }

            return View();
        }

        public ActionResult ImportarActividades()
        {
            try
            {
                if (Importar.ImportarActividades())
                {
                    ViewBag.Success = "Las actividades se importaron con éxito";
                }
                else
                {
                    ViewBag.Warning = "No se encontraron actividades nuevas en el archivo y/o los datos no cumplen con el formato requerido";
                }
            }
            catch (Exception laExc)
            {
                if (laExc.InnerException == null)
                {
                    ViewBag.Error = "Error de sistema: " + laExc.Message;
                }
                while (laExc.InnerException != null)
                {
                    ViewBag.Error = "Error de sistema: " + laExc.InnerException.Message;
                    laExc = laExc.InnerException;
                }
            }
            return View();
        }
        #endregion
    }
}