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

namespace ExportarInformacion
{
     public class Exportar
    {
        public static void ExportarTodo()
        {
            #region Repositorios
            IRepoFormasPago formasPagoRepo = FabricaRepositorios.ObtenerRepositorioFormasPagos();
            IRepoUsuarios usuRepo = FabricaRepositorios.ObtenerRepositorioUsuarios();
            IRepoSocios sociosRepo = FabricaRepositorios.ObtenerRepositorioSocios();
            IRepoPagarMensualidad repoPagarMens = FabricaRepositorios.ObtenerRepositorioPagarMensualidad();
            IRepoActividades actividadRepo = FabricaRepositorios.ObtenerRepositorioActividades();
            IRepoIngresosActividades ingresosRepo = FabricaRepositorios.ObtenerRepositorioIngresosActividades();
            #endregion

            #region Metodos para exportar
            try
            {
                string rutaAplicacion = System.Web.HttpRuntime.AppDomainAppPath;

                #region Exportar Formas de Pago
                string rutaArchivoFormadePago = Path.Combine(rutaAplicacion, "Archivos", "FormasPago.txt");
                Stream stream = new FileStream(rutaArchivoFormadePago, FileMode.Create);
                StreamWriter swFp = new StreamWriter(stream);
                List<FormaPago> formadePago = formasPagoRepo.TraerTodo();
                foreach (FormaPago formasPago in formadePago)
                {
                    if (formasPago is Cuponera)
                    {
                        Cuponera unaCupo = (Cuponera)formasPago;
                        swFp.WriteLine(unaCupo.Id + "|" + unaCupo.CantidadActividades + "|" + "Cuponera");
                    }
                    else if (formasPago is PaseLibre)
                    {
                        PaseLibre unaPase = (PaseLibre)formasPago;
                        swFp.WriteLine(unaPase.Id + "|" + "NULL" + "|" + "PaseLibre");
                    }
                }
                swFp.Close();
                #endregion


                #region Exportar Usuarios
                string rutaArchivoUsuario = Path.Combine(rutaAplicacion, "Archivos", "Usuarios.txt");
                Stream streamUsuarios = new FileStream(rutaArchivoUsuario, FileMode.Create);
                StreamWriter swUsu = new StreamWriter(streamUsuarios);
                List<Usuario> usuarios = usuRepo.TraerTodo();
                foreach (Usuario usu in usuarios)
                {
                    swUsu.WriteLine(usu.Email + "|" + usu.Contrasenia);
                }
                swUsu.Close();
                #endregion


                #region Exportar Socios
                string rutaArchivoSocios = Path.Combine(rutaAplicacion, "Archivos", "Socios.txt");
                Stream streamSocios = new FileStream(rutaArchivoSocios, FileMode.Create);
                StreamWriter swSocio = new StreamWriter(streamSocios);
                List<Socio> socios = sociosRepo.TraerTodo();
                foreach (Socio socio in socios)
                {
                    swSocio.WriteLine(socio.Id + "|" + socio.Cedula + "|" + socio.NombreYapellido + "|" + socio.EstaActivo + "|" + socio.FechaNacimiento.ToShortDateString() + "|" + socio.FechaRegistro.ToShortDateString());
                }
                swSocio.Close();
                #endregion


                #region Exportar Pagar Mensualidades
                string rutaArchivoPagarMens = Path.Combine(rutaAplicacion, "Archivos", "PagarMensualidad.txt");
                Stream streamPagarMens = new FileStream(rutaArchivoPagarMens, FileMode.Create);
                StreamWriter swPagarMens = new StreamWriter(streamPagarMens);
                List<PagarMensualidad> mensualidades = repoPagarMens.TraerTodo();
                foreach (PagarMensualidad unaMensualidad in mensualidades)
                {
                    swPagarMens.WriteLine(unaMensualidad.Id + "|" + unaMensualidad.UnSocio.Id + "|" + unaMensualidad.UnaFormaPago.Id + "|" + unaMensualidad.FechaPago.ToShortDateString());
                }
                swPagarMens.Close();
                #endregion


                #region Exportar Actividades
                string rutaArchivoActividad = Path.Combine(rutaAplicacion, "Archivos", "Actividad.txt");
                Stream streamActiv = new FileStream(rutaArchivoActividad, FileMode.Create);
                StreamWriter swActiv = new StreamWriter(streamActiv);
                List<Actividad> actividades = actividadRepo.TraerTodo();
                foreach (Actividad unaActividad in actividades)
                {
                    swActiv.WriteLine(unaActividad.Id + "|" + unaActividad.Nombre + "|" + unaActividad.EdadMinima + "|" + unaActividad.EdadMaxima);
                }
                swActiv.Close();
                #endregion


                #region Exportar Dias y Horas
                string rutaArchivoDiaYHora = Path.Combine(rutaAplicacion, "Archivos", "Dia y hora.txt");
                Stream streamDyH = new FileStream(rutaArchivoDiaYHora, FileMode.Create);
                StreamWriter swDyH = new StreamWriter(streamDyH);
                IRepoDiaYHora repoDiaYHora = FabricaRepositorios.ObtenerRepositorioDiaYHora();
                List<DiaYHora> diasYHoras = repoDiaYHora.TraerTodo();
                foreach (DiaYHora unDyH in diasYHoras)
                {
                    swDyH.WriteLine(unDyH.Id + "|" + unDyH.Activ.Id + "|" + unDyH.Dia + "|" + unDyH.Hora + "|" + unDyH.CuposMaximos);
                }
                swDyH.Close();
                #endregion


                #region Exportar Ingreso Actividades
                string rutaArchivoIngrActiv = Path.Combine(rutaAplicacion, "Archivos", "Ingreso Actividad.txt");
                Stream streamIngrActiv = new FileStream(rutaArchivoIngrActiv, FileMode.Create);
                StreamWriter swIngrAct = new StreamWriter(streamIngrActiv);
                List<IngresoActividad> ingresos = ingresosRepo.TraerTodo();
                foreach (IngresoActividad unIngreso in ingresos)
                {
                    swIngrAct.WriteLine(unIngreso.Id + "|" + unIngreso.Soc.Id + "|" + unIngreso.FechaYHora.ToShortDateString() + "|" + unIngreso.DiaYHr.Id);
                }
                swIngrAct.Close();
                #endregion


                #region Exportar Tabla Auxiliares
                string rutaTablaAux = Path.Combine(rutaAplicacion, "Archivos", "Auxiliares.txt");
                Stream streamAux = new FileStream(rutaTablaAux, FileMode.Create);
                StreamWriter swAux = new StreamWriter(streamAux);
                dynamic[] arrayAuxiliares = formasPagoRepo.TraerAuxiliares();
                swAux.WriteLine(arrayAuxiliares[0] + "|" + arrayAuxiliares[1] + "|" + arrayAuxiliares[2] + "|" + arrayAuxiliares[3] + "|" + arrayAuxiliares[4] + "|" + arrayAuxiliares[5] + "|");
                swAux.Close();
                #endregion


            }

            catch
            {
                throw;
            }
            #endregion

        }
    }
}
