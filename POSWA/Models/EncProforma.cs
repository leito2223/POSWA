namespace POSWA.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("EncProforma")]
    public partial class EncProforma
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public EncProforma()
        {
            DetProforma = new HashSet<DetProforma>();
        }

        [Key]
        [StringLength(10)]
        public string NumProforma { get; set; }

        public int? CodCliente { get; set; }

        public int? CodUsuario { get; set; }

        public DateTime? FecFactura { get; set; }

        [Column(TypeName = "money")]
        public decimal? SubTotal { get; set; }

        [Column(TypeName = "money")]
        public decimal? Impuesto { get; set; }

        [Column(TypeName = "money")]
        public decimal? Descuento { get; set; }

        [StringLength(2)]
        public string TipoPago { get; set; }

        public virtual Clientes Clientes { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DetProforma> DetProforma { get; set; }

        public virtual Usuarios Usuarios { get; set; }
    }
}
