using CsvMetricsProcessorService.Domain.Entities;
using CsvMetricsProcessorService.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CsvMetricsProcessorService.Infrastructure.Persistence.Configurations;

public class MetricsResultConfiguration : IEntityTypeConfiguration<MetricsResult>
{
    public void Configure(EntityTypeBuilder<MetricsResult> builder)
    {
        builder.ToTable("metrics_results");
        
        builder.HasKey(r => r.Id);
        
        builder.Property(r => r.Id)
            .HasColumnName("id")
            .ValueGeneratedNever()
            .IsRequired();
        
        builder.Property(r => r.FileName)
            .HasConversion(
                fullName => fullName!.Value,
                value => FileName.FromDb(value))
            .HasColumnName("file_name")
            .HasMaxLength(255)
            .IsRequired();
        
        builder.Property(r => r.DeltaDate)
            .HasColumnName("delta_date")
            .IsRequired();
        
        builder.Property(r => r.MinDate)
            .HasColumnName("min_date")
            .IsRequired();
        
        builder.Property(r => r.AvgExecutionTime)
            .HasColumnName("avg_execution_time")
            .IsRequired();
        
        builder.Property(r => r.AvgValue)
            .HasColumnName("avg_value")
            .IsRequired();
        
        builder.Property(r => r.MedianValue)
            .HasColumnName("median_value")
            .IsRequired();
        
        builder.Property(r => r.MaxValue)
            .HasColumnName("max_value")
            .IsRequired();
        
        builder.Property(r => r.MinValue)
            .HasColumnName("min_value")
            .IsRequired();
        
        builder.Property(r => r.CreatedAt)
            .HasColumnName("created_at")
            .IsRequired();
        
        builder.HasIndex(r => r.FileName);
        builder.HasIndex(r => r.MinDate);
        builder.HasIndex(r => r.AvgValue);
        builder.HasIndex(r => r.AvgExecutionTime);
    }
}