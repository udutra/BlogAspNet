using BlogAspNet.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BlogAspNet.Data.Mappings;

public class CategoryMap: IEntityTypeConfiguration<Category>
{
    public void Configure(EntityTypeBuilder<Category> builder)
    {
        //Tabela
        builder.ToTable("Category");
        
        //Chave Primária
        builder.HasKey(x => x.Id);

        //Identity
        builder.Property(x => x.Id)
            .ValueGeneratedOnAdd()
            .UseIdentityColumn(); //Primary key Identity(1,1)
        
        //Outras propriedades
        builder.Property(x => x.Name)
            .IsRequired() //Gera um NOT NULL
            .HasColumnName("Name")
            .HasColumnType("NVARCHAR")
            .HasMaxLength(80);
        
        //Outras propriedades
        builder.Property(x => x.Slug)
            .IsRequired() //Gera um NOT NULL
            .HasColumnName("Slug")
            .HasColumnType("VARCHAR")
            .HasMaxLength(80);
        
        //Índices
        builder.HasIndex(x => x.Slug, "IX_Category_Slug")
            .IsUnique();
    }
}