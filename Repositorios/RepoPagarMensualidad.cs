using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dominio;
using System.Data;
using System.Data.SqlClient;

namespace Repositorios
{
    public class RepoPagarMensualidad : IRepoPagarMensualidad
    {
        public bool AltaPago(int idSocio, int cantidadActiv = 0) //Por defecto cantidadActiv siempre va a estar en 0, aun cuando no le pasemos parametros desde afuera
        {
            bool bandera = false;
            int filasAfectadas = 0;
            string miString = @"Data Source=localhost\SQLEXPRESS; Initial Catalog=BaseObligatorio1P3; Integrated Security=SSPI";
            SqlConnection miConexion = new SqlConnection(miString);
            SqlTransaction miTransaccion = null;
            try
            {
                SqlCommand miComando = new SqlCommand();

                if (cantidadActiv != 0)
                {
                    string miSqlCuponera = @"INSERT INTO FormasPagos (Tipo, CantidadActividades) VALUES(@tipo, @ctdAct); SELECT CAST(SCOPE_IDENTITY() AS INT);";
                    miComando = new SqlCommand(miSqlCuponera, miConexion);
                    miComando.Parameters.AddWithValue("@tipo", "Cuponera");
                    miComando.Parameters.AddWithValue("@ctdAct", cantidadActiv);
                } 
                else if( cantidadActiv == 0)
                {
                    string miSqlPaseLibre = @"INSERT INTO FormasPagos (Tipo) VALUES(@tipo); SELECT CAST(SCOPE_IDENTITY() AS INT);";
                    miComando = new SqlCommand(miSqlPaseLibre, miConexion);
                    miComando.Parameters.AddWithValue("@tipo", "PaseLibre");
                }

                miConexion.Open();
                miTransaccion = miConexion.BeginTransaction();
                miComando.Transaction = miTransaccion;
                int elId = (int)miComando.ExecuteScalar();

                miComando.Parameters.Clear();

                string miSql2 = @"INSERT INTO PagarMensualidades(IdSocio, IdFormasPagos, FechasPagos) VALUES(@idSoc, @idFormPag, @fechasPagos);";
                miComando.CommandText = miSql2;
                miComando.Parameters.AddWithValue("@idSoc", idSocio);
                miComando.Parameters.AddWithValue("@idFormPag", elId);
                miComando.Parameters.AddWithValue("@fechasPagos", DateTime.Now);

                filasAfectadas = miComando.ExecuteNonQuery();

                bandera = filasAfectadas == 1;

                miTransaccion.Commit();

                miConexion.Close();
                miConexion.Dispose();
            }
            catch
            {
                if (miTransaccion != null)
                {
                    miTransaccion.Rollback();
                }
                throw;
            }
            finally
            {
                if (miConexion.State == ConnectionState.Open)
                {
                    miConexion.Close();
                    miConexion.Dispose();
                }
            }
            return bandera;
        }
        public DateTime BuscarUltFechaPagoXIdSocio(int idSocio) 
        {
            DateTime ultFechaPago = DateTime.MinValue;
            string miString = @"Data Source=localhost\SQLEXPRESS; Initial Catalog=BaseObligatorio1P3; Integrated Security=SSPI;";
            SqlConnection miConexion = new SqlConnection(miString);
            try
            {
                string miSql = "SELECT max(FechasPagos) as FechasPagos FROM PagarMensualidades WHERE IdSocio=@id;";
                SqlCommand miComando = new SqlCommand(miSql, miConexion);
                miComando.Parameters.AddWithValue("@id", idSocio);

                miConexion.Open();
                SqlDataReader miReader = miComando.ExecuteReader();
                if (miReader.Read())
                {
                    if(miReader["FechasPagos"] is DBNull)
                    {
                        return ultFechaPago;

                    } else
                    {
                       ultFechaPago = miReader.GetDateTime(0);
                    }
                
                }         
                miConexion.Close();
                miConexion.Dispose();
            }
            catch
            {
                throw;
            }
            finally
            {
                if (miConexion.State == ConnectionState.Open)
                {
                    miConexion.Close();
                    miConexion.Dispose();
                }
            }     
            return ultFechaPago;
        }


        public bool Alta(PagarMensualidad obj)     
        {
            throw new NotImplementedException();
        }


        public bool Baja(int id)
        {
            throw new NotImplementedException();
        }

        public FormaPago BuscarUltFormaPago(int idSocio) 
        {
            FormaPago unaForma = null;

            string miString = @"Data Source=localhost\SQLEXPRESS; Initial Catalog=BaseObligatorio1P3; Integrated Security=SSPI;";
            SqlConnection miConexion = new SqlConnection(miString);
            try
            {
                string miSql = "SELECT TOP 1 FP.Id, FP.CantidadActividades, MAX(PM.FechasPagos) AS Fecha FROM FormasPagos FP, PagarMensualidades PM WHERE FP.Id = PM.IdFormasPagos AND PM.IdSocio = @idSocio GROUP BY FP.Id, FP.CantidadActividades ORDER BY Fecha DESC";


                SqlCommand miComando = new SqlCommand(miSql, miConexion);
                miComando.Parameters.AddWithValue("@idSocio", idSocio);
                miConexion.Open();
                SqlDataReader miReader = miComando.ExecuteReader();
                if (miReader.Read())
                {

                    if (miReader["CantidadActividades"] is DBNull)
                    {
                        unaForma = new PaseLibre()
                        {
                            Id = miReader.GetInt32(0),

                        };
                    } else
                    {
                        unaForma = new Cuponera()
                        {
                            Id = miReader.GetInt32(0),
                            CantidadActividades = miReader.GetInt32(1)
                        };
                    }
                }
                miConexion.Close();
                miConexion.Dispose();
            }
            catch
            {
                throw;
            }
            finally
            {
                if (miConexion.State == ConnectionState.Open)
                {
                    miConexion.Close();
                    miConexion.Dispose();
                }
            }
            return unaForma;
        }

        public bool Modificacion(PagarMensualidad obj)
        {
            throw new NotImplementedException();
        }

        public List<PagarMensualidad> TraerTodo()
        {
            List<PagarMensualidad> pagarmensualidades = new List<PagarMensualidad>();
            string miString = @"Data Source=localhost\SQLEXPRESS; Initial Catalog=BaseObligatorio1P3; Integrated Security=SSPI;";
            SqlConnection miConexion = new SqlConnection(miString);
            try
            {
                string miSql = "SELECT * FROM PagarMensualidades";
                SqlCommand miComando = new SqlCommand(miSql, miConexion);
                miConexion.Open();
                SqlDataReader miReader = miComando.ExecuteReader();
                while (miReader.Read())

                {
                    IRepoSocios socio = new RepoSocios();
                    IRepoFormasPago formpaP = new RepoFormasPago();
                    PagarMensualidad unPagoMensualidad = new PagarMensualidad
                    {
                        Id = miReader.GetInt32(0),
                        UnSocio = socio.BuscarPorId(miReader.GetInt32(1)),
                        UnaFormaPago = formpaP.BuscarPorId(miReader.GetInt32(2)), 
                        FechaPago = miReader.GetDateTime(3),
                    };
                    pagarmensualidades.Add(unPagoMensualidad);
                }
                miConexion.Close();
                miConexion.Dispose();
            }
            catch
            {
                throw;
            }
            finally
            {
                if (miConexion.State == ConnectionState.Open)
                {
                    miConexion.Close();
                    miConexion.Dispose();
                }
            }
            return pagarmensualidades;
        }

        public PagarMensualidad BuscarPorId(int id)
        {
            throw new NotImplementedException();
        }
    }
}
