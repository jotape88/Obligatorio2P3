using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;
using Dominio;

namespace IngresoActividadWCF
{
    [DataContract]
    public class DTOIngresoActividad
    {
        [DataMember]
        public int Id { get; set; }
        
        [DataMember]
        public DiaYHora DiaYHr { get; set; }
        
        [DataMember]
        public DateTime FechaYHora { get; set; }
        
        [DataMember]
        public Socio Socio { get; set; }
    }
}