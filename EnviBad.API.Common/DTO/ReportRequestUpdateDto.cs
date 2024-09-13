namespace EnviBad.API.Common.DTO
{
    /// <summary>
    /// Параметры для обновления запроса на пользовательский отчет (статус, ссылка на результат)
    /// </summary>
    public class ReportRequestUpdateDto
    {
        public int? ReportRequestId { get; set; }
        public string? Status { get; set; }
        public string? ResultId { get; set; }
        public string? ErrorDescription { get; set; }
    }
}
