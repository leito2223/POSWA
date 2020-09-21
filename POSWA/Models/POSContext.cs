namespace POSWA.Models
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class POSContext : DbContext
    {
        public POSContext()
            : base("name=POSContext")
        {
        }

        public virtual DbSet<Clientes> Clientes { get; set; }
        public virtual DbSet<DetProforma> DetProforma { get; set; }
        public virtual DbSet<EncProforma> EncProforma { get; set; }
        public virtual DbSet<Productos> Productos { get; set; }
        public virtual DbSet<ProductosClasificacion> ProductosClasificacion { get; set; }
        public virtual DbSet<sysdiagrams> sysdiagrams { get; set; }
        public virtual DbSet<Usuarios> Usuarios { get; set; }
        public virtual DbSet<Parametros> Parametros { get; set; }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Clientes>()
                .Property(e => e.Nombre)
                .IsUnicode(false);

            modelBuilder.Entity<Clientes>()
                .Property(e => e.Telefono)
                .IsUnicode(false);

            modelBuilder.Entity<Clientes>()
                .Property(e => e.Email)
                .IsUnicode(false);

            modelBuilder.Entity<Clientes>()
                .Property(e => e.Direccion)
                .IsUnicode(false);

            modelBuilder.Entity<DetProforma>()
                .Property(e => e.NumProforma)
                .IsUnicode(false);

            modelBuilder.Entity<DetProforma>()
                .Property(e => e.Codpro)
                .IsUnicode(false);

            modelBuilder.Entity<DetProforma>()
                .Property(e => e.Nompro)
                .IsUnicode(false);

            modelBuilder.Entity<DetProforma>()
                .Property(e => e.Precio)
                .HasPrecision(19, 4);

            modelBuilder.Entity<EncProforma>()
                .Property(e => e.NumProforma)
                .IsUnicode(false);

            modelBuilder.Entity<EncProforma>()
                .Property(e => e.SubTotal)
                .HasPrecision(19, 4);

            modelBuilder.Entity<EncProforma>()
                .Property(e => e.Impuesto)
                .HasPrecision(19, 4);

            modelBuilder.Entity<EncProforma>()
                .Property(e => e.Descuento)
                .HasPrecision(19, 4);

            modelBuilder.Entity<EncProforma>()
                .Property(e => e.TipoPago)
                .IsFixedLength()
                .IsUnicode(false);

            //modelBuilder.Entity<EncProforma>()
            //    .HasMany(e => e.DetProforma)
            //    .WithRequired(e => e.EncProforma)
            //    .WillCascadeOnDelete(false);

            modelBuilder.Entity<Productos>()
                .Property(e => e.Codpro)
                .IsUnicode(false);

            modelBuilder.Entity<Productos>()
                .Property(e => e.Nompro)
                .IsUnicode(false);

            modelBuilder.Entity<Productos>()
                .Property(e => e.Descripcion)
                .IsUnicode(false);

            modelBuilder.Entity<Productos>()
                .Property(e => e.CodCla)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<Productos>()
                .Property(e => e.CodBarras)
                .IsUnicode(false);

            modelBuilder.Entity<Productos>()
                .Property(e => e.Precio)
                .HasPrecision(19, 4);

            modelBuilder.Entity<Productos>()
                .Property(e => e.Imagen)
                .IsUnicode(false);

            modelBuilder.Entity<ProductosClasificacion>()
                .Property(e => e.CodClasificacion)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<ProductosClasificacion>()
                .Property(e => e.Nombre)
                .IsUnicode(false);

            //modelBuilder.Entity<ProductosClasificacion>()
            //    .HasMany(e => e.Productos)
            //    .WithOptional(e => e.ProductosClasificacion)
            //    .HasForeignKey(e => e.CodCla);

            modelBuilder.Entity<Usuarios>()
                .Property(e => e.Nombre)
                .IsUnicode(false);

            modelBuilder.Entity<Usuarios>()
                .Property(e => e.Clave)
                .IsUnicode(false);
        }
    }
}
