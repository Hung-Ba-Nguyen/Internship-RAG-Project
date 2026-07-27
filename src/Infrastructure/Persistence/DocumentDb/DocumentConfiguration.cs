#nullable enable
using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RAGKnowledgeBase.Core.Domain.Entities.Document;
using RAGKnowledgeBase.Core.Domain.ValueObjects;

namespace RAGKnowledgeBase.Infrastructure.Persistence.DocumentDb;

public class DocumentConfiguration : IEntityTypeConfiguration<Document>
{
    private static readonly JsonSerializerOptions JsonOptions = new() { PropertyNameCaseInsensitive = true };

    public void Configure(EntityTypeBuilder<Document> builder)
    {
        builder.ToTable("Documents");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedNever();

        builder.Property(x => x.Title).HasMaxLength(250).IsRequired();
        builder.Property(x => x.Content).HasColumnType("nvarchar(max)").IsRequired();
        builder.Property(x => x.FileName).HasMaxLength(260);
        builder.Property(x => x.FilePath).HasMaxLength(1024);
        builder.Property(x => x.MimeType).HasMaxLength(128);
        builder.Property(x => x.SizeInBytes).IsRequired();
        builder.Property(x => x.Source).HasMaxLength(500).IsRequired();
        builder.Property(x => x.CreatedAt).IsRequired();
        builder.Property(x => x.UpdatedAt).IsRequired();
        builder.Property(x => x.IsDeleted).IsRequired();
        builder.Property(x => x.CreatedByUserId);
        builder.Property(x => x.Status).HasConversion<int>().IsRequired();

        builder.Property(x => x.Category)
            .HasConversion(
                value => value == null ? null : JsonSerializer.Serialize(value, JsonOptions),
                json => string.IsNullOrEmpty(json) ? null : JsonSerializer.Deserialize<DocumentCategory>(json, JsonOptions))
            .HasColumnType("nvarchar(max)");

        builder.Property(x => x.Tags)
            .HasConversion(
                value => JsonSerializer.Serialize(value, JsonOptions),
                json => string.IsNullOrEmpty(json) ? new List<DocumentTag>() : 
                            JsonSerializer.Deserialize<List<DocumentTag>>(json, JsonOptions) 
                            ?? new List<DocumentTag>())
            .Metadata.SetValueComparer(new ValueComparer<List<DocumentTag>>(
                (left, right) => JsonSerializer.Serialize(left, JsonOptions) == JsonSerializer.Serialize(right, JsonOptions),
                tags => JsonSerializer.Serialize(tags, JsonOptions).GetHashCode(),
                tags => tags == null ? new List<DocumentTag>() : JsonSerializer.Deserialize<List<DocumentTag>>(
                    JsonSerializer.Serialize(tags, JsonOptions), JsonOptions) 
                    ?? new List<DocumentTag>()));

        builder.HasMany(x => x.Chunks)
            .WithOne(x => x.Document)
            .HasForeignKey(x => x.DocumentId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
