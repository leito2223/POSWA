 
namespace POSWA.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    public partial class Parametros
    {
        [Key]

        public int Id { get; set; }

        [StringLength(500)]
        public string UrlImagenesApp { get; set; }
    }
}