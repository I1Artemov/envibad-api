using EnviBad.API.Common.DTO;

namespace EnviBad.API.Infrastructure.Interfaces
{
    public interface IMassTransitPublisher
    {
        Task<string?> PublishReportRequestCreated(ReportRequestCreationDto reportParams, int userId, int reportEntryId);
    }
}
