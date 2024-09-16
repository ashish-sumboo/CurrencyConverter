namespace BuildingBlocks;

using System.Net;
using Microsoft.AspNetCore.Mvc;

public static class ApiResult
{
    public static ActionResult From(Error error)
    {
        return error.ErrorType switch
        {
            Error.NotFoundErrorType => new NotFoundResult(),
            Error.ServiceUnavailableErrorType => new StatusCodeResult((int) HttpStatusCode.ServiceUnavailable),
            _ => new UnprocessableEntityObjectResult(new
            {
                error.ErrorType,
                error.ErrorCodes
            })
        };
    }
}