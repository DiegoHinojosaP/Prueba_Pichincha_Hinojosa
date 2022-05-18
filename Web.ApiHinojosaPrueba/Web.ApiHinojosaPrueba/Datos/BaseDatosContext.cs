using Microsoft.EntityFrameworkCore;
using Web.ApiHinojosaPrueba.Modelos;

#nullable disable

namespace Web.ApiHinojosaPrueba.Datos
{
    public partial class BaseDatosContext : DbContext
    {
        public BaseDatosContext(DbContextOptions<BaseDatosContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Cliente> Clientes { get; set; }
        public virtual DbSet<Cuenta> Cuentas { get; set; }
        public virtual DbSet<Movimiento> Movimientos { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "Latin1_General_CI_AI");

            modelBuilder.Entity<Cliente>(entity =>
            {
                entity.HasKey(e => e.CliIdCliente);

                entity.ToTable("cliente");

                entity.Property(e => e.CliIdCliente).HasColumnName("cli_id_cliente");

                entity.Property(e => e.CliContrasenia)
                    .IsRequired()
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("cli_contrasenia");

                entity.Property(e => e.CliDireccion)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("cli_direccion");

                entity.Property(e => e.CliEdad).HasColumnName("cli_edad");

                entity.Property(e => e.CliEstado).HasColumnName("cli_estado");

                entity.Property(e => e.CliGenero)
                    .IsRequired()
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("cli_genero");

                entity.Property(e => e.CliIdentificacion)
                    .IsRequired()
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("cli_identificacion");

                entity.Property(e => e.CliNombre)
                    .IsRequired()
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("cli_nombre");

                entity.Property(e => e.CliTelefono)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("cli_telefono");
            });

            modelBuilder.Entity<Cuenta>(entity =>
            {
                entity.HasKey(e => e.CueNumero)
                    .HasName("PK__cuenta__5138EEC71FAE4CF6");

                entity.ToTable("cuenta");

                entity.Property(e => e.CueNumero)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("cue_numero");

                entity.Property(e => e.CliIdCliente).HasColumnName("cli_id_cliente");

                entity.Property(e => e.CueEstado).HasColumnName("cue_estado");

                entity.Property(e => e.CueSaldo)
                    .HasColumnType("money")
                    .HasColumnName("cue_saldo");

                entity.Property(e => e.CueTipo)
                    .IsRequired()
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("cue_tipo");

                entity.HasOne(d => d.CliIdClienteNavigation)
                    .WithMany(p => p.Cuenta)
                    .HasForeignKey(d => d.CliIdCliente)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_cuenta_cuenta");
            });

            modelBuilder.Entity<Movimiento>(entity =>
            {
                entity.HasKey(e => e.MovIdMovimiento)
                    .HasName("PK__movimien__F074D493CB48DA0D");

                entity.ToTable("movimiento");

                entity.Property(e => e.MovIdMovimiento).HasColumnName("mov_id_movimiento");

                entity.Property(e => e.CueNumero)
                    .IsRequired()
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("cue_numero");

                entity.Property(e => e.MovFecha)
                    .HasColumnType("datetime")
                    .HasColumnName("mov_fecha");

                entity.Property(e => e.MovMovimientoValor)
                    .HasColumnType("money")
                    .HasColumnName("mov_movimiento_valor");

                entity.Property(e => e.MovSaldoActual)
                    .HasColumnType("money")
                    .HasColumnName("mov_saldo_actual");

                entity.Property(e => e.MovSaldoInicial)
                    .HasColumnType("money")
                    .HasColumnName("mov_saldo_inicial");

                entity.Property(e => e.MovTipo)
                    .IsRequired()
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("mov_tipo");

                entity.HasOne(d => d.MoNumeroCuentaNavigation)
                    .WithMany(p => p.Movimientos)
                    .HasForeignKey(d => d.CueNumero)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_movimientos_cuentas");
            });
        }
    }
}
