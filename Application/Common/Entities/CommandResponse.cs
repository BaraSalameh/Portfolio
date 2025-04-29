using Domain.Enums;

namespace Application.Common.Entities
{
    public class CommandResponse
    {
        public List<string> lstError { get; set; } = [];
    }

    public class CommandResponse<T> : CommandResponse
    {
        public T? Data { get; set; }
        public ResultType ResultType { get; set; } = ResultType.Success;
    }
}
