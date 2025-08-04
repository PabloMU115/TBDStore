using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TBD.Models;

namespace TBD.Data
{
    public class ApplicationDbContext : IdentityDbContext<Usuario>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder); // <- ¡IMPORTANTE!

            modelBuilder.Entity<WishList>()
                .HasKey(c => new { c.IdUsuario, c.IdProducto });

            modelBuilder.Entity<Carrito>()
                .HasKey(c => new { c.IdUsuario, c.IdProducto });

            modelBuilder.Entity<Carrito>()
                .HasOne(c => c.Usuario)
                .WithMany(u => u.Carrito)
                .HasForeignKey(c => c.IdUsuario)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Producto>()
                .Property(p => p.Precio)
                .HasColumnType("decimal(18,2)");
        }

        public DbSet<IdentityUserRole<string>> UserRoles { get; set; }
        public DbSet<IdentityRole> Roles { get; set; }
        public DbSet<Proveedor> Proveedores { get; set; }
        public DbSet<Producto> Productos { get; set; }
        public DbSet<Categoria> Categorias { get; set; }
        public DbSet<Carrito> Carrito { get; set; }
        public DbSet<WishList> ListaDeseos { get; set; }
        public DbSet<Direccion> Direcciones { get; set; }
        public DbSet<Pedido> Pedidos { get; set; }
        public DbSet<Orden> Ordenes { get; set; }
        public DbSet<ContadorVistas> ContadorVistas { get; set; }
        public DbSet<HistorialVentas> HistorialVentas { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }
    }
}
