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
        #region Metodo para traer parametros auxiliares
        public dynamic[] TraerAuxiliares()
        {
            dynamic[] arrayDatos = null;

            using (ClubContext db = new ClubContext())
            {
                return arrayDatos = db.Parametros.Where(a => a.Id == 1)
                                                 .ToArray();
            }
        }
        #endregion

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
        public DateTime BuscarUltFechaPagoXIdSocio(int idSocio) 
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
                                                        TipoForma = pg.UnaFormaPago, //Guardamos el objeto completo
                                                        FechaPago = pg.FechaPago,
                                                        MontoPago = pg.MontoPagado,
                                                        DescuentoPago = pg.MontoDescontado,
                                                        CedulaSocio = pg.UnSocio.Cedula,
                                                        NombreSocio = pg.UnSocio.NombreYapellido
                                                     })
                                                     .ToList();                                                        
            }

        }

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
