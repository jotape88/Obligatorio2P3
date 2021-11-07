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
    public class UsuarioController : Controller
    {
        #region Repositorios
        IRepoUsuarios repousu = FabricaRepositorios.ObtenerRepositorioUsuarios();
        #endregion

        #region ActionResults
        // GET: Usuario
        public ActionResult Index()
        {
            return View();
        }

        // GET: Usuario/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Usuario/Create
        //public ActionResult Create()
        //{
        //    if (Session["usuarioLogueado"] == null)
        //    {
        //        return View("~/Views/Shared/NoAutorizado.cshtml");
        //    }
        //    return View();
        //}

        // POST: Usuario/Create
        //[HttpPost]
        //public ActionResult Create(ViewModelUsuario vMUsuario)
        //{
        //    if (Session["usuarioLogueado"] == null)
        //    {
        //        return View("~/Views/Shared/NoAutorizado.cshtml");
        //    }
        //    Usuario unUsu = new Usuario();
        //    if (unUsu.ValidarMail(vMUsuario.Email) && unUsu.ValidarContrasenia(vMUsuario.Contrasenia))
        //    {
        //        string passEncriptada = unUsu.Encriptacion(vMUsuario.Contrasenia);
        //        unUsu = repousu.BuscarPorEmail(vMUsuario.Email);
        //        if (unUsu == null)
        //        {
        //            Usuario usuAux = new Usuario()
        //            {
        //                Email = vMUsuario.Email,
        //                Contrasenia = passEncriptada
        //            };

        //            if(repousu.Alta(usuAux))
        //            {
        //                ViewBag.Success = "El usuario fue dado de alta";
        //                return View();
        //            }
        //        }
        //        else
        //        {
        //            ViewBag.Error = "El email ya se encuentra registrado, intente nuevamente";
        //            return View();
        //        }
        //    }
        //    else
        //    {
        //        ViewBag.Error = "El formato del email y/o contraseña no es válido, intente nuevamente";
        //        return View();
        //    }

        //    return View();
        //}

        // GET: Usuario/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Usuario/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Usuario/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Usuario/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
        #endregion
    }
}
