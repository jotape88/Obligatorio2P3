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
    public class RepoUsuarios : IRepoUsuarios
    {
        public bool Alta(Usuario unUsuario)
        {
            bool bandera = false;
            string miString = @"Data Source=localhost\SQLEXPRESS; Initial Catalog=BaseObligatorio1P3; Integrated Security=SSPI;";
            SqlConnection miConexion = new SqlConnection(miString);
            if (unUsuario != null)
            {
                if(unUsuario.ValidarContrasenia(unUsuario.Contrasenia) && unUsuario.ValidarMail(unUsuario.Email)) 
                {
                    try
                    {
                        string miSql = "INSERT INTO Usuarios(Email, Contrasenia) VALUES(@email, @pass);";
                        SqlCommand miComando = new SqlCommand(miSql, miConexion);
                        miComando.Parameters.AddWithValue("@email", unUsuario.Email);
                        miComando.Parameters.AddWithValue("@pass", unUsuario.Contrasenia);

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
                }

            }
            return bandera;
        }

        public Usuario BuscarPorEmail(string email)
        {
            Usuario unUsuario = null;
            string miString = @"Data Source=localhost\SQLEXPRESS; Initial Catalog=BaseObligatorio1P3; Integrated Security=SSPI;";
            SqlConnection miConexion = new SqlConnection(miString);
                try
                {
                    string miSql = "SELECT * FROM Usuarios WHERE Email=@email;";
                    SqlCommand miComando = new SqlCommand(miSql, miConexion);
                    miComando.Parameters.AddWithValue("@email", email);
                    miConexion.Open();
                    SqlDataReader miReader = miComando.ExecuteReader();

                    if (miReader.Read())
                    {
                        unUsuario = new Usuario()
                        {
                            Email = miReader.GetString(1),
                            Contrasenia = miReader.GetString(2)
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
            return unUsuario;
        }



        public bool Baja(int id)
        {
            throw new NotImplementedException();
        }

        public Usuario BuscarPorId(int id)
        {
            throw new NotImplementedException();
        }

        public bool Modificacion(Usuario obj)
        {
            throw new NotImplementedException();
        }

        public List<Usuario> TraerTodo()
        {
            List<Usuario> usuarios = new List<Usuario>();
            string miString = @"Data Source=localhost\SQLEXPRESS; Initial Catalog=BaseObligatorio1P3; Integrated Security=SSPI;";
            SqlConnection miConexion = new SqlConnection(miString);
            try
            {
                string miSql = "SELECT * FROM Usuarios ";
                SqlCommand miComando = new SqlCommand(miSql, miConexion);

                miConexion.Open();
                SqlDataReader miReader = miComando.ExecuteReader();
                while (miReader.Read())
                {
                    Usuario unUsu = new Usuario
                    {
                        
                        Email = miReader.GetString(1),
                        Contrasenia = miReader.GetString(2)
                    };
                    usuarios.Add(unUsu);
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
            return usuarios;
        }
    }
}
