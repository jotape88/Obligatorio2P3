using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace IngresoActividadWCF
{
    [ServiceContract]
    public interface IServicioIngreso
    {
        [OperationContract]
        bool AltaIngresoActividadDTO(DTOIngresoActividad nuevoIngreso);
    }
}
