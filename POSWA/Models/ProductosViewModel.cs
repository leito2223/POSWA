using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace POSWA.Models
{
    public class ProductosViewModel
    {
        public string Codpro { get; set; }

        
        public string Nompro { get; set; }

       
        public string Descripcion { get; set; }

      
        public string CodCla { get; set; }

  
        public string CodBarras { get; set; }

 
        public decimal? Precio { get; set; }

        public int? Cantidad { get; set; }

 
        public string Imagen { get; set; }

        public DateTime? FechaCreacion { get; set; }

        public bool? Activo { get; set; }
        public string ImagenBase64 { get; set; }
    }
}