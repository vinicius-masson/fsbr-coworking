using Coworking.Domain.Entities;
using Coworking.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Coworking.Infra.Mapping
{
    public class ReservaMapping : IEntityTypeConfiguration<Reserva>
    {
        public void Configure(EntityTypeBuilder<Reserva> builder)
        {
            builder.ToTable("Reservas");
            
            builder.HasKey(r => r.Id)
                .HasName("PK_Reservas");

            builder.Property(r => r.DataInicioReserva)
                .IsRequired()
                .HasColumnType("datetime2(0)");

            builder.Property(r => r.DataFimReserva)
                .IsRequired()
                .HasColumnType("datetime2(0)");

            builder.Property(r => r.UsuarioId).IsRequired();
            builder.Property(r => r.SalaId).IsRequired();

            builder.Property(r => r.Status)
                .IsRequired()
                .HasConversion<string>()
                .HasColumnType("varchar(12)")
                .HasDefaultValue(StatusReserva.Confirmada);

            builder.HasOne(r => r.Sala)
                .WithMany(s => s.Reservas)
                .HasForeignKey(s => s.SalaId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_Reservas_Salas");

            builder.HasOne(r => r.Usuario)
                .WithMany(s => s.Reservas)
                .HasForeignKey(s => s.UsuarioId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_Reservas_Usuarios");
        }
    }
}
