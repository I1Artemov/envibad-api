using EnviBad.API.Common;
using EnviBad.API.Common.DTO;
using EnviBad.API.Common.Models;
using EnviBad.API.Common.Models.MqMessages;
using EnviBad.API.Infrastructure.Contexts;
using MassTransit;
using Microsoft.AspNetCore.Mvc;

namespace EnviBad.API.Web.Controllers
{
    [ApiController]
    [Route("report")]
    public class ReportRequestController : ControllerBase
    {
        private readonly EnviBadApiContext _db;
        private readonly IPublishEndpoint _mqPublishEndpoint;

        public ReportRequestController(EnviBadApiContext db, IPublishEndpoint mqPublishEndpoint)
        {
            _db = db;
            _mqPublishEndpoint = mqPublishEndpoint;
        }

        private List<UserReportRequest> _mockReports = new List<UserReportRequest> {
            new UserReportRequest{
                Id=1, ReportName="Тестовый отчет 1", UserInfoId=1, AreaRadius=125,
                CenterLat=66.4316, CenterLong=59.12894, LastStatus=ReportStatus.InProgress.ToString()
            },
            new UserReportRequest{
                Id=2, ReportName="Второй тестовый отчет", UserInfoId=2, AreaRadius=430.6,
                CenterLat=66.1230, CenterLong=60.0765, LastStatus=ReportStatus.Failed.ToString()
            }
        };

        /// <summary> Список всех запросов на создание отчетов по области </summary>
        /// <returns>Список запросов на отчеты</returns>
        [HttpGet, Route("requested")]
        public IEnumerable<UserReportRequest> GetReportRequestsList()
        {
            return _mockReports;
        }

        /// <summary> Возврат запроса на отчет по его ID в БД </summary>
        /// <returns>Один запрос на создание отчета</returns>
        [HttpGet, Route("requested/{id:int}")]
        public UserReportRequest? GetReportRequestById(int id)
        {
            return _mockReports.FirstOrDefault(x => x.Id == id);
        }

        /// <summary> Создание запроса на построение отчета по области </summary>
        /// <param name="model">Параметры отчета по области</param>
        /// <returns>ID запроса на построение отчета в БД</returns>
        [HttpPost, Route("requested")]
        public async Task<ActionResult<int>> RequestReport([FromBody] ReportRequestCreationDto model)
        {
            if (model.AreaRadius != null && 
                (model.AreaRadius < Const.MinReportAreRadius || model.AreaRadius > Const.MaxReportAreRadius))
            {
                return BadRequest(
                    $"Радиус области меньше {Const.MinReportAreRadius}, либо больше {Const.MaxReportAreRadius}");
            }

            // TODO: Создать запись в БД

            // Сообщение в очереди на то, что нужно обрабатывать новый отчет
            string? mqPublishResult = await publishReportRequestCreated(model, 1, 1);
            if (mqPublishResult != null)
                return StatusCode(500, mqPublishResult);
            return 1;
        }

        private async Task<string?> publishReportRequestCreated(
            ReportRequestCreationDto reportParams, int userId, int reportEntryId)
        {
            using var cancelSource = new CancellationTokenSource(TimeSpan.FromSeconds(10));
            try
            {
                await _mqPublishEndpoint.Publish<ReportRequestCreated>(new
                {
                    Id = reportEntryId, // TODO: ID Создаваемой записи
                    UserInfoId = userId, // TODO: Actual User ID from request
                    CenterLat = reportParams.CenterLat,
                    CenterLong = reportParams.CenterLong,
                    AreaRadius = reportParams.AreaRadius
                }, cancelSource.Token);
                return null;
            }
            catch (Exception ex)
            {
                // TODO: Logging
                return "Error publishing MQ message: " + ex.Message;
            }
        }
    }
}
