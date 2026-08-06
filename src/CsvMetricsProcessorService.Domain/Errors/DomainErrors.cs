using CsvMetricsProcessorService.Domain.Results;

namespace CsvMetricsProcessorService.Domain.Errors;

public static class DomainErrors
{
    // Ошибки для VO - FileName
    public static class FileName
    {
        public static readonly Error Empty = new Error(
            "MetricsValue.EmptyName",
            "File name can't be empty");

        public static readonly Error TooLong = new Error(
            "MetricsValue.MaxFileName",
            "File name should be in range 1-255 symbols");

        public static readonly Error InvalidChars = new Error(
            "MetricsValue.InvalidChars",
            "File name should not contain special characters");

        public static readonly Error InvalidFileType = new Error(
            "MetricsValue.InvalidFileType",
            "File should be .csv type");
    }
    
    // Ошибки для VO - ExecutionTime
    public static class ExecutionTime
    {
        public static readonly Error Negative = new Error(
            "ExecutionTime.Negative",
            "Execution time can't be negative");
        
        public static readonly Error TooLong = new Error(
            "ExecutionTime.TooLong",
            "Execution time should be realistic");
        
        public static readonly Error InfinityNan = new Error(
            "ExecutionTime.InfinityNan",
            "Execution time can't be infinite or Nan");
    }

    public static class Value
    {
        public static readonly Error InfinityNan = new Error(
            "Value.InfinityNan",
            "Value can't be infinite or Nan");
        
        public static readonly Error Negative = new Error(
            "Value.Negative",
            "Value can't be negative");
    }
    
    // Ошибки для сущности MetricsValue
    public static class MetricsValue
    {
        public static readonly Error InvalidDate = new Error(
            "MetricsValue.InvalidDate",
            "Date can't be in the feature and earlier than 01-01-2000");
    }
}