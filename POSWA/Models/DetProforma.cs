namespace POSWA.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("DetProforma")]
    public partial class DetProforma
    {
        [Key]
        [Column(Order = 0)]
        [StringLength(10)]
        public string NumProforma { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int NumLinea { get; set; }

        [StringLength(6)]
        public string Codpro { get; set; }

        
        public string Nompro { get; set; }

        [Column(TypeName = "money")]
        public decimal? Precio { get; set; }

       /* public virtual EncProforma EncProforma { get; set; }

        public virtual Productos Productos { get; set; }*/
    }
}
