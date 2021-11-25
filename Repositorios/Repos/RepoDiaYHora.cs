using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using Dominio;
using System.Data.Entity;


namespace Repositorios
{
    public class RepoDiaYHora : IRepoDiaYHora
    {
        #region Metodos de manejo de datos
        public bool Alta(DiaYHora unDiaYHr)
        {
            bool bandera = false;
            if (unDiaYHr != null)
            {
                try
                {
                    using (ClubContext db = new ClubContext())
                    {
                        db.DiasYHoras.Add(unDiaYHr);
                        db.Entry(unDiaYHr.Activ).State = EntityState.Unchanged;
                        bandera = db.SaveChanges() != 0;
                    }
                }
                catch(Exception laExc)
                {
                    return false;
                }
            }
            return bandera;
        }

        public DiaYHora BuscarPorId(int id)
        {
            DiaYHora unDiaYHora = null;
            try
            {
                using (ClubContext db = new ClubContext())
                {
                    unDiaYHora = db.DiasYHoras.Find(id);
                }
            }
            catch
            {
                throw;
            }
            return unDiaYHora;
        }

        public bool BuscarActivsEnMismoDiaYHora(DiaYHora unDhYHr) //Con esto verificamos si hay actividades repetidas (misma activ en un mismo dia y hora) en el archivo DiasYhoras
        {
            using (ClubContext db = new ClubContext())
            {
                return db.DiasYHoras.Any(dh => dh.Activ.Id == unDhYHr.Activ.Id && dh.Dia == unDhYHr.Dia && dh.Hora == unDhYHr.Hora);
            }
        }
        #endregion

        #region Metodos no implementados

        public bool Baja(int id)
        {
            throw new NotImplementedException();
        }

        public bool Modificacion(DiaYHora obj)
        {
            throw new NotImplementedException();
        }

        public List<DiaYHora> TraerTodo()
        {
            throw new NotImplementedException();
        }

        public List<DiaYHora> TraerTodoFiltrado(string dia, decimal hora)
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
