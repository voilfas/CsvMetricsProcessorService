namespace CsvMetricsProcessorService.Domain.Common;

public abstract class BaseEntity
{
    public Guid Id { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime? UpdatedAt { get; private set; }

    protected BaseEntity()
    {
        Id = Guid.NewGuid();
        CreatedAt = DateTime.UtcNow;
    }

    public override bool Equals(object? obj)
    {
        if (obj is not BaseEntity entity)
            return false;
        
        return Id == entity.Id;
    }
    
    public override int GetHashCode() => Id.GetHashCode();
}