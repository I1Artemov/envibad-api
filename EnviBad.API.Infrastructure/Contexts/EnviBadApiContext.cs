using EnviBad.API.Common.Models;
using Microsoft.EntityFrameworkCore;

namespace EnviBad.API.Infrastructure.Contexts
{
    public class EnviBadApiContext : DbContext
    {
        public EnviBadApiContext(DbContextOptions options) : base(options)
        {}

        public DbSet<UserInfo> UserInfos { get; set; }
        public DbSet<UserReportRequest> UserReportRequests { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            // TODO: Get actual pg connection value
            string connectionString = "";
            optionsBuilder.UseNpgsql(connectionString);
        }
    }
}
