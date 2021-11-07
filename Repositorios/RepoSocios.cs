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
    public class RepoSocios : IRepoSocios
    {
        public bool Alta(Socio unSocio)
        {
            bool bandera = false;
            string miString = @"Data Source=localhost\SQLEXPRESS; Initial Catalog=BaseObligatorio1P3; Integrated Security=SSPI;";
            SqlConnection miConexion = new SqlConnection(miString);
            if (unSocio != null)
            {
                if(unSocio.ValidarNomYApell(unSocio.NombreYapellido) && unSocio.ValidarEdad(unSocio.FechaNacimiento) && unSocio.ValidarCi(unSocio.Cedula)) 
                {
                    try
                    {
                        string miSql = "INSERT INTO Socios(Cedula, NombreYapellido, FechaNacimiento, EstaActivo, FechaRegistro) VALUES(@ci, @nomYAp, @fechaNac, @activo, @fechaReg);";
                        SqlCommand miComando = new SqlCommand(miSql, miConexion);
                        miComando.Parameters.AddWithValue("@ci", unSocio.Cedula);
                        miComando.Parameters.AddWithValue("@nomYAp", unSocio.NombreYapellido);
                        miComando.Parameters.AddWithValue("@fechaNac", unSocio.FechaNacimiento);
                        miComando.Parameters.AddWithValue("@activo", 1);
                        miComando.Parameters.AddWithValue("@fechaReg", DateTime.Now);

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

        public bool Baja(int id)
        {
            bool bandera = false;
            Socio unSocio = new Socio();
            string miString = @"Data Source=localhost\SQLEXPRESS; Initial Catalog=BaseObligatorio1P3; Integrated Security=SSPI;";
            SqlConnection miConexion = new SqlConnection(miString);
            try
            {
                string miSql = "UPDATE Socios SET EstaActivo=@estaActivo WHERE Id=@id;";
                SqlCommand miComando = new SqlCommand(miSql, miConexion);
                miComando.Parameters.AddWithValue("@id", id);
                miComando.Parameters.AddWithValue("@estaActivo", 0);

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


        public Socio BuscarPorCedula(string cedula)
        {
            Socio unSocio = null;
            if(cedula != null) //Esta validacion es para cuando en el DELETE ingresan campos vacios (ademas de la validacion de los data annotations)
            {
                string miString = @"Data Source=localhost\SQLEXPRESS; Initial Catalog=BaseObligatorio1P3; Integrated Security=SSPI;";
                SqlConnection miConexion = new SqlConnection(miString);
                try
                {
                    string miSql = "SELECT * FROM Socios WHERE Cedula=@ci;";
                    SqlCommand miComando = new SqlCommand(miSql, miConexion);
                    miComando.Parameters.AddWithValue("@ci", cedula);

                    miConexion.Open();
                    SqlDataReader miReader = miComando.ExecuteReader();
                    if (miReader.Read())
                    {
                        unSocio = new Socio()
                        {
                            Id = miReader.GetInt32(0),
                            Cedula = miReader.GetString(1),
                            NombreYapellido = miReader.GetString(2),
                            FechaNacimiento = miReader.GetDateTime(3),
                            EstaActivo = miReader.GetString(4),
                            FechaRegistro = miReader.GetDateTime(5)
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
            }
            return unSocio;
        }

        public Socio BuscarPorId(int id) 
        {
            Socio unSocio = null;
                string miString = @"Data Source=localhost\SQLEXPRESS; Initial Catalog=BaseObligatorio1P3; Integrated Security=SSPI;";
                SqlConnection miConexion = new SqlConnection(miString);
                try
                {
                    string miSql = "SELECT * FROM Socios WHERE Id=@id;";
                    SqlCommand miComando = new SqlCommand(miSql, miConexion);
                    miComando.Parameters.AddWithValue("@id", id);

                    miConexion.Open();
                    SqlDataReader miReader = miComando.ExecuteReader();
                    if (miReader.Read())
                    {
                        unSocio = new Socio()
                        {
                            Id = miReader.GetInt32(0),
                            Cedula = miReader.GetString(1),
                            NombreYapellido = miReader.GetString(2),
                            FechaNacimiento = miReader.GetDateTime(3),
                            EstaActivo = miReader.GetString(4),
                            FechaRegistro = miReader.GetDateTime(5)
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
            
            return unSocio;
        }



        public bool Modificacion(Socio unSocio)
        {
            bool bandera = false;
            if(unSocio.ValidarNomYApell(unSocio.NombreYapellido) && unSocio.ValidarEdad(unSocio.FechaNacimiento))
            {
                string miString = @"Data Source=localhost\SQLEXPRESS; Initial Catalog=BaseObligatorio1P3; Integrated Security=SSPI;";
                SqlConnection miConexion = new SqlConnection(miString);
                try
                {
                    string miSql = "UPDATE Socios SET NombreYapellido=@nomYap, FechaNacimiento=@fechaNac WHERE Id=@id;";
                    SqlCommand miComando = new SqlCommand(miSql, miConexion);
                    miComando.Parameters.AddWithValue("@id", unSocio.Id);
                    miComando.Parameters.AddWithValue("@nomYap", unSocio.NombreYapellido);
                    miComando.Parameters.AddWithValue("@fechaNac", unSocio.FechaNacimiento);

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
            return bandera;
        }

        public List<Socio> TraerTodo()
        {
            List<Socio> socios = new List<Socio>();
            string miString = @"Data Source=localhost\SQLEXPRESS; Initial Catalog=BaseObligatorio1P3; Integrated Security=SSPI;";
            SqlConnection miConexion = new SqlConnection(miString);
            try
            {
                string miSql = "SELECT S.Id, S.Cedula, S.NombreYApellido, S.FechaNacimiento, S.EstaActivo, S.FechaRegistro FROM Socios S ORDER BY NombreYApellido ASC, LEN(Cedula) DESC, Cedula DESC;";
                SqlCommand miComando = new SqlCommand(miSql, miConexion);

                miConexion.Open();
                SqlDataReader miReader = miComando.ExecuteReader();
                while (miReader.Read())
                {
                    Socio unSoc = new Socio
                    {
                        Id = miReader.GetInt32(0),
                        Cedula = miReader.GetString(1),
                        NombreYapellido = miReader.GetString(2),
                        FechaNacimiento = miReader.GetDateTime(3),
                        EstaActivo = miReader.GetString(4),
                        FechaRegistro = miReader.GetDateTime(5)
                    };
                        socios.Add(unSoc);
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
            return socios;
        }
    }
}
