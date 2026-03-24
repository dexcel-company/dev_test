using Microsoft.AspNetCore.Diagnostics;
using System.Text;

namespace CelloPark.Api.Common.ExceptionHandlers;

public sealed class InternalExceptionHandler :
    IExceptionHandler
{
    public InternalExceptionHandler(TimeProvider timeProvider)
    {
        _timeProvider = timeProvider;
    }

    private readonly TimeProvider _timeProvider;

    private static readonly string _filePath = Path.Combine(Environment.CurrentDirectory, "Exceptions.txt");

    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        IResult result = Results.Problem(
            title: "Internal server error",
            detail: "An error occurred while processing your request.",
            statusCode: StatusCodes.Status500InternalServerError);

        using StreamReader requestBodyReader = new(httpContext.Request.Body);

        ExceptionFile.SaveToFile(
            filePath: _filePath,
            exception: exception,
            requestMethod: httpContext.Request.Method,
            requestPath: httpContext.Request.Path,
            requestBody: await requestBodyReader.ReadToEndAsync(cancellationToken),
            timeProvider: _timeProvider);

        await result.ExecuteAsync(httpContext);

        return true;
    }
}

public class ExceptionFile
{
    private static readonly object _lockObject = new();

    public static void SaveToFile(
        string filePath, Exception exception, string requestMethod, string requestPath, string requestBody, TimeProvider timeProvider)
    {
        lock (_lockObject)
        {
            using StreamWriter writer = File.AppendText(filePath);
            DateTimeOffset utcNow = timeProvider.GetUtcNow();

            writer.WriteLine($"An error occured at: {utcNow} in endpoint: {requestMethod} {requestPath}");
            writer.WriteLine($"An error request body: {requestBody}");
            writer.WriteLine($"An error message: {exception.Message}");
            writer.WriteLine($"An error stack trace: {exception.StackTrace}");
            writer.WriteLine(Environment.NewLine);
            writer.Flush();
        }
    }

    public static LinkedList<string> ReadFile(string filePath)
    {
        LinkedList<string> lines = new();

        lock (_lockObject)
        {
            string? line;

            using FileStream fileStream = new(filePath, FileMode.OpenOrCreate, FileAccess.Read, FileShare.Read);
            using StreamReader streamReader = new(fileStream, Encoding.UTF8, true);

            while ((line = streamReader.ReadLine()) != null)
            {
                lines.AddLast(line);
            }
        }

        return lines;
    }
}
