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
    public class RepoUsuarios : IRepoUsuarios
    {
        public bool Alta(Usuario unUsuario)
        {
            bool bandera = false;
            int filasAf = 0;
            if (unUsuario != null)
            {
                //if(unUsuario.ValidarContrasenia(unUsuario.ContraseniaDesencriptada) && unUsuario.ValidarMail(unUsuario.Email)) 
               // {
                    try
                    {
                        using(ClubContext db = new ClubContext())
                        {
                            db.Usuarios.Add(unUsuario);
                            filasAf = db.SaveChanges();
                            bandera = filasAf > 0;
                        }
                    }
                    catch(Exception laExc)
                    {
                        return false;
                    }
               // }
            }
            return bandera;
        }

        public Usuario BuscarPorEmail(string email)
        {
            Usuario unUsuario = null;
            try
            {
                using (ClubContext db = new ClubContext())
                {
                    unUsuario = db.Usuarios.Where(u => u.Email == email).SingleOrDefault(); 
                }
            }
            catch
            {
                throw;
            }
            return unUsuario;
        }

        public List<Usuario> TraerTodo()
        {
            List<Usuario> usuarios = new List<Usuario>();
            try
            {
                using (ClubContext db = new ClubContext())
                {
                    usuarios = db.Usuarios.ToList();
                }
            }
            catch
            {
                throw;
            }
            return usuarios;
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
    }
}
