﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dominio
{
    [Table("DiasYHoras")]
    public class DiaYHora
    {
        #region Propiedades
        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)] //Usamos las ids del Archivo
        public int Id { get; set; }
        [RegularExpression(@"^(?:Lunes|Martes|Miercoles|Jueves|Viernes)(?:\s*,\s*(?:Lunes|Martes|Miercoles|Jueves|Viernes))*$")] //validamos que los dias de la semana cumplan los parametros cuando importamos desde el archivo
        [MaxLength(10)]
        public string Dia { get; set; }
        [RegularExpression(@"^([7-9]|1[0-9]|2[0-3])$")] //Validamos que las horas de inicio de la actividad sean entre las 7 y las 23 cuando importamos desde el archivo
        public int Hora { get; set; }
        public Actividad Activ { get; set; }
        public int CuposMaximos { get; set; }
        #endregion
    }
}
