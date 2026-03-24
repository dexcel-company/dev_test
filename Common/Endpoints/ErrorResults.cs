using ErrorOr;

namespace CelloPark.Api.Common.Endpoints;

public static class ErrorResults
{
    public static IResult Problem(List<Error> errors)
    {
        if (errors.Count == 0)
        {
            return Results.Problem();
        }

        if (errors.Any(error => error.Type == ErrorType.Unauthorized))
        {
            return Results.Unauthorized();
        }

        if (errors.Any(error => error.Type == ErrorType.Forbidden))
        {
            return Results.Forbid();
        }

        if (errors.All(error => error.Type == ErrorType.Validation))
        {
            return ValidationProblem(errors);
        }

        return Problem(errors[0]);
    }

    private static IResult Problem(Error error)
    {
        int statusCode = error.Type switch
        {
            ErrorType.Validation => StatusCodes.Status400BadRequest,
            ErrorType.Unauthorized => StatusCodes.Status401Unauthorized,
            ErrorType.Forbidden => StatusCodes.Status403Forbidden,
            ErrorType.NotFound => StatusCodes.Status404NotFound,
            ErrorType.Conflict => StatusCodes.Status409Conflict,
            _ => StatusCodes.Status500InternalServerError,
        };

        return Results.Problem(statusCode: statusCode, detail: error.Description);
    }

    private static IResult ValidationProblem(List<Error> errors)
    {
        IDictionary<string, string[]> errorDictionary = errors.ToDictionary(
            error => error.Code,
            error => new string[] { error.Description });

        return Results.ValidationProblem(errorDictionary);
    }
}
