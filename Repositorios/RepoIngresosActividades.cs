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
    public class RepoIngresosActividades : IRepoIngresosActividades
    {
        public bool Alta(IngresoActividad unIngresoAct)
        {
            bool bandera = false;
            int filasAfectadas = 0;
            string miString = @"Data Source=localhost\SQLEXPRESS; Initial Catalog=BaseObligatorio1P3; Integrated Security=SSPI";
            SqlConnection miConexion = new SqlConnection(miString);
            try
            {
                SqlCommand miComando = new SqlCommand();

                string miSqlIngresoAct = @"INSERT INTO IngresosActividades (IdSocio, fechaHora, IdDiaYHora ) VALUES(@idSoc, @fecha, @idDiaYHr); ";
                miComando = new SqlCommand(miSqlIngresoAct, miConexion);
                miComando.Parameters.AddWithValue("@idSoc", unIngresoAct.Soc.Id);
                miComando.Parameters.AddWithValue("@fecha", DateTime.Now);
                miComando.Parameters.AddWithValue("@idDiaYHr", unIngresoAct.DiaYHr.Id);

                miConexion.Open();

                filasAfectadas = miComando.ExecuteNonQuery();

                bandera = filasAfectadas == 1;

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
            return bandera;
        }

        public int ValidarCupos(int idDiaYHora)
        {
            int cantidadCupos = -1;
            string miString = @"Data Source=localhost\SQLEXPRESS; Initial Catalog=BaseObligatorio1P3; Integrated Security=SSPI;";
            SqlConnection miConexion = new SqlConnection(miString);
            try
            {
                string miSql = @"SELECT COUNT(*) FROM IngresosActividades WHERE IdDiaYHora=@idHora AND DAY(fechaHora)=DAY(GETDATE())";
                SqlCommand miComando = new SqlCommand(miSql, miConexion);
                miComando.Parameters.AddWithValue("@idHora", idDiaYHora);

                miConexion.Open();

                cantidadCupos =  (int)miComando.ExecuteScalar();

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
            return cantidadCupos;
        }

        public bool YaIngresoActividad(int idSocio, int idActividad)
        {
            bool bandera = false;
            int cantidadIngresos = -1;

            string miString = @"Data Source=localhost\SQLEXPRESS; Initial Catalog=BaseObligatorio1P3; Integrated Security=SSPI;";
            SqlConnection miConexion = new SqlConnection(miString);
            try
            {
                string miSql = @"SELECT COUNT(*) FROM IngresosActividades IA, Actividades A, DiaYHora DH WHERE IA.IdDiaYHora = DH.Id AND DH.IdActividad = A.Id AND IA.IdSocio = @idSoc AND DH.IdActividad = @idActiv AND DAY(IA.fechaHora) = DAY(GETDATE())";
                SqlCommand miComando = new SqlCommand(miSql, miConexion);
                miComando.Parameters.AddWithValue("@idSoc", idSocio);
                miComando.Parameters.AddWithValue("@idActiv", idActividad);

                miConexion.Open();

                cantidadIngresos = (int)miComando.ExecuteScalar();

                miConexion.Close();
                miConexion.Dispose();

                if (cantidadIngresos > 0)
                {
                    bandera = true;
                }
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

        public bool Baja(int id)
        {
            throw new NotImplementedException();
        }

        public IngresoActividad BuscarPorId(int id)
        {
            throw new NotImplementedException();
        }

        public bool Modificacion(IngresoActividad obj)
        {
            throw new NotImplementedException();
        }

        public List<IngresoActividad> TraerTodo()
        {
            List<IngresoActividad> ingresosActivs = new List<IngresoActividad>();
            string miString = @"Data Source=localhost\SQLEXPRESS; Initial Catalog=BaseObligatorio1P3; Integrated Security=SSPI;";
            SqlConnection miConexion = new SqlConnection(miString);
            try
            {
                string miSql = "SELECT * FROM IngresosActividades";
                SqlCommand miComando = new SqlCommand(miSql, miConexion);

                RepoSocios repoSoc = new RepoSocios();
                RepoDiaYHora repoDH = new RepoDiaYHora();

                miConexion.Open();
                SqlDataReader miReader = miComando.ExecuteReader();
                while (miReader.Read())

                {
                    IngresoActividad unIngresoAct = new IngresoActividad()
                    {
                        Id = miReader.GetInt32(0),
                        FechaYHora = miReader.GetDateTime(2),
                        DiaYHr = repoDH.BuscarPorId(miReader.GetInt32(3)),
                        Soc = repoSoc.BuscarPorId(miReader.GetInt32(1))
                    };
                    ingresosActivs.Add(unIngresoAct);
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
            return ingresosActivs;
        }

        public List<IngresoActividad> TraerTodoPorFechasXIdSocio(DateTime fechaInicio, DateTime FechaFin, int idSocio)
        {
            List<IngresoActividad> ingresosActivs = new List<IngresoActividad>();
            string miString = @"Data Source=localhost\SQLEXPRESS; Initial Catalog=BaseObligatorio1P3; Integrated Security=SSPI;";
            SqlConnection miConexion = new SqlConnection(miString);
            try
            {
                string miSql = "SELECT * FROM IngresosActividades WHERE IdSocio = @idSoc AND fechaHora >= @fechIni AND fechaHora <= DATEADD(s, -1, DATEADD(d, 1, @fechFin));"; //Agregamos un dia, y restamos un seg para quedarnos con 11:59 del mismo dia

                SqlCommand miComando = new SqlCommand(miSql, miConexion);
                miComando.Parameters.AddWithValue("@idSoc", idSocio);
                miComando.Parameters.AddWithValue("@fechIni", fechaInicio);
                miComando.Parameters.AddWithValue("@fechFin", FechaFin);

                RepoSocios repoSoc = new RepoSocios();
                RepoDiaYHora repoDH = new RepoDiaYHora();

                miConexion.Open();
                SqlDataReader miReader = miComando.ExecuteReader();
                while (miReader.Read())

                {
                    IngresoActividad unIngresoAct = new IngresoActividad()
                    {
                        Id = miReader.GetInt32(0),
                        FechaYHora = miReader.GetDateTime(2),
                        DiaYHr = repoDH.BuscarPorId(miReader.GetInt32(3)),
                        Soc = repoSoc.BuscarPorId(miReader.GetInt32(1))
                    };
                    ingresosActivs.Add(unIngresoAct);
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
            return ingresosActivs;
        }



    }
}
