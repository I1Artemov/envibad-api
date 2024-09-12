using EnviBad.API.Common.Models;
using EnviBad.API.Infrastructure.Contexts;
using EnviBad.API.Infrastructure.Interfaces;

namespace EnviBad.API.Infrastructure.Repositories
{
    public class UserReportRequestRepo : EFRepo<UserReportRequest, EnviBadApiContext>, IUserReportRequestRepo
    {
        public UserReportRequestRepo(EnviBadApiContext context) : base(context)
        {
        }
    }
}
