namespace Domain.Entities
{
    public class LKP_ChartType : AbstractEntity
    {
        public Guid ID { get; set; }
        public string Name { get; set; }
        public List<UserChartPreference> LstChartPreferences { get; set; }
    }
}
