using Coworking.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Coworking.Infra.Mapping
{
    public class SalaMapping : IEntityTypeConfiguration<Sala>
    {
        public void Configure(EntityTypeBuilder<Sala> builder)
        {
            builder.ToTable("Salas");

            builder.HasKey(r => r.Id)
                .HasName("PK_Salas");

            builder.Property(r => r.Descricao)
                .IsRequired()
                .HasColumnType("varchar(30)");

            builder.Property(r => r.Codigo)
                .IsRequired()
                .HasColumnType("varchar(12)")
                .IsUnicode(false);

            builder.Property(r => r.Andar)
                .HasColumnType("varchar(10)");

            builder.Property(r => r.Bloco)
                .HasColumnType("varchar(15)");

        }
    }
}
