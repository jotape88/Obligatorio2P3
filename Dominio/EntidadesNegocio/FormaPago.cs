﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dominio
{
    [Table("FormasPagos")]
    public abstract class FormaPago
    {
        #region Propiedades
        public int Id { get; set; }
        #endregion

        #region Metodos
        public abstract decimal CostoTotal(dynamic[] datos);
        public abstract string Tipo();
        #endregion
    }
}
