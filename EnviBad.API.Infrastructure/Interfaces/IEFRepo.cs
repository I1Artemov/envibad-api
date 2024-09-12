using EnviBad.API.Common.Log;
using Microsoft.EntityFrameworkCore;

namespace EnviBad.API.Infrastructure.Interfaces
{
    /// <summary>
    /// Интерфейс для обобщенного репозитория, использующего EntityFramework
    /// </summary>
    public interface IEFRepo<TEntity, TContext> : IDisposable
        where TEntity : class
        where TContext : DbContext
    {
        void Add(TEntity item);
        void Delete(TEntity item);
        TEntity? Get(int id);
        TEntity? GetWithoutTracking(Func<TEntity, bool> condition);
        IQueryable<TEntity> GetAll();
        IQueryable<TEntity> GetAllWithoutTracking();
        void Update(TEntity item);
        string? Save(IAppLogger logger, string description);
        Task<string?> SaveAsync(IAppLogger logger, string description);
        List<TEntity> ApplyPagination(IQueryable<TEntity> allData, int skip, int pageSize);
    }
}
