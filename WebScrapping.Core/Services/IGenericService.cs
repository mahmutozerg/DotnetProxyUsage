using System.Linq.Expressions;
using WebScrapping.Core.Models;

namespace WebScrapping.Core.Services;

public interface IGenericService<TEntity> where TEntity:Base
{

    Task RemoveAsync(string id,string updatedBy);
    Task AddAsync(TEntity entity,string createdBy);
    IQueryable<TEntity?> Where(Expression<Func<TEntity?, bool>> expression);
    Task UpdateAsync(TEntity? entity,string updatedBy);
    Task<TEntity> GetByIdAsync(string id);
    
}