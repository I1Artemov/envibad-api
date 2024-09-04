namespace EnviBad.API.Common.DTO
{
    /// <summary>
    /// Параметры для запроса на создание отчета по ообласти
    /// </summary>
    public class ReportRequestCreationDto
    {
        public string? ReportName { get; set; }
        public double? CenterLat { get; set; }
        public double? CenterLong { get; set; }
        public double? AreaRadius { get; set; }
    }
}
