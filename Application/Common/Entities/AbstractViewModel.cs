namespace Application.Common.Entities
{
    public class AbstractViewModel
    {
        public bool status { get; set; } = true;
        public List<string> lstError { get; set; } = new List<string>();
    }
}