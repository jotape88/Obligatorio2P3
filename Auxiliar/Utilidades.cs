using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dominio;
using Repositorios;

namespace Auxiliar
{
    public class Utilidades
    {
        public static bool GenerarBDAlInicio() //Metodo para crear la BD si esta aún no existe
        {
            using (ClubContext db = new ClubContext())
            {
                return db.Database.CreateIfNotExists();
            }
        }
    }
}
