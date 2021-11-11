﻿using System;
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
        public bool Alta(Actividad unaAct)
        {
            bool bandera = false;
            int filasAf = 0;
            if (unaAct != null)
            {
                //if (unaAct.ValidarEdadActiv(unaAct.EdadMinima, unaAct.EdadMaxima) && unaAct.ValidarNombreAct(unaAct.Nombre))
                if(unaAct.ValidarEdadActiv(unaAct.EdadMinima, unaAct.EdadMaxima))
                {
                    try
                    {
                        using (ClubContext db = new ClubContext())
                        {
                            db.Actividades.Add(unaAct);
                            filasAf = db.SaveChanges();
                            bandera = filasAf > 0;
                        }
                    }
                    catch(Exception laExc)
                    {
                        return false;
                    }
                }

            }
            return bandera;
        }

        public Actividad BuscarPorId(int id)
        {
            Actividad unaAct = null;
            try
            {
                using (ClubContext db = new ClubContext())
                {
                    unaAct = db.Actividades.Find(id);
                }
            }
            catch
            {
                throw;
            }

            return unaAct;
        }

        public List<Actividad> TraerTodo()
        {
            List<Actividad> actividades = new List<Actividad>();
            try
            {
                using (ClubContext db = new ClubContext())
                {
                    actividades = db.Actividades.ToList();
                }
            }
            catch
            {
                throw;
            }
            return actividades;
        }

        public bool Baja(int id)
        {
            throw new NotImplementedException();
        }

        public bool Modificacion(Actividad obj)
        {
            throw new NotImplementedException();
        }
    }
}
