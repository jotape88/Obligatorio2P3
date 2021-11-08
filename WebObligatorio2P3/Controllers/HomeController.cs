﻿using System;
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
             Usuario unUsu = new Usuario();

             if (unUsu.ValidarMail(vMUsu.Email) && unUsu.ValidarContrasenia(vMUsu.Contrasenia))
             {
                 unUsu = repousu.BuscarPorEmail(vMUsu.Email);
                 if (unUsu != null)
                 {
                     string passEncriptada = unUsu.Encriptacion(vMUsu.Contrasenia); //Encripto la contrasenia recibida para compararla con la contrasenia encriptada de la base
                     if (unUsu.Contrasenia == passEncriptada)
                     {
                         Session["usuarioLogueado"] = vMUsu.Email;
                         return RedirectToAction("Index", "Home");
                     }
                     ViewBag.Error = "Los datos ingresados no son correctos, intente nuevamente y/o verifique que se hayan importado los usuarios";
                     return View();
                 }
                 else
                 {
                     ViewBag.Error = "Los datos ingresados no son correctos, intente nuevamente y/o verifique que se hayan importado los usuarios"; //El mensaje se repite porque validamos mail y contraseña en forma separada, y por seguridad no decimos que es lo que falló
                     return View();
                 }
             }
             else
             {
                 ViewBag.Error = "El formato del email y/o contraseña no es válido, intente nuevamente";
                 return View();
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
            catch(Exception laExc)
            {
                ViewBag.Error = "Error en la importacion: " + laExc.Message;
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
                ViewBag.Error = "Error en la importacion: " + laExc.Message;
            }
            return View();
        }
        #endregion
    }
}