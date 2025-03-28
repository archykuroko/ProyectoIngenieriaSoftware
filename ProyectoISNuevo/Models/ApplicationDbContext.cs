using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace ProyectoISNuevo.Models
{
    public partial class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext()
        {
        }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Genero> Generos { get; set; }
        public virtual DbSet<Role> Roles { get; set; }
        public virtual DbSet<Usuario> Usuarios { get; set; }
        public virtual DbSet<Cita> Citas { get; set; } 

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Genero>(entity =>
            {
                entity.HasKey(e => e.IdGenero);
                entity.Property(e => e.Nombre).HasMaxLength(50);
            });

            modelBuilder.Entity<Role>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");
                entity.Property(e => e.Nombre).HasMaxLength(100);
            });

            modelBuilder.Entity<Usuario>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");
                entity.Property(e => e.ApellidoMaterno).HasMaxLength(100);
                entity.Property(e => e.ApellidoPaterno).HasMaxLength(100);
                entity.Property(e => e.Contraseña).HasMaxLength(255);
                entity.Property(e => e.Correo).HasMaxLength(100);
                entity.Property(e => e.Idrol).HasColumnName("IDRol");
                entity.Property(e => e.Nombre).HasMaxLength(100);
                entity.Property(e => e.Usuario1)
                    .HasMaxLength(100)
                    .HasColumnName("Usuario");

                // ✅ Agregar la relación entre Usuarios y Roles
                entity.HasOne(u => u.Rol)
                    .WithMany()
                    .HasForeignKey(u => u.Idrol)
                    .HasConstraintName("FK_Usuarios_Roles");
            });

            modelBuilder.Entity<Cita>(entity =>
            {
                entity.HasKey(e => e.IdCita);
                entity.Property(e => e.IdCita).HasMaxLength(6).IsRequired();
                entity.Property(e => e.Motivo).HasMaxLength(255).IsRequired();
                entity.Property(e => e.Estado).HasMaxLength(50).HasDefaultValue("Pendiente");

                entity.HasOne(e => e.Usuario)
                    .WithMany()
                    .HasForeignKey(e => e.UsuarioId)
                    .HasConstraintName("FK_Citas_Usuarios");
            });


            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
