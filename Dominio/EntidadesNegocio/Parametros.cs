using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dominio
{
    [Table("ParametrosAuxiliares")]
    public class Parametros
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)] //Tiene una unica fila de valores, la cual va a tener siempre Id 1
        public int Id { get; set; }
        public decimal DescuentoPorAntiguedad { get; set; }
        public int TopeAntiguedad { get; set; }
        public decimal DescPorTopeActiv {get; set;}
        public int TopeActividades { get; set; }
        public decimal ValorMes { get; set; }
        public decimal ValorActividad { get; set; }
    }
}
