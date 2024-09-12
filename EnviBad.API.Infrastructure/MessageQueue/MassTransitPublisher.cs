using EnviBad.API.Common.DTO;
using EnviBad.API.Common.Log;
using EnviBad.API.Infrastructure.Interfaces;
using EnviBad.Shared.Models.MqMessages;
using MassTransit;

namespace EnviBad.API.Infrastructure.MessageQueue
{
    /// <summary>
    /// Отвечает за публикацию сообщений в RabbitMQ через MAssTransit
    /// </summary>
    public class MassTransitPublisher : IMassTransitPublisher
    {
        private readonly IPublishEndpoint _mqPublishEndpoint;
        private readonly IAppLogger _appLogger;

        public MassTransitPublisher(IPublishEndpoint mqPublishEndpoint, IAppLogger appLogger)
        {
            _mqPublishEndpoint = mqPublishEndpoint;
            _appLogger = appLogger;
        }

        /// <summary>
        /// Публикация сообщения в очереди о том, что кто-то запросил новый отчет по области
        /// </summary>
        public async Task<string?> PublishReportRequestCreated(
            ReportRequestCreationDto reportParams, int userId, int reportEntryId)
        {
            using var cancelSource = new CancellationTokenSource(TimeSpan.FromSeconds(10));
            try
            {
                await _mqPublishEndpoint.Publish<ReportRequestCreated>(new
                {
                    Id = reportEntryId,
                    UserInfoId = userId,
                    CenterLat = reportParams.CenterLat,
                    CenterLong = reportParams.CenterLong,
                    AreaRadius = reportParams.AreaRadius
                }, cancelSource.Token);
                return null;
            }
            catch (Exception ex)
            {
                _appLogger.Error(ex, $"Error publishing MQ message for report ID={reportEntryId}");
                return "Error publishing MQ message: " + ex.Message;
            }
        }
    }
}
