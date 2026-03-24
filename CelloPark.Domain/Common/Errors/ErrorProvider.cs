using ErrorOr;

namespace CelloPark.Domain.Common.Errors;

public static class ErrorProvider
{
    public static List<Error> Join(params IErrorOr[] results)
    {
        List<Error> errors = [];

        foreach (IErrorOr result in results)
        {
            if (result.IsError)
            {
                errors.Add(result.Errors![0]);
            }
        }

        return errors;
    }
}
