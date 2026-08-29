namespace Domain.Entities
{
    public class LKP_Widget : AbstractEntity
    {
        public Guid ID { get; set; }
        public string Name { get; set; } = string.Empty;
        public List<UserChartPreference> LstWidgetPreferences { get; set; } = [];
    }
}
