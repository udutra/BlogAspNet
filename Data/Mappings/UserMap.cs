using BlogAspNet.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BlogAspNet.Data.Mappings;

public class UserMap :IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        //Tabela
        builder.ToTable("User");
        
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
        
        builder.Property(x => x.Email)
            .IsRequired() //Gera um NOT NULL
            .HasColumnName("Email")
            .HasColumnType("VARCHAR")
            .HasMaxLength(200);
        
        builder.Property(x => x.PasswordHash)            
            .IsRequired() //Gera um NOT NULL
            .HasColumnName("PasswordHash")
            .HasColumnType("VARCHAR")
            .HasMaxLength(255);
        
        builder.Property(x => x.Bio)
            .IsRequired(false) //Gera um NOT NULL
            .HasColumnName("Bio")
            .HasColumnType("TEXT");
        
        builder.Property(x => x.Image)
            .IsRequired(false) //Gera um NOT NULL
            .HasColumnName("Image")
            .HasColumnType("VARCHAR")
            .HasMaxLength(2000);
        
        builder.Property(x => x.Slug)
            .IsRequired() //Gera um NOT NULL
            .HasColumnName("Slug")
            .HasColumnType("VARCHAR")
            .HasMaxLength(80);
        
        //Índices
        builder.HasIndex(x => x.Slug, "IX_User_Slug")
            .IsUnique();
        
        // Relacionamento muitos para muitos
        builder.HasMany(x => x.Roles)
            .WithMany(x => x.Users)
            .UsingEntity<Dictionary<string,object>>(
                "UserRole",
                role => role.HasOne<Role>()
                    .WithMany()
                    .HasForeignKey("RoleId")
                    .HasConstraintName("FK_UserRole_RoleId")
                    .OnDelete(DeleteBehavior.Cascade),
                user => user.HasOne<User>()
                    .WithMany()
                    .HasForeignKey("UserId")
                    .HasConstraintName("FK_UserRole_UserId")
                    .OnDelete(DeleteBehavior.Cascade));
    }
}