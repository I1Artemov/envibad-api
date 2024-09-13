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
        public string? ResultId { get; set; }
        public string? ErrorDescription { get; set; }
        public double? ExecutionTime { get; set; }

        public UserReportRequest()
        {
            LastStatus = ReportStatus.Created.ToString();
            LastStatusDateTime = DateTimeOffset.Now;
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

        /// <summary>
        /// Обновление полей запроса на отчет по прилетевшей PATCH-модели
        /// </summary>
        public void SetFieldsFromPatchModel(ReportRequestUpdateDto dto)
        {
            if (!string.IsNullOrEmpty(dto.Status))
                LastStatus = dto.Status;
            if (dto.ResultId is not null)
                ResultId = dto.ResultId;
            if (dto.ErrorDescription is not null)
                ErrorDescription = dto.ErrorDescription;
            if (LastStatusDateTime is not null)
                ExecutionTime = (DateTimeOffset.Now - LastStatusDateTime).Value.TotalSeconds;
        }
    }
}
