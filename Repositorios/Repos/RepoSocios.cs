﻿using System;
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
    public class RepoSocios : IRepoSocios
    {
        #region Metodos de manejo de datos
        public bool Alta(Socio unSocio)
        {
            bool bandera = false;
            if (unSocio != null)
            {
                try
                {
                    using (ClubContext db = new ClubContext())
                    {
                        db.Socios.Add(unSocio);
                        bandera = db.SaveChanges() != 0;
                    }
                }
                catch (Exception laExc)
                {
                    return false;
                }
            }
            return bandera;
        }

        public bool Baja(int id)
        {
            bool bandera = false;
            try
            {
                using (ClubContext db = new ClubContext())
                {
                    Socio unSoc = db.Socios.Find(id);
                    unSoc.EstaActivo = "0";
                    bandera = db.SaveChanges() != 0;
                }
            }
            catch (Exception laExc)
            {
                return false;
            }
            return bandera;
        }


        public Socio BuscarPorCedula(string cedula)
        {
            Socio unSoc = new Socio();
            if (cedula != null) //Esta validacion es para cuando en el DELETE ingresan campos vacios (ademas de la validacion de los data annotations)
            {
                try
                {
                    using (ClubContext db = new ClubContext())
                    {
                        unSoc = db.Socios.Where(s => s.Cedula == cedula).SingleOrDefault();
                    }
                }
                catch
                {
                    return null;
                }
            }
            return unSoc;
        }

        public Socio BuscarPorId(int id)
        {
            Socio unSoc = new Socio();
            {
                try
                {
                    using (ClubContext db = new ClubContext())
                    {
                        unSoc = db.Socios.Find(id);
                    }
                }
                catch
                {
                    return null;
                }
            }
            return unSoc;
        }

        public bool Modificacion(Socio unSocio)
        {
            bool bandera = false;

            using (ClubContext db = new ClubContext())
            {
                if (unSocio != null)
                {
                    Socio unSoc = db.Socios.Find(unSocio.Id);

                    if (unSoc != null)
                    {
                        db.Entry(unSoc).State = EntityState.Detached;
                        db.Entry(unSocio).State = EntityState.Modified;
                        bandera = db.SaveChanges() != 0;
                    }
                }
            }
            return bandera;
        }

        public List<Socio> TraerTodo()
        {
            List<Socio> socios = new List<Socio>();

            using (ClubContext db = new ClubContext())
            {
                socios = db.Socios.OrderBy(s => s.NombreYapellido)
                                  .ThenByDescending(s => s.Cedula.Length) //Como la Cedula es un string, tambien tenemos que ordenar por el largo
                                  .ThenByDescending(s => s.Cedula)
                                  .ToList();
            }

            return socios;
        }
        #endregion
    }
}
