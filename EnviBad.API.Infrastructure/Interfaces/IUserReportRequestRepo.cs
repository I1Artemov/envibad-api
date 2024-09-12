using EnviBad.API.Common.Models;
using EnviBad.API.Infrastructure.Contexts;

namespace EnviBad.API.Infrastructure.Interfaces
{
    public interface IUserReportRequestRepo : IEFRepo<UserReportRequest, EnviBadApiContext>
    {
    }
}
