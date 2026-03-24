namespace CelloPark.Domain.Common.Errors;

public static class ErrorDescriptions
{
    public const string Null = "Field '{0}' contains null value.";
    public const string TooShort = "Field '{0}' contains too short value.";
    public const string TooLong = "Field '{0}' contains too long value.";
    public const string TooSmall = "Field '{0}' contains too small value.";
    public const string TooBig = "Field '{0}' contains too big value";
    public const string NotUtc = "Field '{0}' must contains value in UTC format only.";
    public const string Invalid = "Field '{0}' contains invalid value.";
    public const string NotFound = "{0} cannot be found.";
    public const string Conflict = "Record with the same value already exists.";
}
