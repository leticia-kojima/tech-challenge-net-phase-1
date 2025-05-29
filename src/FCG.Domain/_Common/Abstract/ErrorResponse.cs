using FCG.Domain._Common.Exceptions;
using System.Net;

namespace FCG.Domain._Common.Abstract;
public class ErrorResponse
{
    public ErrorResponse(HttpStatusCode statusCode, string message)
    {
        StatusCode = statusCode;
        Message = message;
    }

    public ErrorResponse(FCGDuplicateException duplicateException)
    {
        StatusCode = HttpStatusCode.Conflict;
        Message = duplicateException.Message;
        Entity = duplicateException.Entity;
        Key = duplicateException.Key;
    }

    public ErrorResponse(FCGNotFoundException notFoundException)
    {
        StatusCode = HttpStatusCode.NotFound;
        Message = notFoundException.Message;
        Key = notFoundException.Key;
        Entity = notFoundException.Entity;
    }

    public ErrorResponse(FCGValidationException validationException)
    {
        StatusCode = HttpStatusCode.BadRequest;
        Message = validationException.Message;
        Field = validationException.Field;
    }

    public HttpStatusCode StatusCode { get; private set; }
    public string Message { get; private set; }
    public Guid? Key { get; private set; }
    public string? Field { get; private set; }
    public string? Entity { get; private set; }
}
