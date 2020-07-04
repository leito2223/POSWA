namespace POSWA.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Productos
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Productos()
        {
            DetProforma = new HashSet<DetProforma>();
        }

        [Key]
        [StringLength(6)]
        public string Codpro { get; set; }

        [StringLength(50)]
        public string Nompro { get; set; }

        [StringLength(100)]
        public string Descripcion { get; set; }

        [StringLength(2)]
        public string CodCla { get; set; }

        [StringLength(15)]
        public string CodBarras { get; set; }

        [Column(TypeName = "money")]
        public decimal? Precio { get; set; }

        public int? Cantidad { get; set; }

        [StringLength(100)]
        public string Imagen { get; set; }

        public DateTime? FechaCreacion { get; set; }

        public bool? Activo { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DetProforma> DetProforma { get; set; }

        public virtual ProductosClasificacion ProductosClasificacion { get; set; }
    }
}
