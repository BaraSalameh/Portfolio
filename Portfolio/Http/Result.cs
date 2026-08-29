using Domain.Enums;
using Microsoft.AspNetCore.Mvc;

namespace Application.Common.Entities;

public static class Result
{
    public static IActionResult HandleResult(object? result)
    {
        if (result is null)
        {
            return new NotFoundResult();
        }

        var resultType = result.GetType();
        if (resultType.IsGenericType && resultType.GetGenericTypeDefinition() == typeof(CommandResponse<>))
        {
            dynamic response = result;
            return response.ResultType switch
            {
                ResultType.Unauthorized => new UnauthorizedObjectResult(response.lstError),
                ResultType.Forbidden => new ObjectResult(response.lstError) { StatusCode = StatusCodes.Status403Forbidden },
                ResultType.NotFound => new NotFoundObjectResult(response.lstError),
                ResultType.ValidationError => new BadRequestObjectResult(response.lstError),
                ResultType.ServerError => new ObjectResult(response.lstError) { StatusCode = StatusCodes.Status500InternalServerError },
                ResultType.Conflict => new ConflictObjectResult(response.lstError),
                _ => response.lstError.Count == 0
                    ? new OkObjectResult(response.Data)
                    : new BadRequestObjectResult(response.lstError)
            };
        }

        if (result is CommandResponse command)
        {
            return command.lstError.Count == 0
                ? new NoContentResult()
                : new BadRequestObjectResult(command.lstError);
        }

        if (resultType.IsGenericType && resultType.GetGenericTypeDefinition() == typeof(ListQueryResponse<>))
        {
            var items = resultType.GetProperty("Items")?.GetValue(result) as System.Collections.ICollection;
            return items is null || items.Count == 0
                ? new NoContentResult()
                : new OkObjectResult(result);
        }

        if (resultType.IsGenericType && resultType.GetGenericTypeDefinition() == typeof(SingleQueryResponse<>))
        {
            dynamic response = result;
            return response.lstError?.Count > 0
                ? new BadRequestObjectResult(response.lstError)
                : new OkObjectResult(response.Data);
        }

        return new OkObjectResult(result);
    }
}
