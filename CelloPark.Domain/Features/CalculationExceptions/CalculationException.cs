using CelloPark.Domain.Features.CalculationExceptions.Enums;
using ErrorOr;

namespace CelloPark.Domain.Features.CalculationExceptions;

public sealed class CalculationException
{
    private CalculationException() { }

    private CalculationException(
        CalculationExceptionType type,
        string message,
        DateTimeOffset dateTime)
    {
        Id = Guid.NewGuid();
        Type = type;
        Message = message;
        DateTime = dateTime.UtcDateTime;
    }

    public Guid Id { get; }
    public CalculationExceptionType Type { get; private set; }
    public string Message { get; private set; } = null!;
    public DateTime DateTime { get; private set; }

    public static CalculationException Create(
        string message,
        CalculationExceptionType type,
        DateTimeOffset dateTime)
    {
        CalculationException remoteException = new(
            type: type,
            message: message,
            dateTime: dateTime);

        return remoteException;
    }

    public static CalculationException Create(
        Error error,
        CalculationExceptionType type,
        DateTimeOffset dateTime)
    {
        CalculationException remoteException = new(
            type: type,
            message: error.Description,
            dateTime: dateTime);

        return remoteException;
    }

    public static IReadOnlyCollection<CalculationException> Create(
        List<Error> errors,
        CalculationExceptionType type,
        DateTimeOffset dateTime)
    {
        LinkedList<CalculationException> RemoteExceptions = new();

        foreach (Error error in errors)
        {
            CalculationException remoteException = new(
                type: type,
                message: error.Description,
                dateTime: dateTime);

            RemoteExceptions.AddLast(remoteException);
        }

        return RemoteExceptions;
    }
}
