using Domain.Enums;
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
                    ResultType.Unauthorized => new UnauthorizedObjectResult(res.lstError),
                    ResultType.Forbidden => new ObjectResult(res.lstError) { StatusCode = 403 },
                    ResultType.NotFound => new NotFoundObjectResult(res.lstError),
                    ResultType.ValidationError => new BadRequestObjectResult(res.lstError),
                    _ => res.lstError.Count == 0
                        ? new OkObjectResult(res)
                        : new BadRequestObjectResult(res)
                };
            }

            // Non-generic CommandResponse: basic CUD pattern only
            if (result is CommandResponse cmd)
            {
                return cmd.lstError.Count == 0
                    ? new NoContentResult()
                    : new BadRequestObjectResult(cmd.lstError);
            }

            // ListQuery_Response<T>
            if (resultType.IsGenericType && resultType.GetGenericTypeDefinition() == typeof(ListQuery_Response<>))
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

                return res.Data == null
                    ? new NotFoundResult()
                    : new OkObjectResult(result);
            }

            // Default fallback
            return new OkObjectResult(result);
        }
    }
}
