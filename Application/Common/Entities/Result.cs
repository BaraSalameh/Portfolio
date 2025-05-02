using Domain.Enums;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Application.Common.Entities
{
    public static class Result
    {
        public static IActionResult HandleResult(object result)
        {
            if (result == null)
                return new NotFoundResult();

            var resultType = result.GetType();

            // Generic CommandResponse<T>: can handle auth-related status
            if (resultType.IsGenericType && resultType.GetGenericTypeDefinition() == typeof(CommandResponse<>))
            {
                dynamic res = result;

                return res.ResultType switch
                {
                    ResultType.Unauthorized => new UnauthorizedObjectResult(res.lstError), //401
                    ResultType.Forbidden => new ObjectResult(res.lstError) { StatusCode = StatusCodes.Status403Forbidden },
                    ResultType.NotFound => new NotFoundObjectResult(res.lstError), //404
                    ResultType.ValidationError => new BadRequestObjectResult(res.lstError), //400
                    ResultType.ServerError => new ObjectResult(res.lstError) { StatusCode = StatusCodes.Status500InternalServerError },
                    ResultType.Conflict => new ConflictObjectResult(res.lstError),
                    _ => res.lstError.Count == 0
                        ? new OkObjectResult(res.Data)
                        : new BadRequestObjectResult(res.lstError)
                };
            }

            // Non-generic CommandResponse: basic CUD pattern only
            if (result is CommandResponse cmd)
            {
                return cmd.lstError.Count == 0
                    ? new NoContentResult()
                    : new BadRequestObjectResult(cmd.lstError);
            }

            // ListQueryResponse<T>
            if (resultType.IsGenericType && resultType.GetGenericTypeDefinition() == typeof(ListQueryResponse<>))
            {
                var items = resultType.GetProperty("Items")?.GetValue(result) as System.Collections.ICollection;
                return (items == null || items.Count == 0)
                    ? new NoContentResult()
                    : new OkObjectResult(result);
            }

            // SingleQueryResponse<T>
            if (resultType.IsGenericType && resultType.GetGenericTypeDefinition() == typeof(SingleQueryResponse<>))
            {
                dynamic res = result;

                if (res.lstError?.Count > 0)
                    return new BadRequestObjectResult(res.lstError);

                return new OkObjectResult(res.Data);
            }

            // Default fallback
            return new OkObjectResult(result);
        }
    }
}
