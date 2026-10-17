using Microsoft.AspNetCore.Mvc;
using UniversityApi.Common;

namespace UniversityApi.Controllers;

[ApiController]
public abstract class ApiControllerBase : ControllerBase
{
    protected IActionResult OkResult<T>(T? data) => StatusCode(200, ReturnResult<T>.Success(data, 200, HttpContext.TraceIdentifier));
    protected IActionResult CreatedResult<T>(string action, object routeValues, T data) =>
        StatusCode(201, ReturnResult<T>.Success(data, 201, HttpContext.TraceIdentifier));
    protected IActionResult Error<T>(int status, string code, string message) =>
        StatusCode(status, ReturnResult<T>.Failure(status, code, message, HttpContext.TraceIdentifier));
}
