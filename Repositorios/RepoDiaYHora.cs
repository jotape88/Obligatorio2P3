using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using Dominio;


namespace Repositorios
{
    public class RepoDiaYHora : IRepoDiaYHora
    {
        public bool Alta(DiaYHora obj)
        {
            throw new NotImplementedException();
        }

        public bool Baja(int id)
        {
            throw new NotImplementedException();
        }

        public DiaYHora BuscarPorId(int id)
        {
            DiaYHora unDiaYHora = null;
            string miString = @"Data Source=localhost\SQLEXPRESS; Initial Catalog=BaseObligatorio1P3; Integrated Security=SSPI;";
            SqlConnection miConexion = new SqlConnection(miString);
            try
            {
                string miSql = "SELECT * FROM DiaYHora WHERE Id=@id;";
                SqlCommand miComando = new SqlCommand(miSql, miConexion);
                RepoActividades repoAct = new RepoActividades();
                miComando.Parameters.AddWithValue("@id", id);

                miConexion.Open();
                SqlDataReader miReader = miComando.ExecuteReader();
                if (miReader.Read())
                {
                    unDiaYHora = new DiaYHora()
                    {
                        Id = miReader.GetInt32(0),
                        Activ = repoAct.BuscarPorId(miReader.GetInt32(1)),
                        Dia = miReader.GetString(2),
                        Hora = miReader.GetDecimal(3),
                        CuposMaximos = miReader.GetInt32(4)
                    };
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
            return unDiaYHora;
        }

        public bool Modificacion(DiaYHora obj)
        {
            throw new NotImplementedException();
        }

        public List<DiaYHora> TraerTodo()
        {
            List<DiaYHora> diasYHoras = new List<DiaYHora>();
            string miString = @"Data Source=localhost\SQLEXPRESS; Initial Catalog=BaseObligatorio1P3; Integrated Security=SSPI;";
            SqlConnection miConexion = new SqlConnection(miString);
            try
            {
                string miSql = "SELECT * FROM DiaYHora";
                SqlCommand miComando = new SqlCommand(miSql, miConexion);

                miConexion.Open();
                SqlDataReader miReader = miComando.ExecuteReader();
                RepoActividades repoAct = new RepoActividades();
                while (miReader.Read())
                {
                    DiaYHora unDiaYHora = new DiaYHora
                    {
                        Id = miReader.GetInt32(0),
                        Activ = repoAct.BuscarPorId(miReader.GetInt32(1)),
                        Dia = miReader.GetString(2),
                        Hora = miReader.GetDecimal(3),
                        CuposMaximos = miReader.GetInt32(4)
                    };
                    diasYHoras.Add(unDiaYHora);
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
            return diasYHoras;
        }


        public List<DiaYHora> TraerTodoFiltrado(string dia, decimal hora)
        {
            List<DiaYHora> diasYHoras = new List<DiaYHora>();
            string miString = @"Data Source=localhost\SQLEXPRESS; Initial Catalog=BaseObligatorio1P3; Integrated Security=SSPI;";
            SqlConnection miConexion = new SqlConnection(miString);
            try
            {
                string miSql = "SELECT * FROM DiaYHora WHERE Dia=@dia COLLATE Latin1_General_CI_AI AND Hora > @hora ORDER BY Hora";
                SqlCommand miComando = new SqlCommand(miSql, miConexion);
                miComando.Parameters.AddWithValue("@dia", dia);
                miComando.Parameters.AddWithValue("@hora", hora);

                miConexion.Open();
                SqlDataReader miReader = miComando.ExecuteReader();

                RepoActividades repoAct = new RepoActividades();

                while (miReader.Read())
                {
                    DiaYHora unDiaYHora = new DiaYHora
                    {
                        Id = miReader.GetInt32(0),
                        Activ = repoAct.BuscarPorId(miReader.GetInt32(1)),
                        Dia = miReader.GetString(2),
                        Hora = miReader.GetDecimal(3)
                    };
                    diasYHoras.Add(unDiaYHora);
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
            return diasYHoras;
        }
    }
}
