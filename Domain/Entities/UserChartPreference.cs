namespace Domain.Entities
{
    public class UserChartPreference : AbstractEntity
    {
        public Guid UserID { get; set; }
        public User User { get; set; }
        public Guid LKP_WidgetID { get; set; }
        public LKP_Widget LKP_Widget { get; set; }
        public Guid LKP_ChartTypeID { get; set; }
        public LKP_ChartType LKP_ChartType { get; set; }
        public string GroupBy { get; set; }
        public string? ValueSource { get; set; }
    }
}
