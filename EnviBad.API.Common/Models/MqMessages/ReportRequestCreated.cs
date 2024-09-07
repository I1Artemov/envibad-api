namespace EnviBad.Shared.Models.MqMessages
{
    public interface ReportRequestCreated
    {
        public int Id { get; set; }
        public int UserInfoId { get; set; }
        public double? CenterLat { get; set; }
        public double? CenterLong { get; set; }
        public double? AreaRadius { get; set; }
    }
}
