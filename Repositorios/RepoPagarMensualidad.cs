using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dominio;
using System.Data;
using System.Data.SqlClient;
using System.Data.Entity;

namespace Repositorios
{
    public class RepoPagarMensualidad : IRepoPagarMensualidad
    {
        #region Metodos de manejo de datos
        public bool AltaPago(int idSocio, decimal total, decimal descuento, int cantidadActiv = 0)
        {
            bool bandera = false;
            try
            {
                using (ClubContext db = new ClubContext())
                {
                    RepoSocios repoSoc = new RepoSocios();
                    Socio unSoc = repoSoc.BuscarPorId(idSocio);
                    PagarMensualidad unaMens = null;

                    if (cantidadActiv != 0)
                    {
                        Cuponera unaCupo = new Cuponera { CantidadActividades = cantidadActiv };
                            db.FormaPagos.Add(unaCupo);
                        
                        unaMens = new PagarMensualidad() 
                        { 
                            FechaPago = DateTime.Now, 
                            UnaFormaPago = unaCupo, 
                            UnSocio = unSoc,
                            MontoDescontado = descuento,
                            MontoPagado = total
                        };
                        db.PagarMensualidades.Add(unaMens);
                        db.Entry(unaMens.UnSocio).State = EntityState.Unchanged;
                        bandera = db.SaveChanges() != 0;
                    }
                    else if (cantidadActiv == 0)
                    {
                        PaseLibre unPase = new PaseLibre {};
                        db.FormaPagos.Add(unPase);
                        unaMens = new PagarMensualidad()
                        {
                            FechaPago = DateTime.Now,
                            UnaFormaPago = unPase,
                            UnSocio = unSoc,
                            MontoDescontado = descuento,
                            MontoPagado = total
                        };
                        db.PagarMensualidades.Add(unaMens);
                        db.Entry(unaMens.UnSocio).State = EntityState.Unchanged;
                        bandera = db.SaveChanges() != 0;
                    }
                }
            }
            catch
            {
                throw;
            }
            return bandera;
        }
        public DateTime BuscarUltFechaPagoXIdSocio(int idSocio) //hacer
        {
            DateTime ultFechaPago = DateTime.MinValue;
            try
            {
                using (ClubContext db = new ClubContext())
                {
                    ultFechaPago = db.PagarMensualidades.Where(pg => pg.UnSocio.Id == idSocio)
                                                         .Max(pg => pg.FechaPago);
                };
            }
            catch
            {
               return ultFechaPago;
            }  
            return ultFechaPago;
        }


        public List<DTOMensualidad> ListarFormasPagoPorMesYAnio(int mes, int anio)
        {
            List<DTOMensualidad> mensualidades = new List<DTOMensualidad>();

            using(ClubContext db = new ClubContext())
            {
                return mensualidades = db.PagarMensualidades.Where(pg => pg.FechaPago.Year == anio && pg.FechaPago.Month == mes)
                                                     .Select(pg => new DTOMensualidad() {                                                       
                                                        TipoForma = pg.UnaFormaPago, //Guardamos la entidad
                                                        FechaPago = pg.FechaPago,
                                                        MontoPago = pg.MontoPagado,
                                                        DescuentoPago = pg.MontoDescontado,
                                                        CedulaSocio = pg.UnSocio.Cedula,
                                                        NombreSocio = pg.UnSocio.NombreYapellido
                                                     })
                                                     .ToList();                                                        
            }

        }


        //public FormaPago BuscarUltFormaPago(int idSocio) 
        //{
        //    FormaPago unaForma = null;

        //    string miString = @"Data Source=localhost\SQLEXPRESS; Initial Catalog=BaseObligatorio1P3; Integrated Security=SSPI;";
        //    SqlConnection miConexion = new SqlConnection(miString);
        //    try
        //    {
        //        string miSql = "SELECT TOP 1 FP.Id, FP.CantidadActividades, MAX(PM.FechasPagos) AS Fecha FROM FormasPagos FP, PagarMensualidades PM WHERE FP.Id = PM.IdFormasPagos AND PM.IdSocio = @idSocio GROUP BY FP.Id, FP.CantidadActividades ORDER BY Fecha DESC";


        //        SqlCommand miComando = new SqlCommand(miSql, miConexion);
        //        miComando.Parameters.AddWithValue("@idSocio", idSocio);
        //        miConexion.Open();
        //        SqlDataReader miReader = miComando.ExecuteReader();
        //        if (miReader.Read())
        //        {

        //            if (miReader["CantidadActividades"] is DBNull)
        //            {
        //                unaForma = new PaseLibre()
        //                {
        //                    Id = miReader.GetInt32(0),

        //                };
        //            } else
        //            {
        //                unaForma = new Cuponera()
        //                {
        //                    Id = miReader.GetInt32(0),
        //                    CantidadActividades = miReader.GetInt32(1)
        //                };
        //            }
        //        }
        //        miConexion.Close();
        //        miConexion.Dispose();
        //    }
        //    catch
        //    {
        //        throw;
        //    }
        //    finally
        //    {
        //        if (miConexion.State == ConnectionState.Open)
        //        {
        //            miConexion.Close();
        //            miConexion.Dispose();
        //        }
        //    }
        //    return unaForma;
        //}

        //public List<PagarMensualidad> TraerTodo()
        //{
        //    List<PagarMensualidad> pagarmensualidades = new List<PagarMensualidad>();
        //    string miString = @"Data Source=localhost\SQLEXPRESS; Initial Catalog=BaseObligatorio1P3; Integrated Security=SSPI;";
        //    SqlConnection miConexion = new SqlConnection(miString);
        //    try
        //    {
        //        string miSql = "SELECT * FROM PagarMensualidades";
        //        SqlCommand miComando = new SqlCommand(miSql, miConexion);
        //        miConexion.Open();
        //        SqlDataReader miReader = miComando.ExecuteReader();
        //        while (miReader.Read())

        //        {
        //            IRepoSocios socio = new RepoSocios();
        //            IRepoFormasPago formpaP = new RepoFormasPago();
        //            PagarMensualidad unPagoMensualidad = new PagarMensualidad
        //            {
        //                Id = miReader.GetInt32(0),
        //                UnSocio = socio.BuscarPorId(miReader.GetInt32(1)),
        //                UnaFormaPago = formpaP.BuscarPorId(miReader.GetInt32(2)), 
        //                FechaPago = miReader.GetDateTime(3),
        //            };
        //            pagarmensualidades.Add(unPagoMensualidad);
        //        }
        //        miConexion.Close();
        //        miConexion.Dispose();
        //    }
        //    catch
        //    {
        //        throw;
        //    }
        //    finally
        //    {
        //        if (miConexion.State == ConnectionState.Open)
        //        {
        //            miConexion.Close();
        //            miConexion.Dispose();
        //        }
        //    }
        //    return pagarmensualidades;
        //}


        #endregion

        #region Metodos no implementados
        public bool Alta(PagarMensualidad obj)
        {
            throw new NotImplementedException();
        }

        public bool Baja(int id)
        {
            throw new NotImplementedException();
        }
        public bool Modificacion(PagarMensualidad obj)
        {
            throw new NotImplementedException();
        }

        public PagarMensualidad BuscarPorId(int id)
        {
            throw new NotImplementedException();
        }

        public FormaPago BuscarUltFormaPago(int idSocio)
        {
            throw new NotImplementedException();
        }

        public List<PagarMensualidad> TraerTodo()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
