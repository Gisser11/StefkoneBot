using System.Linq;
using System.Threading.Tasks;

namespace ValiBot.Repository.Interfaces
{
    public interface IBaseRepository <TEntity>
    {
        IQueryable<TEntity> GetAll();

        Task<TEntity> CreateAsync(TEntity entity);

        Task<TEntity> UpdateAsync(TEntity entity);

        Task<TEntity> RemoveAsync(TEntity entity);
    }}