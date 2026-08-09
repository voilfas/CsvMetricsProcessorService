using CsvMetricsProcessorService.Domain.Entities;
using CsvMetricsProcessorService.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CsvMetricsProcessorService.Infrastructure.Persistence.Configurations;

public class MetricsValueConfiguration : IEntityTypeConfiguration<MetricsValue>
{
    public void Configure(EntityTypeBuilder<MetricsValue> builder)
    {
        builder.ToTable("metrics_values");

        builder.HasKey(v => v.Id);

        builder.Property(v => v.Id)
            .HasColumnName("id")
            .ValueGeneratedNever()
            .IsRequired();

        builder.Property(v => v.FileName)
            .HasConversion(
                fullName => fullName.Value,
                value => FileName.FromDb(value))
            .HasColumnName("file_name")
            .HasMaxLength(255)
            .IsRequired();
        
        builder.Property(v => v.Date)
            .HasColumnName("date")
            .IsRequired();
        
        builder.Property(v => v.ExecutionTime)
            .HasConversion(
                executionTime => executionTime.TotalSeconds,
                value => ExecutionTime.Create(value).Value!)
            .HasColumnName("execution_time")
            .IsRequired();
        
        builder.Property(v => v.Value)
            .HasConversion(
                indicator => indicator.Indicator,
                value => Value.FromDb(value))
            .HasColumnName("value")
            .IsRequired();

        builder.Property(v => v.CreatedAt)
            .HasColumnName("created_at")
            .IsRequired();

        builder.HasIndex(v => new { v.FileName, v.Date }).IsDescending(false, true);

    }
}