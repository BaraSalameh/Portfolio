using MediatR;
using System.ComponentModel.DataAnnotations;

namespace Application.Common.Entities
{
    public class PaginationQuery : IValidatableObject
    {
        public const int MaximumOffset = 100_000;

        [StringLength(200)]
        public string? Search { get; set; }

        [Range(0, 100_000)]
        public int PageNumber { get; set; }

        [Range(1, 100)]
        public int PageSize { get; set; } = 10;

        public int Offset => PageSize <= 0
            ? 0
            : PageNumber > int.MaxValue / PageSize
                ? int.MaxValue
                : PageNumber * PageSize;

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (PageNumber >= 0 && PageSize > 0 &&
                (long)PageNumber * PageSize > MaximumOffset)
            {
                yield return new ValidationResult(
                    $"The requested page exceeds the maximum offset of {MaximumOffset} rows.",
                    [nameof(PageNumber), nameof(PageSize)]);
            }
        }
    }

    public class ListQuery<TResponse> : PaginationQuery, IRequest<ListQueryResponse<TResponse>> where TResponse : class
    {
    }

    /// <summary>
    /// Adds bounded pagination to legacy owner collection routes while keeping
    /// their previous practical behavior for normal-sized portfolios.
    /// </summary>
    public class OwnerCollectionQuery<TResponse> : ListQuery<TResponse> where TResponse : class
    {
        public OwnerCollectionQuery()
        {
            PageSize = 100;
        }
    }
}
