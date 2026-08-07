namespace CsvMetricsProcessorService.Domain.Results;

public class Result<T> : Result
{
    public T? Value { get; private set; }
    
    private Result(T value) 
        : base(isSuccess: true, error: null)
    {
        Value = value;
    }

    private Result(Error error) 
        : base(isSuccess: false ,error: error)
    {
        Value = default;
    }

    public static Result<T> Success(T value)
    {
        return new Result<T>(value);
    }

    public new static Result<T> Failure(Error error)
    {
        return new Result<T>(error);
    }
}