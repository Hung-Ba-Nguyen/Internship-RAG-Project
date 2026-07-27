#nullable enable

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RAGKnowledgeBase.Core.Domain.Entities.Document;

namespace RAGKnowledgeBase.Infrastructure.Persistence.DocumentDb.Configurations;

public class DocumentChunkConfiguration : IEntityTypeConfiguration<DocumentChunk>
{
    public void Configure(EntityTypeBuilder<DocumentChunk> builder)
    {
        builder.ToTable("DocumentChunks");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).ValueGeneratedNever();
            builder.Property(x => x.DocumentId).IsRequired();
            builder.Property(x => x.ChunkIndex).IsRequired();
            builder.Property(x => x.Content).HasColumnType("nvarchar(max)").IsRequired();
            builder.Property(x => x.TokenCount).IsRequired();
            builder.Property(x => x.Metadata).HasMaxLength(1024);
            builder.Property(x => x.CreatedAt).IsRequired();

            builder.HasOne(x => x.Document)
                .WithMany(x => x.Chunks)
                .HasForeignKey(x => x.DocumentId)
                .OnDelete(DeleteBehavior.Cascade);
    }
}