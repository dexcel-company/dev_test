using Microsoft.AspNetCore.Diagnostics;

namespace CelloPark.Api.Common.ExceptionHandlers;

public sealed class BadHttpRequestExceptionHandler :
    IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        if (exception is not BadHttpRequestException badHttpRequestException)
        {
            return false;
        }

        IResult result = Results.Problem(
            title: "Bad request",
            detail: badHttpRequestException.Message,
            statusCode: StatusCodes.Status400BadRequest);

        await result.ExecuteAsync(httpContext);

        return true;
    }
}
