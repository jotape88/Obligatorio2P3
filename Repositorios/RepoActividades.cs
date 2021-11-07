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
    public class RepoActividades : IRepoActividades
    {
        public bool Alta(Actividad obj)
        {
            throw new NotImplementedException();
        }

        public bool Baja(int id)
        {
            throw new NotImplementedException();
        }

        public Actividad BuscarPorId(int id)
        {
            Actividad unaAct = null;
            string miString = @"Data Source=localhost\SQLEXPRESS; Initial Catalog=BaseObligatorio1P3; Integrated Security=SSPI;";
            SqlConnection miConexion = new SqlConnection(miString);
            try
            {
                string miSql = "SELECT * FROM Actividades WHERE Id=@id;";
                SqlCommand miComando = new SqlCommand(miSql, miConexion);
                miComando.Parameters.AddWithValue("@id", id);

                miConexion.Open();
                SqlDataReader miReader = miComando.ExecuteReader();
                if (miReader.Read())
                {
                    unaAct = new Actividad()
                    {
                        Id = miReader.GetInt32(0),
                        Nombre = miReader.GetString(1),
                        EdadMinima = miReader.GetInt32(2),
                        EdadMaxima = miReader.GetInt32(3),
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
            return unaAct;
        }

        public bool Modificacion(Actividad obj)
        {
            throw new NotImplementedException();
        }

        public List<Actividad> TraerTodo()
        {
            List<Actividad> actividades = new List<Actividad>();
            string miString = @"Data Source=localhost\SQLEXPRESS; Initial Catalog=BaseObligatorio1P3; Integrated Security=SSPI;";
            SqlConnection miConexion = new SqlConnection(miString);
            try
            {
                string miSql = "SELECT * FROM Actividades";
                SqlCommand miComando = new SqlCommand(miSql, miConexion);

                miConexion.Open();
                SqlDataReader miReader = miComando.ExecuteReader();
                while (miReader.Read())
                {
                    Actividad unaAct = new Actividad
                    {
                        Id = miReader.GetInt32(0),
                        Nombre = miReader.GetString(1),
                        EdadMinima = miReader.GetInt32(2),
                        EdadMaxima = miReader.GetInt32(3),
                    };
                    actividades.Add(unaAct);
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
            return actividades;
        }
    }
}
