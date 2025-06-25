using Coworking.Domain.Entities;
using Coworking.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Coworking.Infra.Mapping
{
    public class UsuarioMapping : IEntityTypeConfiguration<Usuario>
    {
        public void Configure(EntityTypeBuilder<Usuario> builder)
        {
            builder.ToTable("Usuarios");

            builder.HasKey(r => r.Id)
                .HasName("PK_Usuarios");

            builder.Property(r => r.Nome)
                .IsRequired()
                .HasColumnType("varchar(40)");

            builder.Property(r => r.Email)
                .IsRequired()
                .HasColumnType("varchar(30)");

            builder.Property(r => r.Telefone)
                .HasColumnType("varchar(14)")
                .IsUnicode(false);

            builder.Property(r => r.Status)
                .IsRequired()
                .HasConversion<string>()
                .HasColumnType("varchar(12)")
                .HasDefaultValue(StatusUsuario.Ativo);
        }
    }
}
