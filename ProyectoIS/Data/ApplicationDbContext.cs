using Microsoft.EntityFrameworkCore;
using ProyectoIS.Models;

namespace ProyectoIS.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<Usuarios> Usuarios { get; set; }
        public DbSet<Roles> Roles { get; set; }
        public DbSet<Generos> Generos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Usuarios>()
                .HasOne(u => u.Rol)
                .WithMany(r => r.Usuarios)
                .HasForeignKey(u => u.IDRol);

            modelBuilder.Entity<Usuarios>()
                .HasOne(u => u.Generos)
                .WithMany()
                .HasForeignKey(u => u.Genero) // ✅ Aquí usamos `Genero`, que es el nombre correcto en la BD
                .OnDelete(DeleteBehavior.Restrict);

            base.OnModelCreating(modelBuilder);
        }
    }
}
