using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Entities.Entities;

public partial class AgendaVirtualContext : DbContext
{
    public AgendaVirtualContext()
    {
    }

    public AgendaVirtualContext(DbContextOptions<AgendaVirtualContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Comentario> Comentarios { get; set; }

    public virtual DbSet<Equipo> Equipos { get; set; }

    public virtual DbSet<Evento> Eventos { get; set; }

    public virtual DbSet<Recordatorio> Recordatorios { get; set; }

    public virtual DbSet<Tarea> Tareas { get; set; }

    public virtual DbSet<Usuario> Usuarios { get; set; }

    public virtual DbSet<UsuarioEquipo> UsuarioEquipos { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Comentario>(entity =>
        {
            entity.HasKey(e => e.IdComentario).HasName("PK__comentar__1BA6C6F478D01CB0");

            entity.ToTable("comentarios");

            entity.Property(e => e.IdComentario).HasColumnName("id_comentario");
            entity.Property(e => e.FechaHora)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("fecha_hora");
            entity.Property(e => e.IdTarea).HasColumnName("id_tarea");
            entity.Property(e => e.IdUsuario).HasColumnName("id_usuario");
            entity.Property(e => e.Texto)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("texto");

            entity.HasOne(d => d.IdTareaNavigation).WithMany(p => p.Comentarios)
                .HasForeignKey(d => d.IdTarea)
                .HasConstraintName("FK__comentari__id_ta__628FA481");

            entity.HasOne(d => d.IdUsuarioNavigation).WithMany(p => p.Comentarios)
                .HasForeignKey(d => d.IdUsuario)
                .HasConstraintName("FK__comentari__id_us__6383C8BA");
        });

        modelBuilder.Entity<Equipo>(entity =>
        {
            entity.HasKey(e => e.IdEquipo).HasName("PK__equipos__EE01F88AE5FCB585");

            entity.ToTable("equipos");

            entity.Property(e => e.IdEquipo).HasColumnName("id_equipo");
            entity.Property(e => e.Descripcion)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("descripcion");
            entity.Property(e => e.FechaCreacion)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("fecha_creacion");
            entity.Property(e => e.Nombre)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("nombre");
        });

        modelBuilder.Entity<Evento>(entity =>
        {
            entity.HasKey(e => e.IdEvento).HasName("PK__eventos__AF150CA5CB585A72");

            entity.ToTable("eventos");

            entity.Property(e => e.IdEvento).HasColumnName("id_evento");
            entity.Property(e => e.Descripcion)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("descripcion");
            entity.Property(e => e.FechaHoraFin)
                .HasColumnType("datetime")
                .HasColumnName("fecha_hora_fin");
            entity.Property(e => e.FechaHoraInicio)
                .HasColumnType("datetime")
                .HasColumnName("fecha_hora_inicio");
            entity.Property(e => e.IdEquipo).HasColumnName("id_equipo");
            entity.Property(e => e.IdUsuario).HasColumnName("id_usuario");
            entity.Property(e => e.Titulo)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("titulo");
            entity.Property(e => e.Ubicacion)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("ubicacion");

            entity.HasOne(d => d.IdEquipoNavigation).WithMany(p => p.Eventos)
                .HasForeignKey(d => d.IdEquipo)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__eventos__id_equi__59FA5E80");

            entity.HasOne(d => d.IdUsuarioNavigation).WithMany(p => p.Eventos)
                .HasForeignKey(d => d.IdUsuario)
                .HasConstraintName("FK__eventos__id_usua__59063A47");
        });

        modelBuilder.Entity<Recordatorio>(entity =>
        {
            entity.HasKey(e => e.IdRecordatorio).HasName("PK__recordat__F82EFCBB439E89ED");

            entity.ToTable("recordatorios");

            entity.Property(e => e.IdRecordatorio).HasColumnName("id_recordatorio");
            entity.Property(e => e.Enviado)
                .HasDefaultValue(false)
                .HasColumnName("enviado");
            entity.Property(e => e.FechaHora)
                .HasColumnType("datetime")
                .HasColumnName("fecha_hora");
            entity.Property(e => e.IdTarea).HasColumnName("id_tarea");
            entity.Property(e => e.IdUsuario).HasColumnName("id_usuario");
            entity.Property(e => e.Mensaje)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("mensaje");

            entity.HasOne(d => d.IdTareaNavigation).WithMany(p => p.Recordatorios)
                .HasForeignKey(d => d.IdTarea)
                .HasConstraintName("FK__recordato__id_ta__5EBF139D");

            entity.HasOne(d => d.IdUsuarioNavigation).WithMany(p => p.Recordatorios)
                .HasForeignKey(d => d.IdUsuario)
                .HasConstraintName("FK__recordato__id_us__5DCAEF64");
        });

        modelBuilder.Entity<Tarea>(entity =>
        {
            entity.HasKey(e => e.IdTarea).HasName("PK__tareas__C0ECF7074D2CA022");

            entity.ToTable("tareas");

            entity.Property(e => e.IdTarea).HasColumnName("id_tarea");
            entity.Property(e => e.Descripcion)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("descripcion");
            entity.Property(e => e.Estado)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("estado");
            entity.Property(e => e.FechaLimite)
                .HasColumnType("datetime")
                .HasColumnName("fecha_limite");
            entity.Property(e => e.IdEquipo).HasColumnName("id_equipo");
            entity.Property(e => e.IdUsuario).HasColumnName("id_usuario");
            entity.Property(e => e.Prioridad)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("prioridad");
            entity.Property(e => e.Titulo)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("titulo");

            entity.HasOne(d => d.IdEquipoNavigation).WithMany(p => p.Tareas)
                .HasForeignKey(d => d.IdEquipo)
                .HasConstraintName("FK__tareas__id_equip__5629CD9C");

            entity.HasOne(d => d.IdUsuarioNavigation).WithMany(p => p.Tareas)
                .HasForeignKey(d => d.IdUsuario)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__tareas__id_usuar__5535A963");
        });

        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.HasKey(e => e.IdUsuario).HasName("PK__usuarios__4E3E04AD8B164C0B");

            entity.ToTable("usuarios");

            entity.HasIndex(e => e.Correo, "UQ__usuarios__2A586E0BF48DB8F4").IsUnique();

            entity.Property(e => e.IdUsuario).HasColumnName("id_usuario");
            entity.Property(e => e.Contrasena)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("contrasena");
            entity.Property(e => e.Correo)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("correo");
            entity.Property(e => e.FechaRegistro)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("fecha_registro");
            entity.Property(e => e.Nombre)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("nombre");
            entity.Property(e => e.Rol)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("rol");
        });

        modelBuilder.Entity<UsuarioEquipo>(entity =>
        {
            entity.HasKey(e => new { e.IdUsuario, e.IdEquipo }).HasName("PK__usuario___F0DE1B2597A96E71");

            entity.ToTable("usuario_equipo");

            entity.Property(e => e.IdUsuario).HasColumnName("id_usuario");
            entity.Property(e => e.IdEquipo).HasColumnName("id_equipo");
            entity.Property(e => e.FechaAsignacion)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("fecha_asignacion");

            entity.HasOne(d => d.IdEquipoNavigation).WithMany(p => p.UsuarioEquipos)
                .HasForeignKey(d => d.IdEquipo)
                .HasConstraintName("FK__usuario_e__id_eq__52593CB8");

            entity.HasOne(d => d.IdUsuarioNavigation).WithMany(p => p.UsuarioEquipos)
                .HasForeignKey(d => d.IdUsuario)
                .HasConstraintName("FK__usuario_e__id_us__5165187F");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
