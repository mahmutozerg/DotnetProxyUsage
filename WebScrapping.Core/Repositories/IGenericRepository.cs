using System.Linq.Expressions;
using WebScrapping.Core.Models;

namespace WebScrapping.Core.Repositories;

public interface IGenericRepository<TEntity> where TEntity:Base?
{
    IQueryable<TEntity?> Where(Expression<Func<TEntity?, bool>> expression);
    Task<bool> AnyAsync(Expression<Func<TEntity?, bool>> expression);
    void Update(TEntity? entity,string updatedBy);
    Task AddAsync(TEntity? entity,string createdBy);
    void Remove(TEntity entity,string updatedBy);
    Task<TEntity?> GetByIdAsync(string id);
}