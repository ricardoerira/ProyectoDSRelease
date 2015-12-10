﻿//------------------------------------------------------------------------------
// <auto-generated>
//    Este código se generó a partir de una plantilla.
//
//    Los cambios manuales en este archivo pueden causar un comportamiento inesperado de la aplicación.
//    Los cambios manuales en este archivo se sobrescribirán si se regenera el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace MvcApplication2.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    
    public partial class Familia
    {
        public Familia()
        {
            this.HojaVida = new HashSet<HojaVida>();
        }
    
        public int familiaId { get; set; }
        public string primer_nombre_padre { get; set; }
        public string segundo_nombre_padre { get; set; }
        public string primer_apellido_padre { get; set; }
        public string segundo_apellido_padre { get; set; }
        public string direccion_padre { get; set; }
        public long telefono_padre { get; set; }
        public string primer_nombre_madre { get; set; }
        public string segundo_nombre_madre { get; set; }
        public string primer_apellido_madre { get; set; }
        public string segundo_apellido_madre { get; set; }
        public long telefono_madre { get; set; }
        
        public string direccion_madre { get; set; }



        //[Required]
        public string primer_nombre_acudiente { get; set; }
        public string segundo_nombre_acudiente { get; set; }
         // [Required]
        public string primer_apellido_acudiente { get; set; }
          //[Required]
        public string segundo_apellido_acudiente { get; set; }
         // [Required]
        public string direccion_acudiente { get; set; }
         // [Required]
          //[RegularExpression("([1-9][0-9]*)", ErrorMessage = "Campo númerico")]
          public long telefono_acudiente { get; set; }
         // [Required]
         // [Range(3000000000, 3999999999)]
          public long celular_acudiente { get; set; }
        
        public virtual ICollection<HojaVida> HojaVida { get; set; }
    }
}
