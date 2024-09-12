using EnviBad.API.Common;
using EnviBad.API.Common.DTO;
using EnviBad.API.Common.Models;
using Microsoft.AspNetCore.Mvc;
using EnviBad.API.Common.Log;
using EnviBad.API.Infrastructure.Interfaces;

namespace EnviBad.API.Web.Controllers
{
    [ApiController]
    [Route("report")]
    public class ReportRequestController : ControllerBase
    {
        private readonly IUserReportRequestRepo _userReportRequestRepo;
        private readonly IAppLogger _appLogger;
        private readonly IMassTransitPublisher _massTransitPublisher;

        public ReportRequestController(IMassTransitPublisher massTransitPublisher,
            IAppLogger appLogger, IUserReportRequestRepo userReportRequestRepo)
        {
            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
            _userReportRequestRepo = userReportRequestRepo;
            _appLogger = appLogger;
            _massTransitPublisher = massTransitPublisher;
        }

        /// <summary> Список всех запросов на создание отчетов по области </summary>
        /// <returns>Список запросов на отчеты</returns>
        [HttpGet, Route("requested")]
        public IEnumerable<UserReportRequest> GetReportRequestsList()
        {
            return _userReportRequestRepo.GetAllWithoutTracking();
        }

        /// <summary> Возврат запроса на отчет по его ID в БД </summary>
        /// <returns>Один запрос на создание отчета</returns>
        [HttpGet, Route("requested/{id:int}")]
        public ActionResult<UserReportRequest?> GetReportRequestById(int? id)
        {
            if (id is null)
                return BadRequest("Не указан Id");

            return _userReportRequestRepo.GetWithoutTracking(x => x.Id == id);
        }

        /// <summary> Создание запроса на построение отчета по области </summary>
        /// <param name="model">Параметры отчета по области</param>
        /// <returns>ID запроса на построение отчета в БД</returns>
        [HttpPost, Route("requested")]
        public async Task<ActionResult<int>> RequestReport([FromBody] ReportRequestCreationDto model)
        {
            // TODO: FluentValidation
            if (model.AreaRadius != null &&
                (model.AreaRadius < Const.MinReportAreRadius || model.AreaRadius > Const.MaxReportAreRadius))
            {
                return BadRequest(
                    $"Радиус области меньше {Const.MinReportAreRadius}, либо больше {Const.MaxReportAreRadius}");
            }

            // TODO: Реальный ID пользователя
            int userId = 1;
            // Создание записи о новом запросе отчета в БД
            UserReportRequest createdRequest = new UserReportRequest { UserInfoId = userId };
            createdRequest.FillFromCreationDto(model);
            _userReportRequestRepo.Add(createdRequest);
            string? dbError = await _userReportRequestRepo.SaveAsync(_appLogger, $"Adding report request for user {userId}");
            if (dbError != null) 
                return StatusCode(500, "Не удалось сохранить запрос на отчет в БД");

            // Сообщение в очереди на то, что нужно обрабатывать новый отчет
            string? mqPublishResult = await _massTransitPublisher.PublishReportRequestCreated(model, userId, createdRequest.Id);
            if (mqPublishResult != null)
                return StatusCode(500, mqPublishResult);
            return createdRequest.Id;
        }
    }
}
