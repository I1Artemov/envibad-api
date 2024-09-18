using System.ComponentModel.DataAnnotations;

namespace EnviBad.API.Common.DTO
{
    /// <summary>
    /// Параметры для запроса на создание отчета по ообласти
    /// </summary>
    public class ReportRequestCreationDto
    {
        [Required(ErrorMessage = "Укажите название отчета")]
        [StringLength(250, MinimumLength = 2, ErrorMessage = "Название отчета должно быть от 2 до 250 символов")]
        public string? ReportName { get; set; }

        [Required(ErrorMessage = "Укажите широту центра области")]
        [Range(-90.0, 90.0)]
        public double? CenterLat { get; set; }

        [Required(ErrorMessage = "Укажите долготу центра области")]
        [Range(-180.0, 180.0)]
        public double? CenterLong { get; set; }

        [Required(ErrorMessage = "Укажите радиус области")]
        [Range(Const.MinReportAreRadius, Const.MaxReportAreRadius)]
        public double? AreaRadius { get; set; }
    }
}
