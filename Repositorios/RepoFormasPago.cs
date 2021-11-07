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
    public class RepoFormasPago : IRepoFormasPago
    {

        public dynamic[] TraerAuxiliares()
        {
            dynamic[] array2 = null;
            string miString = @"Data Source=localhost\SQLEXPRESS; Initial Catalog=BaseObligatorio1P3; Integrated Security=SSPI;";
            SqlConnection miConexion = new SqlConnection(miString);
            try
            {
                string miSql = "SELECT * FROM Auxiliares;";
                SqlCommand miComando = new SqlCommand(miSql, miConexion);

                miConexion.Open();
                SqlDataReader miReader = miComando.ExecuteReader();
                if (miReader.Read())
                {
                    decimal descuentoPorAntiguedad = miReader.GetDecimal(0);
                    int topeAntiguedad = miReader.GetInt32(1);
                    decimal descPorTopeActiv = miReader.GetDecimal(2);
                    int topeActividades = miReader.GetInt32(3);
                    decimal valorMes = miReader.GetDecimal(4);
                    decimal valorActividad = miReader.GetDecimal(5);

                    dynamic[] array = { descuentoPorAntiguedad, topeAntiguedad, descPorTopeActiv, topeActividades, valorMes, valorActividad };
                  
                    return array;
                };
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
            return array2;
        }


        public bool Alta(FormaPago obj)
        {
            throw new NotImplementedException();
        }

        public bool Baja(int id)
        {
            throw new NotImplementedException();
        }

        public FormaPago BuscarPorId(int id)
        {
            FormaPago formaPago = null;        
            string miString = @"Data Source=localhost\SQLEXPRESS; Initial Catalog=BaseObligatorio1P3; Integrated Security=SSPI;";
            SqlConnection miConexion = new SqlConnection(miString);
            try
            {
                string miSql = "SELECT * FROM FormasPagos WHERE Id=@id;";
                SqlCommand miComando = new SqlCommand(miSql, miConexion);
                miComando.Parameters.AddWithValue("@id", id);

                miConexion.Open();
                SqlDataReader miReader = miComando.ExecuteReader();
                if (miReader.Read())
                {
                    if (miReader.GetString(2) == "Cuponera")
                    {
                        formaPago = new Cuponera()
                        {
                            Id = miReader.GetInt32(0),
                            CantidadActividades = miReader.GetInt32(1),
                        };
                    }
                    else
                    {
                        formaPago = new PaseLibre()
                        {
                            Id = miReader.GetInt32(0),
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
            return formaPago;
    }

        public bool ModificacionCantCuponera(Cuponera unaCuponera)
        {
            bool bandera = false;
            string miString = @"Data Source=localhost\SQLEXPRESS; Initial Catalog=BaseObligatorio1P3; Integrated Security=SSPI;";
            SqlConnection miConexion = new SqlConnection(miString);
            try
            {
                string miSql = "UPDATE FormasPagos SET CantidadActividades=@cantidad WHERE Id=@id;";
                SqlCommand miComando = new SqlCommand(miSql, miConexion);
                miComando.Parameters.AddWithValue("@id", unaCuponera.Id);
                miComando.Parameters.AddWithValue("@cantidad", unaCuponera.CantidadActividades -1);

                miConexion.Open();
                int filasAfectadas = miComando.ExecuteNonQuery();
                miConexion.Close();
                miConexion.Dispose();
                bandera = filasAfectadas == 1;
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
            return bandera;
        }

        public List<FormaPago> TraerTodo()
        {
            List<FormaPago> formasPagos = new List<FormaPago>();
            string miString = @"Data Source=localhost\SQLEXPRESS; Initial Catalog=BaseObligatorio1P3; Integrated Security=SSPI;";
            FormaPago unaFP = null;
            SqlConnection miConexion = new SqlConnection(miString);
            try
            {
                string miSql = "SELECT * FROM FormasPagos";
                SqlCommand miComando = new SqlCommand(miSql, miConexion);

                miConexion.Open();
                SqlDataReader miReader = miComando.ExecuteReader();
                while (miReader.Read())
                {
                    if (miReader.GetString(2) == "Cuponera")
                    {
                        unaFP = new Cuponera()
                        {
                            Id = miReader.GetInt32(0),
                            CantidadActividades = miReader.GetInt32(1),
                        };
                    }
                    else
                    {
                        unaFP = new PaseLibre()
                        {
                            Id = miReader.GetInt32(0),
                        };
                    }
                    formasPagos.Add(unaFP);
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
            return formasPagos;
        }

        public bool Modificacion(FormaPago obj)
        {
            throw new NotImplementedException();
        }

    }
}
