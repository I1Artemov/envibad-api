using EnviBad.API.Common.DTO;

namespace EnviBad.API.Common.Models
{
    /// <summary>
    /// Сущность для запросов пользователей на построение отчетов по области
    /// </summary>
    public class UserReportRequest : IdInfo
    {
        public int? UserInfoId { get; set; }
        public string? ReportName { get; set; }
        public double? CenterLat { get; set; }
        public double? CenterLong { get; set; }
        public double? AreaRadius { get; set; }
        public string? LastStatus { get; set; }
        public DateTimeOffset? LastStatusDateTime { get; set; }

        public UserReportRequest()
        {
            LastStatus = ReportStatus.Created.ToString();
        }

        /// <summary>
        /// Заполнение новой записи о запросе на отчет по модели, переданной в API Add
        /// </summary>
        public void FillFromCreationDto(ReportRequestCreationDto dto)
        {
            CenterLat = dto.CenterLat;
            CenterLong = dto.CenterLong;
            AreaRadius = dto.AreaRadius;
            ReportName = dto.ReportName;
        }
    }
}
