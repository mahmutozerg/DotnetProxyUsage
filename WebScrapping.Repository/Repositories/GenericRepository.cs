using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using WebScrapping.Core.Models;
using WebScrapping.Core.Repositories;

namespace WebScrapping.Repository.Repositories;

public class GenericRepository<TEntity>:IGenericRepository<TEntity> where TEntity:Base
{

    private readonly DbSet<TEntity> _dbSet;

    public GenericRepository(AppDbContext dbContext)
    {
        _dbSet = dbContext.Set<TEntity>();
    }

    public IQueryable<TEntity?> Where(Expression<Func<TEntity?, bool>> expression)
    {
        return _dbSet.Where(expression);
    }

    public async Task<bool> AnyAsync(Expression<Func<TEntity?, bool>> expression)
    {
        return await _dbSet.AnyAsync(expression);
    }

    public void Update(TEntity? entity,string updatedBy)
    {
        entity.UpdatedAt = DateTime.Now;
        entity.UpdatedBy = updatedBy;
        _dbSet.Update(entity);
    }

    public async Task AddAsync(TEntity? entity, string createdBy)
    {
        entity.CreatedAt = DateTime.Now;
        entity.CreatedBy = createdBy;
        
        await _dbSet.AddAsync(entity);
        
    }

    public void Remove(TEntity entity, string updatedBy)
    {
        entity.IsDeleted = true;
        Update(entity,updatedBy);
        
    }

    public async Task<TEntity?> GetByIdAsync(string id)
    {
        return await _dbSet
                .Where(e => !e.IsDeleted && e.Id == id)
                .SingleOrDefaultAsync();
    }
}