using BlogAspNet.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BlogAspNet.Data.Mappings;

public class PostMap : IEntityTypeConfiguration<Post>
{
    public void Configure(EntityTypeBuilder<Post> builder)
    {
        //Tabela
        builder.ToTable("Post");
        
        //Chave Primária
        builder.HasKey(x => x.Id);

        //Identity
        builder.Property(x => x.Id)
            .ValueGeneratedOnAdd()
            .UseIdentityColumn(); //Primary key Identity(1,1)
        
        //Outras propriedades
        builder.Property(x => x.CategoryId)
            .IsRequired() //Gera um NOT NULL
            .HasColumnName("CategoryId")
            .HasColumnType("INT");
        
        builder.Property(x => x.AuthorId)
            .IsRequired() //Gera um NOT NULL
            .HasColumnName("AuthorId")
            .HasColumnType("INT");
        
        builder.Property(x => x.Title)
            .IsRequired() //Gera um NOT NULL
            .HasColumnName("Title")
            .HasColumnType("VARCHAR")
            .HasMaxLength(160);
        
        builder.Property(x => x.Summary)            
            .IsRequired() //Gera um NOT NULL
            .HasColumnName("Summary")
            .HasColumnType("VARCHAR")
            .HasMaxLength(255);
        
        builder.Property(x => x.Body)
            .IsRequired() //Gera um NOT NULL
            .HasColumnName("Body")
            .HasColumnType("TEXT");
        
        builder.Property(x => x.Slug)
            .IsRequired() //Gera um NOT NULL
            .HasColumnName("Slug")
            .HasColumnType("VARCHAR")
            .HasMaxLength(80);

        builder.Property(x => x.CreateDate)
            .IsRequired() //Gera um NOT NULL
            .HasColumnName("CreateDate")
            .HasColumnType("SMALLDATETIME")
            .HasDefaultValueSql("GETDATE()");
            //.HasDefaultValue(DateTime.Now.ToUniversalTime());
        
        builder.Property(x => x.LastUpdateDate)
            .IsRequired() //Gera um NOT NULL
            .HasColumnName("LastUpdateDate")
            .HasColumnType("SMALLDATETIME")
            .HasDefaultValueSql("GETDATE()");
        
        //Índices
        builder.HasIndex(x => x.Slug, "IX_Post_Slug")
            .IsUnique();
        
        //// Relacionamento um para muitos
        builder
            .HasOne(x => x.Author)
            .WithMany(x => x.Posts)
            .HasConstraintName("FK_Post_Author")
            .OnDelete(DeleteBehavior.Cascade);

        builder
            .HasOne(x => x.Category)
            .WithMany(x => x.Posts)
            .HasConstraintName("FK_Post_Category")
            .OnDelete(DeleteBehavior.Cascade);
        
        // Relacionamento muitos para muitos
        builder
            .HasMany(x => x.Tags)
            .WithMany(x => x.Posts)
            .UsingEntity<Dictionary<string, object>>(
                "PostTag",
                post => post.HasOne<Tag>()
                    .WithMany()
                    .HasForeignKey("PostId")
                    .HasConstraintName("FK_PostRole_PostId")
                    .OnDelete(DeleteBehavior.Cascade),
                tag => tag.HasOne<Post>()
                    .WithMany()
                    .HasForeignKey("TagId")
                    .HasConstraintName("FK_PostTag_TagId")
                    .OnDelete(DeleteBehavior.Cascade));
    }
}