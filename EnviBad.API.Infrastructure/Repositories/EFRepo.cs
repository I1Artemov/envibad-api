using EnviBad.API.Common.Log;
using Microsoft.EntityFrameworkCore;

namespace EnviBad.API.Infrastructure.Repositories
{
    /// <summary>
    /// Шаблонный репозиторий для доступа в БД ко всем видам сущностей
    /// </summary>
    /// <typeparam name="TEntity">Тип сущности</typeparam>
    /// <typeparam name="TContext">Контекст БД</typeparam>
    public class EFRepo<TEntity, TContext> : IDisposable
        where TEntity : class
        where TContext : DbContext
    {
        protected TContext _context;
        protected DbSet<TEntity> _dbSet;
        protected bool disposed = false;

        public EFRepo(TContext context)
        {
            _context = context;
            _dbSet = context.Set<TEntity>();
        }

        public void Add(TEntity item)
        {
            _dbSet.Add(item);
        }

        public void Delete(TEntity item)
        {
            _dbSet.Remove(item);
        }

        public TEntity? Get(int id)
        {
            return _dbSet.Find(id);
        }

        public TEntity? GetWithoutTracking(Func<TEntity, bool> condition)
        {
            return _dbSet.AsNoTracking().FirstOrDefault(condition);
        }

        public IQueryable<TEntity> GetAll()
        {
            return _dbSet;
        }

        public IQueryable<TEntity> GetAllWithoutTracking()
        {
            return _dbSet.AsNoTracking();
        }

        public void Update(TEntity item)
        {
            _context.Entry(item).State = EntityState.Modified;
        }

        public string? Save(IAppLogger logger, string description)
        {
            try
            {
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                if (logger != null)
                    logger.Error(ex, description);

                return ex.Message;
            }
            return null;
        }

        public async Task<string?> SaveAsync(IAppLogger logger, string description)
        {
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                if (logger != null)
                    logger.Error(ex, description);

                return ex.Message;
            }
            return null;
        }

        public List<TEntity> ApplyPagination(IQueryable<TEntity> allData, int skip, int pageSize)
        {
            return allData.Skip(skip).Take(pageSize).ToList();
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
