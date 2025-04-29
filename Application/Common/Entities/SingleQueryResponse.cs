namespace Application.Common.Entities
{
    public class SingleQueryResponse<T> where T : class
    {
        public T? Data { get; set; }
        public List<string> lstError { get; set; } = [];
    }
}
