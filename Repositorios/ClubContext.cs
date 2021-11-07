using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dominio; 
using System.Data.Entity; 

namespace Repositorios
{
    public class ClubContext: DbContext 
    {
        public DbSet<Actividad> Actividades { get; set; }
        public DbSet<Cuponera> Cuponeras { get; set; }
        public DbSet<DiaYHora> DiasYHoras { get; set; }
        public DbSet<FormaPago> FormaPagos { get; set; }
        public DbSet<IngresoActividad> IngresosActividades { get; set; }
        public DbSet<PagarMensualidad> PagarMensualidades { get; set; }
        public DbSet<PaseLibre> PasesLibres { get; set; }
        public DbSet<Socio> Socios { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }

        public ClubContext() : base("miConexion")
        {
        }

    }
}
