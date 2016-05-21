using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MvcApplication2.Models
{
    public class RotacionEstudianteDetalle
    {

        public int rotacionEstudianteDetalleId { get; set; }
        public int rotacionEstudianteId { get; set; }

        public int IPS_ESEId { get; set; }

        public string horario { get; set; }

        public string servicio { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]

        public System.DateTime fecha_inicio { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]

        public System.DateTime fecha_terminacion { get; set; }
        public virtual IPS_ESE IPS_ESE { get; set; }

        public virtual RotacionEstudiante RotacionEstudiante { get; set; }
    
    }

      
}