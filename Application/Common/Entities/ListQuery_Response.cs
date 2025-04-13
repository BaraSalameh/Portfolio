namespace Application.Common.Entities
{
    public class ListQuery_Response<TResponse> where TResponse : class
    {
        public List<TResponse> Items { get; set; } = [];
        public int RowCount { get; set; } = 0;
    }
}